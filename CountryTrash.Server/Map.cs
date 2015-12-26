using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace CountryTrash
{
	public sealed class Map : IMap, IEnumerable<ITile>
	{
		private readonly int id;
		private readonly ITile[,] tiles;
		private readonly HashSet<IEntity> entities;

		public Map(int id, int sizeX, int sizeZ)
		{
			this.id = id;
			this.tiles = new ITile[sizeX, sizeZ];
			this.entities = new HashSet<IEntity>();
		}

		public ITile this[int x, int z]
		{
			get { return this.tiles[x, z]; }
			set { this.tiles[x, z] = value; }
		}

		public IReadOnlyCollection<IEntity> Entities => this.entities;

		public int ID => this.id;

		public int SizeX => this.tiles.GetLength(0);

		public int SizeZ => this.tiles.GetLength(1);

		public void AddEntity(IEntity entity)
		{
			this.entities.Add(entity);
		}

		public void RemoveEntity(IEntity entity)
		{
			this.entities.Remove(entity);
		}

		public IEnumerator<ITile> GetEnumerator() => this.tiles.OfType<ITile>().GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
	}
}