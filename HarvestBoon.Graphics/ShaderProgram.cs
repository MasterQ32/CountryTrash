using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HarvestBoon.Graphics
{
	[ResourceExtension(".shader")]
	public sealed class ShaderProgram : GLResource, ILoadableResource
	{
		private Dictionary<string, int> uniforms = new Dictionary<string, int>();

		public ShaderProgram()
			: base(ResourceType.Program)
		{

		}

		public int this[string uniformName]
		{
			get
			{
				if (this.uniforms.ContainsKey(uniformName) == false)
					this.uniforms.Add(uniformName, GL.GetUniformLocation(this.ID, uniformName));
				return this.uniforms[uniformName];
			}
		}

		public void Load(ResourceManager resources, Stream stream)
		{
			int status;

			using (var sr = new StreamReader(stream, Encoding.UTF8))
			{
				while(!sr.EndOfStream)
				{
					var line = sr.ReadLine().Trim();

					// Comment line
					if (line.StartsWith("#"))
						continue;

					var parts = line.Split(':');
					switch(parts[0])
					{
						case "vs":
						{
							AttachShaderFromResource<VertexShader>(resources, parts[1]);
							break;
						}
						case "gs":
						{
							AttachShaderFromResource<GeometryShader>(resources, parts[1]);
							break;
						}
						case "fs":
						{
							AttachShaderFromResource<FragmentShader>(resources, parts[1]);
							break;
						}
						default:
						{
							throw new InvalidOperationException($"Invalid shader type specification: '{parts[0]}'");
						}
					}
				}
			}

			GL.LinkProgram(this.ID);
			GL.GetProgram(this.ID, GetProgramParameterName.LinkStatus, out status);
			var log = GL.GetProgramInfoLog(this.ID);
			if (String.IsNullOrWhiteSpace(log) == false)
				Console.WriteLine(log);
			if (status != 1)
				throw new InvalidOperationException("Could not link shader program.", new Exception(log));
		}

		private void AttachShaderFromResource<T>(ResourceManager resources, string path)
			where T : Shader, new()
		{
			var shader = resources.Get<T>(path);
			GL.AttachShader(this.ID, shader.ID);
		}
		
		protected override int CreateResource() => GL.CreateProgram();

		protected override void DestroyResource(int id) => GL.DeleteProgram(id);

		protected internal override void Bind(int id) => GL.UseProgram(id);
	}
}