using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Sharplike.Core.Messaging;
using System.Collections;
using System.Windows.Forms;

namespace Sharplike.Core.Rendering
{
	/// <summary>
	/// A region of glyps, with child regions.
	/// </summary>
	[Serializable]
	[ChannelSubscriber("Regions")]
	public abstract class AbstractRegion : IComparable<AbstractRegion>, IDeserializationCallback, IDisposable
	{
		internal static AbstractRegion FocusControl = null;

		[NonSerialized]
		private RegionTile[,] regionTiles = new RegionTile[0, 0];
		public RegionTile[,] RegionTiles
		{
			get { return regionTiles; }
		}

		private Size size;
		public virtual Size Size
		{
			get
			{
				return size;
			}
			set
			{
				Invalidate();
				this.AutoSizeToContents = false;
				this.ResizeRegion(value);
				size = value;
				if (this.Resize != null)
					this.Resize();
				Invalidate();
			}
		}

		public bool AutoSizeToContents { get; set; }
		protected bool Dirty = true;

		[NonSerialized]
		private Point location;
		public Point Location
		{
			get
			{
				return this.location;
			}
			set
			{
				Invalidate();
				location = value;
				if (this.Move != null)
					this.Move();
				Invalidate();
			}
		}

		public Rectangle Rectangle
		{
			get { return new Rectangle(location, size); }
		}

		[NonSerialized]
		private Int32 zOrder;
		public Int32 ZOrder
		{
			get { return zOrder; }
			set
			{
				zOrder = value;
				if (this.ZOrderChange != null)
					this.ZOrderChange();
			}
		}

		private AbstractRegion _parent;
		public AbstractRegion Parent
		{
			get;
			private set;
		}

		[NonSerialized]
		private SortedDictionary<Int32, AbstractRegion> childRegions = new SortedDictionary<int,AbstractRegion>();

		public AbstractRegion(AbstractRegion parent)
		{
			this.AutoSizeToContents = false;
			this.ZOrder = 0;
			this.Size = new Size(10, 10);
			this.Location = new Point(0, 0);
			this.RegionAnchor = Anchor.Top | Anchor.Left;

			if (parent != null) {
				parent.AddRegion(this);
			}
		}


		/// <summary>
		/// Change the size of the region.
		/// </summary>
		/// <param name="dimensions">The new size.</param>
		private void ResizeRegion(Size dimensions)
		{
			foreach (KeyValuePair<Int32, AbstractRegion> kvp in this.childRegions)
			{
				AbstractRegion child = kvp.Value;

				Int32 childleft = child.Location.X;
				Int32 childtop  = child.Location.Y;
				Int32 childwidth = child.Size.Width;
				Int32 childheight = child.Size.Height;

				if ((child.RegionAnchor | Anchor.Top) == Anchor.Top)
				{
					if ((child.RegionAnchor | Anchor.Bottom) == Anchor.Bottom)
						childheight += dimensions.Height - size.Height;
				}
				else
				{
					if ((child.RegionAnchor | Anchor.Bottom) == Anchor.Bottom)
						childtop += dimensions.Height - size.Height;
				}

				if ((child.RegionAnchor | Anchor.Left) == Anchor.Left)
				{
					if ((child.RegionAnchor | Anchor.Right) == Anchor.Right)
						childleft += dimensions.Width - size.Width;
				}
				else
				{
					if ((child.RegionAnchor | Anchor.Right) == Anchor.Right)
						childwidth += dimensions.Width - size.Width;
				}

				child.Location = new Point(childleft, childtop);
				child.Size = new Size(childwidth, childheight);
			}

			if (Parent != null)
			{
				Parent.InvalidateTiles(new Rectangle(this.location,
					new Size(Math.Max(this.Size.Width, dimensions.Width),
						Math.Max(this.Size.Height, dimensions.Height))));
			}

			RegionTile[,] newRegion = new RegionTile[dimensions.Width, dimensions.Height];

			for (Int32 y = 0; y < dimensions.Height; y++)
			{
				for (Int32 x = 0; x < dimensions.Width; x++)
				{
					if (x < regionTiles.GetLength(0) && y < regionTiles.GetLength(1))
					{
						newRegion[x, y] = regionTiles[x, y];
					}
					else
					{
						newRegion[x, y] = new RegionTile();
					}
				}
			}

			regionTiles = newRegion;
		}

		/// <summary>
		/// Changes the position of the region in parent region space.
		/// </summary>
		/// <param name="newLocation">The new location of the region.</param>
		private void RelocateRegion(Point newLocation)
		{
			Invalidate();
			this.location = newLocation;
			Parent.InvalidateTiles(new Rectangle(this.location, this.size));
		}

		/// <summary>
		/// Invalidates all tiles within the region.
		/// </summary>
		public void Invalidate()
		{
			InvalidateTiles(new Rectangle(new Point(0, 0), this.Size));
		}

		/// <summary>
		/// Invalidates the tiles in a given subset of the region.
		/// </summary>
		/// <param name="region">
		/// The rectangle to invalidate. 
		/// It is safe for this rectangle to exceed the region bounds.
		/// </param>
		public void InvalidateTiles(Rectangle region)
		{
			for (Int32 y = region.Top; y < region.Bottom; ++y)
			{
				if (y < 0 || y >= Size.Height) continue;
				for (Int32 x = region.Left; x < region.Right; ++x)
				{
					if (x < 0 || x >= Size.Width) continue;

					if (this.regionTiles[x,y].displaytile != null)
						this.regionTiles[x, y].displaytile.MakeStackDirty();
				}
			}
			this.Dirty = true;
		}

		/// <summary>
		/// Returns a list of all child regions. Regions are sorted by Z-order.
		/// This is not a static list; all references refer to the actual child
		/// regions and not to copies.
		/// </summary>
		public IList<AbstractRegion> ChildRegions
		{
			get
			{
				return new List<AbstractRegion>(this.childRegions.Values).AsReadOnly();
			}
		}

		/// <summary>
		/// Adds the specified region as a child of this region.
		/// </summary>
		/// <param name="childRegion">The child region to add.</param>
		public void AddRegion(AbstractRegion childRegion)
		{
			this.AddRegion(childRegion, this.ChildRegions.Count + 1);
		}

		/// <summary>
		/// Adds the specified region as a child of this region. Enables
		/// you to specify the ZOrder for your regions.
		/// </summary>
		/// <param name="childRegion">The child region to add.</param>
		/// <param name="zOrder">The ZOrder of the new region.</param>
		public void AddRegion(AbstractRegion childRegion, Int32 zOrder)
		{
			AbstractRegion oldP = this.Parent;
			if (childRegion.Parent != null)
				childRegion.Parent.RemoveRegion(childRegion);

			this.childRegions.Add(zOrder, childRegion);
			this.OnChildRegionAdded(childRegion);

			childRegion.Parent = this;

			if (this.Reparent != null)
				this.Reparent(oldP, this.Parent);

			InvalidateTiles(new Rectangle(childRegion.Location, childRegion.Size));
		}

		/// <summary>
		/// Removes a child region by ZOrder.
		/// </summary>
		/// <param name="zOrder">The ZOrder of the child region to remove.</param>
		/// <returns>Returns true if a region existed at that ZOrder.</returns>
		public Boolean RemoveRegion(Int32 zOrder)
		{
			AbstractRegion r = null;

			if (this.childRegions.ContainsKey(zOrder))
			{
				r = this.childRegions[zOrder];
				return RemoveRegion(r);
			}

			return false;
		}

		/// <summary>
		/// Changes the ZOrder of the specified child region.
		/// </summary>
		/// <param name="childRegion">The child region to change.</param>
		/// <param name="zOrder">The child region's new ZOrder.</param>
		public void ChangeChildZOrder(AbstractRegion childRegion, Int32 zOrder)
		{
			if (this.childRegions.ContainsValue(childRegion) == false)
				throw new ArgumentException("Child region does not exist in this region.");

			foreach (Int32 i in this.childRegions.Keys)
			{
				if (this.childRegions[i] == childRegion)
				{
					this.childRegions.Remove(i);
				}
			}
			this.childRegions.Add(zOrder, childRegion);
			childRegion.ZOrder = zOrder;
		}

		/// <summary>
		/// Removes a child region.
		/// </summary>
		/// <param name="region">The child region to remove.</param>
		/// <returns>Returns true if a region was removed.</returns>
		public Boolean RemoveRegion(AbstractRegion region)
		{
			AbstractRegion r = region;

			if (this.childRegions.ContainsValue(r))
			{
				foreach (Int32 i in this.childRegions.Keys)
				{
					if (this.childRegions[i] == r)
					{
						r.Invalidate();
						this.childRegions.Remove(i);
						r.Parent = null;
						this.OnChildRegionRemoved(r);
						return true;
					}
				}
			}

			return false;
		}

		public void RemoveAllRegions()
		{
			List<AbstractRegion> allregions = new List<AbstractRegion>(this.ChildRegions);

			foreach (AbstractRegion r in allregions) {
				this.RemoveRegion(r);
			}
		}

		/// <summary>
		/// Gets a child region by ZOrder.
		/// </summary>
		/// <param name="zOrder">The ZOrder of the child region.</param>
		/// <returns>The child region, if found.</returns>
		public AbstractRegion GetRegion(Int32 zOrder)
		{
			if (this.childRegions.ContainsKey(zOrder) == false)
				throw new ArgumentException("No region with that ZOrder was found.");

			return this.childRegions[zOrder];
		}

		/// <summary>
		/// Recursively populates a list of RegionTiles. Only DisplayTile should ever need this.
		/// </summary>
		/// <param name="tiles">The list of displaytiles to populate.</param>
		/// <param name="loc">The index of the tile array to pull from.</param>
		internal void PopulateRegionTiles(List<RegionTile> tiles, Point loc)
		{
			tiles.Add(this.regionTiles[loc.X, loc.Y]);
			foreach (KeyValuePair<Int32, AbstractRegion> r in this.childRegions)
			{
				if (r.Value.Location.X <= loc.X
					&& r.Value.Location.Y <= loc.Y
					&& r.Value.Location.X + r.Value.Size.Width > loc.X
					&& r.Value.Location.Y + r.Value.Size.Height > loc.Y)
				{
					r.Value.PopulateRegionTiles(tiles,
						new Point(loc.X - r.Value.Location.X, loc.Y - r.Value.Location.Y));
				}
			}
		}

		/// <summary>
		/// Clear all tiles.
		/// </summary>
		public void Clear()
		{
			for (Int32 x = 0; x < this.Size.Width; x++)
			{
				for (Int32 y = 0; y < this.Size.Height; y++)
				{
					regionTiles[x, y].Reset();
				}
			}
		}

		public virtual void Update()
		{
			foreach (AbstractRegion child in ChildRegions) {
				child.Update();
			}

			if (this.Dirty) {
				this.Render();
				this.Dirty = false;
			}
		}

		/// <summary>
		/// This is called whenever the region has been marked as "Dirty".
		/// The entire region should be refreshed/redrawn in this function.
		/// </summary>
		protected virtual void Render()
		{

		}

		/// <summary>
		/// Gets or sets the region's anchor setting within its parent region.
		/// </summary>
		public Anchor RegionAnchor
		{
			get;
			set;
		}


		/// <summary>
		/// Compares Z-Ordering of two regions. Only provides semantically
		/// valid data if both regions are children of the same parent.
		/// </summary>
		/// <param name="other">The other region to test against.</param>
		/// <returns>
		/// A value of less than zero indicates that this region has a lower
		/// Z-order than the other region. A value greater than zero indicates
		/// that this region has a higher Z-order than the other region. A
		/// value of zero indicates that the two are equal.
		/// </returns>
		public Int32 CompareTo(AbstractRegion other)
		{
			return this.ZOrder.CompareTo(other.ZOrder);
		}

		public delegate void EmptyDelegate();
		public delegate void ChildNodeDelegate(AbstractRegion childNode);
		public delegate void ReparentDelegate(AbstractRegion oldParent, AbstractRegion newParent);

		public event EmptyDelegate Resize;
		public event EmptyDelegate Move;
		public event EmptyDelegate ZOrderChange;
		public event ChildNodeDelegate ChildRegionAdded;
		public event ChildNodeDelegate ChildRegionRemoved;
		public event ReparentDelegate Reparent;



		protected virtual void OnChildRegionAdded(AbstractRegion childNode)
		{
			if (ChildRegionAdded != null) {
				ChildRegionAdded(childNode);
			}
		}

		protected virtual void OnChildRegionRemoved(AbstractRegion childNode)
		{
			if (ChildRegionRemoved != null) {
				ChildRegionRemoved(childNode);
			}
		}

		public virtual void Dispose()
		{
			if (Parent != null)
				Parent.RemoveRegion(this);
			SortedDictionary<int, AbstractRegion> children = new SortedDictionary<int, AbstractRegion>(childRegions);
			foreach (KeyValuePair<int, AbstractRegion> r in children)
				RemoveRegion(r.Value);
		}

		public void OnDeserialization(object sender)
		{
			childRegions = new SortedDictionary<int, AbstractRegion>();

			regionTiles = new RegionTile[size.Width, size.Height];
			for (int x = 0; x < size.Width; ++x)
			{
				for (int y = 0; y < size.Height; ++y)
				{
					regionTiles[x, y] = new RegionTile();
				}
			}
		}

		/// <summary>
		/// Focuses this control for keyboard input.
		/// </summary>
		public void Focus()
		{
			FocusControl = this;
		}

		/// <summary>
		/// Returns true if this control has keyboard focus.
		/// </summary>
		/// <returns>True if this control is receiving keyboard input - false otherwise.</returns>
		public bool HasFocus()
		{
			return FocusControl == this;
		}

		/// <summary>
		/// Clears all controls from receiving keyboard input.
		/// </summary>
		public static void ClearFocus()
		{
			FocusControl = null;
		}

		/// <summary>
		/// Called whenever a keyboard key is pressed, and the current control has focus.
		/// </summary>
		/// <param name="KeyCode">The key that has been pressed.</param>
		public virtual void OnKeyPress(Keys KeyCode)
		{
			
		}
	}

	/// <summary>
	/// Anchor flags to control what child regions do when their parent resizes.
	/// Multiple options can be selected simultaniously.
	/// </summary>
	[Flags]
	public enum Anchor
	{
		/// <summary>
		/// Let the child float - his screen space will remain unchanges.
		/// </summary>
		None =		0x0,

		/// <summary>
		/// Anchors to the top of the parent.
		/// </summary>
		Top =		0x01 << 0,

		/// <summary>
		/// Anchors to the bottom of the parent.
		/// </summary>
		Bottom =	0x01 << 1,

		/// <summary>
		/// Anchors to the left of the parent.
		/// </summary>
		Left =		0x01 << 2,

		/// <summary>
		/// Anchors to the right of the parent.
		/// </summary>
		Right =		0x01 << 3
	}
}
