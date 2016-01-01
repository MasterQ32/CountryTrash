using System;
using System.Threading;

namespace CountryTrash
{
	/// <summary>
	/// A task that can be done by a player.
	/// </summary>
	public abstract class PlayerTask : ITask
	{
		private static int idCounter = 0;

		internal Player player;
		private readonly int id;

		protected PlayerTask()
		{
			this.id = Interlocked.Increment(ref idCounter);
			this.Title = this.GetType().Name;
		}

		/// <summary>
		/// Starts the task after it has been assigned to a player.
		/// </summary>
		public virtual void Start() { }

		/// <summary>
		/// Does the periodic task job.
		/// </summary>
		public abstract void Tick();

		/// <summary>
		/// Gets if the task is finished and can be dequeued.
		/// </summary>
		public abstract bool IsFinished { get; }

		public Player Player => this.player;

		public int ID => this.id;

		public string Title { get; set; }

		public string Icon { get; set; }
	}
}