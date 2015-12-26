using Lidgren.Network;
using System;

namespace CountryTrash
{
	partial class Client
	{
		public class ClientEvents
		{
			private readonly Client client;

			public event EventHandler<AuthenticateEventArgs> Authenticate;
			public event EventHandler<InvokeTileActionEventArgs> InvokeTileAction;
			public event EventHandler<InvokeEntityAtionEventArgs> InvokeEntityAtion;
			public event EventHandler<MoveInventoryItemEventArgs> MoveInventoryItem;
			
			public ClientEvents(Client client)
			{
				this.client = client;
			}
			
			public void Dispatch(NetIncomingMessage msg)
			{
				var cmd = (ServerToClientNetworkCommand)msg.ReadByte();
				switch (cmd)
				{
					case ServerToClientNetworkCommand.AuthenticationResponse:
					{
						this.OnAuthenticate(msg);
						break;
					}
					default:
					{
						cmd.Log("Unhandled command {0}", "Network");
						break;
					}
				}
			}

			private void OnAuthenticate(NetIncomingMessage msg)
			{
				var name = msg.ReadString();
				var pwhash = msg.ReadBytes(64);
				if (this.Authenticate != null)
					this.Authenticate(this.client, new AuthenticateEventArgs(name, pwhash));
			}
		}
	}
}