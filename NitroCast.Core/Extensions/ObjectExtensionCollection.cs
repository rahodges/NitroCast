using System;
using System.Collections;

namespace NitroCast.Core.Extensions
{
	/// <summary>
	/// Summary of MyClass
	/// </summary>
    public class ObjectExtensionCollection : IList, ICloneable 
	{
		private int itemCount = 0;
		private ObjectExtension[] extensions ;

		public ObjectExtensionCollection() : base()
		{
			extensions = new ObjectExtension[15];
		}

		public ObjectExtensionCollection(int capacity) : base()
		{
			extensions = new ObjectExtension[capacity];
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
				extensions[index] = (ObjectExtension) value;
			}
		}

		public ObjectExtension this[int index]
		{
			get
			{
				if(index > itemCount - 1)
					throw(new Exception("Index out of bounds."));
				return extensions[index];
			}
			set
			{
				extensions[index] = value;
			}
		}

		int IList.Add(object value)
		{
			return Add((ObjectExtension) value);
		}

		public int Add(ObjectExtension value)
		{
			itemCount++;
			if(itemCount > extensions.GetUpperBound(0) + 1)
			{
				ObjectExtension[] tempExtensions = new ObjectExtension[itemCount * 2];
				for(int x = 0; x <= extensions.GetUpperBound(0); x++)
					tempExtensions[x] = extensions[x];
				extensions = tempExtensions;
			}
			extensions[itemCount - 1] = value;
			return itemCount -1;
		}

		public void Clear()
		{
			itemCount = 0;
			extensions = new ObjectExtension[15];
		}

		bool IList.Contains(object value)
		{
			return Contains((ObjectExtension) value);
		}

		public bool Contains(ObjectExtension value)
		{
			return IndexOf(value) != -1;
		}

		int IList.IndexOf(object value)
		{
			return IndexOf((ObjectExtension) value);
		}

		public int IndexOf(ObjectExtension value)
		{
			for(int x = 0; x < itemCount; x++)
				if(extensions[x].Equals(value))
					return x;
			return -1;
		}

		void IList.Insert(int index, object value)
		{
			Insert(index, (ObjectExtension) value);
		}

		public void Insert(int index, ObjectExtension value)
		{
			itemCount++;
			if(itemCount > extensions.Length)
				for(int x = index + 1; x == itemCount - 2; x ++)
					extensions[x] = extensions[x - 1];
			extensions[index] = value;
		}

		void IList.Remove(object value)
		{
			Remove((ObjectExtension) value);
		}

		public void Remove(ObjectExtension value)
		{
			int index = IndexOf(value);
			if(index == -1)
				throw(new Exception("ObjectExtension not found in collection."));
			RemoveAt(index);
		}

		public void RemoveAt(int index)
		{
			for(int x = index + 1; x <= itemCount - 1; x++)
				extensions[x-1] = extensions[x];
			extensions[itemCount - 1] = null;
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
				return extensions.IsSynchronized;
			}
		}

		public object SyncRoot
		{
			get
			{
				return extensions;
			}
		}

		public void CopyTo(Array array, int index)
		{
			extensions.CopyTo(array, index);
		}

		public Enumerator GetEnumerator()
		{
			return new Enumerator(extensions, itemCount);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public class Enumerator : IEnumerator
		{
			private ObjectExtension[] extensions;
			private int virtualLength;
			private int cursor;

			public Enumerator(ObjectExtension[] extensions, int virtualLength)
			{
				this.extensions = extensions;
				this.virtualLength = virtualLength;
				cursor = -1;
			}

			public void Reset()
			{
				cursor = -1;
			}

			public bool MoveNext()
			{
				if(cursor < extensions.Length)
					cursor++;
				return(!(cursor == virtualLength));
			}

			public ObjectExtension Current
			{
				get
				{
					if((cursor < 0) || (cursor == virtualLength))
						throw(new InvalidOperationException());
					return extensions[cursor];
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

		public ObjectExtensionCollection Clone()
		{
			ObjectExtensionCollection clonedObjectExtensions = 
				new ObjectExtensionCollection(itemCount);
			foreach(ObjectExtension item in this)
				clonedObjectExtensions.Add(item);
			return clonedObjectExtensions;
		}
	}
}
