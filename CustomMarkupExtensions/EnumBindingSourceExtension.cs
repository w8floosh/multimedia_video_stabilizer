using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Multimedia.CustomMarkupExtensions
{
    public class EnumBindingSourceExtension : MarkupExtension
    {
        public Type EnumType { get; private set; }

        public EnumBindingSourceExtension(Type enumType)
        {
            if (enumType is null || !enumType.IsEnum)
                throw new Exception("EnumType must be not null and of type Enum");
            EnumType = enumType;
        }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Enum.GetValues(EnumType);
        }
    }
}
