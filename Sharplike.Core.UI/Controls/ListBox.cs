using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sharplike.Core.Rendering;
using System.Drawing;
using System.Windows.Forms;

namespace Sharplike.UI.Controls
{
	public class ListBox : Border
	{
		public bool WrapSelection = true;
		private List<ListBoxItem> Items = new List<ListBoxItem>();
		private int _selectedIndex = -1;

		public ListBox(AbstractRegion parent) : base(parent) { }

		public ListBoxItem SelectedItem
		{
			get
			{
				if (SelectedIndex >= 0 && SelectedIndex < Items.Count) {
					return Items[SelectedIndex];
				} else {
					return null;
				}
			}
			set
			{
				if (value == null || !Items.Contains(value)) {
					SelectedIndex = -1;
				} else {
					SelectedIndex = Items.IndexOf(value);
				}
			}
		}

		public int SelectedIndex
		{
			get
			{
				return _selectedIndex;
			}
			set
			{
				if (this.Items.Count == 0) {
					_selectedIndex = -1;
				}

				int AboveTop = 0;
				int BelowEnd = this.Items.Count - 1;
				if (WrapSelection) {
					AboveTop = BelowEnd;
					BelowEnd = 0;
				}

				if (value < 0) {
					this._selectedIndex = AboveTop;
				} else if (value >= this.Items.Count) {
					this._selectedIndex = BelowEnd;
				} else {
					this._selectedIndex = value;
				}

				this.Invalidate();
			}
		}

		public ListBoxItem AddItem(string itemname)
		{
			this.Invalidate();
			ListBoxItem lbi = new ListBoxItem(this); // Will trigger "OnChildRegionAdded"
			lbi.Text = itemname;
			return lbi;
		}

		protected override void OnChildRegionAdded(AbstractRegion childNode)
		{
			base.OnChildRegionAdded(childNode);

			if (childNode is ListBoxItem) {
				this.Invalidate();
				Items.Add(childNode as ListBoxItem);
				if (SelectedIndex == -1) {
					SelectedIndex = 0;
				}
				childNode.Location = new Point(1, Items.Count);
			}
		}

		protected override void OnChildRegionRemoved(AbstractRegion childNode)
		{
			base.OnChildRegionRemoved(childNode);

			if (childNode is ListBoxItem) {
				this.Invalidate();
				Items.Remove(childNode as ListBoxItem);
			}
		}

		protected override void Render()
		{
			foreach (ListBoxItem item in Items) {
				if (SelectedItem == item) {
					item.Background = Color.Yellow;
					item.Color = Color.Black;
				} else {
					item.Background = Color.Black;
					item.Color = Color.White;
				}
			}
		}

		public override void Update()
		{
			base.Update();
		}

		public override void OnKeyPress(Keys KeyCode)
		{
			if (KeyCode.HasFlag(Keys.Down)) {
				this.SelectedIndex += 1;
				this.Invalidate();
			}

			if (KeyCode.HasFlag(Keys.Up)) {
				this.SelectedIndex -= 1;
				this.Invalidate();
			}

			base.OnKeyPress(KeyCode);
		}
	}
}
