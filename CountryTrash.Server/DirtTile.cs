using System;

namespace CountryTrash
{
	public sealed class DirtTile : Tile, ITicking
	{
		private int lifespan;
		public DirtTile(Map map, int x, int z)
			: base(map, x, z)
		{
			this.Model = "/Models/tile-dirt";
			 this.lifespan = TrashServer.Random.Next(30, 100);
		}

		public void Tick()
		{
			this.lifespan -= 1;
			if (this.lifespan <= 0)
			{
				this.Map[this.X, this.Z] = new GrasTile(this);
			}
		}
	}
}