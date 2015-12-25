using OpenTK;
using OpenTK.Input;
using System;

namespace HarvestBoon.Graphics
{
	public abstract class GameState
	{
		enum CurrentInternalState { None, Render, Update }

		private float totalUpdateTime = 0.0f;
		private float totalRenderTime = 0.0f;

		private CurrentInternalState internalState = CurrentInternalState.None;

		internal Window window;

		public MouseDevice Mouse => this.window?.Mouse;

		public KeyboardDevice Keyboard => this.window?.Keyboard;

		public Vector2 WindowSize => new Vector2(this.window.Width, this.window.Height);

		internal void Load()
		{
			this.OnLoad();
		}

		internal void UpdateFrame(float deltaTime)
		{
			this.OnUpdateFrame(deltaTime);

			// Increase total time after we rendered the time step.
			this.totalUpdateTime += deltaTime;
		}

		internal void RenderFrame(float deltaTime)
		{
			this.OnRenderFrame(deltaTime);

			// Increase total time after we rendered the time step.
			this.totalRenderTime += deltaTime;
		}

		internal void Unload()
		{
			this.OnUnload();
		}

		public ResourceManager Resources => Game.Current.Resources;

		/// <summary>
		/// User implementation of resource loading.
		/// </summary>
		protected virtual void OnLoad() { }

		/// <summary>
		/// User implementation of state updating.
		/// </summary>
		/// <param name="dt"></param>
		protected virtual void OnUpdateFrame(float dt) { }
		
		/// <summary>
		/// User implementation of state rendering
		/// </summary>
		/// <param name="dt"></param>
		protected virtual void OnRenderFrame(float dt) { }

		/// <summary>
		/// User implementation for resource unloading.
		/// </summary>
		protected virtual void OnUnload() { }

		/// <summary>
		/// Gets the total time that has passed since the creation of the state.
		/// </summary>
		/// <remarks>This property returns different results in the OnUpdateFrame and OnRenderFrame methods.</remarks>
		/// <exception cref="System.InvalidOperationException">This exception is thrown when the property is queried outside of OnUpdateFrame or OnRenderFrame.</exception>
		public float TotalTime
		{
			get
			{
				switch (this.internalState)
				{
					case CurrentInternalState.Update:
					{
						return this.totalUpdateTime;
					}
					case CurrentInternalState.Render:
					{
						return this.totalRenderTime;
					}
					default:
					{
						throw new InvalidOperationException("Cannot get total time outside a update or render step.");
					}
				}
			}
		}
	}
}