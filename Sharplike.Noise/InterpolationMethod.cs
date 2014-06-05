using System;
using System.Collections.Generic;
using System.Text;

namespace Sharplike.Noise
{
	public enum InterpolationMethod
	{
		None,
		Linear,
		Cosine,
		Cubic
	}
	
	public abstract class Interpolator
	{
		public abstract Double Interpolate(Double a, Double b, Double t);
	}
	
	public class LinearInterpolator : Interpolator
	{
		public override Double Interpolate(Double a, Double b, Double t)
		{
			return (a * (1-t) + b * t);
		}
	}
	
	public class ClosestInterpolator : Interpolator
	{
		public override Double Interpolate(Double a, Double b, Double t)
		{
			return a;
		}
	}
}
