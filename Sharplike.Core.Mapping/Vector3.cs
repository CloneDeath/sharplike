using System;
using System.Drawing;

namespace Sharplike.Mapping
{
	[Serializable]
	public struct Vector3 : IEquatable<Vector3>
	{
		public static readonly Vector3 Zero = new Vector3(0, 0, 0);

		public static readonly Vector3 North = new Vector3(0, -1, 0);
		public static readonly Vector3 South = new Vector3(0, 1, 0);
		public static readonly Vector3 East = new Vector3(1, 0, 0);
		public static readonly Vector3 West = new Vector3(-1, 0, 0);
		public static readonly Vector3 Up = new Vector3(0, 0, 1);
		public static readonly Vector3 Down = new Vector3(0, 0, -1);

		public Vector3(int x, int y, int z)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
		}
		public Vector3(Point p)
		{
			this.X = p.X;
			this.Y = p.Y;
			this.Z = 0;
		}
		
		public override string ToString ()
		{
			return string.Format("(" + X + "," + Y + "," + Z + ")");
		}

		
		public static void Divide(Vector3 a, Vector3 b, out Vector3 q, out Vector3 r)
		{
			int x = (int)Math.Floor((double)a.X / (double)b.X);
			int y = (int)Math.Floor((double)a.Y / (double)b.Y);
			int z = (int)Math.Floor((double)a.Z / (double)b.Z);
			int xr = a.X - b.X * x;
			int yr = a.Y - b.Y * y;
			int zr = a.Z - b.Z * z;
			
			q = new Vector3(x,y,z);
			r = new Vector3(xr,yr,zr);
		}

		public static Vector3 Add(Vector3 a, Vector3 b)
		{
			return a + b;
		}
		public static Vector3 Add(Vector3 a, Point b)
		{
			return a + b;
		}

		public static Vector3 operator +(Vector3 a, Vector3 b)
		{
			return new Vector3(a.X + b.X,
							   a.Y + b.Y,
							   a.Z + b.Z);
		}
		public static Vector3 operator +(Vector3 a, Point b)
		{
			return new Vector3(a.X + b.X,
								a.Y + b.Y,
								a.Z);
		}

		public static Vector3 operator -(Vector3 a, Vector3 b)
		{
			return new Vector3(a.X - b.X,
							   a.Y - b.Y,
							   a.Z - b.Z);
		}

		public static Vector3 operator /(Vector3 a, Vector3 b)
		{
			return new Vector3(a.X / b.X,
								a.Y / b.Y,
								a.Z / b.Z);
		}

		public static Vector3 operator /(Vector3 a, int b)
		{
			return new Vector3(a.X / b,
								a.Y / b,
								a.Z / b);
		}

		public static Vector3 operator *(Vector3 a, int b)
		{
			return new Vector3(a.X * b,
								a.Y * b,
								a.Z * b);
		}

		public double SquaredDistanceTo(Vector3 target)
		{
			target = target - this;
			return (target.X * target.X) +
				(target.Y * target.Y) +
				(target.Z * target.Z);
		}
		
		
		public readonly int X, Y, Z;

		public override Boolean Equals(object obj)
		{
			if (obj is Vector3)
			{
				Vector3 v = (Vector3)obj;

				return this.X == v.X && this.Y == v.Y && this.Z == v.Z;
			}
			else
			{
				return false;
			}
		}

		bool IEquatable<Vector3>.Equals(Vector3 other)
		{
			return (this.X == other.X && this.Y == other.Y && this.Z == other.Z);
		}

		public override int GetHashCode()
		{
			return (X << 22) + (Y << 11) + (Z << 0);
		}

		public bool IntersectsWith(Rectangle r)
		{
			return this.X >= r.Left && this.X < r.Right && this.Y >= r.Top && this.Y < r.Bottom;
		}

		public bool IntersectsWithEllipse(Vector3 location, Vector3 range)
		{
			Vector3 test = this - location;
			return
				((double)(test.X * test.X) / (range.X * range.X)) +
				((double)(test.Y * test.Y) / (range.Y * range.Y)) +
				((double)(test.Z * test.Z) / (range.Z * range.Z))
				<= 1;
		}

		public bool IntersectsWithExtents(Vector3 location, Vector3 range)
		{
			Vector3 test = this - location;
			return test.X < range.X && test.Y < range.Y && test.Z < range.Z && 
				test.X >= 0 && test.Y >= 0 && test.Z >= 0;
		}
	}
}
