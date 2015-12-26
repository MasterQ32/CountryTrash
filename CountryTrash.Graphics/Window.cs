using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CountryTrash.Graphics
{
	internal class Window : GameWindow
	{
		private readonly GameConfiguration config;
		private readonly DebugProc debugProc;

		private GameState currentState = null;
		private GameState nextState = null;
		private float totalUpdateTime = 0.0f;


		public Window(GameConfiguration config)
			: base(
				  config.Width, config.Height,
				  GraphicsMode.Default,
				  config.Title,
				  config.IsFullscreen ? GameWindowFlags.Fullscreen : GameWindowFlags.Default,
				  DisplayDevice.Default,
				  3, 3,
#if DEBUG
				  GraphicsContextFlags.Debug |
#endif
				  GraphicsContextFlags.ForwardCompatible)
		{
			this.debugProc = this.DebugLog;
			this.config = config;
			this.nextState = config.InitialState;
		}

		protected override void OnLoad(EventArgs e)
		{
#if DEBUG
			GL.Enable(EnableCap.DebugOutputSynchronous);
#endif
			GL.DebugMessageCallback(this.debugProc, IntPtr.Zero);
		}

		protected override void OnResize(EventArgs e)
		{
			GL.Viewport(0, 0, this.Width, this.Height);
		}

		private void DebugLog(DebugSource source, DebugType type, int id, DebugSeverity severity, int length, IntPtr message, IntPtr userParam)
		{
			string msg = Marshal.PtrToStringAnsi(message, length);
			Console.WriteLine("[Graphics] {0} {1} {4}", source, type, id, severity, msg);

#if DEBUG
			if(severity == DebugSeverity.DebugSeverityHigh)
			{
				// Force break point on debug build
				System.Diagnostics.Debugger.Break();
			}
#endif
		}

		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			float dt = (float)e.Time;

			this.config.OnPreUpdate(this.totalUpdateTime, dt);

			// State transition
			if (this.currentState != this.nextState)
			{
				this.currentState?.Unload();
				this.currentState = this.nextState;
				this.currentState.window = this;
				this.currentState?.Load();
			}

			// If we don't have any state, quit the game.
			if (this.currentState == null)
			{
				this.Exit();
				return;
			}

			this.currentState?.UpdateFrame(dt);

			this.config.OnPostUpdate(this.totalUpdateTime, dt);

			this.totalUpdateTime += dt;
		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			this.currentState?.RenderFrame((float)e.Time);

			this.SwapBuffers();
		}

		protected override void OnUnload(EventArgs e)
		{
			this.currentState?.Unload();
		}

		public void SetNextState(GameState state)
		{
			this.nextState = state;
		}

		public GameState CurrentState { get { return this.currentState; } }
	}
}
