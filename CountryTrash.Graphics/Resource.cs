namespace CountryTrash.Graphics
{
	/// <summary>
	/// A resource that can be unloaded.
	/// </summary>
	public abstract class Resource
	{
		/// <summary>
		/// Frees all memory allocated by the resource.
		/// </summary>
		internal protected abstract void Unload();
	}
}