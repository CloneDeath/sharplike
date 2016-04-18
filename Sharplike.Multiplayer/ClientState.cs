using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sharplike.Core.ControlFlow;
using System.Net;
using Lidgren.Network;
using System.Diagnostics;
using Sharplike.Core;

namespace Sharplike.Multiplayer
{
	public class ClientState : AbstractState
	{
		IPEndPoint remote_endpoint;
		NetClient client;
		int timeout;

		RemoteTerminal Display;

		public ClientState(IPAddress Address, int Port, int Timeout = 1000)
		{
			this.remote_endpoint = new IPEndPoint(Address, Port);
			this.timeout = Timeout;
		}

		protected override void StateStarted()
		{
			/* Attempt connection with remote server */
			NetPeerConfiguration config = new NetPeerConfiguration("Sharplike.Multiplayer");
			config.EnableUPnP = true;
			config.NetworkThreadName = "Sharplike.Multiplayer.NetThread";

			client = new NetClient(config);
			client.Connect(remote_endpoint);
			client.RegisterReceivedCallback(new System.Threading.SendOrPostCallback(OnMessageReceived));

			Stopwatch connection_time = Stopwatch.StartNew();
			while (client.ConnectionStatus != NetConnectionStatus.Connected) {
				if (connection_time.ElapsedMilliseconds > this.timeout) {
					// Connection attempt timed out
					connection_time.Stop();
					this.StateMachine.PopState();
					return;
				}
			}
			connection_time.Stop();

			/* Set up the UI */
			Display = new RemoteTerminal(Game.RenderSystem.Window);
			Display.Size = Game.RenderSystem.Window.Size;
		}

		void OnMessageReceived(object peer)
		{
			NetIncomingMessage msg;
			while ((msg = client.ReadMessage()) != null) {
				switch (msg.MessageType) {
					case NetIncomingMessageType.VerboseDebugMessage:
					case NetIncomingMessageType.DebugMessage:
					case NetIncomingMessageType.WarningMessage:
					case NetIncomingMessageType.ErrorMessage:
						Console.WriteLine(msg.ReadString());
						break;
					case NetIncomingMessageType.Data:
						HandleMessage(msg);
						break;
					default:
						Console.WriteLine("Unhandled type: " + msg.MessageType);
						break;
				}
			}
		}

		private void HandleMessage(NetIncomingMessage msg)
		{
			string MessageHandler = msg.ReadString();
			switch (MessageHandler) {
				case RemoteTerminal.Handle:
					Display.Receive(msg);
					break;
				default:
					Console.WriteLine("Unrecognized message handler '" + MessageHandler + "'");
			}
		}
	}
}
