using System;
using System.Collections;

namespace NitroCast.Core
{
	/// <summary>
	/// Summary of MyClass
	/// </summary>
	public class ReferenceEntryCollection : IList, ICloneable 
	{
		private int itemCount = 0;
		internal ReferenceEntry[] ReferenceEntryArray ;

		public ReferenceEntryCollection() : base()
		{
			ReferenceEntryArray = new ReferenceEntry[15];
		}

		public ReferenceEntryCollection(int capacity) : base()
		{
			ReferenceEntryArray = new ReferenceEntry[capacity];
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
				ReferenceEntryArray[index] = (ReferenceEntry) value;
			}
		}

		public ReferenceEntry this[int index]
		{
			get
			{
				if(index > itemCount - 1)
					throw(new Exception("Index out of bounds."));
				return ReferenceEntryArray[index];
			}
			set
			{
				ReferenceEntryArray[index] = value;
			}
		}

		int IList.Add(object value)
		{
			return Add((ReferenceEntry) value);
		}

		public int Add(ReferenceEntry value)
		{
			itemCount++;
			if(itemCount > ReferenceEntryArray.GetUpperBound(0) + 1)
			{
				ReferenceEntry[] tempReferenceEntryArray = new ReferenceEntry[itemCount * 2];
				for(int x = 0; x <= ReferenceEntryArray.GetUpperBound(0); x++)
					tempReferenceEntryArray[x] = ReferenceEntryArray[x];
				ReferenceEntryArray = tempReferenceEntryArray;
			}
			ReferenceEntryArray[itemCount - 1] = value;
			return itemCount -1;
		}

		public void Clear()
		{
			itemCount = 0;
			ReferenceEntryArray = new ReferenceEntry[15];
		}

		bool IList.Contains(object value)
		{
			return Contains((ReferenceEntry) value);
		}

		public bool Contains(ReferenceEntry value)
		{
			return IndexOf(value) != -1;
		}

		int IList.IndexOf(object value)
		{
			return IndexOf((ReferenceEntry) value);
		}

		public int IndexOf(ReferenceEntry value)
		{
			for(int x = 0; x < itemCount; x++)
				if(ReferenceEntryArray[x].Equals(value))
					return x;
			return -1;
		}

		void IList.Insert(int index, object value)
		{
			Insert(index, (ReferenceEntry) value);
		}

		public void Insert(int index, ReferenceEntry value)
		{
			itemCount++;
			if(itemCount > ReferenceEntryArray.Length)
				for(int x = index + 1; x == itemCount - 2; x ++)
					ReferenceEntryArray[x] = ReferenceEntryArray[x - 1];
			ReferenceEntryArray[index] = value;
		}

		void IList.Remove(object value)
		{
			Remove((ReferenceEntry) value);
		}

		public void Remove(ReferenceEntry value)
		{
			int index = IndexOf(value);
			if(index == -1)
				throw(new Exception("ReferenceEntry not found in collection."));
			RemoveAt(index);
		}

		public void RemoveAt(int index)
		{
			for(int x = index + 1; x <= itemCount - 1; x++)
				ReferenceEntryArray[x-1] = ReferenceEntryArray[x];
			ReferenceEntryArray[itemCount - 1] = null;
			itemCount--;
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
				return ReferenceEntryArray.IsSynchronized;
			}
		}

		public object SyncRoot
		{
			get
			{
				return ReferenceEntryArray;
			}
		}

		public void CopyTo(Array array, int index)
		{
			ReferenceEntryArray.CopyTo(array, index);
		}

		public Enumerator GetEnumerator()
		{
			return new Enumerator(ReferenceEntryArray, itemCount);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public class Enumerator : IEnumerator
		{
			private ReferenceEntry[] ReferenceEntryArray;
			private int virtualLength;
			private int cursor;

			public Enumerator(ReferenceEntry[] ReferenceEntryArray, int virtualLength)
			{
				this.ReferenceEntryArray = ReferenceEntryArray;
				this.virtualLength = virtualLength;
				cursor = -1;
			}

			public void Reset()
			{
				cursor = -1;
			}

			public bool MoveNext()
			{
				if(cursor < ReferenceEntryArray.Length)
					cursor++;
				return(!(cursor == virtualLength));
			}

			public ReferenceEntry Current
			{
				get
				{
					if((cursor < 0) || (cursor == virtualLength))
						throw(new InvalidOperationException());
					return ReferenceEntryArray[cursor];
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

		public ReferenceEntryCollection Clone()
		{
			ReferenceEntryCollection clonedReferenceEntry = new ReferenceEntryCollection(itemCount);
			foreach(ReferenceEntry item in this)
				clonedReferenceEntry.Add(item);
			return clonedReferenceEntry;
		}

		public void Sort()
		{
			Array.Sort(ReferenceEntryArray);
		}
	}
}
