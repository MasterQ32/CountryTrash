using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountryTrash
{
	/// <summary>
	/// A map that can be drawn.
	/// </summary>
	public interface IMap
	{
		/// <summary>
		/// Gets the unique identifier of the map.
		/// </summary>
		int ID { get; }

		/// <summary>
		/// Gets the size of the map in x direction.
		/// </summary>
		int SizeX { get; }

		/// <summary>
		/// Gets the size of the map in z direction.
		/// </summary>
		int SizeZ { get; }

		/// <summary>
		/// Gets or sets the a tile of the map.
		/// </summary>
		/// <param name="x">X coordinate of the tile</param>
		/// <param name="z">Z coordinate of the tile</param>
		/// <returns>The tile at (x,z) or null.</returns>
		ITile this[int x, int z] { get; set; }

		/// <summary>
		/// Gets the entities of the map.
		/// </summary>
		IReadOnlyCollection<IEntity> Entities { get; }

		/// <summary>
		/// Adds an entity to the map.
		/// </summary>
		/// <param name="entity">The entity to be added.</param>
		void AddEntity(IEntity entity);

		/// <summary>
		/// Removes the given entity from the map.
		/// </summary>
		/// <param name="entity">The entity to be removed.</param>
		void RemoveEntity(IEntity entity);
	}
}
