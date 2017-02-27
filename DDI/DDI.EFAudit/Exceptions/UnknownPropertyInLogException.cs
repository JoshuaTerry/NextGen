using System;
using DDI.Shared.Models.Client.Audit;

namespace DDI.EFAudit.Exceptions
{
    public class UnknownPropertyInLogException<TPrincipal> : Exception
    {
        private const string message =
            "While retrieving history, a IPropertyChange naming property '{0}' was found in the log that did not correspond " +
            "to any property on the model, which had type '{1}'. This may be because a property has been removed from the model, " +
            "or renamed, since the log record was created. ";

        public readonly IPropertyChange<TPrincipal> propertyChange;

        public UnknownPropertyInLogException(IPropertyChange<TPrincipal> propertyChange)
            : base(string.Format(
                message, 
                (propertyChange != null ? propertyChange.PropertyName : "[Null]"), 
                (propertyChange != null && propertyChange.ObjectChange != null ? propertyChange.ObjectChange.TypeName : "[Null]")
            ))
        {
            this.propertyChange = propertyChange;
        }

        public IPropertyChange<TPrincipal> PropertyChange
        {
            get { return propertyChange; }
        }
    }
}
