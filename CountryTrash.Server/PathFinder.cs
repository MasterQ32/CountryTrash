using System;
using System.Collections.Generic;

namespace CountryTrash
{
	/// <summary>
	/// Provides a utility function to search tile paths.
	/// </summary>
	public static class PathFinder
	{
		/// <summary>
		/// Searches a path from (sx,sz) to (ex, ez) on the given map.
		/// </summary>
		/// <param name="map"></param>
		/// <param name="sx"></param>
		/// <param name="sz"></param>
		/// <param name="ex"></param>
		/// <param name="ez"></param>
		/// <returns></returns>
		public static ITile[] FindPath(Map map, int sx, int sz, int ex, int ez)
		{
			if ((sx == ex) && (sz == ez))
				return new ITile[0];

			if (map[sx, sz] == null)
				throw new InvalidOperationException("Cannot find path with empty start point.");

			if (map[ex, ez] == null)
				throw new InvalidOperationException("Cannot find path with empty end point.");

			var closedList = new HashSet<Node>();
			var openList = new Queue<Node>();
			openList.Enqueue(new Node(map[sx, sz], null));

			while (openList.Count > 0)
			{
				var current = openList.Dequeue();
				if ((current.X == ex) && (current.Z == ez))
				{
					// return path
					var list = new List<ITile>();
					var it = current;
					while (it != null)
					{
						list.Add(it.tile);
						it = it.previous;
					}
					list.Reverse();
					return list.ToArray();
				}
				var innerNodes = new[]
				{
					map.Get(current.X + 0, current.Z - 1),
					map.Get(current.X + 0, current.Z + 1),
					map.Get(current.X - 1, current.Z + 0),
					map.Get(current.X + 1, current.Z + 0),
				};
				foreach (var neighbour in innerNodes)
				{
					if (neighbour == null)
						continue;

					// The "new" only works because it has GetHashCode and Equals overwritten.
					var node = new Node(neighbour, current);
					if (closedList.Contains(node))
						continue;
					if (neighbour.IsBlocked)
						continue;

					closedList.Add(node);
					openList.Enqueue(node);
				}
			}
			return null;
		}

		private class Node : IEquatable<Node>
		{
			public readonly ITile tile;
			public readonly Node previous;
			public readonly int distance;

			public Node(ITile tile, Node previous)
			{
				if (tile == null)
					throw new ArgumentNullException("tile");
				this.tile = tile;
				this.previous = previous;
				if (this.previous != null)
				{
					this.distance = this.previous.distance + 1;
				}
				else
				{
					this.distance = 0;
				}
			}

			public int X => this.tile.X;
			public int Z => this.tile.Z;

			public bool Equals(Node other)
			{
				return this.tile == other.tile;
			}

			public override bool Equals(object obj)
			{
				if (obj is Node)
					return this.Equals((Node)obj);
				else
					return false;
			}

			public override int GetHashCode()
			{
				return this.tile.GetHashCode();
			}
		}
	}
}