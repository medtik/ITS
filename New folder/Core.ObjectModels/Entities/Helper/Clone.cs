namespace Core.ObjectModels.Entities.Helper
{
    using Newtonsoft.Json;

    public static class Clone
    {
        public static T CloneObject<T> (this T entity)
        {
            if (ReferenceEquals(entity, null))
            {
                return default(T);
            }

            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(entity, new JsonSerializerSettings
            {
                ObjectCreationHandling = ObjectCreationHandling.Replace,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }));
        }
    }
}