using System;

namespace CountryTrash.Graphics
{
	public class GameUpdateEventArgs : EventArgs
	{
		public GameUpdateEventArgs(float time, float dt)
		{
			this.TotalTime = time;
			this.DeltaTime = dt;
		}

		/// <summary>
		/// The time that has passed since the last event.
		/// </summary>
		public float DeltaTime { get; private set; }

		/// <summary>
		/// The time that has elapsed since the game start.
		/// </summary>
		public float TotalTime { get; private set; }
	}
}