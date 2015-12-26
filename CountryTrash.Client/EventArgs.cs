using System;

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
		private int id;
		private int x;
		private int z;

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
		private float height;
		private bool interactive;
		private string model;
		private int x;
		private int z;

		public SetTileEventArgs(int x, int z, float height, string model, bool interactive)
		{
			this.x = x;
			this.z = z;
			this.height = height;
			this.model = model;
			this.interactive = interactive;
		}


		public float Height => this.height;
		public bool IsInteractive => this.interactive;
		public string Model => this.model;
		public int X => this.x;
		public int Z => this.z;
	}

	public sealed class CreateEntityEventArgs : EventArgs
	{

	}

	public sealed class UpdateEntityEventArgs : EventArgs
	{

	}

	public sealed class DestroyEntityEventArgs : EventArgs
	{

	}

	public sealed class UpdateInventoryEventArgs : EventArgs
	{

	}
}