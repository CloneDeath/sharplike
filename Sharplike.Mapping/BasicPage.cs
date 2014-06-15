using System;

namespace Sharplike.Mapping
{
	[Serializable]
	public class BasicPage : AbstractPage
	{
		public BasicPage (Vector3 pageSize) 
			: base(pageSize)
		{
		}
		public BasicPage(Int32 x, Int32 y, Int32 z)
			: base(x, y, z)
		{
		}
		public override void Build ()
		{
		}

		/// <summary>
		/// Fills all squares between start and end with a copy of the passed in square.
		/// The actual min/max for each component of both vectors are checked, so don't fret about mixing coordinate bounds.
		/// </summary>
		/// <param name="start">The inclusive upper-bound to fill.</param>
		/// <param name="end">The includive lower-bound to fill.</param>
		/// <param name="square">The prototype square to use.</param>
		protected void Fill(Vector3 start, Vector3 end, AbstractSquare square)
		{
			this.Fill(start, end, square, square);
		}

		/// <summary>
		/// Fills all squares between start and end with a copy of the passed in squares.
		/// The outter square will outline the are, while the inner square will be used to fille. If a dimension
		/// is only one unit thick, the inner_square will be used as a meterial instead.
		/// </summary>
		/// <param name="start">The inclusive upper-bound to fill.</param>
		/// <param name="end">The includive lower-bound to fill.</param>
		/// <param name="outter_square"></param>
		/// <param name="inner_square"></param>
		protected void Fill(Vector3 start, Vector3 end, AbstractSquare outter_square, AbstractSquare inner_square)
		{
			Vector3 min_pos = new Vector3(Math.Min(start.X, end.X), Math.Min(start.Y, end.Y), Math.Min(start.Z, end.Z));
			Vector3 max_pos = new Vector3(Math.Max(start.X, end.X), Math.Max(start.Y, end.Y), Math.Max(start.Z, end.Z));
			Vector3 posdiff = max_pos - min_pos;

			for (int x = min_pos.X; x <= max_pos.X; x++) {
				for (int y = min_pos.Y; y <= max_pos.Y; y++) {
					for (int z = min_pos.Z; z <= max_pos.Z; z++) {
						// Basically, considering each dimension that isn't flat, check if you are on the edge.
						// if so, use an edge piece. otherise, use an inner piece.
						if ((posdiff.X > 1 && (x == min_pos.X || x == max_pos.X)) ||
							(posdiff.Y > 1 && (y == min_pos.Y || y == max_pos.Y)) ||
							(posdiff.Z > 1 && (z == min_pos.Z || z == max_pos.Z))) {
								this[x, y, z] = outter_square;
						} else {
							this[x, y, z] = inner_square;
						}
					}
				}
			}
		}

		
	}
}
