using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Sharplike.Core;
using Sharplike.Core.Rendering;

namespace Sharplike.Frontend.Rendering
{
	/// <summary>
	/// A display window that uses OpenGL (via OpenTK) to render the game display.
	/// </summary>
	/// <remarks>
	/// Developed primarily by Alex Karantza (karantza@notsorandom.com), with some
	/// adjustments to fit into the application. Copyright transferred with permission
	/// to Ed Ropple and code included under the Sharplike License (CPAL).
	/// </remarks>
	public class TKWindow : AbstractWindow
	{
		private TKForm form;
		private int paletteId;

		/// <summary>
		/// The inner GLControl responsible for rendering.
		/// </summary>
		public readonly TKGLControl Control = new TKGLControl();

		public TKWindow(Size displayDimensions, GlyphPalette palette, Control context)
			: base(displayDimensions, palette)
		{
			if (context == null)
			{
				form = new TKForm();
				form.ClientSize = displayDimensions;
				form.FormClosing += new FormClosingEventHandler(Form_FormClosing);
				form.Show();

				context = form;
			}

			context.SuspendLayout();
			Control.Dock = DockStyle.Fill;
			Control.BackColor = Color.Blue;
			Control.VSync = false;
			Control.Resize += new EventHandler(Control_Resize);
			Control.Paint += new PaintEventHandler(Control_Paint);

			Control.Location = new Point(0, 0);
			Control.Size = context.ClientSize;

			context.Controls.Add(Control);
			context.ResumeLayout(false);

			paletteId = GL.GenTexture();
			Bitmap bmp = palette.SourceBitmap;
			GL.BindTexture(TextureTarget.Texture2D, paletteId);
			BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp_data.Width, bmp_data.Height, 0,
				OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);

			bmp.UnlockBits(bmp_data);

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

			base.Resize += new EmptyDelegate(TKWindow_Resize);
			this.WindowSize = Control.Size;

			GL.ClearColor(0.1f, 0.2f, 0.5f, 0.0f);
			GL.Disable(EnableCap.DepthTest);
			GL.Disable(EnableCap.CullFace);
			GL.Enable(EnableCap.Texture2D);

			GL.BindTexture(TextureTarget.Texture2D, paletteId);
			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
		}

		void TKWindow_Resize()
		{
			GL.Viewport(0, 0, Control.ClientSize.Width, Control.ClientSize.Height);
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.Ortho(0, WindowSize.Width, WindowSize.Height, 0, 0, 1);
		}

		void Control_Paint(object sender, PaintEventArgs e)
		{
			Control.MakeCurrent();
			DrawWindow();
			Control.SwapBuffers();
		}

		void Control_Resize(object sender, EventArgs e)
		{
			this.WindowSize = Control.ClientSize;
		}

		void Form_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (Game.InputSystem.HasWindowEvent("OnClosing"))
			{
				e.Cancel = true;
			}
			Game.InputSystem.WindowCommand("OnClosing");
		}

		private void DrawWindow()
		{
			GL.MatrixMode(MatrixMode.Modelview);

			GL.LoadIdentity();

			int w = this.GlyphPalette.GlyphDimensions.Width;
			int h = this.GlyphPalette.GlyphDimensions.Height;

			List<KeyValuePair<Point, IGlyphProvider>> gproviders = new List<KeyValuePair<Point, IGlyphProvider>>();

			for (int x = 0; x < this.Size.Width; x++)
			{
				for (int y = 0; y < this.Size.Height; y++)
				{
					DisplayTile tile = this.tiles[x, y];
					foreach (RegionTile t in tile.RegionTiles) {
						foreach (IGlyphProvider p in t.GlyphProviders) {
							gproviders.Add(new KeyValuePair<Point,IGlyphProvider>(new Point(x, y), p));
						}
					}
				}
			}

			/* Draw Glyphs */
			GL.Begin(PrimitiveType.Quads);
			foreach (KeyValuePair<Point, IGlyphProvider> kvp in gproviders){
				IGlyphProvider glyphpro = kvp.Value;
				Point loc = kvp.Key;

				int screen_x = loc.X *w;
				int screen_y = loc.Y *h;
				{
					int uvrow = (int)Sharplike.UI.GlyphDefault.Block / GlyphPalette.ColumnCount;
					int uvcol = (int)Sharplike.UI.GlyphDefault.Block % GlyphPalette.ColumnCount;

					double u = (double)uvcol / (double)GlyphPalette.ColumnCount;
					double v = (double)uvrow / (double)GlyphPalette.RowCount;
					double du = 1.0 / (double)GlyphPalette.ColumnCount;
					double dv = 1.0 / (double)GlyphPalette.RowCount;

					GL.Color4(glyphpro.BackgroundColor);
					GL.TexCoord2(u, v); GL.Vertex2(screen_x, screen_y);
					GL.TexCoord2(du + u, v); GL.Vertex2(screen_x + w, screen_y);
					GL.TexCoord2(du + u, dv + v); GL.Vertex2(screen_x + w, h + screen_y);
					GL.TexCoord2(u, dv + v); GL.Vertex2(screen_x, h + screen_y);
				}

				foreach (Glyph glyph in glyphpro.Glyphs) {
					int uvrow = glyph.Index / GlyphPalette.ColumnCount;
					int uvcol = glyph.Index % GlyphPalette.ColumnCount;

					double u = (double)uvcol / (double)GlyphPalette.ColumnCount;
					double v = (double)uvrow / (double)GlyphPalette.RowCount;
					double du = 1.0 / (double)GlyphPalette.ColumnCount;
					double dv = 1.0 / (double)GlyphPalette.RowCount;

					GL.Color4(glyph.Color);
					GL.TexCoord2(u, v); GL.Vertex2(screen_x, screen_y);
					GL.TexCoord2(du + u, v); GL.Vertex2(screen_x + w, screen_y);
					GL.TexCoord2(du + u, dv + v); GL.Vertex2(screen_x + w, h + screen_y);
					GL.TexCoord2(u, dv + v); GL.Vertex2(screen_x, h + screen_y);
				}
			}
			GL.End();
		}

		/// <summary>
		/// Changes the form text to the WindowTitle property.
		/// </summary>
		protected override void WindowTitleChange()
		{
			form.Text = this.WindowTitle;
		}

		/// <summary>
		/// Updates the GLControl, and any subregions.
		/// </summary>
		public override void Update()
		{
			base.Update();
			Control.Invalidate();
		}

		internal void FocusWindow()
		{
			Control.Focus();
			form.WindowState = FormWindowState.Normal;
			form.Activate();
		}

		public override void Dispose()
		{
			Control.Dispose();
			base.Dispose();
		}
	}
}
