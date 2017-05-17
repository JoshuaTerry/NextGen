using DDI.EFAudit.Translation.Binders;
using DDI.EFAudit.Translation.Serializers;
using System;

namespace DDI.EFAudit.Translation.ValueTranslators
{
    public class EnumTranslator : IBinder, ISerializer
    {
        public bool Supports(Type type)
        {
            return type.IsEnum;
        }

        public object Bind(string raw, Type type, object existingValue)
        {
            try
            {
                if (raw == null)
                    return 0;

                return Enum.Parse(type, raw, true);
            }
            catch (ArgumentNullException)
            {
                return null;
            }
            catch (FormatException)
            {
                return null;
            }
        }

        public string Serialize(object obj)
        {
            return (obj != null ? obj.ToString() : null);
        }
    }
}
