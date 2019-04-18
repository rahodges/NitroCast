using System;
using System.Collections;

namespace NitroCast.Core
{
    /// <summary>
    /// Summary of MyClass
    /// </summary>
    public class EnumFieldCollection : IList, ICloneable
    {
        private int itemCount = 0;
        internal EnumField[] items;

        public EnumFieldCollection()
            : base()
        {
            items = new EnumField[15];
        }

        public EnumFieldCollection(int capacity)
            : base()
        {
            items = new EnumField[capacity];
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
                items[index] = (EnumField)value;
            }
        }

        public EnumField this[int index]
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
            return Add((EnumField)value);
        }

        public int Add(EnumField value)
        {
            itemCount++;
            if (itemCount > items.GetUpperBound(0) + 1)
            {
                EnumField[] tempitems = new EnumField[itemCount * 2];
                for (int x = 0; x <= items.GetUpperBound(0); x++)
                    tempitems[x] = items[x];
                items = tempitems;
            }
            items[itemCount - 1] = value;
            return itemCount - 1;
        }

        public void Clear()
        {
            itemCount = 0;
            items = new EnumField[15];
        }

        bool IList.Contains(object value)
        {
            return Contains((EnumField)value);
        }

        public bool Contains(EnumField value)
        {
            return IndexOf(value) != -1;
        }

        int IList.IndexOf(object value)
        {
            return IndexOf((EnumField)value);
        }

        public int IndexOf(EnumField value)
        {
            for (int x = 0; x < itemCount; x++)
                if (items[x].Equals(value))
                    return x;
            return -1;
        }

        void IList.Insert(int index, object value)
        {
            Insert(index, (EnumField)value);
        }

        public void Insert(int index, EnumField value)
        {
            itemCount++;
            if (itemCount > items.Length)
                for (int x = index + 1; x == itemCount - 2; x++)
                    items[x] = items[x - 1];
            items[index] = value;
        }

        void IList.Remove(object value)
        {
            Remove((EnumField)value);
        }

        public void Remove(EnumField value)
        {
            int index = IndexOf(value);
            if (index == -1)
                throw (new Exception("ChildEntry not found in collection."));
            RemoveAt(index);
        }

        public void RemoveAt(int index)
        {
            for (int x = index + 1; x <= itemCount - 1; x++)
                items[x - 1] = items[x];
            items[itemCount - 1] = null;
            itemCount--;
        }

        public void MoveUp(EnumField c)
        {
            int i = IndexOf(c);

            // Don't do anything if this field is already on top
            if (i == 0)
                return;

            EnumField swap = items[i - 1];
            items[i - 1] = c;
            items[i] = swap;
        }

        public void MoveDown(EnumField c)
        {
            int i = IndexOf(c);

            // Don't do anything if this field is already on bottom
            if (i == this.Count - 1)
                return;

            EnumField swap = items[i + 1];
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
            private EnumField[] items;
            private int virtualLength;
            private int cursor;

            public Enumerator(EnumField[] items, int virtualLength)
            {
                this.items = items;
                this.virtualLength = virtualLength;
                cursor = -1;
            }

            public void Reset()
            {
                cursor = -1;
            }

            public bool MoveNext()
            {
                if (cursor < items.Length)
                    cursor++;
                return (!(cursor == virtualLength));
            }

            public EnumField Current
            {
                get
                {
                    if ((cursor < 0) || (cursor == virtualLength))
                        throw (new InvalidOperationException());
                    return items[cursor];
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

        public EnumFieldCollection Clone()
        {
            EnumFieldCollection clone = new EnumFieldCollection(itemCount);
            foreach (EnumField item in this)
                clone.Add(item);
            return clone;
        }

        public EnumFieldCollection Copy()
        {
            EnumFieldCollection clone = new EnumFieldCollection(itemCount);
            foreach (EnumField item in this)
                clone.Add(item.Clone());
            return clone;
        }

        public string GenerateName()
        {
            int newIndex = 0;

            string childName;
            int childIndex;

            for (int x = 0; x < this.itemCount; x++)
            {
                childName = items[x].Name;
                if (childName.StartsWith("Child")
                    & childName.Length > 5)
                {
                    try
                    {
                        childIndex = int.Parse(childName.Substring(5, childName.Length - 5));
                    }
                    catch
                    {
                        continue;
                    }

                    if (childIndex >= newIndex)
                        newIndex = childIndex + 1;
                }
            }

            return "Child" + newIndex.ToString();
        }

        public void Sort()
        {
            Array.Sort(items);
        }
    }
}
