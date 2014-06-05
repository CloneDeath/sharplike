using System;
using System.Windows.Forms;

namespace Sharplike.Frontend.Rendering
{
	public class TKGLControl : OpenTK.GLControl
	{
		protected override bool IsInputKey(Keys keyData)
		{
			return true;
		}
	}
}
