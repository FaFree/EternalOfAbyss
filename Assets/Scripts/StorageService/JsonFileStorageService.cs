using System.IO;
using UnityEngine;
using Application = UnityEngine.Device.Application;

namespace Scripts.StorageService
{
    public class JsonFileStorageService : IStorageService
    {
        public void Save(string key, object data)
        {
            var path = BuildPath(key);
            var json = JsonUtility.ToJson(data);

            using (var fileStream = new StreamWriter(path))
            {
                fileStream.Write(json);
            }
        }

        public T Load<T>(string key)
        {
            var path = BuildPath(key);

            using (var fileStream = new StreamReader(path))
            {
                var json = fileStream.ReadToEnd();
                var data = JsonUtility.FromJson<T>(json);

                return data;
            }
        }

        private string BuildPath(string key)
        {
            return Path.Combine(Application.persistentDataPath, key);
        }
    }
}