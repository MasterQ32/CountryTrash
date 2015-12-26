using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CountryTrash
{
	/// <summary>
	/// Simple representation of a map that can be visualized.
	/// </summary>
	internal class Map : IMap, IEnumerable<ITile>
	{
		private readonly int id;
		private readonly ITile[,] tiles;
		private readonly List<IEntity> entities = new List<IEntity>();

		public Map(int id, int sizeX, int sizeZ)
		{
			this.id = id;
			this.tiles = new ITile[sizeX, sizeZ];
		}

		public ITile this[int x, int z]
		{
			get { return this.tiles[x,z ]; }
			set { this.tiles[x, z] = value; }
		}

		public IReadOnlyCollection<IEntity> Entities
		{
			get { return this.entities; }
		}

		public int ID
		{
			get { return this.id; }
		}

		public int SizeX
		{
			get { return this.tiles.GetLength(0); }
		}

		public int SizeZ
		{
			get { return this.tiles.GetLength(1); }
		}

		public void AddEntity(IEntity entity)
		{
			this.entities.Add(entity);
		}

		public void RemoveEntity(IEntity entity)
		{
			this.entities.Remove(entity);
		}

		public IEnumerator<ITile> GetEnumerator()
		{
			return this.tiles.OfType<ITile>().GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}
}