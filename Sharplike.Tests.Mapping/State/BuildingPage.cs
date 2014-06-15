using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sharplike.Mapping;
using Sharplike.Mapping.Squares;

namespace Sharplike.Tests.Mapping.State
{
	class BuildingPage : BasicPage
	{
		public BuildingPage(Vector3 pageSize)
			: base(pageSize)
		{
		}

		public override void Build()
		{
			this.Fill(new Vector3(0, 0, 0), new Vector3(this.Size.X - 1, this.Size.Y - 1, this.Size.Z - 1), new WallSquare(), new FloorSquare());
		}
	}
}
