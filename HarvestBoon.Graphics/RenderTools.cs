using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HarvestBoon.Graphics
{
	/// <summary>
	/// Provides some helper functions to do common render tasks.
	/// </summary>
	public static class RenderTools
	{
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
