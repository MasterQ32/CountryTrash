using System;

namespace CountryTrash
{
	public sealed class Player
	{
		private readonly Client client;
		private readonly string name;
		private readonly PlayerEntity entity;
		private Map map;

		public Player(Client client, string name)
		{
			this.name = name;
			this.client = client;

			this.client.Events.InvokeTileAction += Events_InvokeTileAction;
			this.client.Events.InvokeEntityAtion += Events_InvokeEntityAtion;
			this.client.Events.MoveInventoryItem += Events_MoveInventoryItem;

			this.entity = new PlayerEntity(this);
		}

		private void Events_InvokeTileAction(object sender, InvokeTileActionEventArgs e)
		{
			if (this.Map == null)
				return;
			var tile = this.Map[e.X, e.Z] as Tile;
			if (tile == null)
				return;
			
			if(tile.IsInteractive)
			{
				tile.Interact(this);
			}
			else
			{
				this.entity.WalkTo(e.X, e.Z);
			}
		}

		private void Events_InvokeEntityAtion(object sender, InvokeEntityAtionEventArgs e)
		{
			throw new System.NotImplementedException();
		}

		private void Events_MoveInventoryItem(object sender, MoveInventoryItemEventArgs e)
		{
			throw new System.NotImplementedException();
		}

		private void OnMapChanged(Map map)
		{
			if(map != null)
			{
				// If we have a map change, teleport us to the center of the map.
				// TODO: Change later to stored position or transferred position
				this.entity.Teleport(0.5f * this.map.SizeX, 0.5f * this.map.SizeZ);
			}
		}

		public string Name => this.name;

		public Client Client => this.client;

		public Map Map
		{
			get { return this.map; }
			set
			{
				if (this.map == value)
					return;
				this.map = value;
				this.OnMapChanged(this.map);
			}
		}

		public PlayerEntity Entity => this.entity;
	}
}