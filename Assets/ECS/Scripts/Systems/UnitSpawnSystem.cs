using DefaultNamespace;
using ECS.Scripts.Events;
using Scripts;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using State_Machine.MobStateMachine;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AI;
using UnityEngine.UI;

namespace ECS.Scripts.Components
{
    public class UnitSpawnSystem : UpdateSystem
    {

        private Event<UnitSpawnRequest> unitSpawnRequest;

        public override void OnAwake()
        {
            this.unitSpawnRequest = this.World.GetEvent<UnitSpawnRequest>();
        }

        public override void OnUpdate(float deltaTime)
        {
            if (!unitSpawnRequest.IsPublished)
            {
                return;
            }

            foreach (var evt in unitSpawnRequest.BatchedChanges)
            {
                ref var zoneComponent = ref evt.zone.GetComponent<ZoneComponent>();
            
                var radius = zoneComponent.radius;
                var position = zoneComponent.position.position;
            
                var pos = new Vector3(Random.Range(position.x - radius, position.x + radius), 0,
                    Random.Range(position.z - radius, position.z + radius));
                
                var prefab = Addressables.LoadAssetAsync<GameObject>(evt.config.prefab).WaitForCompletion();

                var go = Object.Instantiate(prefab, pos, Quaternion.identity);
        
                var entity = World.Default.CreateEntity();
        
                entity.SetComponent(new TransformComponent
                {
                    transform = go.transform,
                });
            
                entity.SetComponent(new HealthBarComponent
                {
                    healthBar = go.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>(),
                    canvas = go.transform.GetChild(0)
                });

                var stateMachine = new StateMachine();
            
                stateMachine.AddState(new IdleMobState(stateMachine, ref entity));
                stateMachine.AddState(new RunMobState(stateMachine, ref entity));
                stateMachine.AddState(new AttackMobState(stateMachine, ref entity));
                stateMachine.AddState(new ComeBackMobState(stateMachine, ref entity));
                stateMachine.AddState(new DieMobState(stateMachine, ref entity));
                stateMachine.SetState<IdleMobState>();
            
                var anim = go.GetComponent<Animator>();
            
                var unit = new Unit(evt.config,0.7f, 1.267f);

                entity.SetComponent(new UnitComponent()
                {
                    coinReward = evt.config.coinReward,
                    spawnPosition = pos,
                    zone = evt.zone,
                    stateMachine = stateMachine,
                    animator = anim,
                    unit = unit,
                    dieTime = 0f,
                    xpReward = evt.config.xpReward
                });
                
                entity.SetComponent(new HealthComponent
                {
                    health = unit.MaxHealth
                });
                
                entity.SetComponent(new NavMeshAgentComponent
                {
                    agent = go.GetComponent<NavMeshAgent>()
                });
            }
        }
    }
}