using System;
using HarvestBoon.Graphics;
using OpenTK;

namespace HarvestBoon
{
	internal class Widget : IUserInterfaceSprite
	{
		public Vector2 Position { get; set; } = new Vector2(0, 0);

		public Vector2 Size { get; set; } = new Vector2(100, 50);

		public string Texture { get; set; } = "/Textures/example";
	}
}