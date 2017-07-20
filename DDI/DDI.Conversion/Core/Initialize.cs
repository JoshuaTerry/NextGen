using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DDI.Conversion;
using DDI.Data;
using DDI.Shared.Models.Client.Core;

namespace DDI.Conversion.Core
{
    /// <summary>
    /// Seeding of initial data for the Core module.
    /// </summary>
    internal class Initialize : ConversionBase
    {
        public enum ConversionMethod
        {
            Initialize = 990000,
        }

        public override void Execute(string baseDirectory, IEnumerable<ConversionMethodArgs> conversionMethods)
        {
            MethodsToRun = conversionMethods;
            RunConversion(ConversionMethod.Initialize, () => LoadInitialData());
        }

        private void LoadInitialData()
        {
            using (var context = new DomainContext())
            {

                // Languages
                AddLanguageCode(context, "en", "English");
                AddLanguageCode(context, "fr", "French");
                AddLanguageCode(context, "es", "Spanish");
                AddLanguageCode(context, "de", "German");
                AddLanguageCode(context, "ko", "Korean");
                AddLanguageCode(context, "ja", "Japanese");
                AddLanguageCode(context, "zh", "Chinese");
                AddLanguageCode(context, "pt", "Portuguese");

                context.SaveChanges();
            }

        }

        private void AddLanguageCode(DomainContext context, string code, string description)
        {
            context.Languages.AddOrUpdate(p => p.Code,
                new Language()
                {
                    Code = code,
                    Name = description,
                    IsActive = true
                });
        }
    }
}
