using System;

namespace Configs.BoostsConfig
{
    [Serializable]
    public struct TurretBoostConfig
    {
        public float damage;
        public float buildTime;
        
        public bool isBuilding;
        
        public string buildObj;
        public string ghostObj;
        public string buildingTag;
        
        public override string ToString()
        {
            string text = "";

            if (this.damage > 0)
                text += $"Damage: {(int)this.damage} \n";
            if (this.buildTime > 0)
                text += $"BuildTime: {this.buildTime}";

                return text;
        }
    }
}