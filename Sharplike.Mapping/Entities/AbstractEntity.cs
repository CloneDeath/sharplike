﻿///////////////////////////////////////////////////////////////////////////////
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
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Sharplike.Core;
using Sharplike.Core.Messaging;
using Sharplike.Core.Rendering;
using Sharplike.Mapping;

namespace Sharplike.Mapping.Entities
{
    [ChannelSubscriber("Entities")]
	[Serializable]
    public class AbstractEntity : IGlyphProvider, IMessageReceiver, IDisposable, IPageCallbacker
    {
		public AbstractEntity()
		{
			this.MessageHandler.SetHandler("Reposition", Message_Reposition);
			this.MessageHandler.SetHandler("Ping", Message_Ping);
		}

        public void OnMessage(Message msg)
        {
			MessageHandler.HandleMessage(msg);
        }

		public void AssertArgumentTypes(Message msg)
		{
			MessageHandler.AssertArgumentTypes(msg);
		}

		[MessageArgument(0, typeof(Vector3))]
		void Message_Reposition(Message msg)
		{
			this.Location = (Vector3)msg.Args[0];
		}

		void Message_Ping(Message msg)
		{
			Console.WriteLine("Pong");
		}

        public virtual Color BackgroundColor
        {
            get { return Color.Black; }
        }

        public virtual Glyph[] Glyphs
        {
            get { return new Glyph[0]; }
        }

        public virtual bool Dirty
        {
            get { return false; }
        }

        public Vector3 Location
        {
            get { return loc; }
            set
            {
                if (owner == null || owner.RepositionEntity(this, loc, value))
                    loc = value;
            }
        }

        public AbstractMap Map
        {
            get { return owner; }
            set
            {
				if (owner != null)
				{
					owner.RemoveEntity(this);
				}
                owner = value;
				if (owner != null)
				{
					owner.AddEntity(this);
				}
            }
        }

		/// <summary>
		/// Makes an entity walk along the map world in a given direction.
		/// This function will respect the passable state of the target map square.
		/// </summary>
		/// <param name="dir">The direction to walk in.</param>
		/// <returns>
		/// True if the walk was successful, or 
		/// false if the target square was impassable or didn't exist.
		/// </returns>
		public virtual bool Move(Direction dir)
		{
			Vector3 w;
			switch (dir)
			{
				case Direction.North:
					w = new Vector3(0, -1, 0);
					break;
				case Direction.South:
					w = new Vector3(0, 1, 0);
					break;
				case Direction.East:
					w = new Vector3(1, 0, 0);
					break;
				case Direction.West:
					w = new Vector3(-1, 0, 0);
					break;
				case Direction.Northwest:
					w = new Vector3(-1, -1, 0);
					break;
				case Direction.Southwest:
					w = new Vector3(1, -1, 0);
					break;
				case Direction.Northeast:
					w = new Vector3(-1, 1, 0);
					break;
				case Direction.Southeast:
					w = new Vector3(1, 1, 0);
					break;
				case Direction.Up:
					w = new Vector3(0, 0, -1);
					break;
				case Direction.Down:
					w = new Vector3(0, 0, 1);
					break;
				default:
					throw new ArgumentException("Direction was invalid.", "dir");
			}

			Vector3 newloc = this.Location + w;
			AbstractSquare sq = Map.GetSafeSquare(newloc);
			if (sq != null && sq.IsPassable(DirectionUtils.OppositeDirection(dir)))
			{
				this.Location = newloc;
				return true;
			}
			return false;
		}

		public void Dispose()
		{
			if (Map != null)
				Map.RemoveEntity(this);
		}

		[NonSerialized]
        private AbstractMap owner;

        private Vector3 loc = new Vector3(0, 0, 0);

		public readonly MessageHandler MessageHandler = new MessageHandler();
	}
}
