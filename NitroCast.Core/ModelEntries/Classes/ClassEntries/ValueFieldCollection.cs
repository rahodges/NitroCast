using System;
using System.Collections;

namespace NitroCast.Core
{
	/// <summary>
	/// Summary of MyClass
	/// </summary>
	[Serializable()]
	public class ValueFieldCollection : IList, ICloneable 
	{
		private int itemCount = 0;
		internal ValueField[] FieldEntryArray ;

		public ValueFieldCollection() : base()
		{
			FieldEntryArray = new ValueField[15];
		}

		public ValueFieldCollection(int capacity) : base()
		{
			FieldEntryArray = new ValueField[capacity];
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
				FieldEntryArray[index] = (ValueField) value;
			}
		}

		public ValueField this[int index]
		{
			get
			{
				if(index > itemCount - 1)
					throw(new Exception("Index out of bounds."));
				return FieldEntryArray[index];
			}
			set
			{
				FieldEntryArray[index] = value;
			}
		}

		int IList.Add(object value)
		{
			return Add((ValueField) value);
		}

		public int Add(ValueField value)
		{
			itemCount++;
			if(itemCount > FieldEntryArray.GetUpperBound(0) + 1)
			{
				ValueField[] tempFieldEntryArray = new ValueField[itemCount * 2];
				for(int x = 0; x <= FieldEntryArray.GetUpperBound(0); x++)
					tempFieldEntryArray[x] = FieldEntryArray[x];
				FieldEntryArray = tempFieldEntryArray;
			}
			FieldEntryArray[itemCount - 1] = value;
			return itemCount -1;
		}

		public void Clear()
		{
			itemCount = 0;
			FieldEntryArray = new ValueField[15];
		}

		bool IList.Contains(object value)
		{
			return Contains((ValueField) value);
		}

		public bool Contains(ValueField value)
		{
			return IndexOf(value) != -1;
		}

		public bool Contains(string name)
		{
			for(int x = 0; x < itemCount; x++)
				if(FieldEntryArray[x].Name == name)
					return true;
			return false;
		}

		int IList.IndexOf(object value)
		{
			return IndexOf((ValueField) value);
		}

		public int IndexOf(ValueField value)
		{
			for(int x = 0; x < itemCount; x++)
				if(FieldEntryArray[x].Equals(value))
					return x;
			return -1;
		}

		void IList.Insert(int index, object value)
		{
			Insert(index, (ValueField) value);
		}

		public void Insert(int index, ValueField value)
		{
			itemCount++;
			if(itemCount > FieldEntryArray.Length)
				for(int x = index + 1; x == itemCount - 2; x ++)
					FieldEntryArray[x] = FieldEntryArray[x - 1];
			FieldEntryArray[index] = value;
		}

		void IList.Remove(object value)
		{
			Remove((ValueField) value);
		}

		public void Remove(ValueField value)
		{
			int index = IndexOf(value);
			if(index == -1)
				throw(new Exception("FieldEntry not found in collection."));
			RemoveAt(index);
		}

		public void Remove(string name)
		{
			int i = -1;
			for(int x = 0; x < itemCount; x++)
				if(FieldEntryArray[x].Name == name)
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
				FieldEntryArray[x-1] = FieldEntryArray[x];
			FieldEntryArray[itemCount - 1] = null;
			itemCount--;
		}

		public void MoveUp(ValueField f)
		{
			int i = IndexOf(f);
			
			// Don't do anything if this field is already on top
			if(i == 0)
				return;
			
			ValueField swap = FieldEntryArray[i-1];
			FieldEntryArray[i-1] = f;
			FieldEntryArray[i] = swap;
		}

		public void MoveDown(ValueField f)
		{
			int i = IndexOf(f);
			
			// Don't do anything if this field is already on bottom
			if(i == this.Count - 1)
				return;
			
			ValueField swap = FieldEntryArray[i+1];
			FieldEntryArray[i+1] = f;
			FieldEntryArray[i] = swap;
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
				return FieldEntryArray.IsSynchronized;
			}
		}

		public object SyncRoot
		{
			get
			{
				return FieldEntryArray;
			}
		}

		public void CopyTo(Array array, int index)
		{
			FieldEntryArray.CopyTo(array, index);
		}

		public Enumerator GetEnumerator()
		{
			return new Enumerator(FieldEntryArray, itemCount);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public class Enumerator : IEnumerator
		{
			private ValueField[] FieldEntryArray;
			private int cursor;
			private int virtualLength;

			public Enumerator(ValueField[] FieldEntryArray, int virtualLength)
			{
				this.FieldEntryArray = FieldEntryArray;
				this.virtualLength = virtualLength;
				cursor = -1;
			}

			public void Reset()
			{
				cursor = -1;
			}

			public bool MoveNext()
			{
				if(cursor < FieldEntryArray.Length)
					cursor++;
				return(!(cursor == virtualLength));
			}

			public ValueField Current
			{
				get
				{
					if((cursor < 0) || (cursor == virtualLength))
						throw(new InvalidOperationException());
					return FieldEntryArray[cursor];
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

		public ValueFieldCollection Clone()
		{
			ValueFieldCollection clonedFieldEntry = new ValueFieldCollection(itemCount);
			foreach(ValueField item in this)
				clonedFieldEntry.Add(item);
			return clonedFieldEntry;
		}

		public ValueFieldCollection Copy()
		{
			ValueFieldCollection clonedFieldEntry = new ValueFieldCollection(itemCount);
			foreach(ValueField item in this)
				clonedFieldEntry.Add(item.Clone());
			return clonedFieldEntry;
		}

		public string GenerateName()
		{
			int newIndex = 0;
			
			string fieldName;
			int fieldIndex;

			for(int x = 0; x < this.itemCount; x++)
			{
				fieldName = FieldEntryArray[x].Name;
				if(fieldName.StartsWith("Field")
					& fieldName.Length > 5)
				{
					try
					{
						fieldIndex = int.Parse(fieldName.Substring(5, fieldName.Length - 5));
					}
					catch
					{
						continue;
					}

					if(fieldIndex >= newIndex)
                        newIndex = fieldIndex + 1;
				}
			}

			return "Field" + newIndex.ToString();
		}

		public void Sort()
		{
			Array.Sort(FieldEntryArray);
		}
	}
}