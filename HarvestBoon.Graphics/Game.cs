using System;
using System.Threading;

namespace HarvestBoon.Graphics
{
	public sealed class Game
	{
		static readonly ThreadLocal<Game> current = new ThreadLocal<Game>(() => null);

		public static Game Current { get { return current.Value; } }

		private Game()
		{

		}

		private Window window;
		private ResourceManager resources;
		
		public GameState CurrentState  => this.window.CurrentState;

		public ResourceManager Resources => this.resources;

		public void SetNextState(GameState state) => this.window.SetNextState(state);

		public static void Start(GameConfiguration config)
		{
			if (config == null)
				throw new ArgumentNullException("config");
			if (Game.Current != null)
				throw new InvalidOperationException("Cannot start a game from inside another game using the same thread.");
			try
			{
				var game = new Game();
				current.Value = game;
				try
				{
					game.resources = new ResourceManager(config.AssetRoot);
					game.window = new Window(config);
					game.window.Run(
						config.UpdateRate,
						config.RenderRate);
				}
				finally
				{
					if (game.window != null)
						game.window.Dispose();
					game.window = null;
				}
			}
			finally
			{
				current.Value = null;
			}
		}
	}
}