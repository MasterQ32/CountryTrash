namespace CountryTrash
{
	/// <summary>
	/// An object that has a periodic repeating event.
	/// </summary>
	public interface ITicking
	{
		/// <summary>
		/// The repeating event.
		/// </summary>
		void Tick();
	}
}