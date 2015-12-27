using System;

namespace CountryTrash
{
	internal class Tile : ITile
	{
		private readonly int x;
		private readonly int z;

		public Tile(int x, int z)
		{
			this.x = x;
			this.z = z;
		}

		public float Height { get; set; }

		public bool IsBlocked { get; set; }

		public bool IsInteractive { get; set; }

		public string Model { get; set; }

		public int X => this.x;

		public int Z => this.z;

		internal void Interact(Player player)
		{
			throw new NotImplementedException();
		}
	}
}