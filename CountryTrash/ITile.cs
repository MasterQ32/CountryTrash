using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountryTrash
{
	/// <summary>
	/// A static component of a map.
	/// </summary>
	public interface ITile
	{
		/// <summary>
		/// Gets the x position of this tile.
		/// </summary>
		int X { get; }

		/// <summary>
		/// Gets the y position of this tile.
		/// </summary>
		int Z { get; }

		/// <summary>
		/// Gets the offset on the y axis of this tile.
		/// </summary>
		float Height { get; }

		/// <summary>
		/// Gets the name of the model the tile uses.
		/// </summary>
		string Model { get; }

		/// <summary>
		/// Gets the name of the topping model the tile uses.
		/// </summary>
		string Topping { get; }

		/// <summary>
		/// Gets if the tile is interactive and the user can click it.
		/// </summary>
		bool IsInteractive { get; }

		/// <summary>
		/// Gets if the tile is blocked for walking.
		/// </summary>
		bool IsBlocked { get; }
	}
}
