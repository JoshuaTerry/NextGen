
using System;
using DDI.Shared.Enums.Core;

namespace DDI.Shared
{
    public interface ISQLUtilities
    {
        void SetNextSequenceValue(DatabaseSequence sequence, Int64 newValue);
        Int64 GetNextSequenceValue(DatabaseSequence sequence);
        int ExecuteSQL(string sql, params object[] parameters);
    }
}
