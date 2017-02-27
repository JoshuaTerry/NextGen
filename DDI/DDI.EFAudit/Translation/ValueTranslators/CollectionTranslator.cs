using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DDI.EFAudit.Translation.Binders;
using DDI.EFAudit.Translation.Serializers;
using DDI.EFAudit.Contexts;

namespace DDI.EFAudit.Translation.ValueTranslators
{
    public class CollectionTranslator :  IBinder, ISerializer
    {
        private IBindManager _bindManager;
        private ISerializationManager _serializationManager;
        private IHistoryContext _db;

        public CollectionTranslator(IBindManager bindManager, 
            ISerializationManager serializationManager, IHistoryContext db)
        {
            this._bindManager = bindManager;
            this._serializationManager = serializationManager;
            this._db = db;
        }

        public bool Supports(Type type)
        {
            return type.IsGenericType
                && typeof(ICollection<>).MakeGenericType(type.GetGenericArguments().First()).IsAssignableFrom(type);
        }

        public virtual object Bind(string raw, Type type, object existingValue)
        {
            var itemType = type.GetGenericArguments().First();
            object collection = existingValue ?? CreateCollection(type, itemType);
            GetType().GetMethod("FillCollection", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy)
                .MakeGenericMethod(new Type[] { itemType })
                .Invoke(this, new object[] { collection, raw });
            return collection;
        }

        public string Serialize(object obj)
        {
            if (obj == null)
                return null;

            var items = ((ICollection) obj).OfType<object>();
            var raw = (_serializationManager != null)
                ? items.Select(x => _serializationManager.Serialize(x))
                : items.Select(x => x.ToString());

            return String.Join(",", raw);
        }

        protected virtual object CreateCollection(Type type, Type itemType)
        {
            if (type.IsInterface)
            {
                var concreteType = typeof(List<>).MakeGenericType(itemType);
                return Activator.CreateInstance(concreteType);
            }
            else
            {
                return Activator.CreateInstance(type);
            }
        }

        protected virtual void FillCollection<ItemType>(ICollection<ItemType> collection, string raw)
        {
            foreach (var reference in raw.Split(new char[] { ',' }).Where(r => !string.IsNullOrEmpty(r)))
            {
                var item = _bindManager.Bind<ItemType>(reference);
                if (collection.All(i => !EqualCollectionItems(i, item)))
                    collection.Add(item);
            }
        }
        
        protected virtual bool EqualCollectionItems(object a, object b)
        {
            return (_db.ObjectHasReference(a))
                ? (_db.GetReferenceForObject(a) == _db.GetReferenceForObject(b))
                : Equals(a, b);
        }
    }
}
