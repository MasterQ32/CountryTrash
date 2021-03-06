﻿using OpenTK;
using System.Collections.Generic;

namespace CountryTrash.Graphics
{
	/// <summary>
	/// An abstract scene object that has a position, a model and a rotation.
	/// </summary>
	public interface ISceneObject
	{
		Model Model { get; }

		Vector3 Position { get; }

		float Rotation { get; }
	}

	/// <summary>
	/// An abstract scene node that contains multiple objects.
	/// </summary>
	public interface ISceneNode
	{
		IEnumerable<ISceneObject> Children { get; }
	}
}