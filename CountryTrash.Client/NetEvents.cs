using Lidgren.Network;
using OpenTK;
using System;
using System.Collections.Generic;

namespace CountryTrash
{
	partial class Network
	{
		public sealed class NetEvents
		{
			private readonly Network network;
			private readonly Dictionary<ServerToClientNetworkCommand, Action<NetIncomingMessage>> handlers;

			public event EventHandler<AuthenticationResponseEventArgs> AuthenticationResponse;
			public event EventHandler<CreateMapEventArgs> CreateMap;
			public event EventHandler<SetTileEventArgs> SetTile;
			public event EventHandler<NetworkEntityEventArgs> CreateEntity;
			public event EventHandler<NetworkEntityEventArgs> UpdateEntity;
			public event EventHandler<NetworkEntityEventArgs> DestroyEntity;
			public event EventHandler<UpdateInventoryEventArgs> UpdateInventory;

			public NetEvents(Network network)
			{
				this.network = network;
				this.handlers = new Dictionary<ServerToClientNetworkCommand, Action<NetIncomingMessage>>()
				{
					{ ServerToClientNetworkCommand.AuthenticationResponse, OnAuthenticationResponse },
					{ ServerToClientNetworkCommand.CreateMap, OnCreateMap },
					{ ServerToClientNetworkCommand.CreateEntity, OnCreateEntity },
					{ ServerToClientNetworkCommand.SetTile, OnSetTile },
					{ServerToClientNetworkCommand.UpdateEntity, OnUpdateEntity },
					{ServerToClientNetworkCommand.DestroyEntity, OnDestroyEntity }
				};
			}

			public void Dispatch(NetIncomingMessage msg)
			{
				var cmd = (ServerToClientNetworkCommand)msg.ReadByte();
				if (this.handlers.ContainsKey(cmd))
					this.handlers[cmd](msg);
				else
					cmd.Log("Unhandled command {0}", "Network");
			}

			private void OnCreateEntity(NetIncomingMessage msg)
			{
				var id = msg.ReadInt32();
				var pos = msg.ReadVector3();
				var rotation = msg.ReadSingle();
				var visualization = msg.ReadString();

				if (this.CreateEntity != null)
					this.CreateEntity(this.network, new NetworkEntityEventArgs(id, pos, rotation, visualization));
			}

			private void OnUpdateEntity(NetIncomingMessage obj)
			{
				var id = obj.ReadInt32();
				var pos = obj.ReadVector3();
				var rot = obj.ReadSingle();
				var vis = obj.ReadString();
				if (this.UpdateEntity != null)
					this.UpdateEntity(this.network, new NetworkEntityEventArgs(id, pos, rot, vis));
			}

			private void OnDestroyEntity(NetIncomingMessage obj)
			{
				var id = obj.ReadInt32();
				if (this.DestroyEntity != null)
					this.DestroyEntity(this.network, new NetworkEntityEventArgs(id, Vector3.Zero, 0.0f, null));
			}

			private void OnSetTile(NetIncomingMessage msg)
			{
				var x = msg.ReadInt32();
				var z = msg.ReadInt32();
				var height = msg.ReadSingle();
				var model = msg.ReadString();
				var topping = msg.ReadString();
				var interactive = msg.ReadBoolean();
				if (this.SetTile != null)
					this.SetTile(this.network, new SetTileEventArgs(x, z, height, model, string.IsNullOrWhiteSpace(topping) ? null :  topping, interactive));
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