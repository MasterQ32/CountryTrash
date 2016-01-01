using OpenTK;

namespace CountryTrash.Graphics
{
	public interface IUserInterfaceSprite
	{
		Vector2 Position { get; }

		Vector2 Size { get; }

		string Texture { get; }

		Font Font { get; }

		string Text { get; }
	}
}