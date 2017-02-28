using System;
using System.Data.Entity;

namespace DDI.EFAudit.Logging.ValuePairs
{
    internal class ValuePair : IValuePair
    {
        protected readonly Func<object> _originalValue;
        protected readonly Func<object> _newValue;
        protected readonly string _propertyName;
        protected readonly EntityState _state;

        internal ValuePair(Func<object> originalValue, Func<object> newValue, string propertyName, EntityState state)
        {
            this._originalValue = checkDbNull(originalValue);
            this._newValue = checkDbNull(newValue);
            this._propertyName = propertyName;
            this._state = state;
        }

        private Func<object> checkDbNull(Func<object> value)
        {
            return () =>
            {
                var obj = (value != null ? value() : null);
                if (obj is DBNull)
                    return null;
                return obj;
            };
        }

        internal IChangeType Type
        {
            get
            {
                var value = _originalValue() ?? _newValue();
                return value.GetChangeType();
            }
        }

        public bool HasChanged
        {
            get
            {
                return _state == EntityState.Added
                    || _state == EntityState.Deleted
                    || !object.Equals(_newValue(), _originalValue());
            }
        }

        public string PropertyName
        {
            get { return _propertyName; }
        }

        public Func<object> NewValue
        {
            get { return _newValue; }
        }

        public Func<object> OriginalValue
        {
            get { return _originalValue; }
        }

        public EntityState State
        {
            get { return _state; }
        }
    }
}