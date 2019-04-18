using System;
using System.Collections;

namespace NitroCast.Core
{
	/// <summary>
	/// Summary of MyClass
	/// </summary>
	public class ReferenceFieldCollection : IList, ICloneable 
	{
		private int itemCount = 0;
		internal ReferenceField[] ChildEntryArray ;

		public ReferenceFieldCollection() : base()
		{
			ChildEntryArray = new ReferenceField[15];
		}

		public ReferenceFieldCollection(int capacity) : base()
		{
			ChildEntryArray = new ReferenceField[capacity];
		}

		public int LastOneToManyRelation
		{
			get
			{
				int lastRelation = -1;
				for(int x = 0; x < itemCount; x++)
					if(!ChildEntryArray[x].HasChildrenTables)
						lastRelation = x;
				return lastRelation;
			}
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
				ChildEntryArray[index] = (ReferenceField) value;
			}
		}

		public ReferenceField this[int index]
		{
			get
			{
				if(index > itemCount - 1)
					throw(new Exception("Index out of bounds."));
				return ChildEntryArray[index];
			}
			set
			{
				ChildEntryArray[index] = value;
			}
		}

		int IList.Add(object value)
		{
			return Add((ReferenceField) value);
		}

		public int Add(ReferenceField value)
		{
			itemCount++;
			if(itemCount > ChildEntryArray.GetUpperBound(0) + 1)
			{
				ReferenceField[] tempChildEntryArray = new ReferenceField[itemCount * 2];
				for(int x = 0; x <= ChildEntryArray.GetUpperBound(0); x++)
					tempChildEntryArray[x] = ChildEntryArray[x];
				ChildEntryArray = tempChildEntryArray;
			}
			ChildEntryArray[itemCount - 1] = value;
			return itemCount -1;
		}

		public void Clear()
		{
			itemCount = 0;
			ChildEntryArray = new ReferenceField[15];
		}

		bool IList.Contains(object value)
		{
			return Contains((ReferenceField) value);
		}

		public bool Contains(ReferenceField value)
		{
			return IndexOf(value) != -1;
		}

		int IList.IndexOf(object value)
		{
			return IndexOf((ReferenceField) value);
		}

		public int IndexOf(ReferenceField value)
		{
			for(int x = 0; x < itemCount; x++)
				if(ChildEntryArray[x].Equals(value))
					return x;
			return -1;
		}

		void IList.Insert(int index, object value)
		{
			Insert(index, (ReferenceField) value);
		}

		public void Insert(int index, ReferenceField value)
		{
			itemCount++;
			if(itemCount > ChildEntryArray.Length)
				for(int x = index + 1; x == itemCount - 2; x ++)
					ChildEntryArray[x] = ChildEntryArray[x - 1];
			ChildEntryArray[index] = value;
		}

		void IList.Remove(object value)
		{
			Remove((ReferenceField) value);
		}

		public void Remove(ReferenceField value)
		{
			int index = IndexOf(value);
			if(index == -1)
				throw(new Exception("ChildEntry not found in collection."));
			RemoveAt(index);
		}

		public void RemoveAt(int index)
		{
			for(int x = index + 1; x <= itemCount - 1; x++)
				ChildEntryArray[x-1] = ChildEntryArray[x];
			ChildEntryArray[itemCount-1] = null;
			itemCount--;
		}

		public void MoveUp(ReferenceField c)
		{
			int i = IndexOf(c);
			
			// Don't do anything if this field is already on top
			if(i == 0)
				return;
			
			ReferenceField swap = ChildEntryArray[i-1];
			ChildEntryArray[i-1] = c;
			ChildEntryArray[i] = swap;
		}

		public void MoveDown(ReferenceField c)
		{
			int i = IndexOf(c);
			
			// Don't do anything if this field is already on bottom
			if(i == this.Count - 1)
				return;
			
			ReferenceField swap = ChildEntryArray[i+1];
			ChildEntryArray[i+1] = c;
			ChildEntryArray[i] = swap;
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
				return ChildEntryArray.IsSynchronized;
			}
		}

		public object SyncRoot
		{
			get
			{
				return ChildEntryArray;
			}
		}

		public void CopyTo(Array array, int index)
		{
			ChildEntryArray.CopyTo(array, index);
		}

		public Enumerator GetEnumerator()
		{
			return new Enumerator(ChildEntryArray, itemCount);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public class Enumerator : IEnumerator
		{
			private ReferenceField[] ChildEntryArray;
			private int virtualLength;
			private int cursor;

			public Enumerator(ReferenceField[] ChildEntryArray, int virtualLength)
			{
				this.ChildEntryArray = ChildEntryArray;
				this.virtualLength = virtualLength;
				cursor = -1;
			}

			public void Reset()
			{
				cursor = -1;
			}

			public bool MoveNext()
			{
				if(cursor < ChildEntryArray.Length)
					cursor++;
				return(!(cursor == virtualLength));
			}

			public ReferenceField Current
			{
				get
				{
					if((cursor < 0) || (cursor == virtualLength))
						throw(new InvalidOperationException());
					return ChildEntryArray[cursor];
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

		public ReferenceFieldCollection Clone()
		{
			ReferenceFieldCollection clonedChildEntry = new ReferenceFieldCollection(itemCount);
			foreach(ReferenceField item in this)
				clonedChildEntry.Add(item);
			return clonedChildEntry;
		}

		public ReferenceFieldCollection Copy()
		{
			ReferenceFieldCollection clonedChildEntry = new ReferenceFieldCollection(itemCount);
			foreach(ReferenceField item in this)
				clonedChildEntry.Add(item.Clone());
			return clonedChildEntry;
		}

		public string GenerateName()
		{
			int newIndex = 0;
			
			string childName;
			int childIndex;

			for(int x = 0; x < this.itemCount; x++)
			{
				childName = ChildEntryArray[x].Name;
				if(childName.StartsWith("Child")
					& childName.Length > 5)
				{
					try
					{
						childIndex = int.Parse(childName.Substring(5, childName.Length - 5));
					}
					catch
					{
						continue;
					}

					if(childIndex >= newIndex)
						newIndex = childIndex + 1;
				}
			}

			return "Child" + newIndex.ToString();
		}

		public void Sort()
		{
			Array.Sort(ChildEntryArray);
		}
	}
}
