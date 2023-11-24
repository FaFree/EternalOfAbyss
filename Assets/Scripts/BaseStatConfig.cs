using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class BaseStatConfig : ScriptableObject
    {
        public float regeneration;
        
        public float maxHealth;

        public override string ToString()
        {
            return $" Health: {this.maxHealth}\n Regeneration: {this.regeneration}";
        }
    }
}
