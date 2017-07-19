using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DDI.Shared.Models;

namespace DDI.Shared.Extensions
{
    public class Enum<T> where T : struct, IConvertible
    {
        public static List<EnumEntity> ToList()
        {
            List<EnumEntity> items = new List<EnumEntity>();

            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("Type of argument must be Enum.");
            }

            var type = typeof(T);

            FieldInfo[] fields = type.GetFields();

            foreach (var field in fields)
            {
                object[] attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);

                foreach (var attribute in attributes)
                {
                    items.Add(new EnumEntity()
                    {
                        Id = (int)field.GetValue(field),
                        DisplayName = ((DescriptionAttribute)attribute).Description
                    });
                }
            }

            return items;
        }

        public static DataResponse<List<EnumEntity>> GetDataResponse()
        {
            try
            {
                var response = new DataResponse<List<EnumEntity>>();

                var data = Enum<T>.ToList();

                if (data != null)
                {
                    response.IsSuccessful = true;
                    response.Data = data;
                    response.TotalResults = data.Count;
                }

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
