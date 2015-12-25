using Lidgren.Network;
using System.Collections.Generic;
using System;

namespace HarvestBoon
{
	internal class Network
	{
		private static readonly NetPeerConfiguration peerConfig = new NetPeerConfiguration("HarvestBoon")
		{
			Port = 1025
		};
		private readonly NetClient client;

		public Network(string hostName)
		{
			this.client = new NetClient(peerConfig);
			this.client.Start();
			this.client.Connect(hostName, peerConfig.Port);
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
						break;
					}
					case NetIncomingMessageType.Data:
					{
						msg.LengthBytes.Log("Received {0} bytes of data.", "Network");
						this.DecodeMessage(msg);
						break;
					}
					default:
					{
						msg.MessageType.Log("Unhandled message type {0}", "Network");
						break;
					}
				}
			}
		}

		private void DecodeMessage(NetIncomingMessage msg)
		{
			var cmd = (ServerToClientNetworkCommand)msg.ReadByte();
			switch (cmd)
			{
				default:
				{
					cmd.Log("Unhandled command type: {0}", "Network");
					break;
				}
			}
		}

		public void SendMessages()
		{
			this.client.FlushSendQueue();
		}
	}
}