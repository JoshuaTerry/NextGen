﻿using System;
using DDI.EFAudit.Translation.Binders;
using DDI.EFAudit.Translation.Serializers;

namespace DDI.EFAudit.Translation.ValueTranslators
{
    public class BinaryBlobTranslator : IBinder, ISerializer
    {
        public bool Supports(Type type)
        {
            return typeof(byte[]).IsAssignableFrom(type);
        }

        public object Bind(string raw, Type type, object existingValue)
        {
            if (raw == null)
                return null;

            return Convert.FromBase64String(raw);
        }

        public string Serialize(object obj)
        {
            var blob = (obj as byte[]);
            if (blob == null)
                return null;

            return Convert.ToBase64String(blob);
        }
    }
}
