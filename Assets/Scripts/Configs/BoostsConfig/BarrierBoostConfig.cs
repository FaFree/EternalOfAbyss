using System;

namespace Configs.BoostsConfig
{
    [Serializable]
    public struct BarrierBoostConfig
    {
        public float health;
        
        public string buildObj;
        public string ghostObj;
        public string buildingTag;

        public override string ToString()
        {
            string text = "";
            
            if (this.health > 0)
                text += $"Health: {(int)this.health} \n";

            return text;
        }
    }
}