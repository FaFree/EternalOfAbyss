using System;

namespace Configs.BoostsConfig
{
    [Serializable]
    public struct BaseBoostConfig
    {
        public float health;
        public float regeneration;
        
        public override string ToString()
        {
            string text = "";

            if (this.health > 0)
                text += $"Health: {(int)this.health} \n";
            if (this.regeneration > 0)
                text += $"Regeneration: {(int)this.regeneration} \n";
        
            return text;
        }
    }
}