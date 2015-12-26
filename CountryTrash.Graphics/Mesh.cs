using System.Xml.Serialization;

namespace CountryTrash.Graphics
{
	/// <summary>
	/// A mesh is a part of a model that is rendered with a given texture.
	/// </summary>
	public class Mesh
	{
		/// <summary>
		/// The name of the texture resource used for diffuse shading.
		/// </summary>
		[XmlElement("diffuse")]
		public string DiffuseTexturePath { get; set; }

		/// <summary>
		/// The offset of the mesh indices in the model indices.
		/// </summary>
		[XmlElement("index-base")]
		public int IndexBase { get; set; }

		/// <summary>
		/// The number of indices for this mesh.
		/// </summary>
		[XmlElement("index-count")]
		public int IndexCount { get; set; }
	}
}