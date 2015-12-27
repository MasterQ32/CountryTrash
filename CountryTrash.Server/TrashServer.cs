using Lidgren.Network;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CountryTrash
{
	class TrashServer
	{
		static void Main(string[] args)
		{
			var server = new TrashServer();
			server.LoadData();
			server.Start();
		}

		private readonly Queue<Player> spawningPlayers = new Queue<Player>();

		private readonly Dictionary<int, Map> maps = new Dictionary<int, Map>()
		{
			{ 1, new Map(1, 4, 4) }
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
						map[x, z] = new Tile(x, z)
						{
							Model = "/Models/tile"
						};
					}
				}
				map.AddEntity(new Entity()
				{
					Position = new Vector3(1.5f, 0.2f, 1.5f),
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
			};

			while (true)
			{
				network.ReceiveMessages();

				this.SpawnClients();

				// TODO: Add game logic here

				network.SendMessages();

				Thread.Sleep(1);
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
