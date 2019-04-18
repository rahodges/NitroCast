using System;
using System.Collections;

namespace NitroCast.Core
{
	/// <summary>
	/// Summary of MyClass
	/// </summary>
	public class ClassFolderCollection : IList, ICloneable 
	{
		private int itemCount = 0;
		internal ClassFolder[] folders;

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

		public ClassFolderCollection() : base()
		{
			folders = new ClassFolder[15];
		}

		public ClassFolderCollection(int capacity) : base()
		{
			folders = new ClassFolder[capacity];
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
				folders[index] = (ClassFolder) value;
			}
		}

		public ClassFolder this[int index]
		{
			get
			{
				if(index > itemCount - 1)
					throw(new Exception("Index out of bounds."));
				return folders[index];
			}
			set
			{
				folders[index] = value;
			}
		}

		public ClassFolder this[string name]
		{
			get
			{
				for(int x = 0; x <= folders.GetUpperBound(0); x++)
					if(folders[x].Name == name)
						return folders[x];
				return null;
			}
			set
			{
				int i = -1;
				for(int x = 0; x <= folders.GetUpperBound(0); x++)
					if(folders[x].Name == name)
						i = x;
                if (i > -1)
                {
                    folders[i] = value;
                }
			}
		}

		int IList.Add(object value)
		{
			return Add((ClassFolder) value);
		}

		public int Add(ClassFolder value)
		{			
			itemCount++;
			if(itemCount > folders.GetUpperBound(0) + 1)
			{
				ClassFolder[] tempfolders = new ClassFolder[itemCount * 2];
				for(int x = 0; x <= folders.GetUpperBound(0); x++)
					tempfolders[x] = folders[x];
				folders = tempfolders;
			}
			folders[itemCount - 1] = value;
			
			OnAdd(EventArgs.Empty);
			return itemCount -1;
		}

		public void Clear()
		{
			itemCount = 0;
			folders = new ClassFolder[15];
		}

		bool IList.Contains(object value)
		{
			return Contains((ClassFolder) value);
		}

		public bool Contains(ClassFolder value)
		{
			return IndexOf(value) != -1;
		}

		int IList.IndexOf(object value)
		{
			return IndexOf((ClassFolder) value);
		}

		public int IndexOf(ClassFolder value)
		{
			for(int x = 0; x < itemCount; x++)
				if(folders[x].Equals(value))
					return x;
			return -1;
		}

		void IList.Insert(int index, object value)
		{
			Insert(index, (ClassFolder) value);
		}

		public void Insert(int index, ClassFolder value)
		{
			itemCount++;
			if(itemCount > folders.Length)
				for(int x = index + 1; x == itemCount - 2; x ++)
					folders[x] = folders[x - 1];
			folders[index] = value;
		}

		void IList.Remove(object value)
		{
			Remove((ClassFolder) value);
		}

		public void Remove(ClassFolder value)
		{
			int index = IndexOf(value);
            if (index == -1)
            {
                //throw (new Exception("ClassFolder not found in collection."));
            }
            else
            {
                RemoveAt(index);
            }
		}

		public void RemoveAt(int index)
		{
			for(int x = index + 1; x <= itemCount - 1; x++)
				folders[x-1] = folders[x];
			folders[itemCount - 1] = null;
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
				return folders.IsSynchronized;
			}
		}

		public object SyncRoot
		{
			get
			{
				return folders;
			}
		}

		public void CopyTo(Array array, int arrayIndex)
		{
			for(int x = 0; x < itemCount; x++)
				array.SetValue(folders[x], x);
		}

		public Enumerator GetEnumerator()
		{
			return new Enumerator(folders, itemCount);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public class Enumerator : IEnumerator
		{
			private ClassFolder[] ClassFolderArray;
			private int virtualLength;
			private int cursor;

			public Enumerator(ClassFolder[] ClassFolderArray, int virtualLength)
			{
				this.ClassFolderArray = ClassFolderArray;
				this.virtualLength = virtualLength;
				cursor = -1;
			}

			public void Reset()
			{
				cursor = -1;
			}

			public bool MoveNext()
			{
				if(cursor < ClassFolderArray.Length)
					cursor++;
				return(!(cursor == virtualLength));
			}

			public ClassFolder Current
			{
				get
				{
					if((cursor < 0) || (cursor == virtualLength))
						throw(new InvalidOperationException());
					return ClassFolderArray[cursor];
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

		public ClassFolderCollection Clone()
		{
			ClassFolderCollection clonedClassFolder = new ClassFolderCollection(itemCount);
			foreach(ClassFolder item in this)
				clonedClassFolder.Add(item);
			return clonedClassFolder;
		}

		public ClassFolderCollection Copy()
		{
			ClassFolderCollection copiedFolders = new ClassFolderCollection(itemCount);
			foreach(ClassFolder folder in this)
				copiedFolders.Add(folder.Copy());
			return copiedFolders;
		}
	}
}