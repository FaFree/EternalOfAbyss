namespace Scripts.StorageService
{
    public interface IStorageService
    {
        public void Save(string key, object data);
        public T Load<T>(string key);
    }
}