using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

namespace Multimedia.CustomMarkupExtensions
{
    public class ObjectReferenceExtension : MarkupExtension
    {
        [ConstructorArgument("ObjectName")]
        public string ObjectName { get; set; }

        public ObjectReferenceExtension(string name)
        {
            ObjectName = name;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (serviceProvider.GetService(typeof(IProvideValueTarget)) is IProvideValueTarget targetProvider &&
                targetProvider.TargetObject is FrameworkElement targetElement)
            {
                return targetElement.FindName(ObjectName);
            }

            return null;
        }
    }
}
