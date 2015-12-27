using OpenTK;
using System;

namespace CountryTrash
{
	public sealed class PlayerEntity : Entity
	{
		private readonly Player player;

		public PlayerEntity(Player player)
		{
			this.player = player;
			this.Visualization = "/Models/quad";
		}

		public void Teleport(float x, float z)
		{
			this.Position = new Vector3(x, GetHeight(x, z) + 0.5f, z);
		}

		private float GetHeight(float fx, float fz)
		{
			int x = (int)Math.Floor(fx);
			int z = (int)Math.Floor(fz);
			if ((x < 0) || (z < 0))
				return 0.0f;
			if ((x >= this.Map.SizeX) || (z >= this.Map.SizeZ))
				return 0.0f;
			var tile = this.Map[x, z];
			if (tile == null)
				return 0.0f;
			return tile.Height;
		}
	}
}