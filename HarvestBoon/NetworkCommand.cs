namespace HarvestBoon
{
	/// <summary>
	/// A command sent from the client to the server.
	/// </summary>
	public enum ClientToServerNetworkCommand : byte
	{
		/// <summary>
		/// Default command
		/// </summary>
		None,

		/// <summary>
		/// Authenticate the client.
		/// </summary>
		Authenticate,

		/// <summary>
		/// Tells the server the player wants to walk to a specific tile
		/// position on the current map.
		/// </summary>
		WalkTo,

		/// <summary>
		/// Tells the server the player wants to invoke an action on a
		/// specific tile on the current map.
		/// </summary>
		InvokeTileAction,

		/// <summary>
		/// Tells the server the player wants to invoke an action on a
		/// specific entity on the current map.
		/// </summary>
		InvokeEntityAction,

		/// <summary>
		/// The player wants to move an item or item stack from one inventory
		/// slot to another.
		/// </summary>
		MoveInventoryItem,
	}

	/// <summary>
	/// A command sent from the server to the client
	/// </summary>
	public enum ServerToClientNetworkCommand : byte
	{
		/// <summary>
		/// Default command
		/// </summary>
		None,

		/// <summary>
		/// The client has requested authentication and is now
		/// either accepted or rejected.
		/// </summary>
		AuthenticationResponse,

		/// <summary>
		/// Creates a new map on the client. If no map exists,
		/// the client will discard the message and log a warning.
		/// 
		/// The client should now show the newly
		/// created map and dispose the previous one.
		/// </summary>
		CreateMap,

		/// <summary>
		/// Sets a tile on a current map. If no map exists,
		/// the client will discard the message and log a warning.
		/// </summary>
		SetTile,

		/// <summary>
		/// Creates an entity on the current map.
		/// </summary>
		CreateEntity,

		/// <summary>
		/// Updates an existing entity. If the entity does not exist,
		/// the client will discard the message.
		/// </summary>
		UpdateEntity,

		/// <summary>
		/// Destroys an entity on the current map. If the entity does
		/// not exist, the client will discard the message.
		/// </summary>
		DestroyEntity,

		/// <summary>
		/// Updates an inventory slot.
		/// </summary>
		UpdateInventory,
	}
}