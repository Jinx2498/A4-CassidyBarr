namespace GameBrains.Extensions.Attributes
{
    public class HidePropertiesInInspectorAttribute : System.Attribute
    {
        public HidePropertiesInInspectorAttribute(params string[] hiddenProperties)
        {
            HiddenProperties = hiddenProperties;
        }

        public string[] HiddenProperties { get; }
    }
}