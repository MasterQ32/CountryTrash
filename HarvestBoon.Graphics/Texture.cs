using System;
using OpenTK.Graphics.OpenGL4;
using System.Linq;

namespace HarvestBoon.Graphics
{
	public abstract class Texture : GLResource
	{
		private readonly TextureTarget target;

		protected Texture(TextureTarget target)
			: base(ResourceType.Texture)
		{
			this.target = target;
		}

		/// <summary>
		/// Generates mip maps for the texture.
		/// </summary>
		/// <exception cref="System.InvalidOperationException">The texture does not have a target that allows mip map generation.</exception>
		/// <remarks>Rebinds the textures target.</remarks>
		public void GenerateMipMaps()
		{
			// A bit hacky, but a quite clean solution:
			// Checks if Texture.Target is a valid enumaration value for GenerateMipmapTarget
			// This works because TEXTURE_2D is an OpenGL constant and GenerateMipmapTarget
			// as well as TextureTarget do contain the OpenGL constants.
			if (Enum.GetValues(typeof(GenerateMipmapTarget)).OfType<TextureTarget>().Contains(this.target) == false)
				throw new InvalidOperationException("Invalid texture target for mipmap generation.");
			using (var b = this.Bind())
				GL.GenerateMipmap((GenerateMipmapTarget)this.target);
		}

		internal protected override void Bind(int id) => GL.BindTexture(this.target, id);

		protected override int CreateResource() => GL.GenTexture();

		protected override void DestroyResource(int id) => GL.DeleteTexture(id);

		public TextureTarget Target => this.target;

		/// <summary>
		/// Gets or sets the minifying filter for this texture.
		/// </summary>
		/// <remarks>Rebinds the textures target.</remarks>
		public TextureMinFilter MinFilter
		{
			get
			{
				int i;
				using (var b = this.Bind())
					GL.GetTexParameter(this.target, GetTextureParameter.TextureMinFilter, out i);
				return (TextureMinFilter)i;
			}
			set
			{
				using (var b = this.Bind())
					GL.TexParameter(this.target, TextureParameterName.TextureMinFilter, (int)value);
			}
		}

		/// <summary>
		/// Gets or sets the magnifying filter for this texture.
		/// </summary>
		/// <remarks>Rebinds the textures target.</remarks>
		public TextureMagFilter MagFilter
		{
			get
			{
				int i;
				using (var b = this.Bind())
					GL.GetTexParameter(this.target, GetTextureParameter.TextureMagFilter, out i);
				return (TextureMagFilter)i;
			}
			set
			{
				using (var b = this.Bind())
					GL.TexParameter(this.target, TextureParameterName.TextureMagFilter, (int)value);
			}
		}

		/// <summary>
		/// Gets or sets the wrapping for this texture.
		/// </summary>
		/// <remarks>Rebinds the textures target.</remarks>
		public TextureWrapMode WrapS
		{
			get
			{
				int i;
				using (var b = this.Bind())
					GL.GetTexParameter(this.target, GetTextureParameter.TextureWrapS, out i);
				return (TextureWrapMode)i;
			}
			set
			{
				using (var b = this.Bind())
					GL.TexParameter(this.target, TextureParameterName.TextureWrapS, (int)value);
			}
		}

		/// <summary>
		/// Gets or sets the wrapping for this texture.
		/// </summary>
		/// <remarks>Rebinds the textures target.</remarks>
		public TextureWrapMode WrapT
		{
			get
			{
				int i;
				using (var b = this.Bind())
					GL.GetTexParameter(this.target, GetTextureParameter.TextureWrapT, out i);
				return (TextureWrapMode)i;
			}
			set
			{
				using (var b = this.Bind())
					GL.TexParameter(this.target, TextureParameterName.TextureWrapT, (int)value);
			}
		}

		/// <summary>
		/// Gets or sets the wrapping for this texture.
		/// </summary>
		/// <remarks>Rebinds the textures target.</remarks>
		public TextureWrapMode WrapR
		{
			get
			{
				int i;
				using (var b = this.Bind())
					GL.GetTexParameter(this.target, GetTextureParameter.TextureWrapR, out i);
				return (TextureWrapMode)i;
			}
			set
			{
				using (var b = this.Bind())
					GL.TexParameter(this.target, TextureParameterName.TextureWrapR, (int)value);
			}
		}
	}
}