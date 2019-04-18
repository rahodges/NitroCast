using System;
using System.Collections;

namespace NitroCast.Core
{
	/// <summary>
	/// Summary of MyClass
	/// </summary>
	public class ModelFolderCollection : IList, ICloneable 
	{
		private int itemCount = 0;
		internal ModelFolder[] folders;

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

		public ModelFolderCollection() : base()
		{
			folders = new ModelFolder[15];
		}

		public ModelFolderCollection(int capacity) : base()
		{
			folders = new ModelFolder[capacity];
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
				folders[index] = (ModelFolder) value;
			}
		}

		public ModelFolder this[int index]
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

		public ModelFolder this[string name]
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
				if(i > -1)
					folders[i] = value;
			}
		}

		int IList.Add(object value)
		{
			return Add((ModelFolder) value);
		}

		public int Add(ModelFolder value)
		{			
			itemCount++;
			if(itemCount > folders.GetUpperBound(0) + 1)
			{
				ModelFolder[] tempfolders = new ModelFolder[itemCount * 2];
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
			folders = new ModelFolder[15];
		}

		bool IList.Contains(object value)
		{
			return Contains((ModelFolder) value);
		}

		public bool Contains(ModelFolder value)
		{
			return IndexOf(value) != -1;
		}

		int IList.IndexOf(object value)
		{
			return IndexOf((ModelFolder) value);
		}

		public int IndexOf(ModelFolder value)
		{
			for(int x = 0; x < itemCount; x++)
				if(folders[x].Equals(value))
					return x;
			return -1;
		}

		void IList.Insert(int index, object value)
		{
			Insert(index, (ModelFolder) value);
		}

		public void Insert(int index, ModelFolder value)
		{
			itemCount++;
			if(itemCount > folders.Length)
				for(int x = index + 1; x == itemCount - 2; x ++)
					folders[x] = folders[x - 1];
			folders[index] = value;
		}

		void IList.Remove(object value)
		{
			Remove((ModelFolder) value);
		}

		public void Remove(ModelFolder value)
		{
			int index = IndexOf(value);
			if(index == -1)
				throw(new Exception("ModelFolder not found in collection."));
			RemoveAt(index);
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
			private ModelFolder[] ModelFolderArray;
			private int virtualLength;
			private int cursor;

			public Enumerator(ModelFolder[] ModelFolderArray, int virtualLength)
			{
				this.ModelFolderArray = ModelFolderArray;
				this.virtualLength = virtualLength;
				cursor = -1;
			}

			public void Reset()
			{
				cursor = -1;
			}

			public bool MoveNext()
			{
				if(cursor < ModelFolderArray.Length)
					cursor++;
				return(!(cursor == virtualLength));
			}

			public ModelFolder Current
			{
				get
				{
					if((cursor < 0) || (cursor == virtualLength))
						throw(new InvalidOperationException());
					return ModelFolderArray[cursor];
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

		public ModelFolderCollection Clone()
		{
			ModelFolderCollection clonedModelFolder = new ModelFolderCollection(itemCount);
			foreach(ModelFolder item in this)
				clonedModelFolder.Add(item);
			return clonedModelFolder;
		}

		public ModelFolderCollection Copy()
		{
			ModelFolderCollection copiedFolders = new ModelFolderCollection(itemCount);
			foreach(ModelFolder folder in this)
				copiedFolders.Add(folder.Copy());
			return copiedFolders;
		}

		public void Sort(params ModelEntryCompareKey[] keys)
		{
			if(this.Count > 0)
				Array.Sort(this.folders, 0, this.Count, new ModelEntryComparer(keys));
		}
	}
}
