using System;
using System.Collections;

namespace NitroCast.Core
{
	/// <summary>
	/// Summary of MyClass
	/// </summary>
	[Serializable()]
	public class EnumItemCollection : IList, ICloneable 
	{
		private int itemCount = 0;
		internal EnumItem[] items ;

		public EnumItemCollection() : base()
		{
			items = new EnumItem[15];
		}

		public EnumItemCollection(int capacity) : base()
		{
			items = new EnumItem[capacity];
		}

		public bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				items[index] = (EnumItem) value;
			}
		}

		public EnumItem this[int index]
		{
			get
			{
				if(index > itemCount - 1)
					throw(new Exception("Index out of bounds."));
				return items[index];
			}
			set
			{
				items[index] = value;
			}
		}

		int IList.Add(object value)
		{
			return Add((EnumItem) value);
		}

		public int Add(EnumItem value)
		{
			itemCount++;
			if(itemCount > items.GetUpperBound(0) + 1)
			{
				EnumItem[] tempitems = new EnumItem[itemCount * 2];
				for(int x = 0; x <= items.GetUpperBound(0); x++)
					tempitems[x] = items[x];
				items = tempitems;
			}
			items[itemCount - 1] = value;
			return itemCount -1;
		}

		public void Clear()
		{
			itemCount = 0;
			items = new EnumItem[15];
		}

		bool IList.Contains(object value)
		{
			return Contains((EnumItem) value);
		}

		public bool Contains(EnumItem value)
		{
			return IndexOf(value) != -1;
		}

		public bool Contains(string name)
		{
			for(int x = 0; x < itemCount; x++)
				if(items[x].Name == name)
					return true;
			return false;
		}

		int IList.IndexOf(object value)
		{
			return IndexOf((EnumItem) value);
		}

		public int IndexOf(EnumItem value)
		{
			for(int x = 0; x < itemCount; x++)
				if(items[x].Equals(value))
					return x;
			return -1;
		}

		void IList.Insert(int index, object value)
		{
			Insert(index, (EnumItem) value);
		}

		public void Insert(int index, EnumItem value)
		{
			itemCount++;
			if(itemCount > items.Length)
				for(int x = index + 1; x == itemCount - 2; x ++)
					items[x] = items[x - 1];
			items[index] = value;
		}

		void IList.Remove(object value)
		{
			Remove((EnumItem) value);
		}

		public void Remove(EnumItem value)
		{
			int index = IndexOf(value);
			if(index == -1)
				throw(new Exception("EnumItem not found in collection."));
			RemoveAt(index);
		}

		public void Remove(string name)
		{
			int i = -1;
			for(int x = 0; x < itemCount; x++)
				if(items[x].Name == name)
				{
					i = x;
					break;
				}
			if(i != -1)
				RemoveAt(i);
		}

		public void RemoveAt(int index)
		{
			for(int x = index + 1; x <= itemCount - 1; x++)
				items[x-1] = items[x];
			items[itemCount - 1] = null;
			itemCount--;
		}

		public void MoveUp(EnumItem f)
		{
			int i = IndexOf(f);
			
			// Don't do anything if this field is already on top
			if(i == 0)
				return;
			
			EnumItem swap = items[i-1];
			items[i-1] = f;
			items[i] = swap;
		}

		public void MoveDown(EnumItem f)
		{
			int i = IndexOf(f);
			
			// Don't do anything if this field is already on bottom
			if(i == this.Count - 1)
				return;
			
			EnumItem swap = items[i+1];
			items[i+1] = f;
			items[i] = swap;
		}

		public int Count
		{
			get
			{
				return itemCount;
			}
		}

		public bool IsSynchronized
		{
			get
			{
				return items.IsSynchronized;
			}
		}

		public object SyncRoot
		{
			get
			{
				return items;
			}
		}

		public void CopyTo(Array array, int index)
		{
			items.CopyTo(array, index);
		}

		public Enumerator GetEnumerator()
		{
			return new Enumerator(items, itemCount);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public class Enumerator : IEnumerator
		{
			private EnumItem[] items;
			private int cursor;
			private int virtualLength;

			public Enumerator(EnumItem[] items, int virtualLength)
			{
				this.items = items;
				this.virtualLength = virtualLength;
				cursor = -1;
			}

			public void Reset()
			{
				cursor = -1;
			}

			public bool MoveNext()
			{
				if(cursor < items.Length)
					cursor++;
				return(!(cursor == virtualLength));
			}

			public EnumItem Current
			{
				get
				{
					if((cursor < 0) || (cursor == virtualLength))
						throw(new InvalidOperationException());
					return items[cursor];
				}
			}

			Object IEnumerator.Current
			{
				get
				{
					return Current;
				}
			}
		}

		object ICloneable.Clone()
		{
			return Clone();
		}

		public EnumItemCollection Clone()
		{
			EnumItemCollection clonedEnumItem = new EnumItemCollection(itemCount);
			foreach(EnumItem item in this)
				clonedEnumItem.Add(item);
			return clonedEnumItem;
		}

		public EnumItemCollection Copy()
		{
			EnumItemCollection clonedEnumItem = new EnumItemCollection(itemCount);
			foreach(EnumItem item in this)
				clonedEnumItem.Add(item.Clone());
			return clonedEnumItem;
		}

		public string GenerateName()
		{
			int newIndex = 0;
			
			string name;
			int nameIndex;

			for(int x = 0; x < this.itemCount; x++)
			{
				name = items[x].Name;
				if(name.StartsWith("Item")& name.Length > 5)
				{
					try
					{
						nameIndex = int.Parse(name.Substring(5, name.Length - 5));
					}
					catch
					{
						continue;
					}

					if(nameIndex >= newIndex)
                        newIndex = nameIndex + 1;
				}
			}

			return "Item" + newIndex.ToString();
		}

		public void Sort()
		{
			Array.Sort(items);
		}
	}
}