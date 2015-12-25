using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Serialization;

namespace HarvestBoon.Graphics
{
	[XmlRoot("model")]
	[ResourceExtension(".mesh")]
	public class Model : Resource, ILoadableResource, ISaveableResource
	{
		private static readonly object block = new object();
		private static bool isDeserializing = false;
		private static readonly XmlSerializer ser = new XmlSerializer(typeof(Model));
		private readonly GLBuffer vertexBuffer;
		private readonly GLBuffer indexBuffer;
		private readonly VertexArray vertexArray;

		public Model(bool dontGenerateData)
		{
			if (dontGenerateData)
				return;

			this.vertexArray = new VertexArray();
			this.vertexBuffer = new GLBuffer(BufferTarget.ArrayBuffer);
			this.indexBuffer = new GLBuffer(BufferTarget.ElementArrayBuffer);

			using (var b = this.vertexArray.Bind())
			{
				GL.EnableVertexAttribArray(0); // position
				GL.EnableVertexAttribArray(1); // normal
				GL.EnableVertexAttribArray(2); // uv

				this.vertexBuffer.Bind();
				GL.VertexAttribPointer(
					0,
					3,
					VertexAttribPointerType.Float,
					false,
					Marshal.SizeOf(typeof(Vertex)),
					0);
				GL.VertexAttribPointer(
					1,
					3,
					VertexAttribPointerType.Float,
					true,
					Marshal.SizeOf(typeof(Vertex)),
					12);
				GL.VertexAttribPointer(
					2,
					2,
					VertexAttribPointerType.Float,
					false,
					Marshal.SizeOf(typeof(Vertex)),
					24);

				this.indexBuffer.Bind();
			}
		}

		public Model() : this(isDeserializing)
		{

		}

		[XmlElement("mesh")]
		public Mesh[] Meshes { get; set; }

		[XmlArray("vertices")]
		[XmlArrayItem("vertex")]
		public Vertex[] Vertices { get; set; }

		[XmlArray("indices")]
		[XmlArrayItem("int")]
		public int[] Indices { get; set; }

		public void Load(ResourceManager resources, Stream stream)
		{
			Model model;

			// When using the deserializer, do not create mesh data
			lock (block)
			{
				isDeserializing = true;
				try
				{
					model = (Model)ser.Deserialize(stream);
				}
				finally
				{
					isDeserializing = false;
				}
			}

			this.Indices = model.Indices;
			this.Meshes = model.Meshes;
			this.Vertices = model.Vertices;

			using (var b = this.indexBuffer.Bind())
			{
				GL.BufferData(
					this.indexBuffer.Target,
					new IntPtr(sizeof(int) * this.Indices.Length),
					this.Indices,
					BufferUsageHint.StaticDraw);
			}

			using (var b = this.vertexBuffer.Bind())
			{
				GL.BufferData(
					this.vertexBuffer.Target,
					new IntPtr(Marshal.SizeOf(typeof(Vertex)) * this.Vertices.Length),
					this.Vertices,
					BufferUsageHint.StaticDraw);
			}
		}

		protected internal override void Unload()
		{
			this.vertexBuffer.Unload();
			this.indexBuffer.Unload();
		}

		public void DrawMesh(Mesh mesh)
		{
			if (this.Meshes.Contains(mesh) == false)
				throw new InvalidOperationException("Cannot draw a mesh that is not in this model.");
			using (var b = this.vertexArray.Bind())
			{
				GL.DrawElements(
					PrimitiveType.Triangles,
					mesh.IndexCount,
					DrawElementsType.UnsignedInt,
					new IntPtr(sizeof(int) * mesh.IndexBase));
			}
		}

		public void Save(Stream stream)
		{
			ser.Serialize(stream, this);
		}
	}
}
