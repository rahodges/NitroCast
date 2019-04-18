using System;
using System.Collections;

namespace NitroCast.Core.Extensions
{
	/// <summary>
	/// Summary of MyClass
	/// </summary>
	public class ClassOutputConnectorCollection : IList, ICloneable 
	{
		private int itemCount = 0;
		private OutputExtensionConnector[] extensions ;

		public ClassOutputConnectorCollection() : base()
		{
			extensions = new OutputExtensionConnector[15];
		}

		public ClassOutputConnectorCollection(int capacity) : base()
		{
			extensions = new OutputExtensionConnector[capacity];
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
				extensions[index] = (OutputExtensionConnector) value;
			}
		}

		public OutputExtensionConnector this[int index]
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
			return Add((OutputExtensionConnector) value);
		}

		public int Add(OutputExtensionConnector value)
		{
			itemCount++;
			if(itemCount > extensions.GetUpperBound(0) + 1)
			{
				OutputExtensionConnector[] tempExtensions = new OutputExtensionConnector[itemCount * 2];
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
			extensions = new OutputExtensionConnector[15];
		}

		bool IList.Contains(object value)
		{
			return Contains((OutputExtensionConnector) value);
		}

		public bool Contains(OutputExtensionConnector value)
		{
			return IndexOf(value) != -1;
		}

		int IList.IndexOf(object value)
		{
			return IndexOf((OutputExtensionConnector) value);
		}

		public int IndexOf(OutputExtensionConnector value)
		{
			for(int x = 0; x < itemCount; x++)
				if(extensions[x].Equals(value))
					return x;
			return -1;
		}

		void IList.Insert(int index, object value)
		{
			Insert(index, (OutputExtensionConnector) value);
		}

		public void Insert(int index, OutputExtensionConnector value)
		{
			itemCount++;
			if(itemCount > extensions.Length)
				for(int x = index + 1; x == itemCount - 2; x ++)
					extensions[x] = extensions[x - 1];
			extensions[index] = value;
		}

		void IList.Remove(object value)
		{
			Remove((OutputExtensionConnector) value);
		}

		public void Remove(OutputExtensionConnector value)
		{
			int index = IndexOf(value);
			if(index == -1)
				throw(new Exception("ClassOutputConnector not found in collection."));
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
			private OutputExtensionConnector[] extensions;
			private int virtualLength;
			private int cursor;

			public Enumerator(OutputExtensionConnector[] extensions, int virtualLength)
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

			public OutputExtensionConnector Current
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

		public ClassOutputConnectorCollection Clone()
		{
			ClassOutputConnectorCollection clonedClassOutputConnectors = 
				new ClassOutputConnectorCollection(itemCount);
			foreach(OutputExtensionConnector item in this)
				clonedClassOutputConnectors.Add(item);
			return clonedClassOutputConnectors;
		}
	}
}
