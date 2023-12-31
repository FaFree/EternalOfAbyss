using System;
using System.Collections.Generic;
using System.Linq;
using ECS.Scripts.Events.BankEvents;
using Scellecs.Morpeh;
using Scripts.StorageService;

namespace ResourceFeature
{
    public static class Resources
    {
        private static IStorageService storageService;
        
        private static Dictionary<string, Resource> resourcesMap;

        private const string KEY = "Resources";
        
        static Resources()
        {
            storageService = new JsonFileStorageService();
            
            resourcesMap = new Dictionary<string, Resource>();
            
            resourcesMap.Add("Coin", new Resource("Coin")); 
            resourcesMap.Add("Exp", new Resource("Exp"));
            resourcesMap.Add("Diamond", new Resource("Diamond"));
            
            GetResources();
            resourcesMap["Exp"].SetAmount(0);
            resourcesMap["Diamond"].SetAmount(50000);
        }

        public static Resource GetResource(string resourceName)
        {
            if (resourcesMap.TryGetValue(resourceName, out var resource))
            {
                return resource;
            }
            else
            {
                return null;
            }
        }

        public static void SaveResources()
        {
            var saver = new ResourceSaver();
            
            saver.resourcesList = resourcesMap
                .Select(x => new Res(x.Value.ResourceType, x.Value.ResourceCount))
                .ToList();

            storageService.Save(KEY, saver);
        }

        private static void GetResources()
        {
            ResourceSaver saver = new ResourceSaver();
            
            try
            {
                saver = storageService.Load<ResourceSaver>(KEY);
            }
            catch
            {
                SaveResources();
                return;
            }
            
            
            foreach (var savedResource in saver.resourcesList)
            {
                if (!resourcesMap.ContainsKey(savedResource.resourceType))
                {
                    resourcesMap.Add(savedResource.resourceType, new Resource(savedResource.resourceType));
                }
                
                resourcesMap[savedResource.resourceType].SetAmount(savedResource.resourceCount);
            }
        }
    }
    
    public class Resource
    {
        public string ResourceType { get; private set; }
        
        public double ResourceCount { get; private set; }
        
        private Event<OnResourceChanged> onResourceChanged;
        
        public Resource(string resourceType)
        {
            this.ResourceType = resourceType;

            onResourceChanged = World.Default.GetEvent<OnResourceChanged>();
        }
        
        public void AddResource(double resourceCount)
        {
            this.ResourceCount += resourceCount;
            
            onResourceChanged.NextFrame(new OnResourceChanged());
        }
        
        public void SetAmount(double amount)
        {
            this.ResourceCount = amount;
            
            onResourceChanged.NextFrame(new OnResourceChanged());
        }

        public bool IsEnough(double resourceCount)
        {
            return this.ResourceCount >= resourceCount;
        }

        public bool TakeResource(double resourceCount)
        {
            if (this.ResourceCount < resourceCount)
            {
                return false;
            }
                
            this.ResourceCount -= resourceCount;
            
            onResourceChanged.NextFrame(new OnResourceChanged());

            return true;
        }

        public override string ToString()
        {
            var resource = (int)ResourceCount;
            
            return resource.ToString();
        }
    }

    [Serializable]
    public class ResourceSaver
    {
        public List<Res> resourcesList;

        public ResourceSaver()
        {
            resourcesList = new List<Res>();
        }
    }
    
    [Serializable]
    public struct Res
    {
        public double resourceCount;
        public string resourceType;

        public Res(string resourceType, double resourceCount)
        {
            this.resourceType = resourceType;
            this.resourceCount = resourceCount;
        }
    }
}