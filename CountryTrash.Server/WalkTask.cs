using System;

namespace CountryTrash
{
	internal class WalkTask : PlayerTask
	{
		private readonly int x;
		private readonly int z;

		public WalkTask(int x, int z)
		{
			this.x = x;
			this.z = z;
			this.Title = $"Walk ({x},{z})";
			this.Icon = "/Textures/Icons/foot";
		}

		public override bool IsFinished
		{
			get
			{
				return this.Player.Entity.HasReachedTarget;
			}
		}

		public override void Start()
		{
			this.Player.Entity.WalkTo(this.x, this.z);
		}

		public override void Tick()
		{

		}
	}
}