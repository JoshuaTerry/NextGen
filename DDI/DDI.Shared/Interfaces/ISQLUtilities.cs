
using DDI.Shared.Enums.Core;
using System;

namespace DDI.Shared
{
    public interface ISQLUtilities
    {
        void SetNextSequenceValue(DatabaseSequence sequence, Int64 newValue);

        Int64 GetNextSequenceValue(DatabaseSequence sequence);
    }
}
