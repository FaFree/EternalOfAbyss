using DefaultNamespace;
using ECS.Scripts.Events;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Scripts;
using UnityEngine;
using State_Machine;

namespace ECS.Scripts.Components.AttackSystems
{
    public class RangeAttackSystem : UpdateSystem
    {
        private Event<RangeAttackRequest> rangeAttackRequest;
        private Event<ArrowRequest> arrowRequest;

        private Filter playerFilter;
        public override void OnAwake()
        {
            this.rangeAttackRequest = this.World.GetEvent<RangeAttackRequest>();
            this.arrowRequest = this.World.GetEvent<ArrowRequest>();

            this.playerFilter = this.World.Filter.With<PlayerComponent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            if (!this.rangeAttackRequest.IsPublished)
                return;

            var playerEntity = this.playerFilter.FirstOrDefault();
            
            ref var playerTransform = ref playerEntity.GetComponent<TransformComponent>().transform;
            
            var playerModel = WorldModels.Default.Get<Player>();
            
            foreach (var evt in this.rangeAttackRequest.BatchedChanges)
            {
                Quaternion rotate = Quaternion.LookRotation(new Vector3(0, 0, 1));

                rotate = rotate * Quaternion.Euler(0, 90, 0);

                playerTransform.rotation = rotate;
                        
                this.arrowRequest.NextFrame(new ArrowRequest
                {
                    direction = new Vector3(0, 0, 1),
                    spawnPosition = playerTransform.position,
                    damage = playerModel.GetDamage(),
                    isPlayer = true
                });
            }
        }
    }
}