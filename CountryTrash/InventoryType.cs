using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountryTrash
{
	public enum InventoryType : byte
	{
		None,

		/// <summary>
		/// The inventory displayed at the bottom of the screen.
		/// </summary>
		Toolbar,

		/// <summary>
		/// The inventory the player has.
		/// </summary>
		PlayerInventory,
	}
}
