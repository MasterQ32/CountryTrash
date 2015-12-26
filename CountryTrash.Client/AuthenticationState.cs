using System;
using CountryTrash.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace CountryTrash
{
	internal class AuthenticationState : GameState
	{
		private readonly Network network;

		public AuthenticationState(Network network)
		{
			this.network = network;
		}

		protected override void OnLoad()
		{
			this.network.Events.AuthenticationResponse += this.AuthenticationResponse;

			// TODO: Replace with UI input
			this.network.Commands.Authenticate("user", "password");
		}

		private void AuthenticationResponse(object sender, AuthenticationResponseEventArgs e)
		{
			if(e.Success)
			{
				Game.Current.SetNextState(new MapDisplayState(this.network));
				return;
			}
		}

		protected override void OnRenderFrame(float dt)
		{
			GL.ClearColor(0.0f, 0.5f, 0.0f, 1.0f);
			GL.Clear(ClearBufferMask.ColorBufferBit);
		}

		protected override void OnUnload()
		{
			this.network.Events.AuthenticationResponse -= this.AuthenticationResponse;
		}
	}
}