using System;
using System.Collections;

namespace NitroCast.Core
{
	/// <summary>
	/// Summary of MyClass
	/// </summary>
	public class EnumTypeCollection : IList, ICloneable 
	{
		private int itemCount = 0;
		internal EnumType[] fields;

		#region Events

		/// <summary>
		/// Notifies the listener when a field has been added to the collection.
		/// </summary>
		public EventHandler FieldAdded;
		public void OnAdd(EventArgs e)
		{
			if(FieldAdded != null)
				FieldAdded(this, e);
		}

		#endregion

		public EnumTypeCollection() : base()
		{
			fields = new EnumType[15];
		}

		public EnumTypeCollection(int capacity) : base()
		{
			fields = new EnumType[capacity];
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
				fields[index] = (EnumType) value;
			}
		}

		public EnumType this[int index]
		{
			get
			{
				if(index > itemCount - 1)
					throw(new Exception("Index out of bounds."));
				return fields[index];
			}
			set
			{
				fields[index] = value;
			}
		}

		public EnumType this[string name]
		{
			get
			{
				for(int x = 0; x < itemCount; x++)
					if(fields[x].Name == name)
						return fields[x];
				return null;
			}
		}

		int IList.Add(object value)
		{
			return Add((EnumType) value);
		}

		public int Add(EnumType value)
		{			
			itemCount++;
			if(itemCount > fields.GetUpperBound(0) + 1)
			{
				EnumType[] tempFields = new EnumType[itemCount * 2];
				for(int x = 0; x <= fields.GetUpperBound(0); x++)
					tempFields[x] = fields[x];
				fields = tempFields;
			}
			fields[itemCount - 1] = value;
			
			OnAdd(EventArgs.Empty);
			return itemCount -1;
		}

		public void Clear()
		{
			itemCount = 0;
			fields = new EnumType[15];
		}

		bool IList.Contains(object value)
		{
			return Contains((EnumType) value);
		}

		public bool Contains(EnumType value)
		{
			return IndexOf(value) != -1;
		}

		int IList.IndexOf(object value)
		{
			return IndexOf((EnumType) value);
		}

		public int IndexOf(EnumType value)
		{
			for(int x = 0; x < itemCount; x++)
				if(fields[x].Equals(value))
					return x;
			return -1;
		}

		void IList.Insert(int index, object value)
		{
			Insert(index, (EnumType) value);
		}

		public void Insert(int index, EnumType value)
		{
			itemCount++;
			if(itemCount > fields.Length)
				for(int x = index + 1; x == itemCount - 2; x ++)
					fields[x] = fields[x - 1];
			fields[index] = value;
		}

		void IList.Remove(object value)
		{
			Remove((EnumType) value);
		}

		public void Remove(EnumType value)
		{
			int index = IndexOf(value);
			if(index == -1)
				throw(new Exception("FieldDataType not found in collection."));
			RemoveAt(index);
		}

		public void RemoveAt(int index)
		{
			for(int x = index + 1; x <= itemCount - 1; x++)
				fields[x-1] = fields[x];
			fields[itemCount - 1] = null;
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
				return fields.IsSynchronized;
			}
		}

		public object SyncRoot
		{
			get
			{
				return fields;
			}
		}

		public void CopyTo(Array array, int arrayIndex)
		{
			for(int x = 0; x < itemCount; x++)
				array.SetValue(fields[x], x);
		}

		public Enumerator GetEnumerator()
		{
			return new Enumerator(fields, itemCount);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public class Enumerator : IEnumerator
		{
			private EnumType[] FieldDataTypeArray;
			private int virtualLength;
			private int cursor;

			public Enumerator(EnumType[] FieldDataTypeArray, int virtualLength)
			{
				this.FieldDataTypeArray = FieldDataTypeArray;
				this.virtualLength = virtualLength;
				cursor = -1;
			}

			public void Reset()
			{
				cursor = -1;
			}

			public bool MoveNext()
			{
				if(cursor < FieldDataTypeArray.Length)
					cursor++;
				return(!(cursor == virtualLength));
			}

			public EnumType Current
			{
				get
				{
					if((cursor < 0) || (cursor == virtualLength))
						throw(new InvalidOperationException());
					return FieldDataTypeArray[cursor];
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
				Array.Sort(this.fields, 0, this.Count, new ModelEntryComparer(keys));
		}

		object ICloneable.Clone()
		{
			return Clone();
		}

		public EnumTypeCollection Clone()
		{
			EnumTypeCollection clonedFieldDataType = new EnumTypeCollection(itemCount);
			foreach(EnumType item in this)
				clonedFieldDataType.Add(item);
			return clonedFieldDataType;
		}
	}
}
