using OpenTK;
using System.Collections.Generic;

namespace HarvestBoon.Graphics
{
	public static class Picking
	{
		/// <summary>
		/// Casts a ray into a set of objects and returns the nearest object or null.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="mouseX"></param>
		/// <param name="mouseY"></param>
		/// <param name="screenSizeX"></param>
		/// <param name="screenSizeY"></param>
		/// <param name="viewProjectionMatrix"></param>
		/// <param name="objects"></param>
		/// <returns></returns>
		public static T Pick<T>(
			float mouseX, float mouseY,
			float screenSizeX, float screenSizeY,
			Matrix4 viewProjectionMatrix,
			IEnumerable<T> objects)
			where T : class, IBoxCollider
		{
			float x = 2.0f * (mouseX / screenSizeX) - 1.0f;
			float y = 1.0f - 2.0f * (mouseY / screenSizeY);

			var invViewProj = Matrix4.Invert(viewProjectionMatrix);

			var from = Vector4.Transform(new Vector4(x, y, 0.0f, 1.0f), invViewProj);
			var to = Vector4.Transform(new Vector4(x, y, 1.0f, 1.0f), invViewProj);

			from *= (1.0f / from.W);
			to *= (1.0f / to.W);

			var ray = new Ray(from.Xyz, to.Xyz);

			return ray.Intersect(objects);
		}
	}
}