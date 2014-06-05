using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Sharplike.Core.Rendering;

namespace Sharplike.Frontend.Rendering
{
    public class TKRenderSystem : AbstractRenderSystem
    {
        public override AbstractWindow CreateWindow(Size displayDimensions, GlyphPalette palette, Object context)
        {
            if (win == null)
                win = new TKWindow(displayDimensions, palette, context as Control);
            return win;
        }

        public override AbstractWindow Window
        {
            get { return win; }
        }
        private TKWindow win = null;

        public override void Dispose()
        {
            if (win != null)
                win.Dispose();
        }

        public override void Process()
        {
            win.Update();
        }
    }
}
