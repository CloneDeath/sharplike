using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sharplike.Core.Rendering;
using System.Drawing;

namespace Sharplike.UI.Controls
{
	public class ListBox : Border
	{
		public ListBox(AbstractRegion parent) : base(parent) { }

		public List<ListBoxItem> Items = new List<ListBoxItem>();

		private int SelectedIndex = -1;

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

		public ListBoxItem AddItem(string itemname)
		{
			this.Invalidate();
			ListBoxItem lbi = new ListBoxItem(this);
			lbi.Text = itemname;
			return lbi;
		}

		protected override void OnChildRegionAdded(AbstractRegion childNode)
		{
			base.OnChildRegionAdded(childNode);

			if (childNode is ListBoxItem) {
				this.Invalidate();
				Items.Add(childNode as ListBoxItem);
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

		public override void Render()
		{
			int y_offset = 1;
			foreach (ListBoxItem item in Items) {
				item.Location = new Point(1, y_offset);
				y_offset += item.Size.Height;

				if (SelectedItem == item) {
					item.Background = Color.Blue;
				} else {
					item.Background = Color.Black;
				}
			}
		}
	}
}
