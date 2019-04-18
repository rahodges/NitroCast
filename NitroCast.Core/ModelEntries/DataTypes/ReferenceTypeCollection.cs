using System;
using System.Collections;

namespace NitroCast.Core
{
	/// <summary>
	/// Summary of MyClass
	/// </summary>
	public class ReferenceTypeCollection : IList, ICloneable 
	{
		private int itemCount = 0;
		private ReferenceType[] ChildDataTypeArray ;

		public ReferenceTypeCollection() : base()
		{
			ChildDataTypeArray = new ReferenceType[15];
		}

		public ReferenceTypeCollection(int capacity) : base()
		{
			ChildDataTypeArray = new ReferenceType[capacity];
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
				ChildDataTypeArray[index] = (ReferenceType) value;
			}
		}

		public ReferenceType this[int index]
		{
			get
			{
				if(index > itemCount - 1)
					throw(new Exception("Index out of bounds."));
				return ChildDataTypeArray[index];
			}
			set
			{
				ChildDataTypeArray[index] = value;
			}
		}

		int IList.Add(object value)
		{
			return Add((ReferenceType) value);
		}

		public int Add(ReferenceType value)
		{
			itemCount++;
			if(itemCount > ChildDataTypeArray.GetUpperBound(0) + 1)
			{
				ReferenceType[] tempChildDataTypeArray = new ReferenceType[itemCount * 2];
				for(int x = 0; x <= ChildDataTypeArray.GetUpperBound(0); x++)
					tempChildDataTypeArray[x] = ChildDataTypeArray[x];
				ChildDataTypeArray = tempChildDataTypeArray;
			}
			ChildDataTypeArray[itemCount - 1] = value;
			return itemCount -1;
		}

		public void Clear()
		{
			itemCount = 0;
			ChildDataTypeArray = new ReferenceType[15];
		}

		bool IList.Contains(object value)
		{
			return Contains((ReferenceType) value);
		}

		public bool Contains(ReferenceType value)
		{
			return IndexOf(value) != -1;
		}

		int IList.IndexOf(object value)
		{
			return IndexOf((ReferenceType) value);
		}

		public int IndexOf(ReferenceType value)
		{
			for(int x = 0; x < itemCount; x++)
				if(ChildDataTypeArray[x].Equals(value))
					return x;
			return -1;
		}

		void IList.Insert(int index, object value)
		{
			Insert(index, (ReferenceType) value);
		}

		public void Insert(int index, ReferenceType value)
		{
			itemCount++;
			if(itemCount > ChildDataTypeArray.Length)
				for(int x = index + 1; x == itemCount - 2; x ++)
					ChildDataTypeArray[x] = ChildDataTypeArray[x - 1];
			ChildDataTypeArray[index] = value;
		}

		void IList.Remove(object value)
		{
			Remove((ReferenceType) value);
		}

		public void Remove(ReferenceType value)
		{
			int index = IndexOf(value);
			if(index == -1)
				throw(new Exception("ChildDataType not found in collection."));
			RemoveAt(index);
		}

		public void RemoveAt(int index)
		{
			for(int x = index + 1; x <= itemCount - 1; x++)
				ChildDataTypeArray[x] = ChildDataTypeArray[x-1];
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
				return ChildDataTypeArray.IsSynchronized;
			}
		}

		public object SyncRoot
		{
			get
			{
				return ChildDataTypeArray;
			}
		}

		public void CopyTo(Array array, int arrayIndex)
		{
			for(int x = 0; x < itemCount; x++)
				array.SetValue(ChildDataTypeArray[x], x);
		}

		public Enumerator GetEnumerator()
		{
			return new Enumerator(ChildDataTypeArray, itemCount);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public class Enumerator : IEnumerator
		{
			private ReferenceType[] ChildDataTypeArray;
			private int virtualLength;
			private int cursor;

			public Enumerator(ReferenceType[] ChildDataTypeArray, int virtualLength)
			{
				this.ChildDataTypeArray = ChildDataTypeArray;
				this.virtualLength = virtualLength;
				cursor = -1;
			}

			public void Reset()
			{
				cursor = -1;
			}

			public bool MoveNext()
			{
				if(cursor < ChildDataTypeArray.Length)
					cursor++;
				return(!(cursor == virtualLength));
			}

			public ReferenceType Current
			{
				get
				{
					if((cursor < 0) || (cursor == virtualLength))
						throw(new InvalidOperationException());
					return ChildDataTypeArray[cursor];
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

		public void Sort(params ModelEntryCompareKey[] keys)
		{
			if(this.Count > 0)
				Array.Sort(this.ChildDataTypeArray, 0, this.Count, new ModelEntryComparer(keys));
		}

		object ICloneable.Clone()
		{
			return Clone();
		}

		public ReferenceTypeCollection Clone()
		{
			ReferenceTypeCollection clonedChildDataType = new ReferenceTypeCollection(itemCount);
			foreach(ReferenceType item in this)
				clonedChildDataType.Add(item);
			return clonedChildDataType;
		}
	}
}
