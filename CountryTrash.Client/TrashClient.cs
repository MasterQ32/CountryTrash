using CountryTrash.Graphics;
using OpenTK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CountryTrash
{
	public class TrashClient
	{
		static void Main(string[] args)
		{
			var network = new Network("localhost");

			var config = new GameConfiguration()
			{
				Title = "Country Trash - Version 1",
				InitialState = new ConnectingToServerState(network),
				AssetRoot = "./Assets/"
			};
			config.PreUpdateLoop += (s, e) => network.ReceiveMessages();
			config.PostUpdateLoop += (s, e) => network.SendMessages();

			Game.Start(config);

			network.Disconnect();
		}
	}
}
