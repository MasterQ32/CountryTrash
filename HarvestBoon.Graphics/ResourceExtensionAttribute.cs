using System.Linq;

namespace HarvestBoon.Graphics
{
	/// <summary>
	/// Defines a number of extensions that can be used for a resource file.
	/// </summary>
	/// The extensions must have the format ".\w+"
	[System.AttributeUsage(System.AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
	public sealed class ResourceExtensionAttribute : System.Attribute
	{
		private readonly string[] extensions;

		// This is a positional argument
		public ResourceExtensionAttribute(string extension, params string[] extensions)
		{
			this.extensions = new[] { extension }.Concat(extensions).ToArray();
		}

		public string[] Extensions => this.extensions;
	}
}