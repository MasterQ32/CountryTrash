using Assimp;
using CountryTrash.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Mesh = CountryTrash.Graphics.Mesh;

namespace CountryTrash.Tools
{
	class Program
	{
		static void Main(string[] args)
		{
			AssimpContext context = new AssimpContext();
			var scene = context.ImportFile(
				@"C:\Users\Felix\Desktop\Models\cuboid-tile.obj",
				PostProcessSteps.PreTransformVertices |
				PostProcessSteps.GenerateNormals |
				PostProcessSteps.JoinIdenticalVertices |
				PostProcessSteps.Triangulate);

			var vertices = new List<Vertex>();
			var indices = new List<int>();
			var meshes = new List<Mesh>();
			foreach(var amesh in scene.Meshes)
			{
				var mesh = new Mesh();
				mesh.IndexBase = indices.Count;
				mesh.IndexCount = 3 * amesh.FaceCount;
				mesh.DiffuseTexturePath = "/" + Path.GetFileNameWithoutExtension(scene.Materials[amesh.MaterialIndex]?.TextureDiffuse.FilePath ?? "default");
				meshes.Add(mesh);

				var indexlist = amesh.GetIndices();
				for (int i = 0; i < indexlist.Length; i++)
					indexlist[i] += vertices.Count;
				indices.AddRange(indexlist);

				for (int i = 0; i < amesh.VertexCount; i++)
				{
					Vertex v = new Vertex();
					v.Position.X = amesh.Vertices[i].X;
					v.Position.Y = amesh.Vertices[i].Y;
					v.Position.Z = amesh.Vertices[i].Z;
					if(amesh.HasNormals)
					{
						v.Normal.X = amesh.Normals[i].X;
						v.Normal.Y = amesh.Normals[i].Y;
						v.Normal.Z = amesh.Normals[i].Z;
					}
					if (amesh.HasTextureCoords(0))
					{
						v.UV.X = amesh.TextureCoordinateChannels[0][i].X;
						v.UV.Y = amesh.TextureCoordinateChannels[0][i].Y;
					}
					vertices.Add(v);
				}
			}

			var model = new Model(true);
			model.Meshes = meshes.ToArray();
			model.Indices = indices.ToArray();
			model.Vertices = vertices.ToArray();

			using (var fs = File.Open(@"C:\Users\Felix\Desktop\Models\model.xml", FileMode.Create, FileAccess.Write))
			{
				model.Save(fs);
			}
		}
	}
}
