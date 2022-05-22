using System;
using System.Linq;
using System.Reflection;
using Direct.Client.Attributes;

namespace Direct.Client.Helpers
{
    public static class JsonValidator
    {
        public static void NullCheck<TClass>(this TClass @class) where TClass : class 
        {
            @class
                .GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(propertyInfo => propertyInfo.GetCustomAttribute<NotNullAttribute>() != null)
                .ToList()
                .ForEach(propertyInfo => {
                        if ( propertyInfo.GetValue(@class) == null ) 
                        throw new ArgumentNullException($"{propertyInfo.Name} cannot be null in {@class.GetType().Name}");
                    }
                );
        }
    }
}
