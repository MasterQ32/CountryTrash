using System;
using Lidgren.Network;

namespace CountryTrash
{
	public sealed partial class Client
	{
		private readonly NetServer server;
		private readonly NetConnection connection;

		public ClientCommands Commands { get; private set; }
		public ClientEvents Events { get; private set; }

		internal Client(NetServer server, NetConnection senderConnection)
		{
			this.server = server;
			this.connection = senderConnection;
			this.Commands = new ClientCommands(this);
			this.Events = new ClientEvents(this);
		}

		internal void DecodeMessage(NetIncomingMessage msg)
		{
			this.Events.Dispatch(msg);
		}
	}
}