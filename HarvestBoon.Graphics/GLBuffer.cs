using OpenTK.Graphics.OpenGL4;
using System;

namespace HarvestBoon.Graphics
{
	public sealed class GLBuffer : GLResource
	{
		private readonly BufferTarget target;

		public GLBuffer(BufferTarget target) 
			 : base(ResourceType.Buffer)
		{
			this.target = target;
		}

		protected override int CreateResource() => GL.GenBuffer();

		protected override void DestroyResource(int id) => GL.DeleteBuffer(id);

		protected internal override void Bind(int id) => GL.BindBuffer(this.target, id);

		public BufferTarget Target => this.target;
	}
}