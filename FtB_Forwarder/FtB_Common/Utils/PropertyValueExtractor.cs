namespace FtB_Common.Utils
{
    public static class PropertyValueExtractor
    {
        public static string GetDataFormatId(object type)
        {
            return GetPropertyValueFromObject<string>(type, "DataFormatId");
        }

        public static string GetDataFormatVersion(object type)
        {
            return GetPropertyValueFromObject<string>(type, "DataFormatVersion");
        }

        private static T GetPropertyValueFromObject<T>(object obj, string propName)
        {
            var prop = obj.GetType().GetProperty(propName);
            T value = default(T);
            try
            {
                value = (T)prop.GetValue(obj);
            }
            catch { }
            return value;
        }
    }
}
