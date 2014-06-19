using System;
using System.Windows.Forms;
using OpenTK;

namespace Sharplike.OpenTK.Rendering
{
	public class TKGLControl : GLControl
	{
		protected override bool IsInputKey(Keys keyData)
		{
			return true;
		}
	}
}
