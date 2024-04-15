namespace EasyAbp.AbpHelper.Core.Models
{
    public class PropertyInfo
    {
        public string Type { get; }

        public string Name { get; }

        public string DisplayName { get; }

        public string Document { get; set; }

        public PropertyInfo(string type, string name, string document, string displayName)
        {
            Type = type;
            Name = name;
            Document = document;
            DisplayName = displayName;
        }
    }
}