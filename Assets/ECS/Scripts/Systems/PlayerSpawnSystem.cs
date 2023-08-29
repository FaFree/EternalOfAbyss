using DefaultNamespace;
using Scripts;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Scripts.InventoryFeature;
using State_Machine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AI;
using UnityEngine.UI;

namespace ECS.Scripts.Components
{
    public class PlayerSpawnSystem : UpdateSystem
    {
        private const string PLAYER_PREFAB = "Assets/Addressables/Player.prefab";
        
        private const float ATTACK_LENGTH = 1.267f;

        public override void OnAwake()
        {
            var prefab = Addressables.LoadAssetAsync<GameObject>(PLAYER_PREFAB).WaitForCompletion();

            var go = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
        
            var entity = this.World.CreateEntity();
        
            entity.SetComponent(new TransformComponent
            {
                transform = go.transform,
            });

            StateMachine stateMachine = new StateMachine();
            
            stateMachine.AddState(new IdleState(stateMachine, ref entity));
            stateMachine.AddState(new RunState(stateMachine, ref entity));
            stateMachine.AddState(new AttackState(stateMachine, ref entity));
            stateMachine.SetState<IdleState>();
            
            Animator anim = go.GetComponent<Animator>();

            var config = new UnitConfig("Assets/Addressables/Player.prefab", 
                15, 2000, 2, 50, 2, 0, 0);
            
            UnitPlayer unitPlayer = new UnitPlayer(config, 0.7f,ATTACK_LENGTH, 
                0.2f, 5f, ref entity);

            entity.SetComponent(new PlayerComponent
            {
                speed = 5,
                stateMachine = stateMachine,
                animator = anim,
                UnitPlayerModel = unitPlayer
            });
            
            unitPlayer.ChangeItem(WorldModels.Default.Get<Inventory>().CurrentItems);
            
            entity.SetComponent(new HealthBarComponent
            {
                healthBar = go.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>(),
                canvas = go.transform.GetChild(0)
            });
            
            entity.SetComponent(new HealthComponent
            {
                health = unitPlayer.MaxHealth
            });

            entity.SetComponent(new NavMeshAgentComponent
            {
                agent = go.GetComponent<NavMeshAgent>()
            });
            
            entity.SetComponent(new BoostComponent
            {
                isReboundArrow = false,
                isTripleArrow = false
            });
        }

        public override void OnUpdate(float deltaTime)
        {
            
        }
    }
}