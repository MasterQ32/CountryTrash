﻿using Lidgren.Network;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace CountryTrash
{
	class TrashServer
	{
		private static readonly ThreadLocal<Random> rng = new ThreadLocal<System.Random>(() => new Random());

		public static Random Random => rng.Value;

		static void Main(string[] args)
		{
			var server = new TrashServer();
			server.LoadData();
			server.Start();
		}

		private readonly Queue<Player> spawningPlayers = new Queue<Player>();

		private readonly Dictionary<int, Map> maps = new Dictionary<int, Map>()
		{
			{ 1, new Map(1, 8, 6) }
		};

		private readonly HashSet<Player> players = new HashSet<Player>();

		private void LoadData()
		{
			foreach (var map in maps.Values)
			{
				for (int z = 0; z < map.SizeZ; z++)
				{
					for (int x = 0; x < map.SizeX; x++)
					{
						if (x >= 6)
						{
							map[x, z] = new DirtTile(map, x, z);
						}
						else
						{
							map[x, z] = new Tile(map, x, z)
							{
								Model = "/Models/tile-path"
							};
						}
					}
				}
				map[1, 1] = null;
				map[1, 2] = null;
				map[2, 1] = null;
				map[4, 1] = null;
				map[4, 4] = null;
				map[1, 4] = null;
				map[3, 4] = null;
				map[4, 3] = null;

				map[3, 1] = new Tile(map, 3, 1)
				{
					Model = "/Models/tile-path",
					Topping = "/Models/boulder",
					IsBlocked = true
				};

				map.AddEntity(new Entity()
				{
					Position = new Vector3(6.5f, 0.2f, 6.5f),
					Rotation = MathHelper.DegreesToRadians(45.0f),
					Visualization = "/Models/quad"
				});
			}
		}

		private void Start()
		{
			var network = new Network();

			// Client initialization
			network.ClientConnected += (s, e) =>
			{
				e.Client.Events.Authenticate += AuthenticateClient;
			};
			network.ClientDisconnected += (s, e) =>
			{
				e.Client.Events.Authenticate -= AuthenticateClient;
				this.players.Remove(e.Client.Player);
			};

			var watch = Stopwatch.StartNew();
			var rate = 1.0f / 30.0f; // Server runs with 30 FPS
			var tickRate = 1.0f / 10.0f; // Server ticks with 10 FPS
			var tickTimer = 0.0f;
			while (true)
			{
				watch.Reset();
				network.ReceiveMessages();

				this.SpawnClients();

				while(tickTimer >= tickRate)
				{
					// First tick the map so the tiles will be ready for the player tasks.
					foreach (var map in this.maps.Values)
						map.Tick();

					// Then tick the players so they can finish their tasks.
					foreach (var player in this.players)
						player.Tick();
					tickTimer -= tickRate;
				}
				
				foreach(var map in this.maps.Values)
				{
					map.Update(rate);
				}

				network.SendMessages();

				int time = (int)Math.Max(0, 1000 * rate - watch.ElapsedMilliseconds);
				Thread.Sleep(time);

				tickTimer += rate;
			}
		}

		private void SpawnClients()
		{
			while (this.spawningPlayers.Count > 0)
			{
				var player = this.spawningPlayers.Dequeue();

				// TODO: Restore from database
				int lastMap = 1;

				var map = this.maps[lastMap];

				map.AddPlayer(player);

				this.players.Add(player);
			}
		}

		private void AuthenticateClient(object sender, AuthenticateEventArgs e)
		{
			var passwordHash = new byte[] { 177, 9, 243, 187, 188, 36, 78, 184, 36, 65, 145, 126, 208, 109, 97, 139, 144, 8, 221, 9, 179, 190, 253, 27, 94, 7, 57, 76, 112, 106, 139, 185, 128, 177, 215, 120, 94, 89, 118, 236, 4, 155, 70, 223, 95, 19, 38, 175, 90, 46, 166, 209, 3, 253, 7, 201, 83, 133, 255, 171, 12, 172, 188, 134 };
			var client = (Client)sender;

			if (e.Name != "user")
			{
				client.Commands.AuthenticationResponse(false, "Unknown user");
			}
			else if (e.Hash.SequenceEqual(passwordHash) == false)
			{
				client.Commands.AuthenticationResponse(false, "Invalid password");
			}
			else
			{
				client.Commands.AuthenticationResponse(true, "");
				client.Events.Authenticate -= AuthenticateClient;

				var player = new Player(client, e.Name);

				// Add authenticated clients to the list of all clients.
				this.spawningPlayers.Enqueue(player);
			}
		}
	}
}
