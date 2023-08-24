using System;
using System.Collections.Generic;

namespace Scripts
{
    public class WorldModels
    {
        private Dictionary<Type, Object> container;

        private static WorldModels defaultWorld;
        
        public static WorldModels Default
        {
            get
            {
                if (defaultWorld == null)
                {
                    defaultWorld = new WorldModels();
                }

                return defaultWorld;
            }
        }

        public WorldModels()
        {
            this.container = new Dictionary<Type, Object>();
        }

        public T Get<T>() where T : class
        {
            return (T)container[typeof(T)];
        }
        
        public void Set<T>(T t) where T : class
        {
            if (container.ContainsKey(typeof(T)))
            {
                container[typeof(T)] = t;
            }
            else
            {
                container.Add(typeof(T), t);
            }
        }

        public bool isNull()
        {
            return container.Count == 0;
        }
    }
}