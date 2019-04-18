using System;
using System.Collections;

namespace NitroCast.Core
{
	/// <summary>
	/// Summary of MyClass
	/// </summary>
	public class MetaAttributeCollection : IList, ICloneable 
	{
		private int itemCount = 0 ;
		private MetaAttribute[] attributes ;

		public MetaAttributeCollection() : base()
		{
			attributes = new MetaAttribute[15];
		}

		public MetaAttributeCollection(int capacity) : base()
		{
			attributes = new MetaAttribute[capacity];
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
				attributes[index] = (MetaAttribute) value;
			}
		}

        public string this[string name]
        {
            get
            {
                for (int i = 0; i < Count; i++)
                {
                    if (attributes[i].Name == name)
                    {
                        return attributes[i].Value;
                    }
                }

                return string.Empty;
            }
            set
            {
                for (int i = 0; i < Count; i++)
                {
                    if (attributes[i].Name == name)
                    {
                        attributes[i].Value = value;
                        return;
                    }
                }

                Add(new MetaAttribute(name, value));
            }
        }

        public MetaAttribute this[int index]
		{
			get
			{
				if(index > itemCount - 1)
					throw(new Exception("Index out of bounds."));
				return attributes[index];
			}
			set
			{
				attributes[index] = value;
			}
		}

		int IList.Add(object value)
		{
			return Add((MetaAttribute) value);
		}

		public int Add(MetaAttribute value)
		{
			itemCount++;
			if(itemCount > attributes.GetUpperBound(0) + 1)
			{
				MetaAttribute[] tempMetaAttributeArray = new MetaAttribute[itemCount * 2];
				for(int x = 0; x <= attributes.GetUpperBound(0); x++)
					tempMetaAttributeArray[x] = attributes[x];
				attributes = tempMetaAttributeArray;
			}
			attributes[itemCount - 1] = value;
			return itemCount -1;
		}

		public void Clear()
		{
			itemCount = 0;
			attributes = new MetaAttribute[15];
		}

		bool IList.Contains(object value)
		{
			return Contains((MetaAttribute) value);
		}

		public bool Contains(MetaAttribute value)
		{
			return IndexOf(value) != -1;
		}

		int IList.IndexOf(object value)
		{
			return IndexOf((MetaAttribute) value);
		}

		public int IndexOf(MetaAttribute value)
		{
			for(int x = 0; x < itemCount; x++)
				if(attributes[x].Equals(value))
					return x;
			return -1;
		}

		void IList.Insert(int index, object value)
		{
			Insert(index, (MetaAttribute) value);
		}

		public void Insert(int index, MetaAttribute value)
		{
			itemCount++;
			if(itemCount > attributes.Length)
				for(int x = index + 1; x == itemCount - 2; x ++)
					attributes[x] = attributes[x - 1];
			attributes[index] = value;
		}

		void IList.Remove(object value)
		{
			Remove((MetaAttribute) value);
		}

		public void Remove(MetaAttribute value)
		{
			int index = IndexOf(value);
			if(index == -1)
				throw(new Exception("MetaAttribute not found in collection."));
			RemoveAt(index);
		}

		public void RemoveAt(int index)
		{
			for(int x = index + 1; x <= itemCount - 1; x++)
				attributes[x-1] = attributes[x];
			attributes[itemCount - 1] = null;
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
				return attributes.IsSynchronized;
			}
		}

		public object SyncRoot
		{
			get
			{
				return attributes;
			}
		}

		public void CopyTo(Array array, int index)
		{
			attributes.CopyTo(array, index);
		}

		public Enumerator GetEnumerator()
		{
			return new Enumerator(attributes, itemCount);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public class Enumerator : IEnumerator
		{
			private MetaAttribute[] MetaAttributeArray;
			private int virtualLength;
			private int cursor;

			public Enumerator(MetaAttribute[] MetaAttributeArray, int virtualLength)
			{
				this.MetaAttributeArray = MetaAttributeArray;
				this.virtualLength = virtualLength;
				cursor = -1;
			}

			public void Reset()
			{
				cursor = -1;
			}

			public bool MoveNext()
			{
				if(cursor < MetaAttributeArray.Length)
					cursor++;
				return(!(cursor == virtualLength));
			}

			public MetaAttribute Current
			{
				get
				{
					if((cursor < 0) || (cursor == virtualLength))
						throw(new InvalidOperationException());
					return MetaAttributeArray[cursor];
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

		public MetaAttributeCollection Clone()
		{
			MetaAttributeCollection clonedMetaAttribute = new MetaAttributeCollection(itemCount);
			foreach(MetaAttribute item in this)
				clonedMetaAttribute.Add(item);
			return clonedMetaAttribute;
		}
	}
}