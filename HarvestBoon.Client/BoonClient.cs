using HarvestBoon.Graphics;
using OpenTK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace HarvestBoon
{
	public class BoonClient
	{
		static void Main(string[] args)
		{
			var network = new Network("localhost");

			var config = new GameConfiguration()
			{
				Title = "Harvest Boon - Version 1",
				InitialState = new MapDisplayState(network),
				AssetRoot = "./Assets/"
			};
			config.PreUpdateLoop += (s, e) => network.ReceiveMessages();
			config.PostUpdateLoop += (s, e) => network.SendMessages();

			Game.Start(config);
		}
	}
}
