using System;
using OpenTK;

namespace CountryTrash
{
	public sealed class AuthenticationResponseEventArgs : EventArgs
	{
		public AuthenticationResponseEventArgs(bool success, string reason)
		{
			this.Success = success;
			this.Reason = reason;
		}

		/// <summary>
		/// Gets the reason the authentication failed.
		/// </summary>
		public string Reason { get; private set; }
		
		/// <summary>
		/// Gets if the authentication was successful.
		/// </summary>
		public bool Success { get; private set; }
	}

	public sealed class CreateMapEventArgs : EventArgs
	{
		private readonly int id;
		private readonly int x;
		private readonly int z;

		public CreateMapEventArgs(int id, int x, int z)
		{
			this.id = id;
			this.x = x;
			this.z = z;
		}

		public int ID => this.id;
		public int SizeX => this.x;
		public int SizeZ => this.z;
	}

	public sealed class SetTileEventArgs : EventArgs
	{
		private readonly float height;
		private readonly bool interactive;
		private readonly string model;
		private readonly string topping;
		private readonly int x;
		private readonly int z;

		public SetTileEventArgs(int x, int z, float height, string model, string topping, bool interactive)
		{
			this.x = x;
			this.z = z;
			this.height = height;
			this.model = model;
			this.topping = topping;
			this.interactive = interactive;
		}


		public float Height => this.height;
		public bool IsInteractive => this.interactive;
		public string Model => this.model;
		public string Topping => this.topping;
		public int X => this.x;
		public int Z => this.z;
	}

	public sealed class NetworkEntityEventArgs : EventArgs
	{
		private readonly int id;
		private readonly Vector3 pos;
		private readonly float rotation;
		private readonly string visualization;

		public NetworkEntityEventArgs(int id, Vector3 pos, float rotation, string visualization)
		{
			this.id = id;
			this.pos = pos;
			this.rotation = rotation;
			this.visualization = visualization;
		}

		public int ID => this.id;
		public Vector3 Position => this.pos;
		public float Rotation => this.rotation;
		public string Visualization => this.visualization;
	}

	public sealed class UpdateInventoryEventArgs : EventArgs
	{

	}
}