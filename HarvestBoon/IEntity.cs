using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HarvestBoon
{
    public interface IEntity
    {
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

		/// <summary>
		/// Gets or sets the visualization of the entity.
		/// </summary>
		IVisualObject Visualization { get; set; }
    }
}
