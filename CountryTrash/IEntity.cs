using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CountryTrash
{
	/// <summary>
	/// Declares the common interface for an entity class that 
	/// is a model on a map.
	/// </summary>
	public interface IEntity
	{
		/// <summary>
		/// The entity changed its position, rotation or visualization.
		/// </summary>
		event EventHandler<EntityChangedEventArgs> Changed;

		/// <summary>
		/// Gets the unique identifier of the entity.
		/// </summary>
		int ID { get; }

		/// <summary>
		/// Gets or sets the position of this entity.
		/// </summary>
		Vector3 Position { get; set; }

		/// <summary>
		/// Gets or sets the rotation around the y axis of the entity.
		/// </summary>
		float Rotation { get; set; }

		// TODO: Change to some better object type that allows animation and stuff
		/// <summary>
		/// Gets or sets the visualization of the entity.
		/// </summary>
		string Visualization { get; set; }
	}

	public sealed class EntityChangedEventArgs : EventArgs
	{
		public EntityChangedEventArgs(EntityProperty property)
		{
			this.Property = property;
		}

		public EntityProperty Property { get; private set; }
	}

	public enum EntityProperty
	{
		Position,
		Rotation,
		Visualization
	}
}
