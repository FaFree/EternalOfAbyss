using Scripts;
using Scripts.StorageService;

namespace DefaultNamespace
{
    public class Player : BaseUnit
    {
        private IStorageService storageService;
        
        public Player(UnitConfig config, float firstAttackTime, float attackAnimationTime) 
            : base(config, firstAttackTime, attackAnimationTime)
        {
            
        }
    }
}