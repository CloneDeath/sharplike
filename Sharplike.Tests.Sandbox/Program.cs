using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Sharplike.Core;
using Sharplike.Core.Rendering;
using Sharplike.Core.Audio;
using Sharplike.Core.Runtime;
using Sharplike.Core.Input;
using Sharplike.Mapping;
using System.Reflection;
using System.IO;

namespace Sharplike.Tests.Sandbox
{
	class Program
	{
		static Vector3 cameraVector = new Vector3(0, 0, 0);
		static Boolean cameraMoved = true;
		static MapStack map;
		static ZColdCachingAlgorithm cache;


		//[STAThread]
		static void Main()
		{

			Game.Initialize(".");

			AbstractWindow gwin;

			String glyphPath = Game.PathTo("curses_640x300.png");
			using (Stream imgstream = File.OpenRead(glyphPath))
			{
				GlyphPalette pal = new GlyphPalette(imgstream, 16, 16);

				Int32 width = 80 * pal.GlyphDimensions.Width;
				Int32 height = 25 * pal.GlyphDimensions.Height;

				try
				{
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

			Game.InputSystem.LoadConfiguration(Game.PathTo("commands.ini"));

			gwin.Clear();

			map = new MapStack(Game.RenderSystem.Window.WindowSize, 20, 15, "SandboxMap", gwin);
			map.AddPage(new YellowWallPage(map.PageSize), new Vector3(0, 0, 0));
			map.AddPage(new YellowWallPage(map.PageSize), new Vector3(1, 0, 0));
			map.AddPage(new YellowWallPage(map.PageSize), new Vector3(0, 1, 0));
			map.AddPage(new YellowWallPage(map.PageSize), new Vector3(1, 1, 0));
			map.AddPage(new YellowWallPage(map.PageSize), new Vector3(-1, 0, 0));
			map.AddPage(new YellowWallPage(map.PageSize), new Vector3(1, 1, 1));
			map.AddPage(new YellowWallPage(map.PageSize), new Vector3(-1, 0, 1));

			cache = new ZColdCachingAlgorithm(map);

			ent = new WanderingEntity();
			ent.Location = new Vector3(2, 2, 0);
			ent.Map = map;

			Sharplike.UI.Controls.Label l = new UI.Controls.Label(map);
			l.Size = new Size(50, 1);
			l.Location = new Point(0, 0);
			l.Text = "Label on the map.";

			Sharplike.UI.Controls.Window win = new UI.Controls.Window(map);
			win.Size = new Size(20, 10);
			win.Location = new Point(5, 5);
			win.Title = "Dialog Window";
			win.BackgroundColor = Color.FromArgb(100, 0, 0, 200);

			Game.OnGameInitialization += new EventHandler<EventArgs>(game_OnGameInitialization);
			Game.GameProcessing += new EventHandler<EventArgs>(game_GameProcessing);
			StepwiseGameLoop loop = new StepwiseGameLoop(RunGame);
			Game.Run(loop);

			Game.Terminate();
		}

		static void game_OnGameInitialization(object sender, EventArgs e)
		{
			map.ViewFrom(cameraVector, true);
		}

		static void game_GameProcessing(object sender, EventArgs e)
		{
			if (cameraMoved == true)
			{
				map.ViewFrom(cameraVector);
				cameraMoved = false;
			}
		}

		static WanderingEntity ent;

		static Boolean RunGame(StepwiseGameLoop loop)
		{
			CommandData cmd = null;
			do
			{
				map.BroadcastMessage(cameraVector, new Vector3(5, 5, 1), "Ping");

				cache.ActiveLevel = cameraVector.z;
				cache.AssessCache();

				cmd = loop.WaitForInput();
				Console.WriteLine(cmd.ToString());
				//if (ent != null)
				//	ent.Wander();
				switch (cmd.Command)
				{
					case "move_left":
						cameraVector = new Vector3(cameraVector.x - 1,
											cameraVector.y, cameraVector.z);
						cameraMoved = true;
						break;
					case "move_right":
						cameraVector = new Vector3(cameraVector.x + 1,
											cameraVector.y, cameraVector.z);
						cameraMoved = true;
						break;
					case "move_up":
						cameraVector = new Vector3(cameraVector.x,
											cameraVector.y - 1, cameraVector.z);
						cameraMoved = true;
						break;
					case "move_down":
						cameraVector = new Vector3(cameraVector.x,
											cameraVector.y + 1, cameraVector.z);
						cameraMoved = true;
						break;
					case "move_in":
						cameraVector = new Vector3(cameraVector.x,
											cameraVector.y, cameraVector.z + 1);
						cameraMoved = true;
						break;
					case "move_out":
						cameraVector = new Vector3(cameraVector.x,
											cameraVector.y, cameraVector.z - 1);
						cameraMoved = true;
						break;
					case "spacebar":
						if (ent != null) {
							ent.Dispose();
							ent = null;
						}
						break;
				}
			} while (cmd.Command != "quit");

			return false;
		}
	}
}
