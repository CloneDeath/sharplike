using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Sharplike.Core;
using Sharplike.Core.Rendering;
using Sharplike.Core.Audio;
using Sharplike.Core.Input;
using Sharplike.Core.Runtime;
using System.Reflection;
using System.IO;

namespace Sharplike.Tests.TKTest
{
	static class Program
	{
		static Random RNG = new Random();
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{

			Game.Initialize();

			AbstractWindow gwin;
			Assembly ea = Assembly.GetExecutingAssembly();


			using (Stream imgstream = ea.GetManifestResourceStream("Sharplike.Tests.TKTest.curses_640x300.png"))
			{
				GlyphPalette pal = new GlyphPalette(imgstream, 16, 16);

				Int32 width = 80 * pal.GlyphDimensions.Width;
				Int32 height = 25 * pal.GlyphDimensions.Height;

				try
				{
					//game.SetAudioSystem("OpenTK");
					Game.SetRenderSystem("OpenTK");
					gwin = Game.RenderSystem.CreateWindow(new Size(width, height), pal);
					Game.SetInputSystem("OpenTK");
				}
				catch (System.NullReferenceException e)
				{
					Console.WriteLine("Error when loading plugin: " + e.Message + "\n" + e.Source);
					return;
				}

			}
			
			//Game.Scripting.Run(Game.PathTo("Test.py"));
			//game.Scripting.Run(game.PathTo("Test.rb"));

			Game.InputSystem.LoadConfiguration(Game.PathTo("commands.ini"));
			Game.InputSystem.SaveConfiguration(Game.PathTo("commands.out.ini"));

			
			gwin.Clear();

			//ac.Play();
			Game.GameProcessing += new EventHandler<EventArgs>(game_GameProcessing);
			Game.InputSystem.Command.CommandTriggered += new EventHandler<CommandEventArgs>(Command_CommandTriggered);
			StepwiseGameLoop loop = new StepwiseGameLoop(RunGame);
			Game.Run(loop);

			Game.Terminate();
		}

		static void Command_CommandTriggered(object sender, CommandEventArgs e)
		{
			Console.WriteLine(e.CommandData.Command);

			if (e.CommandData.Command == "quit") {
				Game.Terminate();
			}
		}

		static void game_GameProcessing(object sender, EventArgs e)
		{
			AbstractWindow gwin = Game.RenderSystem.Window;
			gwin.Clear();
			for (Int32 i = 0; i < 256; i++)
			{
				Int32 x = i % 16;
				Int32 y = i / 16;

				Int32 r = Program.RNG.Next(0, 255);
				Int32 g = Program.RNG.Next(0, 255);
				Int32 b = Program.RNG.Next(0, 255);

				//gwin[x, y].AddGlyph(i, Color.White, Color.FromArgb(r, g, b));
			}
		}

		static void RunGame(StepwiseGameLoop loop)
		{
			
		}
	}
}
