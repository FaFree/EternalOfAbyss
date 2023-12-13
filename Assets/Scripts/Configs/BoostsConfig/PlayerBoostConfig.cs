using System;

namespace Configs.BoostsConfig
{
    [Serializable]
    public struct PlayerBoostConfig
    {
        public float damage;
        
        public float coinScaler;
        
        public override string ToString()
        {
            string text = "";

            if (this.damage > 0)
                text += $"Damage: {(int)this.damage} \n";

            return text;
        }
    }
}