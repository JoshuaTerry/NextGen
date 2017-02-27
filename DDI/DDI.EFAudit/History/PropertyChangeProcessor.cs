using DDI.EFAudit.Exceptions;
using DDI.EFAudit.Helpers;
using DDI.Shared.Models.Client.Audit;
using DDI.EFAudit.Translation.Binders;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Reflection;

namespace DDI.EFAudit.History
{
    /// <summary>
    /// Recurses down through property changes that describe complex
    /// property changes like Foo.Bar.Berries
    /// </summary>
    public class PropertyChangeProcessor<TPrincipal>
    {
        private IPropertyChange<TPrincipal> wrapped;

        public PropertyChangeProcessor(IPropertyChange<TPrincipal> wrapped)
        {
            this.wrapped = wrapped;
        }
         
        public void ApplyTo<TModel>(TModel model, IBindManager binder, string prefix)
        {
            ApplyTo(model, binder, typeof(TModel), prefix);
        }
        private void ApplyTo(object model, IBindManager binder, Type modelType, string prefix)
        {
            string remainder = StripPrefix(prefix);
            var propertyParts = Split(remainder);
            string propertyName = propertyParts.First();

            var property = modelType.GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            if (property != null)
            {
                ApplyToProperty(model, property, binder, prefix, remainder);
            }
            else
            {
                throw new UnknownPropertyInLogException<TPrincipal>(wrapped);
            }
        }

        private void ApplyToProperty(object model, PropertyInfo property, IBindManager binder, string prefix, string remainder)
        { 
            if (Split(remainder).Count() > 1)
            {
                var value = GetValue(model, property);
                ApplyTo(value, binder, property.PropertyType, ExpressionHelper.Join(prefix, property.Name));
            }
            // Simple property change, Bind value and Set Property
            else
            {
                var existingValue = property.GetValue(model, null);
                var value = binder.Bind(wrapped.Value, property.PropertyType, existingValue);
                if (IsEntityCollection(property) && existingValue != null)
                {
                    // Don't do anything, the contents were already updated and you can't use property setter on an entity collection
                }
                else
                {
                    property.SetValue(model, value, null);
                }
            }
        }

        private bool IsEntityCollection(PropertyInfo property)
        {
            var type = property.PropertyType;
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(EntityCollection<>);
        }

        private string StripPrefix(string prefix)
        {
            return wrapped.PropertyName.Substring(prefix.Length).TrimStart(new char[] { '.' });
        }
         
        private object GetValue(object model, PropertyInfo containerProperty)
        {
            var value = containerProperty.GetValue(model, null);

            if (value == null)
            {
                value = HistoryHelpers.Instantiate(containerProperty.PropertyType);
                containerProperty.SetValue(model, value, null);
            }

            return value;
        }

        private IEnumerable<string> Split(string text)
        {
            return text.Split(new char[] { '.' });
        }

        public override string ToString()
        {
            return wrapped.ToString();
        }
    }
}
