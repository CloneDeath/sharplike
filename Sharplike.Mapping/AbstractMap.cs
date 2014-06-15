using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Sharplike.Core;
using System.IO;
using Sharplike.Core.Rendering;
using Sharplike.Core.Messaging;
using Sharplike.Core.Scheduling;
using Sharplike.Mapping.Entities;
using System.Runtime.Serialization.Formatters.Binary;

namespace Sharplike.Mapping
{
	/// <summary>
	/// Represents a single continuous location in space.
	/// A map is made up of pages - 3D chunks of the world.
	/// </summary>
	[Serializable]
	[ChannelSubscriber("Maps")]
	public abstract class AbstractMap : AbstractRegion, IMessageReceiver
	{
		/// <summary>
		/// Name of the map.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Thread safe. Gets the size of pages that the map uses.
		/// </summary>
		public Vector3 PageSize
		{
			get;
			private set;
		}

		public readonly Dictionary<Vector3, AbstractPage> Pages;
		Vector3 view;

		/// <summary>
		/// The location of the camera in world-space. This will be the top-left location in the screen space.
		/// </summary>
		public Vector3 View
		{
			get { return view; }
			set { ViewFrom(value); }
		}

		public AbstractMap(string Name, Vector3 PageSize, AbstractRegion parent) : base(parent)
		{
			this.PageSize = PageSize;
			this.Name = Name;

			Pages = new Dictionary<Vector3, AbstractPage>();

			MessageHandler.SetHandler("LookAt", Message_LookAt);
			MessageHandler.SetHandler("ViewFrom", Message_ViewFrom);
			MessageHandler.SetHandler("RepositionEntity", Message_RepositionEntity);
		}

		#region Message Handler Functions
		[MessageArgument(0, typeof(AbstractEntity))]
		void Message_LookAt(Message m)
		{
			ViewFrom((AbstractEntity)m.Args[0]);
		}

		[MessageArgument(0, typeof(Vector3))]
		void Message_ViewFrom(Message m)
		{
			ViewFrom((Vector3)m.Args[0]);
		}

		[MessageArgument(0, typeof(AbstractEntity))]
		[MessageArgument(1, typeof(Vector3))]
		[MessageArgument(2, typeof(Vector3))]
		void Message_RepositionEntity(Message m)
		{
			RepositionEntity((AbstractEntity)m.Args[0], (Vector3)m.Args[1], (Vector3)m.Args[2]);
		}
		#endregion


		public static T Deserialize<T>(Stream file) where T : AbstractMap
		{
			BinaryFormatter f = new BinaryFormatter();
			return (T)f.Deserialize(file);
		}

		public void Serialize(Stream file)
		{
			BinaryFormatter f = new BinaryFormatter();
			f.Serialize(file, this);
		}

		/// <summary>
		/// Get operations are thread safe. Set operations are not.
		/// Gets or sets the square to be used at a particular map coordinate.
		/// </summary>
		/// <param name="x">X coordinate to retrieve a square from.</param>
		/// <param name="y">Y coordinate to retrieve a square from.</param>
		/// <param name="z">Z coordinate to retrieve a square from.</param>
		/// <returns>The square at the specified coordinate.</returns>
		public virtual AbstractSquare this[Int32 x, Int32 y, Int32 z]
		{
			get
			{
				return this.GetSafeSquare(new Vector3(x,y,z));
			}
			protected set
			{
				SetSquare(new Vector3(x, y, z), value);
			}
		}

		/// <summary>
		/// Thread safe. Gets/Sets the current position and size of the viewport.
		/// The location of the viewport is in world space, the size of the viewport is tied to both
		/// the world space and screen space.
		/// To change the location on the screen, use the <see cref="Location"/> property.
		/// </summary>
		public Rectangle Viewport
		{
			get
			{
				return new Rectangle(new Point(view.X, view.Y), this.Size);
			}
			set
			{
				this.ViewFrom(new Vector3(value.X, value.Y, view.Z));
				this.Size = value.Size;
			}
		}

		public void RemovePage(AbstractPage page)
		{
			List<Vector3> keys = new List<Vector3>();
			foreach (KeyValuePair<Vector3, AbstractPage> pair in Pages)
			{
				if (pair.Value == page) keys.Add(pair.Key);
			}
			foreach (Vector3 key in keys)
			{
				Pages.Remove(key);
			}
		}
		
		/// <summary>
		/// Adds the passed in page to this map, at the specified map slot.
		/// </summary>
		/// <param name="newPage">The page to add to this map.</param>
		/// <param name="pageLocation">The map slot to place this page.</param>
		public void AddPage(AbstractPage newPage, Vector3 pageLocation)
		{
			if (newPage.Size.Equals(this.PageSize) == false)
				throw new ArgumentException("All pages must be the size specified to the Map object.");

			if (Pages.ContainsKey(pageLocation)) throw new ArgumentException("Page already exists at " + pageLocation.ToString() + ".");

			// Let's allow them to make disjointed space -- maybe they have infinite terrain, as well as teleportation
			//bool Found = false;
			//for (int dx = -1; dx <= 1; dx++) {
			//    for (int dy = -1; dy <= 1; dy++) {
			//        for (int dz = -1; dz <= 1; dz++){
			//            if (Pages.ContainsKey(pageLocation + new Vector3(dx, dy, dz))) {
			//                Found = true;
			//                break;
			//            }
			//        }
			//        if (Found) break;
			//    }
			//    if (Found) break;
			//}
			//if (!Found)
			//{
			//    throw new ArgumentException("Provided coordinates for new page are not contiguous with existing pages.");
			//}

			newPage.ParentMap = this;
			newPage.address = pageLocation;

			Game.Scheduler.AddTask(newPage);

			Pages.Add(pageLocation, newPage);
			newPage.Build();
		}

		public AbstractPage GetPage(Vector3 addr)
		{
			if (!Pages.ContainsKey(addr)) return null;
			return this.Pages[addr];
		}

		public AbstractSquare GetSquare(AbstractPage p, Vector3 offset)
		{
			Vector3 npos = new Vector3(p.address.X * PageSize.X + offset.X,
										   p.address.Y * PageSize.Y + offset.Y,
										   p.address.Z * PageSize.Z + offset.Z);
			return GetSquare(npos);
		}

		/// <summary>
		/// Returns the square at the specified location.
		/// </summary>
		/// <param name="location">The location of the square.</param>
		/// <returns>The square at the specified location.</returns>
		public AbstractSquare GetSquare(Vector3 location)
		{
			Vector3 addr;
			Vector3 newoff;
			Vector3.Divide(location, this.PageSize, out addr, out newoff);
			if (!Pages.ContainsKey(addr)) throw new ArgumentException("Specified square not in map.");
			return Pages[addr].GetSquare(newoff.X, newoff.Y, newoff.Z);
		}

		/// <summary>
		/// Sets a location in the map to the specified square.
		/// </summary>
		/// <param name="location">Location on the map to edit.</param>
		/// <param name="square">Square to use</param>
		public void SetSquare(Vector3 location, AbstractSquare square)
		{
			Vector3 addr;
			Vector3 newoff;
			Vector3.Divide(location, this.PageSize, out addr, out newoff);
			if (!Pages.ContainsKey(addr))
			{
				AddPage(new BasicPage(PageSize), addr);
			}

			Pages[addr].SetSquare(newoff.X, newoff.Y, newoff.Z, square);

			if (location.Z == view.Z)
			{
				if (Viewport.Contains(new Point(location.X, location.Y)))
				{
					InvalidateTiles(new Rectangle(location.X - view.X, location.Y - view.Y, 1, 1));
					this.Dirty = false;
				}
			}

			if (square != null)
			{
				square.Map = this;
				square.Location = location;
			}
		}

		/// <summary>
		/// Thread safe. Get the square at the specified map location.
		/// </summary>
		/// <param name="location">The coordinates on the map to fetch the square from.</param>
		/// <returns>The square if one exists, or null if no square has been assigned there.</returns>
		public AbstractSquare GetSafeSquare(Vector3 location)
		{
			Vector3 addr;
			Vector3 newoff;

			lock (this)
			{
				Vector3.Divide(location, this.PageSize, out addr, out newoff);

				AbstractPage p;
				if (!Pages.TryGetValue(addr, out p))
					return null;
				return p.GetSquare(newoff.X, newoff.Y, newoff.Z);
			}
		}


		public Vector3 GetVisibleSquareLocation(AbstractSquare square)
		{
			lock (this)
			{
				foreach (AbstractPage page in this.GetPagesInRange(this.View, 
					new Vector3(this.Viewport.Width, this.Viewport.Height, 1)))
				{
					for (int y = 0; y < PageSize.Y; ++y)
					{
						for (int x = 0; x < PageSize.X; ++x)
						{
							Vector3 location = new Vector3(x, y, this.View.Z);
							if (page[location].Equals(square))
								return location;
						}
					}
				}
			}
			return Vector3.Zero;
		}

		/// <summary>
		/// Sets the viewport to look at a specific entity on the map.
		/// This will center the entity on the screen.
		/// </summary>
		/// <param name="ent">The entity to view from.</param>
		public void ViewFrom(AbstractEntity ent)
		{
			int x = ent.Location.X - (Size.Width / 2);
			int y = ent.Location.Y - (Size.Height / 2);
			int z = ent.Location.Z;
			ViewFrom(new Vector3(x, y, z));
		}

		/// <summary>
		/// Sets the viewport to a specific location on the map.
		/// </summary>
		/// <param name="nView">The map coordinates of the top-left most square to view from.</param>
		public void ViewFrom(Vector3 nView) { this.ViewFrom(nView, false); }

		/// <summary>
		/// Sets the viewport to a specific location on the map.
		/// </summary>
		/// <param name="newView">The coordinates of the top-left most square to view from.</param>
		/// <param name="forceRender">Whether or not to force a redraw of the viewport.
		/// This only has an effect when the viewport doesn't change.</param>
		public void ViewFrom(Vector3 newView, Boolean forceRender)
		{
			if (newView.Equals(view) == false || forceRender == true)
			{
				view = newView;
				this.Invalidate();				
			}
		}

		AbstractPage[] GetPagesInRange(Vector3 start, Vector3 extents)
		{
			Vector3 addr;
			Vector3 newoff;
			Vector3.Divide(start, this.PageSize, out addr, out newoff);

			List<AbstractPage> ret = new List<AbstractPage>();

			for (int x = addr.X; x <= addr.X + Math.Ceiling((double)extents.X / PageSize.X); ++x)
			{
				for (int y = addr.Y; y <= addr.Y + Math.Ceiling((double)extents.Y / PageSize.Y); ++y)
				{
					for (int z = addr.Z; z <= addr.Z + Math.Ceiling((double)extents.Z / PageSize.Z); ++z)
					{
						AbstractPage p;
						if (Pages.TryGetValue(new Vector3(x, y, z), out p))
							ret.Add(p);
					}
				}
			}

			return ret.ToArray();
		}

		bool IsPageVisible(AbstractPage p)
		{
			Vector3 worldloc = new Vector3(
				p.address.X * PageSize.X,
				p.address.Y * PageSize.Y,
				p.address.Z * PageSize.Z);
			Rectangle pagerect = new Rectangle(new Point(worldloc.X, worldloc.Y), new Size(p.Size.X, p.Size.Y));
			return pagerect.IntersectsWith(this.Viewport) && view.Z >= worldloc.Z && view.Z < worldloc.Z + PageSize.Z;
		}

		#region Entity Operations
		internal void AddEntity(AbstractEntity ent)
		{
			Vector3 addr;
			Vector3 newoff;
			Vector3.Divide(ent.Location, this.PageSize, out addr, out newoff);

			if (ent.Location.IntersectsWith(this.Viewport) && ent.Location.Z == view.Z) {
				this.RegionTiles[ent.Location.X - view.X, ent.Location.Y - view.Y].AddGlyphProvider(ent);
			}

			AbstractPage p = Pages[addr];
			if (p != null) {
				p.Entities.Add(ent);
			}
		}

		internal void RemoveEntity(AbstractEntity ent)
		{
			Vector3 addr;
			Vector3 newoff;
			Vector3.Divide(ent.Location, this.PageSize, out addr, out newoff);

			if (ent.Location.IntersectsWith(this.Viewport) && ent.Location.Z == view.Z)
			{
				this.RegionTiles[ent.Location.X - view.X, ent.Location.Y - view.Y].RemoveGlyphProvider(ent);
			}

			AbstractPage p = Pages[addr];
			if (p != null) {
				p.Entities.Remove(ent);
			}
		}

		public void SwapEntityCallbackOwner(AbstractEntity ent, AbstractPage oldpage, AbstractPage newpage)
		{
			List<PageCallbackInfo> pci = oldpage.RemoveAllAIDelegates(ent);
			foreach (PageCallbackInfo callback in pci)
			{
				newpage.RegisterAIDelegate(callback);
			}
		}

		internal bool RepositionEntity(AbstractEntity ent, Vector3 oldloc, Vector3 newloc)
		{
			Vector3 oldpageaddr, newpageaddr;
			Vector3 oldpageloc, newpageloc;
			Vector3.Divide(oldloc, this.PageSize, out oldpageaddr, out oldpageloc);
			Vector3.Divide(newloc, this.PageSize, out newpageaddr, out newpageloc);

			AbstractPage oldpage;
			AbstractPage newpage;

			if (Pages.TryGetValue(newpageaddr, out newpage))
			{
				if (Pages.TryGetValue(oldpageaddr, out oldpage))
				{
					if (oldloc.IntersectsWith(this.Viewport) && oldloc.Z == view.Z)
					{
						int x = oldloc.X - view.X;
						int y = oldloc.Y - view.Y;
						this.RegionTiles[x, y].RemoveGlyphProvider(ent);
					}
					if (!oldpageaddr.Equals(newpageaddr)) 
					{
						SwapEntityCallbackOwner(ent, oldpage, newpage);
						oldpage.Entities.Remove(ent);
					}
				}

				if (newloc.IntersectsWith(this.Viewport) && ent.Location.Z == view.Z) {
					this.RegionTiles[newloc.X - view.X, newloc.Y - view.Y].AddGlyphProvider(ent);
				}
				if (!oldpageaddr.Equals(newpageaddr))
				{
					newpage.Entities.Add(ent);
				}

				return true;
			}

			return false;
		}

		/// <summary>
		/// Thread safe. Gets all entities within a specified distance from a specified point.
		/// </summary>
		/// <param name="location">The center of the extents to search.</param>
		/// <param name="range">The ellipsoid extents to search.</param>
		/// <returns>An array of all entities found within the specified extents.</returns>
		public AbstractEntity[] EntitiesInElipticalRange(Vector3 location, Vector3 range)
		{
			List<AbstractEntity> ret = new List<AbstractEntity>();
			lock (this)
			{
				Vector3 topleft = location - range;
				Vector3 extents = (range * 2);
				foreach (AbstractPage p in GetPagesInRange(topleft, extents))
				{
					foreach (AbstractEntity ent in p.Entities)
					{
						if (ent.Location.IntersectsWithEllipse(location, range))
						{
							ret.Add(ent);
						}
					}
				}
			}
			return ret.ToArray();
		}

		/// <summary>
		/// Thread safe. Gets all entities within a specified distance from a specified point.
		/// </summary>
		/// <param name="location">The center of the extents to search.</param>
		/// <param name="range">The rectangular extents to search.</param>
		/// <returns>An array of all entities found within the specified extents.</returns>
		public AbstractEntity[] EntitiesInRectangularRange(Vector3 location, Vector3 range)
		{
			List<AbstractEntity> ret = new List<AbstractEntity>();
			lock (this)
			{
				Vector3 topleft = location - range;
				Vector3 extents = (range * 2);
				foreach (AbstractPage p in GetPagesInRange(topleft, extents))
				{
					foreach (AbstractEntity ent in p.Entities)
					{
						if (ent.Location.IntersectsWithExtents(location, range))
						{
							ret.Add(ent);
						}
					}
				}
			}
			return ret.ToArray();
		}

		/// <summary>
		/// Thread Safe. Broadcasts a messages to all entities within a given range from the specified point.
		/// </summary>
		/// <param name="location">The center of the ellipsoid to broadcast to.</param>
		/// <param name="range">The extents of the ellipsoid to broadcast within.</param>
		/// <param name="message">The message to broadcast.</param>
		/// <returns>All entities that received the message.</returns>
		public AbstractEntity[] BroadcastMessage(Vector3 location, Vector3 range, String message)
		{
			return BroadcastMessage(location, range, message, new Object[0]);
		}

		/// <summary>
		/// Thread Safe. Broadcasts a messages to all entities within a given range from the specified point.
		/// </summary>
		/// <param name="location">The center of the ellipsoid to broadcast to.</param>
		/// <param name="range">The extents of the ellipsoid to broadcast within.</param>
		/// <param name="message">The message to broadcast.</param>
		/// <param name="args">Additional arguments that this message requires.</param>
		/// <returns>All entities that received the message.</returns>
		public AbstractEntity[] BroadcastMessage(Vector3 location, Vector3 range, String message, params Object[] args)
		{
			AbstractEntity[] ents = EntitiesInElipticalRange(location, range);

			foreach (AbstractEntity ent in ents)
			{
				Game.SendMessage(ent, message, args);
			}

			return ents;
		}

		/// <summary>
		/// Broadcasts a message only to the single nearest entity.
		/// </summary>
		/// <param name="location">The center of the ellipsoid to broadcast to.</param>
		/// <param name="maxrange">The extents of the ellipsoid to broadcast within.</param>
		/// <param name="message">The message to broadcast.</param>
		/// <returns>The entity that received the message, or null if no other entity existed within the range.</returns>
		public AbstractEntity BroadcastToNearest(Vector3 location, Vector3 maxrange, String message)
		{
			return BroadcastToNearest(location, maxrange, message, new Object[0]);
		}

		/// <summary>
		/// Broadcasts a message only to the single nearest entity.
		/// </summary>
		/// <param name="location">The center of the ellipsoid to broadcast to.</param>
		/// <param name="maxrange">The extents of the ellipsoid to broadcast within.</param>
		/// <param name="message">The message to broadcast.</param>
		/// <param name="args">Additional arguments for the message.</param>
		/// <returns>The entity that received the message, or null if no other entity existed within the range.</returns>
		public AbstractEntity BroadcastToNearest(Vector3 location, Vector3 maxrange, String message, params Object[] args)
		{
			List<AbstractEntity> ents = new List<AbstractEntity>(EntitiesInElipticalRange(location, maxrange));
			ents.Sort(delegate(AbstractEntity a, AbstractEntity b)
			{
				return a.Location.SquaredDistanceTo(location) > b.Location.SquaredDistanceTo(location) ? 1 : -1;
			});
			if (ents.Count > 0)
				return ents[0];
			return null;
		}

		/// <summary>
		/// Broadcasts a message only to the single nearest entity.
		/// </summary>
		/// <param name="location">The center of the ellipsoid to broadcast to.</param>
		/// <param name="maxrange">The extents of the ellipsoid to broadcast within.</param>
		/// <param name="exclude">One entity to exclude from the broadcast.</param>
		/// <param name="message">The message to broadcast.</param>
		/// <returns>The entity that received the message, or null if no other entity existed within the range.</returns>
		public AbstractEntity BroadcastToNearest(Vector3 location, Vector3 maxrange, AbstractEntity exclude, String message)
		{
			return BroadcastToNearest(location, maxrange, exclude, message, new Object[0]);
		}

		/// <summary>
		/// Broadcasts a message only to the single nearest entity.
		/// </summary>
		/// <param name="location">The center of the ellipsoid to broadcast to.</param>
		/// <param name="maxrange">The extents of the ellipsoid to broadcast within.</param>
		/// <param name="exclude">One entity to exclude from the broadcast.</param>
		/// <param name="message">The message to broadcast.</param>
		/// <param name="args">Additional arguments for the message.</param>
		/// <returns>The entity that received the message, or null if no other entity existed within the range.</returns>
		public AbstractEntity BroadcastToNearest(Vector3 location, Vector3 maxrange, AbstractEntity exclude, String message, params Object[] args)
		{
			List<AbstractEntity> ents = new List<AbstractEntity>(EntitiesInElipticalRange(location, maxrange));
			ents.Remove(exclude);
			ents.Sort(delegate(AbstractEntity a, AbstractEntity b)
			{
				return a.Location.SquaredDistanceTo(location) > b.Location.SquaredDistanceTo(location) ? 1 : -1;
			});
			if (ents.Count > 0)
				return ents[0];
			return null;
		}
		#endregion

		public void OnMessage(Message msg)
		{
			MessageHandler.HandleMessage(msg);
		}

		public void AssertArgumentTypes(Message msg)
		{
			MessageHandler.AssertArgumentTypes(msg);
		}
		public readonly MessageHandler MessageHandler = new MessageHandler();

		protected override void Render()
		{
			for (int screen_x = 0; screen_x < this.Size.Width; screen_x++) {
				int world_x = screen_x + View.X;

				for (int screen_y = 0; screen_y < this.Size.Height; screen_y++) {
					int world_y = screen_y + View.Y;
					this.RegionTiles[screen_x, screen_y].Reset();

					AbstractSquare sq = this.GetSafeSquare(new Vector3(world_x, world_y, View.Z));
					if (sq != null) {
						this.RegionTiles[screen_x, screen_y].AddGlyphProvider(sq);
					} else {
						//Draw a nice red error tile if we're looking out of bounds in debug mode,
						//but just black if we're in release mode.
#if DEBUG
						int n = 249; // circle
						if (world_x % 2 == 0 && world_y % 2 == 0) n = 197; // +
						if (world_x % 2 != 0 && world_y % 2 == 0) n = 196; // -
						if (world_x % 2 == 0 && world_y % 2 != 0) n = 179; // |

						this.RegionTiles[screen_x, screen_y].AddGlyphProvider(new ErrorSquare(n));
#else
						this.RegionTiles[x, y].AddGlyphProvider(new EmptySquare());
#endif
					}

				}
			}

			Vector3 addr;
			Vector3 newoff;
			Vector3.Divide(this.View, this.PageSize, out addr, out newoff);

			foreach (AbstractPage p in GetPagesInRange(View, new Vector3(this.Size.Width, this.Size.Height, 1))) {
				foreach (AbstractEntity ent in p.Entities) {
					if (ent.Location.IntersectsWith(this.Viewport) && ent.Location.Z == view.Z) {
						this.RegionTiles[ent.Location.X - view.X, ent.Location.Y - view.Y].AddGlyphProvider(ent);
					}
				}
			}
		}
	}
}