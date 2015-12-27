using OpenTK;
using System;
using System.Collections.Generic;

namespace CountryTrash
{
	public sealed class PlayerEntity : Entity, IUpdateable
	{
		private readonly Player player;
		private readonly Queue<ITile> walkQueue = new Queue<ITile>();
		private Vector3 targetPosition;

		public PlayerEntity(Player player)
		{
			this.player = player;
			this.Visualization = "/Models/quad";
		}

		public void Teleport(float x, float z)
		{
			this.Position = new Vector3(x, GetHeight(x, z) + 0.5f, z);
			this.targetPosition = this.Position;
		}

		public void WalkTo(int x, int z)
		{
			int cx = (int)Math.Floor(this.Position.X);
			int cz = (int)Math.Floor(this.Position.Z);

			var path = PathFinder.FindPath(this.Map, cx, cz, x, z);

			this.walkQueue.Clear();
			if (path != null)
			{
				foreach (var node in path)
				{
					this.walkQueue.Enqueue(node);
				}
			}
		}

		public void Update(float dt)
		{
			var dist = (this.targetPosition - this.Position);
			if ((dist.Length == 0) && (this.walkQueue.Count > 0))
			{
				var target = this.walkQueue.Dequeue();
				while (target == null) target = this.walkQueue.Dequeue(); // HACK: For non-working path finding
				this.targetPosition = new Vector3(target.X, GetHeight(target.X, target.Z) + 0.5f, target.Z);

				dist = (this.targetPosition - this.Position);
			}
			if (dist.Length > 0)
			{
				if (dist.Length > dt)
				{
					dist = dt * dist.Normalized();
				}
				this.Position += dist;
			}
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