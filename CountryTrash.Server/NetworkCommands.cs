namespace CountryTrash
{
	partial class Network
	{
		/// <summary>
		/// Server-to-client commands
		/// </summary>
		public sealed class NetCommands
		{
			private readonly Network network;

			public NetCommands(Network network)
			{
				this.network = network;
			}
		}
	}
}