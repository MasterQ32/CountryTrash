namespace CountryTrash
{
	/// <summary>
	/// A player task 
	/// </summary>
	public interface ITask
	{
		/// <summary>
		/// Gets the title of the task.
		/// </summary>
		string Title { get; }

		/// <summary>
		/// Gets the icon of the task
		/// </summary>
		string Icon { get; }

		/// <summary>
		/// Gets the id of the task
		/// </summary>
		int ID { get; }
	}
}