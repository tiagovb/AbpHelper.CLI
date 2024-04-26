namespace AbpTools.AbpHelper.Core.Models
{
    public class PropertyInfo
    {
        public string Type { get; }

        public string Name { get; }

        public string DisplayName { get; }

        public string Document { get; set; }
        public string? Modifiers { get; set; }

        public PropertyInfo(string type, string name, string document, string modifiers, string displayName)
        {
            Type = type;
            Name = name;
            Document = document;
            DisplayName = displayName;
            Modifiers = modifiers;
        }
    }
}