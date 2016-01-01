namespace CountryTrash
{
	internal class ChangeTileTask : PlayerTask
	{
		private bool isFinished = false;
		private int timer = 10;
		private readonly Tile target;

		public ChangeTileTask(Tile target)
		{
			this.target = target;
			this.Icon = "/Textures/Icons/shovel";
			this.Title = "Dig";
		}

		public override bool IsFinished => this.isFinished;

		public override void Start()
		{
			this.Player.Entity.WalkTo(this.target.X, this.target.Z);
		}

		public override void Tick()
		{
			if(this.Player.Entity.HasReachedTarget == false)
			{
				// Just wait for reaching the target
				return;
			}
			this.timer -= 1;
			if(this.timer <= 0)
			{
				this.Player.Map[this.target.X, this.target.Z] = this.target;
				this.isFinished = true;
			}
		}
	}
}