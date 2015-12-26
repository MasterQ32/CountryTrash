using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CountryTrash.Graphics
{
	public abstract class Shader : GLResource, ILoadableResource
	{
		private readonly ShaderType shaderType;

		protected Shader(ShaderType type)
			: base(ResourceType.Shader)
		{
			this.shaderType = type;
		}

		public void Load(ResourceManager resources, Stream stream)
		{
			string code;
			int status;
			using (var sr = new StreamReader(stream, Encoding.UTF8))
			{
				code = sr.ReadToEnd();
			}
			GL.ShaderSource(this.ID, code);
			GL.CompileShader(this.ID);
			GL.GetShader(this.ID, ShaderParameter.CompileStatus, out status);
			string log = GL.GetShaderInfoLog(this.ID).Trim();
			if(String.IsNullOrWhiteSpace(log) == false)
				Console.WriteLine(log);
			if (status != 1)
				throw new InvalidOperationException("The selected shader could not be compiled", new Exception(log));
		}

		protected override void DestroyResource(int id) => GL.DeleteShader(id);

		protected internal override void Bind(int id) { }
	}

	[ResourceExtension(".vs")]
	public sealed class VertexShader : Shader
	{
		public VertexShader()
			: base(ShaderType.VertexShader)
		{

		}

		protected override int CreateResource() => GL.CreateShader(ShaderType.VertexShader);
	}

	[ResourceExtension(".fs")]
	public sealed class FragmentShader : Shader
	{
		public FragmentShader()
			: base(ShaderType.FragmentShader)
		{

		}

		protected override int CreateResource() => GL.CreateShader(ShaderType.FragmentShader);
	}

	[ResourceExtension(".gs")]
	public sealed class GeometryShader : Shader
	{
		public GeometryShader()
			: base(ShaderType.GeometryShader)
		{

		}

		protected override int CreateResource() => GL.CreateShader(ShaderType.GeometryShader);
	}
}
