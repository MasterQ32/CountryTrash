using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HarvestBoon.Graphics
{
	public sealed class Font : Texture
	{
		public Font() 
			: base(TextureTarget.Texture2D)
		{
			this.WrapR = TextureWrapMode.ClampToEdge;
			this.WrapS = TextureWrapMode.ClampToEdge;
			this.WrapT = TextureWrapMode.ClampToEdge;

			this.MinFilter = TextureMinFilter.Linear;
			this.MagFilter = TextureMagFilter.Linear;
		}
	}
}
