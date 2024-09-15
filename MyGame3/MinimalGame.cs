using System;
using Stride.Audio;
using Stride.Core.IO;
using Stride.Core.Storage;
using Stride.Engine;
using Stride.Engine.Design;
using Stride.Engine.Processors;
using Stride.Games;
using Stride.Graphics;
using Stride.Graphics.Font;
using Stride.Input;
using Stride.Profiling;
using Stride.Rendering;
using Stride.Rendering.Fonts;
using Stride.Rendering.Sprites;
using Stride.Shaders.Compiler;
using Stride.Streaming;

namespace MyGame3;

/// <summary>
/// A stripped down version of the <see cref="Game"/> class.
/// </summary>
public class MinimalGame : GameBase, ISceneRendererContext, IGameSettingsService
{

	private readonly GameFontSystem gameFontSystem;

	private DatabaseFileProvider databaseFileProvider;

	/// <summary>
	/// Readonly game settings as defined in the GameSettings asset
	/// Please note that it will be populated during initialization
	/// It will be ok to read them after the GameStarted event or after initialization
	/// </summary>
	public GameSettings Settings { get; private set; } // for easy transfer from PrepareContext to Initialize

	/// <summary>
	/// Gets the graphics device manager.
	/// </summary>
	/// <value>The graphics device manager.</value>
	public GraphicsDeviceManager GraphicsDeviceManager { get; internal set; }

	/// <summary>
	/// Gets the script system.
	/// </summary>
	/// <value>The script.</value>
	public ScriptSystem Script { get; }

	/// <summary>
	/// Gets the input manager.
	/// </summary>
	/// <value>The input.</value>
	public InputManager Input { get; set; }

	/// <summary>
	/// Gets the scene system.
	/// </summary>
	/// <value>The scene system.</value>
	public SceneSystem SceneSystem { get; }

	/// <summary>
	/// Gets the effect system.
	/// </summary>
	/// <value>The effect system.</value>
	public EffectSystem EffectSystem { get; private set; }

	/// <summary>
	/// Gets the streaming system.
	/// </summary>
	/// <value>The streaming system.</value>
	public StreamingManager Streaming { get; }

	/// <summary>
	/// Gets the audio system.
	/// </summary>
	/// <value>The audio.</value>
	public AudioSystem Audio { get; }

	/// <summary>
	/// Gets the sprite animation system.
	/// </summary>
	/// <value>The sprite animation system.</value>
	public SpriteAnimationSystem SpriteAnimation { get; }

	/// <summary>
	/// Gets the game profiler system.
	/// </summary>
	public DebugTextSystem DebugTextSystem { get; }

	/// <summary>
	/// Gets the game profiler system.
	/// </summary>
	public GameProfilingSystem ProfilingSystem { get; }

	/// <summary>
	/// Initializes a new instance of the <see cref="MinimalGame"/> class.
	/// </summary>
	public MinimalGame()
	{
		// Create all core services, except Input which is created during `Initialize'.
		// Registration takes place in `Initialize'.
		Script = new ScriptSystem(Services);
		Services.AddService(Script);

		SceneSystem = new SceneSystem(Services);
		Services.AddService(SceneSystem);

		Streaming = new StreamingManager(Services);

		Audio = new AudioSystem(Services);
		Services.AddService(Audio);
		Services.AddService<IAudioEngineProvider>(Audio);

		gameFontSystem = new GameFontSystem(Services);
		Services.AddService(gameFontSystem.FontSystem);
		Services.AddService<IFontFactory>(gameFontSystem.FontSystem);

		SpriteAnimation = new SpriteAnimationSystem(Services);
		Services.AddService(SpriteAnimation);

		DebugTextSystem = new DebugTextSystem(Services);
		Services.AddService(DebugTextSystem);

		ProfilingSystem = new GameProfilingSystem(Services);
		Services.AddService(ProfilingSystem);

		// Creates the graphics device manager
		GraphicsDeviceManager = new GraphicsDeviceManager(this);
		Services.AddService<IGraphicsDeviceManager>(GraphicsDeviceManager);
		Services.AddService<IGraphicsDeviceService>(GraphicsDeviceManager);
	}

	/// <inheritdoc/>
	protected override void Destroy()
	{
		DestroyAssetDatabase();

		base.Destroy();
	}

	/// <summary>
	/// Massive assumption that all requirements are met to prepare context
	/// </summary>
	protected override void PrepareContext()
	{
		base.PrepareContext();
		var deviceManager = (GraphicsDeviceManager)graphicsDeviceManager;

		// Create and mount database file system
		var objDatabase = ObjectDatabase.CreateDefaultDatabase();

		// Only set a mount path if not mounted already
		var mountPath = VirtualFileSystem.ResolveProviderUnsafe("/asset", true).Provider == null ? "/asset" : null;
		var result = new DatabaseFileProvider(objDatabase, mountPath);

		databaseFileProvider = result;
		((DatabaseFileProviderService)Services.GetService<IDatabaseFileProviderService>()).FileProvider = databaseFileProvider;

		Settings = Content.Load<GameSettings>("GameSettings");

		deviceManager.ShaderProfile = GraphicsProfile.Level_11_0;
		deviceManager.PreferredGraphicsProfile = [GraphicsProfile.Level_11_0];

		deviceManager.PreferredBackBufferWidth = 800;
		deviceManager.PreferredBackBufferHeight = 600;

		SceneSystem.InitialSceneUrl = "MainScene";
		SceneSystem.InitialGraphicsCompositorUrl = "GraphicsCompositor";

		Services.AddService<IGameSettingsService>(this);
	}

	public override void ConfirmRenderingSettings(bool gameCreation)
	{
		var deviceManager = (GraphicsDeviceManager)graphicsDeviceManager;
		//if our device width or height is actually smaller then requested we use the device one
		deviceManager.PreferredBackBufferWidth = Context.RequestedWidth = Math.Min(deviceManager.PreferredBackBufferWidth, Window.ClientBounds.Width);
		deviceManager.PreferredBackBufferHeight = Context.RequestedHeight = Math.Min(deviceManager.PreferredBackBufferHeight, Window.ClientBounds.Height);
	}

	protected override void Initialize()
	{
		// Add the input manager
		// Add it first so that it can obtained by the UI system
		var inputSystem = new InputSystem(Services);
		Input = inputSystem.Manager;
		Services.AddService(Input);
		GameSystems.Add(inputSystem);

		// Initialize the systems
		base.Initialize();

		Content.Serializer.LowLevelSerializerSelector = ParameterContainerExtensions.DefaultSceneSerializerSelector;

		// Add the scheduler system
		// - Must be after Input, so that scripts are able to get latest input
		// - Must be before Entities/Camera/Audio/UI, so that scripts can apply
		// changes in the same frame they will be applied
		GameSystems.Add(Script);

		// Add the Font system
		GameSystems.Add(gameFontSystem);

		//Add the sprite animation System
		GameSystems.Add(SpriteAnimation);

		GameSystems.Add(DebugTextSystem);
		GameSystems.Add(ProfilingSystem);

		EffectSystem = new EffectSystem(Services);
		Services.AddService(EffectSystem);

		// If requested in game settings, compile effects remotely and/or notify new shader requests
		EffectSystem.Compiler = EffectCompilerFactory.CreateEffectCompiler(Content.FileProvider, EffectSystem, Settings?.PackageName, Settings?.EffectCompilation ?? EffectCompilationMode.Local, Settings?.RecordUsedEffects ?? false);

		// Setup shader compiler settings from a compilation mode. 
		EffectSystem.SetCompilationMode(Settings.CompilationMode);

		GameSystems.Add(EffectSystem);

		Streaming.SetStreamingSettings(Settings.Configurations.Get<StreamingSettings>());
		GameSystems.Add(Streaming);
		GameSystems.Add(SceneSystem);

		// Add the Audio System
		GameSystems.Add(Audio);
	}

	void DestroyAssetDatabase()
	{
		if (Services.GetService<IDatabaseFileProviderService>() is DatabaseFileProviderService dbfp)
			dbfp.FileProvider = null;
		databaseFileProvider.Dispose();
		databaseFileProvider = null;
	}
}
