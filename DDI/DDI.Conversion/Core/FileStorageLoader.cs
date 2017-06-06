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
    /// Load Attachments
    /// </summary>
    internal class FileStorageLoader : ConversionBase
    {
        public enum ConversionMethod
        {
            FileStorage = 990002,
        }

        public override void Execute(string baseDirectory, IEnumerable<ConversionMethodArgs> conversionMethods)
        {
            MethodsToRun = conversionMethods;
            RunConversion(ConversionMethod.FileStorage, () => LoadFileStorage());
        }

        private void LoadFileStorage()
        {

        }

    }
}
