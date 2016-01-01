using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SharpFont;

namespace CountryTrash.Graphics
{
	[ResourceExtension(".ttf")]
	public sealed class Font : Texture, ILoadableResource
	{
		static Library library = null;

		private Face face = null;

		public Font()
			: base(TextureTarget.Texture2D)
		{
			library = library ?? new Library();

			this.WrapR = TextureWrapMode.ClampToEdge;
			this.WrapS = TextureWrapMode.ClampToEdge;
			this.WrapT = TextureWrapMode.ClampToEdge;

			this.MinFilter = TextureMinFilter.Linear;
			this.MagFilter = TextureMagFilter.Linear;
		}

		private static byte[] ReadFully(Stream input)
		{
			using (MemoryStream ms = new MemoryStream())
			{
				input.CopyTo(ms);
				return ms.ToArray();
			}
		}

		public void Load(ResourceManager resources, Stream stream)
		{
			var bits = ReadFully(stream);

			if (this.face != null)
				this.face.Dispose();

			this.face = new Face(library, bits, 0);
			this.face.SetPixelSizes(0, (uint)this.Size);
		}

		public void LoadChar(char c)
		{
			this.face.LoadChar(c, LoadFlags.Render, LoadTarget.Normal);

			GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);

			using (var b = this.Bind())
			{
				GL.TexImage2D(
					TextureTarget.Texture2D,
					0,
					PixelInternalFormat.R8,
					this.face.Glyph.Bitmap.Width,
					this.face.Glyph.Bitmap.Rows,
					0,
					PixelFormat.Red,
					PixelType.UnsignedByte,
					this.face.Glyph.Bitmap.Buffer);
			}
		}

		public GlyphSlot Glyph => this.face.Glyph;

		protected internal override void Unload()
		{
			base.Unload();
			this.face.Dispose();
			this.face = null;
		}

		public int Size { get; } = 24;
	}
}
