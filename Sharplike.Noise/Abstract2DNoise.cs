﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Sharplike.Noise
{
	/// <summary>
	/// Serves to both provide a blueprint for two-dimensional noise generators
	/// and a jumping-off point for three- and four-dimensional noise generators.
	/// </summary>
	public abstract class Abstract2DNoise : Abstract1DNoise
	{
		protected Int32 width = 1;


		/// <summary>
		/// Defines the width (Y coordinate) of the noise generator's array.
		/// </summary>
		public Int32 Width
		{
			get
			{
				return width;
			}
			set
			{
				if (this.Generated == true)
					throw new InvalidOperationException(AlreadyGenError);
				width = value;
			}
		}


		/// <summary>
		/// Accessor. Look up an integral position in the noise field.
		/// </summary>
		/// <param name="x">The X-coordinate in the noise field.</param>
		/// <param name="y">The Y-coordinate in the noise field.</param>
		/// <returns>The value at the specified coordinate.</returns>
		public Double this[Int32 x, Int32 y]
		{
			get
			{
				if (this.Generated == false)
					throw new InvalidOperationException(MustGenError);
				return this.GetNoiseValue(x, y);
			}
		}
		/// <summary>
		/// Accessor. Look up a non-integral position in the noise field
		/// and, if InterpolationMethod is set to anything other than
		/// InterpolationMethod.None, return the interpolated value (else
		/// return the nearest neighbor).
		/// </summary>
		/// <param name="x">The X-coordinate in the noise field.</param>
		/// <param name="y">The Y-coordinate in the noise field.</param>
		/// <returns>The value at the specified coordinate.</returns>
		public Double this[Double x, Double y]
		{
			get
			{
				if (this.Generated == false)
					throw new InvalidOperationException(MustGenError);
				return this.GetNoiseValue(x, y);
			}
		}


		/// <summary>
		/// Accessor. Look up an integral position in the noise field.
		/// </summary>
		/// <param name="x">The X-coordinate in the noise field.</param>
		/// <param name="y">The Y-coordinate in the noise field.</param>
		/// <returns>The value at the specified coordinate.</returns>
		public abstract Double GetNoiseValue(Int32 x, Int32 y);
		/// <summary>
		/// Accessor. Look up a non-integral position in the noise field
		/// and, if InterpolationMethod is set to anything other than
		/// InterpolationMethod.None, return the interpolated value (else
		/// return the nearest neighbor).
		/// </summary>
		/// <param name="x">The X-coordinate in the noise field.</param>
		/// <param name="y">The Y-coordinate in the noise field.</param>
		/// <returns>The value at the specified coordinate.</returns>
		public abstract Double GetNoiseValue(Double x, Double y);
	}
}
