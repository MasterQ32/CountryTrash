using System;

namespace HarvestBoon.Graphics
{
	/// <summary>
	/// The configuration that describes how a game should look and behave.
	/// </summary>
	public sealed class GameConfiguration
	{
		/// <summary>
		/// Gets or sets the horizontal game resolution.
		/// </summary>
		public int Width { get; set; } = 1280;

		/// <summary>
		/// Gets or sets the vertical game resolution
		/// </summary>
		public int Height { get; set; } = 720;

		/// <summary>
		/// Gets or sets weather the game should start in fullscreen or not.
		/// </summary>
		public bool IsFullscreen { get; set; } = false;

		/// <summary>
		/// Gets or sets the game title.
		/// </summary>
		public string Title { get; set; } = "Untitled Game";

		/// <summary>
		/// Gets or sets the initial game state.
		/// </summary>
		public GameState InitialState { get; set; } = null;

		/// <summary>
		/// Gets or sets the games update rate.
		/// </summary>
		public double UpdateRate { get; set; } = 120.0;

		/// <summary>
		/// Gets or sets the games render rate.
		/// </summary>
		public double RenderRate { get; set; } = 60.0;

		/// <summary>
		/// Gets or sets the root folder for the default resource manager.
		/// </summary>
		/// <remarks>The default value is the current working directory.</remarks>
		public string AssetRoot { get; set; } = Environment.CurrentDirectory;
		
		/// <summary>
		/// An event that is fired before the game state is updated.
		/// </summary>
		public event EventHandler<GameUpdateEventArgs> PreUpdateLoop;


		/// <summary>
		/// An event that is fired after the game state is updated.
		/// </summary>
		public event EventHandler<GameUpdateEventArgs> PostUpdateLoop;

		internal void OnPreUpdate(float totalTime, float deltaTime)
		{
			if (this.PreUpdateLoop != null)
				this.PreUpdateLoop(this, new GameUpdateEventArgs(totalTime, deltaTime));
		}

		internal void OnPostUpdate(float totalTime, float deltaTime)
		{
			if (this.PostUpdateLoop != null)
				this.PostUpdateLoop(this, new GameUpdateEventArgs(totalTime, deltaTime));
		}
	}
	
}