namespace CountryTrash
{
	/// <summary>
	/// A visual object that can be rendered.
	/// </summary>
	public interface IVisualObject
	{
		/// <summary>
		/// Gets the name of the model that is visualized.
		/// </summary>
		string Model { get; }
	}
}