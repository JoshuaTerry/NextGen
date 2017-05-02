
using System;

namespace DDI.Shared
{
    public interface ISQLUtilities
    {
        void SetNextSequenceValue(string sequenceName, Int64 newValue);

        Int64 GetNextSequenceValue(string sequenceName);
    }
}
