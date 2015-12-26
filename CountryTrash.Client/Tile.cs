using CountryTrash.Graphics;
using System;
using OpenTK;

namespace CountryTrash
{
	internal class Tile : ITile, ISceneObject, IBoxCollider
	{
		public float Height { get; set; }

		public bool IsInteractive { get; set; }

		public string Model { get; set; } = "/quad";

		public int X { get; set; }

		public int Z { get; set; }

		Vector3 IBoxCollider.LowerCorner => new Vector3(this.X, this.Height, this.Z) - new Vector3(0.5f, 10.0f, 0.5f);

		Vector3 IBoxCollider.UpperCorner => new Vector3(this.X, this.Height, this.Z) + new Vector3(0.5f,  0.0f, 0.5f);

		Model ISceneObject.Model => Game.Current.Resources.Get<Model>(this.Model); 

		Vector3 ISceneObject.Position => new Vector3(this.X, this.Height, this.Z);

		float ISceneObject.Rotation => 0.0f;
	}
}