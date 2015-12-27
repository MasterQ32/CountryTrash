using CountryTrash.Graphics;
using System;
using OpenTK;
using System.Collections.Generic;

namespace CountryTrash
{
	internal class Tile : ITile, ISceneNode, IBoxCollider
	{
		public float Height { get; set; }

		public bool IsInteractive { get; set; }
		
		public bool IsBlocked { get; set; }
		
		public string Model { get; set; } = "/quad";
		
		public string Topping { get; set; }

		public int X { get; set; }

		public int Z { get; set; }

		private class Visualization : ISceneObject
		{
			private readonly bool isTopping;
			private readonly Tile tile;

			public Visualization(Tile tile, bool isTopping)
			{
				this.tile = tile;
				this.isTopping = isTopping;
			}

			public Model Model => Game.Current.Resources.Get<Model>(isTopping ? tile.Topping : tile.Model);

			public Vector3 Position => new Vector3(tile.X, tile.Height, tile.Z);

			public float Rotation => 0.0f;
		}

		Vector3 IBoxCollider.LowerCorner => new Vector3(this.X, this.Height, this.Z) - new Vector3(0.5f, 10.0f, 0.5f);

		Vector3 IBoxCollider.UpperCorner => new Vector3(this.X, this.Height, this.Z) + new Vector3(0.5f,  0.0f, 0.5f);

		IEnumerable<ISceneObject> ISceneNode.Children => new[] { new Visualization(this, false), new Visualization(this, true) }; 
	}
}