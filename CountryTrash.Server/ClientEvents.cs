using Lidgren.Network;
using System;
using System.Collections.Generic;

namespace CountryTrash
{
	partial class Client
	{
		public class ClientEvents
		{
			private readonly Client client;
			private readonly Dictionary<ClientToServerNetworkCommand, Action<NetIncomingMessage>> handlers;

			public event EventHandler<AuthenticateEventArgs> Authenticate;
			public event EventHandler<InvokeTileActionEventArgs> InvokeTileAction;
			public event EventHandler<InvokeEntityAtionEventArgs> InvokeEntityAtion;
			public event EventHandler<MoveInventoryItemEventArgs> MoveInventoryItem;
			
			public ClientEvents(Client client)
			{
				this.client = client;
				this.handlers = new Dictionary<ClientToServerNetworkCommand, Action<NetIncomingMessage>>()
				{
					{ ClientToServerNetworkCommand.Authenticate, OnAuthenticate },
					{ ClientToServerNetworkCommand.InvokeTileAction, OnInvokeTileAction }
				};
			}

			public void Dispatch(NetIncomingMessage msg)
			{
				var cmd = (ClientToServerNetworkCommand)msg.ReadByte();
				if (this.handlers.ContainsKey(cmd))
					this.handlers[cmd](msg);
				else
					cmd.Log("Unhandled command {0}", "Network");
			}

			private void OnAuthenticate(NetIncomingMessage msg)
			{
				var name = msg.ReadString();
				var pwhash = msg.ReadBytes(64);
				if (this.Authenticate != null)
					this.Authenticate(this.client, new AuthenticateEventArgs(name, pwhash));
			}

			private void OnInvokeTileAction(NetIncomingMessage msg)
			{
				var x = msg.ReadInt32();
				var z = msg.ReadInt32();
				if (this.InvokeTileAction != null)
					this.InvokeTileAction(this.client, new InvokeTileActionEventArgs(x, z));
			}
		}
	}
}