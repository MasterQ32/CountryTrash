using System.IO;

namespace CountryTrash.Graphics
{
	/// <summary>
	/// A resource that is loadable.
	/// </summary>
	public interface ILoadableResource
	{
		/// <summary>
		/// Loads the resource from the stream.
		/// </summary>
		/// <param name="resources">The resource manager that loads the resource. Can be used to load depending resources.</param>
		/// <param name="stream">The stream that provides the resource data.</param>
		void Load(ResourceManager resources, Stream stream);
	}
}