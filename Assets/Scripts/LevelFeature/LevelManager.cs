using System;
using Scripts.StorageService;
using UnityEngine;
using Resources = ResourceFeature.Resources;

namespace Scripts.LevelFeature
{
    public static class LevelManager
    {
        private const string resourceKey = "Exp";
        private const string saveKey = "Level";
        private const double linearFactor = 5.0;
        private const double baseNeedXp = 10.0;
        
        public static bool isView = false;
        
        private static IStorageService storageService;
        
        private static int maxLevel;
        
        public static int CurrentLevel { get; private set; }

        private static double CurrentXp
        {
            get
            {
                var res = Resources.GetResource(resourceKey);
                return res.ResourceCount;
            }
        }

        static LevelManager()
        {
            storageService = new JsonFileStorageService();
            maxLevel = 10;
            CurrentLevel = 1;

           // try
           // {
           //     Load();
           // }
           // catch
           // {
           //     Save();
           // }
        }

        public static bool TryAddLevel()
        {
            if (CurrentXp > GetRequiredXp())
            {
                return AddLevel();
            }

            return false;
        }

        private static bool AddLevel()
        {
            if (CurrentLevel < maxLevel)
            {
                var res = Resources.GetResource(resourceKey);
                res.TakeResource(GetRequiredXp());
                
                CurrentLevel += 1;
                
                Save();
                
                return true;
            }

            return false;
        }

        private static void Save()
        {
            var levelSaver = new LevelSaver();
            levelSaver.currentLevel = CurrentLevel;
            
            storageService.Save(saveKey, levelSaver);
        }

        private static void Load()
        {
            var levelSaver = storageService.Load<LevelSaver>(saveKey);
            CurrentLevel = levelSaver.currentLevel;
        }
        
        public static double GetRequiredXp()
        {
            return baseNeedXp + (CurrentLevel + 1) * linearFactor;
        }
    }

    [Serializable]
    public struct LevelSaver
    {
        public int currentLevel;
    }
}