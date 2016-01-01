using CountryTrash.Graphics;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using System;
using System.Collections.Generic;
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

		readonly List<Task> tasks = new List<Task>();

		public MapDisplayState(Network network)
		{
			this.network = network;
			this.network.Events.CreateMap += CreateMap;
			this.network.Events.SetTile += SetTile;
			this.network.Events.CreateEntity += CreateEntity;
			this.network.Events.UpdateEntity += UpdateEntity;
			this.network.Events.EnqueueTask += EnqueueTask;
			this.network.Events.DequeueTask += DequeueTask;

			this.ui = new UserInterface();
		}

		private void DequeueTask(object sender, TaskEventArgs e)
		{
			this.tasks.RemoveAll(t => (t.ID == e.ID));
		}

		private void EnqueueTask(object sender, TaskEventArgs e)
		{
			this.tasks.Add(new Task()
			{
				ID = e.ID,
				Icon = e.Icon,
				Title = e.Title
			});
		}

		private void CreateEntity(object sender, NetworkEntityEventArgs e)
		{
			var entity = new Entity()
			{
				ID = e.ID,
				Position = e.Position,
				Rotation = e.Rotation,
				Visualization = e.Visualization
			};
			this.map.AddEntity(entity);
		}

		private void UpdateEntity(object sender, NetworkEntityEventArgs e)
		{
			var ent = this.map.Entities.FirstOrDefault(_ => (_.ID == e.ID));
			if (ent == null)
				return;
			ent.Position = e.Position;
			ent.Rotation = e.Rotation;
			ent.Visualization = e.Visualization;
		}

		private void SetTile(object sender, SetTileEventArgs e)
		{
			this.map[e.X, e.Z] = new Tile()
			{
				X = e.X,
				Z = e.Z,
				Height = e.Height,
				Model = e.Model,
				Topping = e.Topping,
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
				Visualization = "/Models/selector"
			};

			this.Mouse.ButtonDown += Mouse_ButtonDown;
		}

		private void Mouse_ButtonDown(object sender, MouseButtonEventArgs e)
		{
			switch (e.Button)
			{
				case MouseButton.Left:
				{
					var pick = this.Pick(e.X, e.Y);
					var tile = pick as Tile;
					var entity = pick as Entity;
					if (tile != null)
						this.network.Commands.InvokeTileAction(tile);
					else if (entity != null)
						this.network.Commands.InvokeEntityAction(entity);
					break;
				}
			}
		}

		protected override void OnUpdateFrame(float dt)
		{
			this.matViewProjection =
				Matrix4.LookAt(new Vector3(-2, 4.5f, -2), new Vector3(2.5f, 0, 2.5f), Vector3.UnitY) *
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
			if (this.map == null)
				return null;
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

			if (this.map != null)
			{
				RenderTools.RenderScene(
					this.Resources,
					this.shader,
					this.matViewProjection,
					this.map.OfType<ISceneNode>().SelectMany(n => n.Children));
				RenderTools.RenderScene(
					this.Resources,
					this.shader,
					this.matViewProjection,
					this.map.Entities.OfType<ISceneObject>());
			}
			RenderTools.RenderScene(
				this.Resources,
				this.shader,
				this.matViewProjection,
				new[] { this.helper });

			GL.Enable(EnableCap.Blend);
			GL.Disable(EnableCap.DepthTest);
			GL.Clear(ClearBufferMask.DepthBufferBit);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			RenderTools.RenderUI(
				this.Resources,
				this.WindowSize.X, this.WindowSize.Y,
				this.ui);

			var tasklist = this.tasks.SelectMany((t, i) => new[] {
				new Widget()
				{
					Position = new Vector2(16, 16 + 32 * i),
					Size = new Vector2(128, 32),
					Texture = "/Textures/task"
				},
				new Widget()
				{
					Position = new Vector2(16, 16 + 32 * i),
					Size = new Vector2(32, 32),
					Texture = t.Icon ?? "/Textures/Icons/unknown"
				},
				new Widget()
				{
					Position = new Vector2(16 + 32 + 4, 16 + 32 * i + 2),
					Text = t.Title,
					Font = Resources.Get<Font>("/Fonts/steelfish")
				}
			});
			RenderTools.RenderUI(
				this.Resources,
				this.WindowSize.X, this.WindowSize.Y,
				tasklist);
		}
	}
}