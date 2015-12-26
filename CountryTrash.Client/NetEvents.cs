using Lidgren.Network;
using System;

namespace CountryTrash
{
	partial class Network
	{
		public sealed class NetEvents
		{
			private readonly Network network;

			public event EventHandler<AuthenticationResponseEventArgs> AuthenticationResponse;
			public event EventHandler<CreateMapEventArgs> CreateMap;
			public event EventHandler<SetTileEventArgs> SetTile;
			public event EventHandler<CreateEntityEventArgs> CreateEntity;
			public event EventHandler<UpdateEntityEventArgs> UpdateEntity;
			public event EventHandler<DestroyEntityEventArgs> DestroyEntity;
			public event EventHandler<UpdateInventoryEventArgs> UpdateInventory;

			public NetEvents(Network network)
			{
				this.network = network;
			}

			public void Dispatch(NetIncomingMessage msg)
			{
				var cmd = (ServerToClientNetworkCommand)msg.ReadByte();
				switch (cmd)
				{
					case ServerToClientNetworkCommand.AuthenticationResponse:
					{
						this.OnAuthenticationResponse(msg);
						break;
					}
					case ServerToClientNetworkCommand.CreateMap:
					{
						this.OnCreateMap(msg);
						break;
					}
					case ServerToClientNetworkCommand.SetTile:
					{
						this.OnSetTile(msg);
						break;
					}
					default:
					{
						cmd.Log("Unhandled command {0}", "Network");
						break;
					}
				}
			}

			private void OnSetTile(NetIncomingMessage msg)
			{
				var x = msg.ReadInt32();
				var z = msg.ReadInt32();
				var height = msg.ReadSingle();
				var model = msg.ReadString();
				var interactive = msg.ReadBoolean();
				if (this.SetTile != null)
					this.SetTile(this.network, new SetTileEventArgs(x, z, height, model, interactive));
			}

			private void OnCreateMap(NetIncomingMessage msg)
			{
				var id = msg.ReadInt32();
				var x = msg.ReadInt32();
				var z = msg.ReadInt32();
				if (this.CreateMap != null)
					this.CreateMap(this.network, new CreateMapEventArgs(id, x, z));
			}

			private void OnAuthenticationResponse(NetIncomingMessage msg)
			{
				bool success = msg.ReadBoolean();
				string reason = msg.ReadString();
				if (this.AuthenticationResponse != null)
					this.AuthenticationResponse(this.network, new AuthenticationResponseEventArgs(success, reason));
			}
		}
	}
}