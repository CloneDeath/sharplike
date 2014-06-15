using System;
using System.Collections.Generic;
using System.Text;

namespace Sharplike.Mapping
{
	/// <summary>
	/// Direction enumeration, for traversing a map.
	/// See <see cref="DirectionUtils"/> for useful utilities.
	/// </summary>
	public enum Direction
	{
		/// <summary>
		/// NorthWest.
		/// </summary>
		NorthWest,

		/// <summary>
		/// North.
		/// </summary>
		North,

		/// <summary>
		/// NorthEast.
		/// </summary>
		NorthEast,

		/// <summary>
		/// East.
		/// </summary>
		East,

		/// <summary>
		/// SouthEast
		/// </summary>
		SouthEast,

		/// <summary>
		/// South
		/// </summary>
		South,

		/// <summary>
		/// SouthWest
		/// </summary>
		SouthWest,

		/// <summary>
		/// West
		/// </summary>
		West,

		/// <summary>
		/// Upwards - towards the sky
		/// </summary>
		Up,

		/// <summary>
		/// Downwards - towards the earth
		/// </summary>
		Down,

		/// <summary>
		/// Current position.
		/// </summary>
		Here
	}

	public static class DirectionUtils
	{
		/// <summary>
		/// Returns the oposite direction. If Direction.Here is passed in, it will be echoes out.
		/// </summary>
		/// <param name="from">The direction to reverse.</param>
		/// <returns>The oposite direction.</returns>
		public static Direction OppositeDirection(Direction from)
		{
			switch (from)
			{
				case Direction.NorthWest:
					return Direction.SouthEast;
				case Direction.North:
					return Direction.South;
				case Direction.NorthEast:
					return Direction.SouthWest;

				case Direction.East:
					return Direction.West;
				case Direction.West:
					return Direction.East;

				case Direction.SouthEast:
					return Direction.NorthWest;
				case Direction.South:
					return Direction.North;
				case Direction.SouthWest:
					return Direction.NorthEast;

				case Direction.Up:
					return Direction.Down;
				case Direction.Down:
					return Direction.Up;
			}
			return Direction.Here;
		}

		/// <summary>
		/// Returns the direction as a vector. For example, Direction.North -> new Vector(0, -1, 0)
		/// </summary>
		/// <param name="of">The direction to translate.</param>
		/// <returns>A vector3 representation of the vector</returns>
		public static Vector3 DirectionVector(Direction of)
		{
			switch (of) {
				case Direction.North:
					return new Vector3(0, -1, 0);

				case Direction.South:
					return new Vector3(0, 1, 0);

				case Direction.East:
					return new Vector3(1, 0, 0);

				case Direction.West:
					return new Vector3(-1, 0, 0);

				case Direction.NorthWest:
					return new Vector3(-1, -1, 0);

				case Direction.SouthWest:
					return new Vector3(-1, 1, 0);

				case Direction.NorthEast:
					return new Vector3(1, -1, 0);

				case Direction.SouthEast:
					return new Vector3(1, 1, 0);

				case Direction.Up:
					return new Vector3(0, 0, -1);

				case Direction.Down:
					return new Vector3(0, 0, 1);
			}

			throw new NotImplementedException("Direction '" + of + "' is not a valid direction");
		}
	}
}
