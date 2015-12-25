using System;
using static System.Math;
using OpenTK;
using System.Collections.Generic;

namespace HarvestBoon.Graphics
{
	/// <summary>
	/// A threedimensional ray that has collision checks.
	/// </summary>
	public sealed class Ray
	{
		private readonly Vector3 direction;
		private readonly Vector3 origin;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="origin">The origin of the ray</param>
		/// <param name="direction">The direction of the ray which is normalized</param>
		public Ray(Vector3 origin, Vector3 direction)
		{
			this.origin = origin;
			this.direction = direction.Normalized();
		}

		/// <summary>
		/// Gets the origin of the ray.
		/// </summary>
		public Vector3 Origin => this.origin;

		/// <summary>
		/// Gets the direction of the ray.
		/// </summary>
		public Vector3 Direction => this.direction;

		/// <summary>
		/// Intersects the ray with a collection of objects and returns the nearest object.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="colliders"></param>
		/// <returns></returns>
		public T Intersect<T>(IEnumerable<T> colliders)
			where T : class, IBoxCollider
		{
			T nearest = null;
			double distance = double.PositiveInfinity;
			foreach (var collider in colliders)
			{
				var dist = this.Intersect(collider);
				if (dist == null)
					continue;
				if (dist > distance)
					continue;
				distance = dist.Value;
				nearest = collider;
			}
			return nearest;
		}

		/// <summary>
		/// Intersects the ray with an collider object.
		/// </summary>
		/// <param name="collider"></param>
		/// <returns>The distance to the object or null if the object was not hit.</returns>
		public double? Intersect(IBoxCollider collider)
		{
			if (collider == null)
				return null;

			float tmin, tmax, tymin, tymax, tzmin, tzmax;

			var bounds = new[] { collider.LowerCorner, collider.UpperCorner };

			if (this.direction.X >= 0)
			{
				tmin = (bounds[0].X - this.origin.X) / this.direction.X;
				tmax = (bounds[1].X - this.origin.X) / this.direction.X;
			}
			else {
				tmin = (bounds[1].X - this.origin.X) / this.direction.X;
				tmax = (bounds[0].X - this.origin.X) / this.direction.X;
			}
			if (this.direction.Y >= 0)
			{
				tymin = (bounds[0].Y - this.origin.Y) / this.direction.Y;
				tymax = (bounds[1].Y - this.origin.Y) / this.direction.Y;
			}
			else {
				tymin = (bounds[1].Y - this.origin.Y) / this.direction.Y;
				tymax = (bounds[0].Y - this.origin.Y) / this.direction.Y;
			}
			if ((tmin > tymax) || (tymin > tmax))
				return null;

			if (tymin > tmin)
				tmin = tymin;
			if (tymax < tmax)
				tmax = tymax;
			if (this.direction.Z >= 0)
			{
				tzmin = (bounds[0].Z - this.origin.Z) / this.direction.Z;
				tzmax = (bounds[1].Z - this.origin.Z) / this.direction.Z;
			}
			else {
				tzmin = (bounds[1].Z - this.origin.Z) / this.direction.Z;
				tzmax = (bounds[0].Z - this.origin.Z) / this.direction.Z;
			}
			if ((tmin > tzmax) || (tzmin > tmax))
				return null;
			if (tzmin > tmin)
				tmin = tzmin;
			if (tzmax < tmax)
				tmax = tzmax;
			return tmin;
		}

	}
}