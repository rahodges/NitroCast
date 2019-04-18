using System;
using System.Collections;

namespace NitroCast.Core
{
	/// <summary>
	/// Summary of MyClass
	/// </summary>
	public class ValueTypeCollection : IList, ICloneable 
	{
		private int itemCount = 0;
		internal ValueType[] fields;

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

		public ValueTypeCollection() : base()
		{
			fields = new ValueType[15];
		}

		public ValueTypeCollection(int capacity) : base()
		{
			fields = new ValueType[capacity];
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
				fields[index] = (ValueType) value;
			}
		}

		public ValueType this[int index]
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

		public ValueType this[string name]
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
			return Add((ValueType) value);
		}

		public int Add(ValueType value)
		{			
			itemCount++;
			if(itemCount > fields.GetUpperBound(0) + 1)
			{
				ValueType[] tempFields = new ValueType[itemCount * 2];
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
			fields = new ValueType[15];
		}

		bool IList.Contains(object value)
		{
			return Contains((ValueType) value);
		}

		public bool Contains(ValueType value)
		{
			return IndexOf(value) != -1;
		}

		int IList.IndexOf(object value)
		{
			return IndexOf((ValueType) value);
		}

		public int IndexOf(ValueType value)
		{
			for(int x = 0; x < itemCount; x++)
				if(fields[x].Equals(value))
					return x;
			return -1;
		}

		void IList.Insert(int index, object value)
		{
			Insert(index, (ValueType) value);
		}

		public void Insert(int index, ValueType value)
		{
			itemCount++;
			if(itemCount > fields.Length)
				for(int x = index + 1; x == itemCount - 2; x ++)
					fields[x] = fields[x - 1];
			fields[index] = value;
		}

		void IList.Remove(object value)
		{
			Remove((ValueType) value);
		}

		public void Remove(ValueType value)
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
			private ValueType[] FieldDataTypeArray;
			private int virtualLength;
			private int cursor;

			public Enumerator(ValueType[] FieldDataTypeArray, int virtualLength)
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

			public ValueType Current
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

		public ValueTypeCollection Clone()
		{
			ValueTypeCollection clonedFieldDataType = new ValueTypeCollection(itemCount);
			foreach(ValueType item in this)
				clonedFieldDataType.Add(item);
			return clonedFieldDataType;
		}
	}
}
