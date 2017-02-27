using System;

namespace DDI.EFAudit.History
{
    [Flags]
    public enum HistoryExplorerCloneStrategies
    { 
        // No cloning, will trigger an UnableToCloneObjectException 
        None = 0,
        // Use the ICloneable interface for cloning (only if the model implements ICloneable) 
        UseCloneable = 0x01,
        // Do a recusive/deep copy of the object using Object.MemberwiseClone()       
        DeepCopy = 0x02,
        Default = (UseCloneable | DeepCopy)
    }
}
