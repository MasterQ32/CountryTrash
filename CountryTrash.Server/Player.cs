using System;
using System.Collections.Generic;

namespace CountryTrash
{
	public sealed class Player : ITicking
	{
		private readonly Client client;
		private readonly string name;
		private readonly PlayerEntity entity;
		private Map map;
		private Queue<PlayerTask> tasks = new Queue<PlayerTask>();

		public Player(Client client, string name)
		{
			this.name = name;
			this.client = client;
			this.client.Player = this;

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
			
			// Default behaviour for tiles is walking to them...
			tile.Interact(this);
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
			if (map != null)
			{
				// If we have a map change, teleport us to the center of the map.
				// TODO: Change later to stored position or transferred position
				this.entity.Teleport(3, 3);
			}
		}

		/// <summary>
		/// Ticks the players current task.
		/// </summary>
		public void Tick()
		{
			if (this.tasks.Count == 0)
			{
				this.CurrentTask = null;
				return;
			}
			var task = this.tasks.Peek();
			if(task != this.CurrentTask)
			{
				task.Start();
				this.CurrentTask = task;
			}
			task.Tick();
			if (task.IsFinished)
			{ 
				this.client.Commands.DequeueTask(task);
				this.tasks.Dequeue();
			}
		}

		/// <summary>
		/// Adds a task to the players task queue.
		/// </summary>
		/// <param name="task"></param>
		public void AddTask(PlayerTask task)
		{
			if (task == null)
				throw new ArgumentNullException("task");
			task.player = this;
			this.tasks.Enqueue(task);
			this.client.Commands.EnqueueTask(task);
		}

		public string Name => this.name;

		public Client Client => this.client;

		public PlayerTask CurrentTask { get; private set; }

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