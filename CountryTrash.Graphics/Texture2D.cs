using OpenTK.Graphics.OpenGL4;
using System.Drawing;
using System.IO;

namespace CountryTrash.Graphics
{
	/// <summary>
	/// A two-dimensional texture
	/// </summary>
	[ResourceExtension(".png", ".jpg", ".bmp")]
	public sealed class Texture2D : Texture, ILoadableResource
	{
		public Texture2D()
			: base(TextureTarget.Texture2D)
		{
			this.MinFilter = TextureMinFilter.Linear;
			this.MagFilter = TextureMagFilter.Linear;
		}


		/// <summary>
		/// Loads the texture content.
		/// </summary>
		/// <param name="source">The file that should be loaded.</param>
		/// <remarks>Rebinds the texture target.</remarks>
		public void Load(string fileName)
		{
			using (var bmp = new Bitmap(fileName))
			{
				this.Load(bmp);
			}
		}

		/// <summary>
		/// Loads the texture content.
		/// </summary>
		/// <param name="source">A stream to an image file.</param>
		/// <remarks>Rebinds the texture target.</remarks>
		public void Load(Stream source)
		{
			using (var bmp = new Bitmap(source))
			{
				this.Load(bmp);
			}
		}

		/// <summary>
		/// Loads the texture content.
		/// </summary>
		/// <param name="source">A stream to an image file.</param>
		/// <remarks>Rebinds the texture target.</remarks>
		public void Load(ResourceManager resources, Stream source)
		{
			using (var bmp = new Bitmap(source))
			{
				bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
				this.Load(bmp);
			}
		}

		/// <summary>
		/// Loads the texture content.
		/// </summary>
		/// <param name="source">The bitmap that is loaded into this texture.</param>
		/// <remarks>Rebinds the texture target.</remarks>
		public void Load(Bitmap source)
		{
			using (var b = this.Bind())
			{
				var data = source.LockBits(
					new Rectangle(0, 0, source.Width, source.Height),
					System.Drawing.Imaging.ImageLockMode.ReadOnly,
					System.Drawing.Imaging.PixelFormat.Format32bppArgb);
				GL.TexImage2D(this.Target,
					0,
					PixelInternalFormat.Rgba,
					source.Width,
					source.Height,
					0,
					PixelFormat.Bgra,
					PixelType.UnsignedByte,
					data.Scan0);
				source.UnlockBits(data);
			}
		}
	}
}