using System;
using CountryTrash.Graphics;
using OpenTK;

namespace CountryTrash
{
	internal class Widget : IUserInterfaceSprite
	{
		public Font Font { get; set; }

		public Vector2 Position { get; set; } = new Vector2(0, 0);

		public Vector2 Size { get; set; } = new Vector2(100, 50);

		public string Text { get; set; }

		public string Texture { get; set; }
	}
}