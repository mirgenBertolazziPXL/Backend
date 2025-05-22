using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using System.Text;
using System.Threading.Tasks;
using ClassLib13.Utils;

namespace ClassLib13.Utils
{
    public class ObjectCreator
    {
        public static (object instance, string error) CreateProduct(string typeName, Dictionary<string, string> values)
        {
            Type type = Type.GetType($"ClassLib13.Business.Entities.{typeName}, ClassLib13");
            if (type == null || !DataValidator.TypeExists(typeName)) { return (null, $"Invalid type: '{typeName}'"); }
            
            object instance = Activator.CreateInstance(type);
            PropertyInfo[] properties = instance.GetType().GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (values.TryGetValue(property.Name, out var value))
                {
                    if (property.PropertyType == typeof(int))
                    {
                        if (!int.TryParse(value, out _))
                        {
                            return (null, $"{property.Name} '{value}' isn't a valid number");
                        }
                        property.SetValue(instance, Convert.ToInt32(value));
                    }
                    else
                    {
                        property.SetValue(instance, value);
                    }
                }
            }
            if (properties.Any(prop => string.IsNullOrWhiteSpace(Convert.ToString(prop.GetValue(instance)))))
            {
                return (null, "Error: Some data is empty");
            }

            return (instance, null);
        }
    }
}
