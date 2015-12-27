using Lidgren.Network;
using System;

namespace CountryTrash
{
	partial class Client
	{ 
		public class ClientCommands
		{
			private readonly Client client;

			public ClientCommands(Client client)
			{
				this.client = client;
			}

			public void AuthenticationResponse(bool success, string reason)
			{
				var msg = CreateMessage(ServerToClientNetworkCommand.AuthenticationResponse);
				msg.Write(success);
				msg.Write(reason);
				SendMessage(msg, true);
			}

			public void UpdateInventory()
			{
				throw new NotImplementedException();
			}



			private NetOutgoingMessage CreateMessage(ServerToClientNetworkCommand cmd)
			{
				var msg = this.client.server.CreateMessage();
				msg.Write((byte)cmd);
				return msg;
			}

			private void SendMessage(NetOutgoingMessage msg, bool reliable = true)
			{
				this.client.server.SendMessage(msg, this.client.connection, reliable ? NetDeliveryMethod.ReliableOrdered : NetDeliveryMethod.Unreliable);
			}

			public void SetTile(ITile tile)
			{
				var msg = CreateMessage(ServerToClientNetworkCommand.SetTile);
				msg.Write(tile.X);
				msg.Write(tile.Z);
				msg.Write(tile.Height);
				msg.Write(tile.Model);
				msg.Write(tile.Topping);
				msg.Write(tile.IsInteractive);
				SendMessage(msg);
			}

			public void CreateEntity(IEntity entity)
			{
				var msg = CreateMessage(ServerToClientNetworkCommand.CreateEntity);

				msg.Write(entity.ID);
				msg.Write(entity.Position);
				msg.Write(entity.Rotation);
				msg.Write(entity.Visualization);

				SendMessage(msg);
			}

			public void UpdateEntity(IEntity entity, bool reliable)
			{
				var msg = CreateMessage(ServerToClientNetworkCommand.UpdateEntity);
				msg.Write(entity.ID);
				msg.Write(entity.Position);
				msg.Write(entity.Rotation);
				msg.Write(entity.Visualization);
				SendMessage(msg, reliable);
			}

			public void DestroyEntity(IEntity entity)
			{
				var msg = CreateMessage(ServerToClientNetworkCommand.DestroyEntity);
				msg.Write(entity.ID);
				SendMessage(msg);
			}

			public void CreateMap(IMap map)
			{
				var msg = CreateMessage(ServerToClientNetworkCommand.CreateMap);
				msg.Write(map.ID);
				msg.Write(map.SizeX);
				msg.Write(map.SizeZ);
				SendMessage(msg);
			}
		}
	}
}