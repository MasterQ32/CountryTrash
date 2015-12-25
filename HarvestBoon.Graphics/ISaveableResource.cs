using System.IO;

namespace HarvestBoon.Graphics
{
	/// <summary>
	/// A resource that can be saved to a stream.
	/// </summary>
	public  interface ISaveableResource
	{
		void Save(Stream stream);
	}
}