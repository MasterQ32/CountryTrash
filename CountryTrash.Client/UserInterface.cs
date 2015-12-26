using System.Collections;
using System.Collections.Generic;

namespace CountryTrash
{
	internal class UserInterface : IList<Widget>
	{
		private IList<Widget> widgets = new List<Widget>();

		public Widget this[int index]
		{
			get
			{
				return widgets[index];
			}

			set
			{
				widgets[index] = value;
			}
		}

		public int Count
		{
			get
			{
				return widgets.Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return widgets.IsReadOnly;
			}
		}

		public void Add(Widget item)
		{
			widgets.Add(item);
		}

		public void Clear()
		{
			widgets.Clear();
		}

		public bool Contains(Widget item)
		{
			return widgets.Contains(item);
		}

		public void CopyTo(Widget[] array, int arrayIndex)
		{
			widgets.CopyTo(array, arrayIndex);
		}

		public IEnumerator<Widget> GetEnumerator()
		{
			return widgets.GetEnumerator();
		}

		public int IndexOf(Widget item)
		{
			return widgets.IndexOf(item);
		}

		public void Insert(int index, Widget item)
		{
			widgets.Insert(index, item);
		}

		public bool Remove(Widget item)
		{
			return widgets.Remove(item);
		}

		public void RemoveAt(int index)
		{
			widgets.RemoveAt(index);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return widgets.GetEnumerator();
		}
	}
}