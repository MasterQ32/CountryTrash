using CountryTrash.Graphics;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using System;
using System.Linq;

namespace CountryTrash
{
	internal class MapDisplayState : GameState
	{
		private readonly Network network;

		Matrix4 matViewProjection;
		ShaderProgram shader;
		Map map;
		UserInterface ui;

		Entity helper;

		public MapDisplayState(Network network)
		{
			this.network = network;
			this.network.Events.CreateMap += CreateMap;
			this.network.Events.SetTile += SetTile;

			this.ui = new UserInterface();

			this.map = new Map(0, 10, 10);
			for (int x = 0; x < this.map.SizeX; x++)
			{
				for (int z = 0; z < this.map.SizeZ; z++)
				{
					this.map[x, z] = new Tile()
					{
						X = x,
						Z = z,
						Height = 0.1f * Math.Max(x, z),
						Model = "/Models/tile"
					};
				}
			}
		}

		private void SetTile(object sender, SetTileEventArgs e)
		{
			this.map[e.X, e.Z] = new Tile()
			{
				X = e.X,
				Z = e.Z,
				Height = e.Height,
				Model = e.Model,
				IsInteractive = e.IsInteractive
			};
		}

		private void CreateMap(object sender, CreateMapEventArgs e)
		{
			this.map = new Map(e.ID, e.SizeX, e.SizeZ);
		}

		protected override void OnLoad()
		{
			this.shader = this.Resources.Get<ShaderProgram>("/Shaders/model");

			this.ui.Add(new Widget() { Position = new Vector2(50, 50) });

			this.helper = new Entity()
			{
				Model = this.Resources.Get<Model>("/Models/selector")
			};
		}

		protected override void OnUpdateFrame(float dt)
		{
			this.matViewProjection =
				Matrix4.LookAt(new Vector3(-2, 4.5f, -2), new Vector3(5, 0, 5), Vector3.UnitY) *
				Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(60), 16.0f / 9.0f, 0.1f, 10000.0f);

			var picked = this.Pick(this.Mouse.X, this.Mouse.Y);
			if (picked is Tile)
			{
				var tile = (Tile)picked;
				this.helper.Position = new Vector3(tile.X, tile.Height, tile.Z) + 0.05f * Vector3.UnitY;
			}
		}

		private IBoxCollider Pick(int mouseX, int mouseY)
		{
			return Picking.Pick(
				this.Mouse.X, this.Mouse.Y,
				this.WindowSize.X, this.WindowSize.Y,
				this.matViewProjection,
				this.map.OfType<IBoxCollider>().Concat(this.map.Entities.OfType<IBoxCollider>()));
		}

		protected override void OnRenderFrame(float dt)
		{
			GL.Enable(EnableCap.DepthTest);
			GL.Disable(EnableCap.Blend);
			GL.ClearColor(0.2f, 0.2f, 0.9f, 1.0f);
			GL.ClearDepth(1.0f);
			GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);

			RenderTools.RenderScene(
				this.Resources,
				this.shader,
				this.matViewProjection,
				this.map.OfType<ISceneObject>());
			RenderTools.RenderScene(
				this.Resources,
				this.shader,
				this.matViewProjection,
				this.map.Entities.OfType<ISceneObject>());
			RenderTools.RenderScene(
				this.Resources,
				this.shader,
				this.matViewProjection,
				new[] { this.helper });

			// GL.Enable(EnableCap.Blend);
			GL.Disable(EnableCap.DepthTest);
			// GL.Clear(ClearBufferMask.DepthBufferBit);
			// GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			RenderTools.RenderUI(
				this.Resources,
				this.WindowSize.X, this.WindowSize.Y,
				this.ui);
		}
	}
}