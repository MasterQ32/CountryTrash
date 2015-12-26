using OpenTK;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace CountryTrash.Graphics
{
	[StructLayout(LayoutKind.Sequential)]
	public struct Vertex
	{
		[XmlElement("position")]
		public Vector3 Position;

		[XmlElement("normal")]
		public Vector3 Normal;

		[XmlElement("uv")]
		public Vector2 UV;
	}
}