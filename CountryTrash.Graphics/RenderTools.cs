using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CountryTrash.Graphics
{
	/// <summary>
	/// Provides some helper functions to do common render tasks.
	/// </summary>
	public static class RenderTools
	{
		public static string UIShader { get; set; } = "/Shaders/ui";
		public static string FontShader { get; set; } = "/Shaders/font";
		public static string UISprite { get; set; } = "/Models/widget";

		public static void RenderUI(
			ResourceManager resources,
			float screenWidth, float screenHeight,
			IEnumerable<IUserInterfaceSprite> elements)
		{
			var transformScreen = Matrix4.CreateOrthographicOffCenter(
				0.0f, screenWidth - 1.0f,
  				-(screenHeight - 1.0f), 0.0f,
				0.0f, 1.0f);

			var uishader = resources.Get<ShaderProgram>(UIShader);
			uishader.Bind();
			GL.Uniform1(uishader["texInterface"], 0);
			GL.UniformMatrix4(uishader["matScreen"], false, ref transformScreen);

			var fontshader = resources.Get<ShaderProgram>(FontShader);
			fontshader.Bind();
			GL.Uniform1(fontshader["texInterface"], 0);
			GL.UniformMatrix4(fontshader["matScreen"], false, ref transformScreen);


			var model = resources.Get<Model>(UISprite);

			foreach (var element in elements)
			{
				if (element == null)
					continue;

				GL.ActiveTexture(TextureUnit.Texture0);
				if (string.IsNullOrWhiteSpace(element.Texture) == false)
				{
					var transformModel =
						Matrix4.CreateScale(element.Size.X, -element.Size.Y, 1.0f) *
						Matrix4.CreateTranslation(element.Position.X, -element.Position.Y, 0);

					uishader.Bind();
					GL.UniformMatrix4(uishader["matModel"], false, ref transformModel);

					if (resources.Exist<Texture2D>(element.Texture))
					{
						resources.Get<Texture2D>(element.Texture).Bind();
						foreach (var mesh in model.Meshes)
						{
							model.DrawMesh(mesh);
						}
					}
				}

				if((element.Font != null) && (string.IsNullOrWhiteSpace(element.Text) == false))
				{
					var font = element.Font;
					var offset = Vector2.Zero;
					offset.Y = font.Size;

					fontshader.Bind();

					foreach (var c in element.Text)
					{
						font.LoadChar(c);

						var glyph = font.Glyph;
						
						var transformModel =
							Matrix4.CreateScale(glyph.Bitmap.Width, -glyph.Bitmap.Rows, 1.0f) *
							Matrix4.CreateTranslation(
								element.Position.X + offset.X + glyph.BitmapLeft, 
								-element.Position.Y - offset.Y + glyph.BitmapTop, 
								0);

						GL.UniformMatrix4(fontshader["matModel"], false, ref transformModel);

						font.Bind();
						foreach (var mesh in model.Meshes)
						{
							model.DrawMesh(mesh);
						}

						offset.X += glyph.Advance.X.ToSingle();
						offset.Y += glyph.Advance.Y.ToSingle();
					}
				}
			}
		}

		/// <summary>
		/// Renders a scene of objects with the given shader.
		/// </summary>
		/// <param name="resources">The resource manager that should provide the model textures.</param>
		/// <param name="shader">The shader that is used to render the scene.</param>
		/// <param name="transformViewProjection">The view and projection matrix.</param>
		/// <param name="scene">The scene that is drawn</param>
		public static void RenderScene(
			ResourceManager resources,
			ShaderProgram shader,
			Matrix4 transformViewProjection,
			IEnumerable<ISceneObject> scene)
		{
			shader.Bind();
			GL.Uniform1(shader["texDiffuse"], 0);
			GL.UniformMatrix4(shader["matViewProj"], false, ref transformViewProjection);

			foreach (var entity in scene)
			{
				if (entity== null)
					continue;
				if (entity.Model == null)
					continue;
				var transformModel =
					Matrix4.CreateRotationY(entity.Rotation) *
					Matrix4.CreateTranslation(entity.Position);

				GL.UniformMatrix4(shader["matModel"], false, ref transformModel);

				foreach (var mesh in entity.Model.Meshes)
				{
					GL.ActiveTexture(TextureUnit.Texture0);
					var texture = resources.Get<Texture2D>(mesh.DiffuseTexturePath);
					texture.Bind();

					entity.Model.DrawMesh(mesh);
				}
			}
		}
	}
}
