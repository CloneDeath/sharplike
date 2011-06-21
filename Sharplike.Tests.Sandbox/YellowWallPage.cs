///////////////////////////////////////////////////////////////////////////////
/// Sharplike, The Open Roguelike Library (C) 2010 Ed Ropple.               ///
///                                                                         ///
/// This code is part of the Sharplike Roguelike library, and is licensed   ///
/// under the Common Public Attribution License (CPAL), version 1.0. Use of ///
/// this code is purusant to this license. The CPAL grants you certain      ///
/// permissions and requirements and should be read carefully before using  ///
/// this library.                                                           ///
///                                                                         ///
/// A copy of this license can be found in the Sharplike root directory,    ///
/// and must be included with all projects released using this library.     ///
///////////////////////////////////////////////////////////////////////////////

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
					map[x, 0, 0] = new FloorSquare(x, 0, 0);
					map[x, this.size.y - 1, 0] = new FloorSquare(x, this.size.y - 1, 0);
				}
				else
				{
					map[x, 0, 0] = new NormalWallSquare(x, 0, 0);
					map[x, this.size.y - 1, 0] = new NormalWallSquare(x, this.size.y - 1, 0);
				}
			}
			
			for (int y = 1; y < this.size.y - 1; y++) {
				if (y == this.size.y / 2)
				{
					map[0, y, 0] = new FloorSquare(0, y, 0);
					map[this.size.x - 1, y, 0] = new FloorSquare(this.size.y - 1, y, 0);
				}
				else
				{
					map[0, y, 0] = new NormalWallSquare(0, y, 0);
					map[this.size.x - 1, y, 0] = new NormalWallSquare(this.size.y - 1, y, 0);
				}
			}

            for (int x = 1; x < this.size.x - 1; ++x)
            {
                for (int y = 1; y < this.size.y - 1; ++y)
                {
                    map[x, y, 0] = new FloorSquare(x, y, 0);
                }
            }
		}

		
	}
}
