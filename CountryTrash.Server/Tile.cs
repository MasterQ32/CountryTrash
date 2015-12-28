using System;

namespace CountryTrash
{
	public class Tile : ITile
	{
		private readonly Map map;
		private readonly int x;
		private readonly int z;

		public Tile(Map map, int x, int z)
		{
			if (map == null)
				throw new ArgumentNullException("map");
			this.map = map;

			if ((x < 0) || (x >= this.map.SizeX))
				throw new ArgumentOutOfRangeException("x");

			if ((z < 0) || (z >= this.map.SizeZ))
				throw new ArgumentOutOfRangeException("z");

			this.x = x;
			this.z = z;
		}

		/// <summary>
		/// Copies a tile except for IsBlocked, IsInteractive and Topping.
		/// </summary>
		/// <param name="other"></param>
		public Tile(Tile other) : 
			this(other.Map, other.X, other.Z)
		{
			this.Height = other.Height;
			this.Model = other.Model;
		}

		public float Height { get; set; }

		public bool IsBlocked { get; set; }

		public bool IsInteractive { get; set; }

		public string Model { get; set; }
		public string Topping { get; set; }

		public int X => this.x;

		public int Z => this.z;

		public Map Map => this.map;

		public virtual void Interact(Player player)
		{
			// Default behaviour for tiles: Walk to the tile
			player.AddTask(new WalkTask(this.X, this.Z));
		}
	}
}