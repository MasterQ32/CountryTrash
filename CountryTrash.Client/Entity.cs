using CountryTrash.Graphics;
using OpenTK;
using System;

namespace CountryTrash
{
	internal class Entity : IEntity, ISceneObject, IBoxCollider
	{
		private Vector3 position;
		private float rotation;
		private string visualization;

		public event EventHandler<EntityChangedEventArgs> Changed;

		protected void OnChanged(EntityProperty property)
		{
			if (this.Changed != null)
				this.Changed(this, new EntityChangedEventArgs(property));
		}

		public int ID { get; set; }

		public Vector3 Position
		{
			get { return this.position; }
			set { this.position = value; this.OnChanged(EntityProperty.Position); }
		}

		public float Rotation
		{
			get { return this.rotation; }
			set { this.rotation = value; this.OnChanged(EntityProperty.Rotation); }
		}

		public string Visualization
		{
			get { return this.visualization; }
			set { this.visualization = value; this.OnChanged(EntityProperty.Visualization); }
		}

		public Vector3 ColliderMin { get; set; } = new Vector3(-0.3f, 0.0f, -0.3f);
		public Vector3 ColliderMax { get; set; } = new Vector3(0.3f, 1.0f, 0.3f);

		Vector3 IBoxCollider.LowerCorner => this.Position + this.ColliderMin;

		Vector3 IBoxCollider.UpperCorner => this.Position + this.ColliderMax;

		public Model Model => Game.Current.Resources.Get<Model>(this.Visualization);
	}
}