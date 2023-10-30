using UnityEngine;

namespace Scripts
{
    public class LevelsModel
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