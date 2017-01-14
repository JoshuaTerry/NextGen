using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared.Helpers;

namespace DDI.Conversion
{
    internal abstract class ConversionBase
    {
        protected IEnumerable<ConversionMethodArgs> MethodsToRun { get; set; }
        protected ConversionMethodArgs MethodArgs { get; set; }

        public abstract void Execute(string baseDirectory, IEnumerable<ConversionMethodArgs> conversionMethods);

        protected void RunConversion(object method, Action action)
        {
            var methodNum = (int)method;
            if (MethodsToRun == null)
            {
                MethodArgs = new ConversionMethodArgs(methodNum);
                action.Invoke();
            }
            else
            {
                var methodArgs = MethodsToRun.FirstOrDefault(p => p.MethodNum == methodNum);
                if (methodArgs != null && methodArgs.Skip == false)
                {
                    MethodArgs = methodArgs;
                    action.Invoke();

                }
            }
        }


        protected string GetConversionMethodName<T>() where T : struct
        {
            return Enum.GetName(typeof(T), Enum.ToObject(typeof(T), MethodArgs.MethodNum));
        }

        protected FileImport CreateFileImporter(string baseDirectory, string defaultFilename, Type enumType, char delimiter = ',')
        {
            string filename = null;

            if (MethodArgs.Filenames != null && MethodArgs.FileNum < MethodArgs.Filenames.Length)
            {
                filename = MethodArgs.Filenames[MethodArgs.FileNum++];
            }
            if (string.IsNullOrWhiteSpace(filename))
            {
                filename = defaultFilename;
            }

            filename = Path.Combine(baseDirectory, filename);
            string logName = Enum.GetName(enumType, Enum.ToObject(enumType, MethodArgs.MethodNum));

            return new FileImport(filename, logName, delimiter);
        }

        protected Dictionary<string,Guid> LoadLegacyIds(string baseDirectory, string filename)
        {
            Dictionary<string, Guid> legacyIds = new Dictionary<string, Guid>();
            using (var importer = new FileImport(Path.Combine(baseDirectory, filename), string.Empty))
            {
                while (importer.GetNextRow())
                {
                    string legacyKey = importer.GetString(0);
                    if (!string.IsNullOrWhiteSpace(legacyKey))
                    {
                        legacyIds[legacyKey] = importer.GetGuid(1);
                    }
                }
            }
            return legacyIds;
        }


        public static void StartAllConversions(string baseDirectory, IEnumerable<ConversionMethodArgs> conversionMethods)
        {
            foreach (var type in ReflectionHelper.GetDerivedTypes<ConversionBase>(typeof(ConversionBase).Assembly))
            {
                var enumType = type.GetNestedType("ConversionMethod");

                var conversionclass = Activator.CreateInstance(type) as ConversionBase;
                conversionclass.Execute(baseDirectory, conversionMethods);
            }
        }

        /// <summary>
        /// Class for exporting rows that map a legacy ID (string) to an entity ID (Guid)
        /// </summary>
        protected class LegacyToID
        {
            public string LegacyKey { get; set; }
            public Guid Id { get; set; }

            public LegacyToID(string legacyKey, Guid id)
            {
                LegacyKey = legacyKey;
                Id = id;
            }

            public LegacyToID(int legacyKey, Guid id)
            {
                LegacyKey = legacyKey.ToString();
                Id = id;
            }
        }

        /// <summary>
        /// Class for exporting rows that map a parent ID to a child ID
        /// </summary>
        protected class JoinRow
        {
            public Guid LeftId { get; set; }
            public Guid RightId { get; set; }

            public JoinRow(Guid leftId, Guid rightId)
            {
                LeftId = leftId;
                RightId = rightId;
            }

        }

    }
}
