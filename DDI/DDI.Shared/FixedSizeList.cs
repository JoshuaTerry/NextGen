using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared
{
    /// <summary>
    /// A list that implements IReadOnlyList with the ability to modify values in the list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FixedSizeList<T> : IFixedSizeList<T>
    {
        private IEnumerable<T> _enumerable;
        private Func<int, T> _getter;
        private Action<int, T> _setter;
        private int _capacity;
        private T[] _array;

        /// <summary>
        /// Initializes a new instance of the FixedSizeList class of the specified capacity which uses an IEnumerable for its elements.
        /// </summary>
        /// <param name="enumerable">An enumerable set of elements for the list.</param>
        /// <param name="getter">A function that returns an element at a specified index.</param>
        /// <param name="setter">An action that sets an element at a specfied index.</param>
        /// <param name="capacity">The number of elements in the list.</param>
        public FixedSizeList(IEnumerable<T> enumerable, Func<int, T> getter, Action<int, T> setter, int capacity)
        {
            _enumerable = enumerable;
            _getter = getter;
            _setter = setter;
            _capacity = capacity;
        }

        /// <summary>
        /// Initializes a new instance of the FixedSizeList class based on an internal array of the specified capacity.
        /// </summary>
        /// <param name="capacity">The number of elements in the list.</param>
        public FixedSizeList(int capacity)
        {
            _capacity = capacity;
            _array = new T[capacity];
            _enumerable = _array.AsEnumerable();
            _getter = index => _array[index];
            _setter = (index, value) => _array[index] = value;
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <returns>The element at the specified index.</returns>
        public T this[int index]
        {
            get
            {
                return _getter(index);
            }

            set
            {
                _setter(index, value);
            }
        }

        /// <summary>
        /// Gets the number of elements contained in the list.
        /// </summary>
        public int Count => _capacity;

        /// <summary>
        /// Gets the fixed capacity of the list.
        /// </summary>
        public int Capacity => _capacity;

        /// <summary>
        /// Sets all elements in the list to their default value.
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < _capacity; i++)
            {
                _setter(i, default(T));
            }
        }

        /// <summary>
        /// Returns a list of key-value pairs consisting of the indexes and their values.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<int, T>> AsTable()
        {
            return Enumerable.Range(0, _capacity).Zip(_enumerable, (p, q) => new KeyValuePair<int, T>(p, q));
        }

        /// <summary>
        /// Performs the specified action on each element of the list.
        /// </summary>
        /// <param name="action">The delegate to perform on each element of the list.</param>
        public void ForEach(Action<int,T> action)
        {
            int index = 0;
            foreach (T value in _enumerable)
            {
                action(++index, value);
            }
        }

        /// <summary>
        /// Copies the entire list to another compatible FixedSizeList.  Both lists must have the same capacity.
        /// </summary>
        public void CopyTo(FixedSizeList<T> destination)
        {
            if (Capacity != destination.Capacity)
            {
                throw new InvalidOperationException("The capacity of the destination list must be equal to the capacity of the source.");
            }

            ForEach((index, value) => destination[index] = value);
        }

        /// <summary>
        /// Copies the entire list to a compatible array.
        /// </summary>
        public void CopyTo(T[] array)
        {
            if (Capacity < array.Length)
            {
                throw new InvalidOperationException("The length of the destination array must not be less than the capacity of the source.");
            }

            ForEach((index, value) => array[index] = value);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _enumerable.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_enumerable).GetEnumerator();
        }

    }
}
