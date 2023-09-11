using UnityEngine;

namespace Scripts.LevelModel
{
    public class LevelModel
    {
        private int currentLevel = 1;
        
        public void AddLevel()
        {
            currentLevel++;
        }

        public int GetCurrentLevel()
        {
            return currentLevel;
        }
    }
}