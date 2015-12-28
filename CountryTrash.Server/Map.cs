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

		private List<Tuple<IEntity, EntityProperty>> changedEntities = new List<Tuple<IEntity, EntityProperty>>();

		public void Update(float dt)
		{
			foreach (var tile in this.OfType<IUpdateable>())
			{
				tile.Update(dt);
			}
			foreach (var entity in this.Entities.OfType<IUpdateable>())
			{
				entity.Update(dt);
			}

			// Send all entity update messages
			{
				this.changedEntities.Reverse();
				
				// Get all entities which have their visualization property changed.
				var forcedUpdate = this.changedEntities.Where(e => (e.Item2 == EntityProperty.Visualization)).Select(e => e.Item1).Distinct();
				foreach (var ce in forcedUpdate)
				{
					this.InvokeAll(cl => cl.Commands.UpdateEntity(ce, true));
				}

				// Now get all entities that have something else changed
				var maybeUpdate = this.changedEntities.Select(e => e.Item1).Except(forcedUpdate).Distinct();
				foreach (var ce in maybeUpdate)
				{
					this.InvokeAll(cl => cl.Commands.UpdateEntity(ce, false));
				}
			}
		}

		/// <summary>
		/// Gets a tile without boundary checks. If the coordinate is out of bounds, null will be returned;
		/// </summary>
		/// <param name="x"></param>
		/// <param name="z"></param>
		/// <returns></returns>
		public ITile Get(int x, int z)
		{
			if ((x < 0) || (z < 0))
				return null;
			if ((x >= this.SizeX) || (z >= this.SizeZ))
				return null;
			return this.tiles[x, z];
		}

		/// <summary>
		/// Gets or sets a tile with boundary checks
		/// </summary>
		/// <param name="x"></param>
		/// <param name="z"></param>
		/// <returns></returns>
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
			this.changedEntities.Add(new Tuple<IEntity, EntityProperty>(entity, e.Property));
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

			this.changedEntities.RemoveAll(e => (e.Item1 == entity));

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