using DDI.EFAudit.Contexts;
using DDI.EFAudit.Translation.Binders;
using DDI.EFAudit.Translation.Serializers;
using DDI.EFAudit.Translation.ValueTranslators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DDI.EFAudit.Translation
{
    /// <summary>
    /// The default binding and serialization manager
    /// </summary>
    /// <remarks>
    /// This manager supports the formats: 
    /// Primitive, Guid, DateTime, DateTimeOffset, TimeSpan, Enum, Binary Blob, Nullable, and Collection
    /// </remarks>
    public partial class ValueTranslationManager : ISerializationManager, IBindManager
    {
        protected List<IValueTranslator> _translators;
        private IHistoryContext _db;

        public ValueTranslationManager(IHistoryContext db)
        {
            this._db = db;
            _translators = new List<IValueTranslator>()
            {
                new PrimitiveTranslator(),
                new GuidTranslator(),
                new DateTimeTranslator(),
                new DateTimeOffsetTranslator(),
                new TimeSpanTranslator(),
                new EnumTranslator(),
                new BinaryBlobTranslator(),
                new NullableBinder(this),
                new CollectionTranslator(this, this, db),
            };
        }

        public virtual TValue Bind<TValue>(string raw, object existingValue = null)
        {
            return (TValue)Bind(raw, typeof(TValue), existingValue);
        }
        public virtual object Bind(string raw, Type type, object existingValue = null)
        {
            if (raw == null)
                return null;
            
            foreach (var binder in _translators.OfType<IBinder>())
            {
                if (binder.Supports(type))
                    return binder.Bind(raw, type, existingValue);
            }
            return _db.GetObjectByReference(type, raw);
        }
        public string Serialize(object obj)
        {
            if (obj == null)
                return null;

            var type = obj.GetType();
            foreach (var serializer in _translators.OfType<ISerializer>())
            {
                if (serializer.Supports(type))
                    return serializer.Serialize(obj);
            }
            return obj.ToString();
        }
    }
}
