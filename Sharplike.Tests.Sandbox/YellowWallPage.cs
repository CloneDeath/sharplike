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
			for (int x = 0; x < this.size.x; x++) {
				if (x == this.size.x / 2)
				{
					map[x, 0, 0] = new FloorSquare();
					map[x, this.size.y - 1, 0] = new FloorSquare();
				}
				else
				{
					map[x, 0, 0] = new NormalWallSquare();
					map[x, this.size.y - 1, 0] = new NormalWallSquare();
				}
			}
			
			for (int y = 1; y < this.size.y - 1; y++) {
				if (y == this.size.y / 2)
				{
					map[0, y, 0] = new FloorSquare();
					map[this.size.x - 1, y, 0] = new FloorSquare();
				}
				else
				{
					map[0, y, 0] = new NormalWallSquare();
					map[this.size.x - 1, y, 0] = new NormalWallSquare();
				}
			}

			for (int x = 1; x < this.size.x - 1; ++x)
			{
				for (int y = 1; y < this.size.y - 1; ++y)
				{
					map[x, y, 0] = new FloorSquare();
				}
			}
		}

		
	}
}
