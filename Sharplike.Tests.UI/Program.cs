using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sharplike.Core.Rendering;
using Sharplike.Core;
using System.IO;
using System.Drawing;
using Sharplike.Core.ControlFlow;
using Sharplike.Core.Runtime;

namespace Sharplike.Tests.UI
{
	class Program
	{
		public static Size WindowSize = new Size(120, 40);

		static void Main(string[] args)
		{
			AbstractWindow gameWindow;

			Game.Initialize();	// creates a game object pathed to the
			// executable root

			using (Stream imgStream = File.OpenRead(Game.PathTo("curses_640x300.png"))) {
				// create the glyph palette; specifies how many rows/cols there are
				GlyphPalette glyphPalette = new GlyphPalette(imgStream, 16, 16);

				// the size, in pixels, of the game window
				Size WindowDimensions = new Size(glyphPalette.GlyphDimensions.Width * WindowSize.Width,
												 glyphPalette.GlyphDimensions.Height * WindowSize.Height);
#if !DEBUG
				try
				{
#endif
				Game.SetRenderSystem("OpenTK"); // no other render systems exist
				gameWindow = Game.RenderSystem.CreateWindow(WindowDimensions, glyphPalette);
				Game.SetInputSystem("OpenTK");
				// Game.SetAudioSystem("OpenTK"); // audio still has trouble starting
#if !DEBUG
				}
				catch (NullReferenceException e)
				{
					Console.WriteLine("Exception in startup: " + e.ToString());
					Console.ReadLine();
					return;
				}
#endif
			}

			// load the INI file with proper game configuration...
			Game.InputSystem.LoadConfiguration(Game.PathTo("commands.ini"));

			gameWindow.Clear();
			gameWindow.WindowTitle = "UI Test";

			// Creates the core game state machine, and gives it the MainMenuState as a start state.
			// When we invoke the state machine, that will be the first state that pops up. The state
			// machine is one of Sharplike's most powerful concepts, and will save you a boatload of
			// time during development.
			StateMachine gameState = new StateMachine(new MainMenuState());

			StepwiseGameLoop gameLoop = new StepwiseGameLoop(gameState);
			Game.Run(gameLoop); // and off we go!
		}
	}
}
