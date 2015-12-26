using Lidgren.Network;
using System.Collections.Generic;
using System;

namespace CountryTrash
{
	/// <summary>
	/// Client network
	/// </summary>
	internal partial class Network
	{
		private static readonly NetPeerConfiguration peerConfig = new NetPeerConfiguration("CountryTrash")
		{

		};
		private readonly NetClient client;

		public NetConnectionStatus Status { get; private set; }
		public NetCommands Commands { get; private set; }
		public NetEvents Events { get; private set; }

		public Network(string hostName)
		{
			this.client = new NetClient(peerConfig);
			this.client.Start();
			this.client.Connect(hostName, 1025);
			this.Commands = new NetCommands(this);
			this.Events = new NetEvents(this);
		}

		public void ReceiveMessages()
		{
			var list = new List<NetIncomingMessage>();
			this.client.ReadMessages(list);
			foreach (var msg in list)
			{
				switch (msg.MessageType)
				{
					case NetIncomingMessageType.StatusChanged:
					{
						var status = (NetConnectionStatus)msg.ReadByte();
						status.Log("Network status changed: {0}", "Network");
						this.Status = status;
						break;
					}
					case NetIncomingMessageType.Data:
					{
						this.Events.Dispatch(msg);
						break;
					}
					default:
					{
						msg.MessageType.Log("Unhandled message type {0}", "Network");
						break;
					}
				}
			}
			this.client.Recycle(list);
		}

		public void SendMessages()
		{
			this.client.FlushSendQueue();
		}
		
		public void Disconnect()
		{
			this.client.Disconnect("bye!");
		}
	}
}