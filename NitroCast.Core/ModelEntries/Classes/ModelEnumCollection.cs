using System;
using System.Collections;

namespace NitroCast.Core
{
    /// <summary>
    /// Summary of MyClass
    /// </summary>
    public class ModelEnumCollection : IList, ICloneable
    {
        private int itemCount = 0;
        internal ModelEnum[] items;

        public ModelEnumCollection()
            : base()
        {
            items = new ModelEnum[15];
        }

        public ModelEnumCollection(int capacity)
            : base()
        {
            items = new ModelEnum[capacity];
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
                items[index] = (ModelEnum)value;
            }
        }

        public ModelEnum this[int index]
        {
            get
            {
                if (index > itemCount - 1)
                    throw (new Exception("Index out of bounds."));
                return items[index];
            }
            set
            {
                items[index] = value;
            }
        }

        int IList.Add(object value)
        {
            OnObjectAdded(new EnumClassEntryCollectionEventArgs((ModelEnum)value));
            return Add((ModelEnum)value);

        }

        public int Add(ModelEnum value)
        {
            itemCount++;
            if (itemCount > items.GetUpperBound(0) + 1)
            {
                ModelEnum[] tempEnumClassEntryArray = new ModelEnum[itemCount * 2];
                for (int x = 0; x <= items.GetUpperBound(0); x++)
                    tempEnumClassEntryArray[x] = items[x];
                items = tempEnumClassEntryArray;
            }
            items[itemCount - 1] = value;
            OnObjectAdded(new EnumClassEntryCollectionEventArgs(value));
            return itemCount - 1;

        }

        public void Clear()
        {
            itemCount = 0;
            items = new ModelEnum[15];
            OnCleared(EventArgs.Empty);
        }

        bool IList.Contains(object value)
        {
            return Contains((ModelEnum)value);
        }

        public bool Contains(ModelEnum value)
        {
            return IndexOf(value) != -1;
        }

        int IList.IndexOf(object value)
        {
            return IndexOf((ModelEnum)value);
        }

        public int IndexOf(ModelEnum value)
        {
            for (int x = 0; x < itemCount; x++)
                if (items[x].Equals(value))
                    return x;
            return -1;
        }

        void IList.Insert(int index, object value)
        {
            OnObjectAdded(new EnumClassEntryCollectionEventArgs((ModelEnum)value));
            Insert(index, (ModelEnum)value);
        }

        public void Insert(int index, ModelEnum value)
        {
            itemCount++;
            if (itemCount > items.Length)
                for (int x = index + 1; x == itemCount - 2; x++)
                    items[x] = items[x - 1];
            OnObjectAdded(new EnumClassEntryCollectionEventArgs(value));
            items[index] = value;
        }

        void IList.Remove(object value)
        {
            Remove((ModelEnum)value);
            OnObjectRemoved(new EnumClassEntryCollectionEventArgs((ModelEnum)value));
        }

        public void Remove(ModelEnum value)
        {
            int index = IndexOf(value);
            if (index == -1)
                throw (new Exception("EnumClassEntry not found in collection."));
            RemoveAt(index);
            OnObjectRemoved(new EnumClassEntryCollectionEventArgs(value));
        }

        public void RemoveAt(int index)
        {
            OnObjectRemoved(new EnumClassEntryCollectionEventArgs(items[index]));			// Must retreive reference before delete.
            for (int x = index + 1; x <= itemCount - 1; x++)
                items[x - 1] = items[x];
            items[itemCount - 1] = null;
            itemCount--;
        }

        public void MoveUp(ModelEnum c)
        {
            int i = IndexOf(c);

            // Don't do anything if this field is already on top
            if (i == 0)
                return;

            ModelEnum swap = items[i - 1];
            items[i - 1] = c;
            items[i] = swap;
        }

        public void MoveDown(ModelEnum c)
        {
            int i = IndexOf(c);

            // Don't do anything if this field is already on bottom
            if (i == this.Count - 1)
                return;

            ModelEnum swap = items[i + 1];
            items[i + 1] = c;
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
            private ModelEnum[] EnumClassEntryArray;
            private int virtualLength;
            private int cursor;

            public Enumerator(ModelEnum[] EnumClassEntryArray, int virtualLength)
            {
                this.EnumClassEntryArray = EnumClassEntryArray;
                this.virtualLength = virtualLength;
                cursor = -1;
            }

            public void Reset()
            {
                cursor = -1;
            }

            public bool MoveNext()
            {
                if (cursor < EnumClassEntryArray.Length)
                    cursor++;
                return (!(cursor == virtualLength));
            }

            public ModelEnum Current
            {
                get
                {
                    if ((cursor < 0) || (cursor == virtualLength))
                        throw (new InvalidOperationException());
                    return EnumClassEntryArray[cursor];
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

        public ModelEnumCollection Clone()
        {
            ModelEnumCollection clonedEnumClassEntry = new ModelEnumCollection(itemCount);
            foreach (ModelEnum item in this)
                clonedEnumClassEntry.Add(item);
            return clonedEnumClassEntry;
        }

        #region Events

        public event EnumClassEntryCollectionEventHandler ObjectAdded;
        protected virtual void OnObjectAdded(EnumClassEntryCollectionEventArgs e)
        {
            if (ObjectAdded != null)
                ObjectAdded(this, e);
        }

        public event EnumClassEntryCollectionEventHandler ObjectRemoved;
        protected virtual void OnObjectRemoved(EnumClassEntryCollectionEventArgs e)
        {
            if (ObjectRemoved != null)
                ObjectRemoved(this, e);
        }

        public event EventHandler Cleared;
        protected virtual void OnCleared(EventArgs e)
        {
            if (Cleared != null)
                Cleared(this, e);
        }

        #endregion

        public void Sort(params ModelEntryCompareKey[] keys)
        {
            if (this.Count > 0)
                Array.Sort(this.items, 0, this.Count, new ModelEntryComparer(keys));
        }
    }

    public delegate void EnumClassEntryCollectionEventHandler(object sender, EnumClassEntryCollectionEventArgs e);

    public class EnumClassEntryCollectionEventArgs : System.EventArgs
    {
        public ModelEnum ClassObject;

        public EnumClassEntryCollectionEventArgs(ModelEnum e)
        {
            this.ClassObject = e;
        }
    }
}