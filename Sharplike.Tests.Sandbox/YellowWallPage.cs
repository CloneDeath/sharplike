using System;
using Sharplike.Mapping;
using System.Drawing;

namespace Sharplike.Tests.Sandbox
{
	[Serializable]
	public class YellowWallPage : AbstractPage
	{

		public YellowWallPage (Vector3 pageSize) : base(pageSize)
		{
			Build();
		}
		
		public override void Build()
		{
			for (int x = 0; x < this.Size.X; x++) {
				if (x == this.Size.X / 2)
				{
					this[x, 0, 0] = new FloorSquare();
					this[x, this.Size.Y - 1, 0] = new FloorSquare();
				}
				else
				{
					this[x, 0, 0] = new NormalWallSquare();
					this[x, this.Size.Y - 1, 0] = new NormalWallSquare();
				}
			}

			for (int y = 1; y < this.Size.Y - 1; y++) {
				if (y == this.Size.Y / 2)
				{
					this[0, y, 0] = new FloorSquare();
					this[this.Size.X - 1, y, 0] = new FloorSquare();
				}
				else
				{
					this[0, y, 0] = new NormalWallSquare();
					this[this.Size.X - 1, y, 0] = new NormalWallSquare();
				}
			}

			for (int x = 1; x < this.Size.X - 1; ++x)
			{
				for (int y = 1; y < this.Size.Y - 1; ++y)
				{
					this[x, y, 0] = new FloorSquare();
				}
			}
		}

		
	}
}
