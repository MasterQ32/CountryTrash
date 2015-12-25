using OpenTK;

namespace HarvestBoon.Graphics
{
	public interface IUserInterfaceSprite
	{
		Vector2 Position { get; }

		Vector2 Size { get; }

		string Texture { get; }
	}
}