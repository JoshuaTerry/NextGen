
namespace DDI.Shared
{
    public interface ISQLUtilities
    {
        void SetNextSequenceValue(string sequenceName, int newValue);

        int GetNextSequenceValue(string sequenceName);
    }
}
