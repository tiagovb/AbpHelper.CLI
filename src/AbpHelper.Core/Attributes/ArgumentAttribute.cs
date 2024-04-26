using System;

namespace AbpTools.AbpHelper.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ArgumentAttribute : BaseAttribute
    {
        public ArgumentAttribute(string name) : base(name)
        {
        }
    }
}
