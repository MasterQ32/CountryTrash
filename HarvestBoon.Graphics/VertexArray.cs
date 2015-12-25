using OpenTK.Graphics.OpenGL4;
using System;

namespace HarvestBoon.Graphics
{
	public sealed class VertexArray : GLResource
	{
		public VertexArray()
			: base(ResourceType.VertexArray)
		{

		}

		protected override int CreateResource() => GL.GenVertexArray();

		protected override void DestroyResource(int id) => GL.DeleteVertexArray(id);

		protected internal override void Bind(int id) => GL.BindVertexArray(id);
	}
}