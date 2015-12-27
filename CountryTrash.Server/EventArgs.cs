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
		private readonly int x;
		private readonly int z;

		public InvokeTileActionEventArgs(int x, int z)
		{
			this.x = x;
			this.z = z;
		}

		public int X => this.x;
		public int Z => this.z;
	}

	public sealed class InvokeEntityAtionEventArgs : EventArgs
	{

	}

	public sealed class MoveInventoryItemEventArgs : EventArgs
	{

	}
}