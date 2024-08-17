namespace TGB.Web
{
    public static class TagHelpers
    {
        public static List<string> UnflattenTags(string value)
        {
            return value?.Split(';').ToList() ?? [];
        }

        public static string FlattenTags(IEnumerable<string> tags)
        {
            return string.Join(';', tags);
        }

        public static string FormatTags(string value)
        {
            return value
                .Replace(" ", "-")
                .ToLower();
        }
    }
}
