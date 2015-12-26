using OpenTK;

namespace CountryTrash.Graphics
{
	/// <summary>
	/// An abstract axis aligned box collider.
	/// </summary>
	public interface IBoxCollider
	{
		/// <summary>
		/// The low coordinate corner of the box.
		/// </summary>
		Vector3 LowerCorner { get; }

		/// <summary>
		/// The high coordinate corner of the box.
		/// </summary>
		Vector3 UpperCorner { get; }
	}
}