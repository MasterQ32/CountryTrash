using System;

namespace CountryTrash
{
	public sealed class ClientEventArgs : EventArgs
	{
		private readonly Client client;

		public ClientEventArgs(Client client)
		{
			this.client = client;
		}

		public Client Client => this.client;
	}

	public sealed class AuthenticateEventArgs : EventArgs
	{
		private readonly string name;
		private readonly byte[] pwhash;

		public AuthenticateEventArgs(string name, byte[] pwhash)
		{
			this.name = name;
			this.pwhash = pwhash;
		}

		public string Name => this.name;
		public byte[] Hash => this.pwhash;
	}

	public sealed class InvokeTileActionEventArgs : EventArgs
	{

	}

	public sealed class InvokeEntityAtionEventArgs : EventArgs
	{

	}

	public sealed class MoveInventoryItemEventArgs : EventArgs
	{

	}
}