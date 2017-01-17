using System;
using System.Collections.Generic;
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

        public static void StartAllConversions(string baseDirectory, IEnumerable<ConversionMethodArgs> conversionMethods)
        {
            foreach (var type in ReflectionHelper.GetDerivedTypes<ConversionBase>(typeof(ConversionBase).Assembly))
            {
                var enumType = type.GetNestedType("ConversionMethod");

                var conversionclass = Activator.CreateInstance(type) as ConversionBase;
                conversionclass.Execute(baseDirectory, conversionMethods);
            }
        }
    }
}
