using System;

namespace CountryTrash.Graphics
{
	/// <summary>
	/// An abstract opengl resource that can be managed by the ResourceManager.
	/// </summary>
	public abstract class GLResource : Resource
	{
		private readonly ResourceType type;
		private int id;

		protected GLResource(ResourceType type)
		{
			this.type = type;
			this.id = this.CreateResource();
		}

		/// <summary>
		/// Unloads the resource and destroys it.
		/// </summary>
		internal protected override void Unload()
		{
			this.DestroyResource(this.id);
			this.id = 0;
		}

		/// <summary>
		/// Creates a binding for the resource.
		/// </summary>
		/// <returns>The binding that allows releasing of the resource.</returns>
		public ResourceBinding Bind()
		{
			if (this.id == 0)
				throw new InvalidOperationException("Cannot bind an already unloaded resource.");
			return new ResourceBinding(this);
		}

		/// <summary>
		/// Binds a named resource.
		/// </summary>
		/// <param name="id">The name of the OpenGL object.</param>
		internal protected abstract void Bind(int id);

		/// <summary>
		/// Creates a new instance of the resource.
		/// </summary>
		/// <returns>The name of the OpenGL object.</returns>
		protected abstract int CreateResource();

		/// <summary>
		/// Destroys an already created resource.
		/// </summary>
		/// <param name="id">The name of the OpenGL object.</param>
		protected abstract void DestroyResource(int id);

		/// <summary>
		/// Gets the name of the OpenGL object.
		/// </summary>
		public int ID => this.id;

		/// <summary>
		/// Gets the type of the OpenGL object.
		/// </summary>
		public ResourceType Type => this.type;

		/// <summary>
		/// Converts the OpenGL resource into an resource name.
		/// </summary>
		/// <param name="resource"></param>
		public static explicit operator int (GLResource resource) =>
			resource != null ? resource.id : 0;

		public override string ToString() => $"{this.Type}({this.ID})";
    }

	/// <summary>
	/// A binding of an OpenGL resource.
	/// </summary>
	public sealed class ResourceBinding : IDisposable
	{
		private readonly GLResource resource;

		/// <summary>
		/// Creates a new binding of a resource and binds it.
		/// </summary>
		/// <param name="resource"></param>
		public ResourceBinding(GLResource resource)
		{
			this.resource = resource;
			this.resource.Bind(this.resource.ID);
		}

		/// <summary>
		/// Releases the binding of the resource.
		/// </summary>
		public void Release()
		{
			this.resource.Bind(0);
		}

		void IDisposable.Dispose()
		{
			this.Release();
		}
	}
}