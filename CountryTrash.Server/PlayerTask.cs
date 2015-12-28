namespace CountryTrash
{
	/// <summary>
	/// A task that can be done by a player.
	/// </summary>
	public abstract class PlayerTask
	{
		internal Player player;

		protected PlayerTask() { }

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
	}
}