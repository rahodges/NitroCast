using System;
using System.Collections;

namespace NitroCast.Core
{
	/// <summary>
	/// Summary of MyClass
	/// </summary>
	public class ModelClassCollection : IList, ICloneable 
	{
		private int itemCount = 0;
		internal ModelClass[] items ;

		public ModelClassCollection() : base()
		{
			items = new ModelClass[15];
		}

		public ModelClassCollection(int capacity) : base()
		{
			items = new ModelClass[capacity];
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
				items[index] = (ModelClass) value;
			}
		}

		public ModelClass this[int index]
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
			OnObjectAdded(new ModelClassCollectionEventArgs((ModelClass) value));
			return Add((ModelClass) value);
			
		}

		public int Add(ModelClass value)
		{
			itemCount++;
			if(itemCount > items.GetUpperBound(0) + 1)
			{
				ModelClass[] tempClassEntryArray = new ModelClass[itemCount * 2];
				for(int x = 0; x <= items.GetUpperBound(0); x++)
					tempClassEntryArray[x] = items[x];
				items = tempClassEntryArray;
			}
			items[itemCount - 1] = value;
			OnObjectAdded(new ModelClassCollectionEventArgs(value));
			return itemCount -1;
			
		}

		public void Clear()
		{
			itemCount = 0;
			items = new ModelClass[15];
			OnCleared(EventArgs.Empty);
		}

		bool IList.Contains(object value)
		{
			return Contains((ModelClass) value);
		}

		public bool Contains(ModelClass value)
		{
			return IndexOf(value) != -1;
		}

		int IList.IndexOf(object value)
		{
			return IndexOf((ModelClass) value);
		}

		public int IndexOf(ModelClass value)
		{
			for(int x = 0; x < itemCount; x++)
				if(items[x].Equals(value))
					return x;
			return -1;
		}

		void IList.Insert(int index, object value)
		{
			OnObjectAdded(new ModelClassCollectionEventArgs((ModelClass) value));
			Insert(index, (ModelClass) value);			
		}

		public void Insert(int index, ModelClass value)
		{
			itemCount++;
			if(itemCount > items.Length)
				for(int x = index + 1; x == itemCount - 2; x ++)
					items[x] = items[x - 1];
			OnObjectAdded(new ModelClassCollectionEventArgs(value));
			items[index] = value;			
		}

		void IList.Remove(object value)
		{
			Remove((ModelClass) value);
			OnObjectRemoved(new ModelClassCollectionEventArgs((ModelClass) value));
		}

		public void Remove(ModelClass value)
		{
			int index = IndexOf(value);
			if(index == -1)
				throw(new Exception("ClassEntry not found in collection."));
			RemoveAt(index);
			OnObjectRemoved(new ModelClassCollectionEventArgs(value));
		}

		public void RemoveAt(int index)
		{
            // Must retreive reference before delete.
			OnObjectRemoved(new ModelClassCollectionEventArgs(items[index]));			
			for(int x = index + 1; x <= itemCount - 1; x++)
				items[x-1] = items[x];
			items[itemCount-1] = null;
			itemCount--;			
		}

		public void MoveUp(ModelClass c)
		{
			int i = IndexOf(c);
			
			// Don't do anything if this field is already on top
			if(i == 0)
				return;
			
			ModelClass swap = items[i-1];
			items[i-1] = c;
			items[i] = swap;
		}

		public void MoveDown(ModelClass c)
		{
			int i = IndexOf(c);
			
			// Don't do anything if this field is already on bottom
			if(i == this.Count - 1)
				return;
			
			ModelClass swap = items[i+1];
			items[i+1] = c;
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
			private ModelClass[] ClassEntryArray;
			private int virtualLength;
			private int cursor;

			public Enumerator(ModelClass[] ClassEntryArray, int virtualLength)
			{
				this.ClassEntryArray = ClassEntryArray;
				this.virtualLength = virtualLength;
				cursor = -1;
			}

			public void Reset()
			{
				cursor = -1;
			}

			public bool MoveNext()
			{
				if(cursor < ClassEntryArray.Length)
					cursor++;
				return(!(cursor == virtualLength));
			}

			public ModelClass Current
			{
				get
				{
					if((cursor < 0) || (cursor == virtualLength))
						throw(new InvalidOperationException());
					return ClassEntryArray[cursor];
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

		public ModelClassCollection Clone()
		{
			ModelClassCollection clonedClassEntry = 
                new ModelClassCollection(itemCount);
			foreach(ModelClass item in this)
				clonedClassEntry.Add(item);
			return clonedClassEntry;
		}

		#region Events

		public event ClassEntryCollectionEventHandler ObjectAdded;
		protected virtual void OnObjectAdded(ModelClassCollectionEventArgs e)
		{
			if(ObjectAdded != null)
				ObjectAdded(this, e);
		}

		public event ClassEntryCollectionEventHandler ObjectRemoved;
		protected virtual void OnObjectRemoved(ModelClassCollectionEventArgs e)
		{
			if(ObjectRemoved != null)
				ObjectRemoved(this, e);
		}

		public event EventHandler Cleared;
		protected virtual void OnCleared(EventArgs e)
		{
			if(Cleared != null)
				Cleared(this, e);
		}

		#endregion

		public void Sort(params ModelEntryCompareKey[] keys)
		{
			if(this.Count > 0)
				Array.Sort(this.items, 0, this.Count, 
                    new ModelEntryComparer(keys));
		}
	}

	public delegate void ClassEntryCollectionEventHandler(object sender, 
        ModelClassCollectionEventArgs e);
    
	public class ModelClassCollectionEventArgs : System.EventArgs
	{
		public ModelClass ClassObject;

		public ModelClassCollectionEventArgs(ModelClass e)
		{
			this.ClassObject = e;
		}
	}
}
