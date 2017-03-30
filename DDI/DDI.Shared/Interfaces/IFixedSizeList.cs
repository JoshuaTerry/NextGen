using System;
using System.Collections.Generic;

namespace DDI.Shared
{
    /// <summary>
    /// A list that implements IReadOnlyList with the ability to modify values in the list.
    /// </summary>
    public interface IFixedSizeList<T> : IReadOnlyList<T>
    {
        int Capacity { get; }
        IEnumerable<KeyValuePair<int, T>> AsTable();
        void Clear();
        void CopyTo(T[] array);
        void CopyTo(FixedSizeList<T> destination);
        void ForEach(Action<int, T> action);
    }
}