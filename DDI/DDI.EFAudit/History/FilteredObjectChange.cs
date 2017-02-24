using DDI.Shared.Models.Client.Audit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDI.EFAudit.History
{
    /// <summary>
    /// Respresents an IObjectChange with only some of the property changes.
    /// This is used when reconstructing the history of only some properties.
    /// </summary>
    public class FilteredObjectChange<TPrincipal> : IObjectChange<TPrincipal>
    {
        private IObjectChange<TPrincipal> _objectChange;
        private IEnumerable<IPropertyChange<TPrincipal>> _propertyChanges;

        public FilteredObjectChange(IObjectChange<TPrincipal> objectChange, IEnumerable<IPropertyChange<TPrincipal>> propertyChanges)
        {
            this._objectChange = objectChange;
            this._propertyChanges = propertyChanges;
        }

        public IChangeSet<TPrincipal> ChangeSet
        {
            get { return _objectChange.ChangeSet; }
            set { _objectChange.ChangeSet = value; }
        }

        public IEnumerable<IPropertyChange<TPrincipal>> PropertyChanges
        {
            get { return _propertyChanges; }
        }

        public void Add(IPropertyChange<TPrincipal> propertyChange)
        {
            throw new NotImplementedException();
        }
        public long Id { get; set; }
        public long ChangeSetId { get; set; }
        public string TypeName
        {
            get { return _objectChange.TypeName; }
            set { _objectChange.TypeName = value; }
        }

        public string EntityId
        {
            get { return _objectChange.EntityId; }
            set { _objectChange.EntityId = value; }
        }
        public string ChangeType { get; set; }
        public string DisplayName { get; set; }
    }
}
