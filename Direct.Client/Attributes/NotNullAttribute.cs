using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Direct.Client.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    internal class NotNullAttribute : Attribute{}
}
