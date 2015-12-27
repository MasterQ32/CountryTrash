using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace CountryTrash
{
	public sealed class Map : IMap, IEnumerable<ITile>, IUpdateable
	{
		private readonly int id;
		private readonly ITile[,] tiles;
		private readonly HashSet<IEntity> entities;

		private HashSet<Player> players = new HashSet<Player>();
		
		public Map(int id, int sizeX, int sizeZ)
		{
			this.id = id;
			this.tiles = new ITile[sizeX, sizeZ];
			this.entities = new HashSet<IEntity>();
		}

		public void Update(float dt)
		{
			foreach(var tile in this.OfType<IUpdateable>())
			{
				tile.Update(dt);
			}
			foreach (var entity in this.Entities.OfType<IUpdateable>())
			{
				entity.Update(dt);
			}
		}

		public ITile[] FindPath(int startX, int startZ, int endX, int endZ)
		{
			if ((startX == endX) && (startZ == endZ))
				return new ITile[0];

			// TODO: Implement correct pathfinding :P
			var path = new List<ITile>();

			for(int x = startX; x != endX; x += Math.Sign(endX - startX))
			{
				path.Add(this[x, startZ]);
			}
			path.Add(this[endX, startZ]);

			for (int z = startZ; z != endZ; z += Math.Sign(endZ - startZ))
			{
				path.Add(this[endX, z]);
			}
			path.Add(this[endX, endZ]);

			return path.ToArray();
		}

		public ITile this[int x, int z]
		{
			get { return this.tiles[x, z]; }
			set { this.tiles[x, z] = value; }
		}

		public IReadOnlyCollection<IEntity> Entities => this.entities;

		public IReadOnlyCollection<Player> Players => this.players;

		public int ID => this.id;

		public int SizeX => this.tiles.GetLength(0);

		public int SizeZ => this.tiles.GetLength(1);

		private void InvokeAll(Action<Client> invoker)
		{
			foreach (var player in this.players)
				invoker(player.Client);
		}

		public void AddEntity(IEntity entity)
		{
			if (entity == null)
				throw new ArgumentNullException("entity");
			if (entity is Entity)
			{
				// Move the entities 
				var ent = (Entity)entity;
				if (ent.Map != null)
					ent.Map.RemoveEntity(entity);
				ent.Map = this;
			}
			this.entities.Add(entity);

			this.InvokeAll(cl => cl.Commands.CreateEntity(entity));

			entity.Changed += Entity_Changed;
		}

		private void Entity_Changed(object sender, EntityChangedEventArgs e)
		{
			var entity = (Entity)sender;

			this.InvokeAll(cl => cl.Commands.UpdateEntity(entity, (e.Property == EntityProperty.Visualization)));
		}

		public void RemoveEntity(IEntity entity)
		{
			if (entity == null)
				throw new ArgumentNullException("entity");
			entity.Changed -= Entity_Changed;
			this.entities.Remove(entity);
			// Remove the entity from the map
			if (entity is Entity)
				((Entity)entity).Map = null;
			
			this.InvokeAll(cl => cl.Commands.DestroyEntity(entity));
		}

		public void AddPlayer(Player player)
		{
			player.Map?.RemovePlayer(player);
			this.AddEntity(player.Entity);
			
			player.Client.Commands.CreateMap(this);
			foreach (var tile in this)
			{
				player.Client.Commands.SetTile(tile);
			}
			foreach (var entity in this.Entities)
			{
				player.Client.Commands.CreateEntity(entity);
			}
			
			this.players.Add(player);
			player.Map = this;
		}

		public void RemovePlayer(Player player)
		{
			this.players.Remove(player);
			this.RemoveEntity(player.Entity);
			player.Map = null;
		}

		public IEnumerator<ITile> GetEnumerator() => this.tiles.OfType<ITile>().GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
	}
}