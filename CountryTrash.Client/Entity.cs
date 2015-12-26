using CountryTrash.Graphics;
using OpenTK;
using System;

namespace CountryTrash
{
	internal class Entity : IEntity, ISceneObject, IBoxCollider
	{
		public int ID { get; set; }

		public Model Model { get; set; }

		public Vector3 Position { get; set; }

		public float Rotation { get; set; }

		public IVisualObject Visualization { get; set; }

		public Vector3 ColliderMin { get; set; } = new Vector3(-0.3f, 0.0f, -0.3f);
		public Vector3 ColliderMax { get; set; } = new Vector3( 0.3f, 1.0f,  0.3f);

		Vector3 IBoxCollider.LowerCorner => this.Position + this.ColliderMin;

		Vector3 IBoxCollider.UpperCorner => this.Position + this.ColliderMax;
	}
}