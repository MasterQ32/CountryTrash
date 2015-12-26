using Lidgren.Network;
using System;
using System.Security.Cryptography;
using System.Text;

namespace CountryTrash
{
	partial class Network
	{
		/// <summary>
		/// Client-to-server commands
		/// </summary>
		public sealed class NetCommands
		{
			private readonly Network network;

			public NetCommands(Network network)
			{
				this.network = network;
			}

			public void Authenticate(string name, string password)
			{
				SHA512 sha = SHA512.Create();
				var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(password));

				var msg = CreateMessage(ClientToServerNetworkCommand.Authenticate);
				msg.Write(name);
				msg.Write(hash, 0, 64);
				SendMessage(msg, true);
			}

			public void InvokeTileAction(ITile tile)
			{
				var msg = CreateMessage(ClientToServerNetworkCommand.InvokeTileAction);
				msg.Write(tile.X);
				msg.Write(tile.Z);
				SendMessage(msg, true);
			}

			public void InvokeEntityAction(IEntity entity)
			{
				var msg = CreateMessage(ClientToServerNetworkCommand.InvokeEntityAction);
				msg.Write(entity.ID);
				SendMessage(msg, true);
			}

			public void MoveInventoryItem(/* TODO: Insert parameters */)
			{
				throw new NotImplementedException();
			}

			private NetOutgoingMessage CreateMessage(ClientToServerNetworkCommand cmd)
			{
				var msg = this.network.client.CreateMessage();
				msg.Write((byte)cmd);
				return msg;
			}

			private void SendMessage(NetOutgoingMessage msg, bool reliable)
			{
				this.network.client.SendMessage(msg, reliable ? NetDeliveryMethod.ReliableOrdered : NetDeliveryMethod.Unreliable);
			}
		}
	}
}