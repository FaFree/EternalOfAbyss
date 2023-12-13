using Configs;
using DefaultNamespace;
using Scripts;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Scripts.InventoryFeature;
using Scripts.LevelModel;
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

        private Filter baseFilter;
        
        public override void OnAwake()
        {
            this.baseFilter = this.World.Filter.With<BaseComponent>();

            var baseEntity = this.baseFilter.FirstOrDefault();

            if (baseEntity == default)
                return;

            ref var baseComponent = ref baseEntity.GetComponent<BaseComponent>();
            
            var inventory = WorldModels.Default.Get<Inventory>();
            
            var prefab = Addressables.LoadAssetAsync<GameObject>(PLAYER_PREFAB).WaitForCompletion();

            var go = Instantiate(prefab, baseComponent.playerSpawnPosition.position , Quaternion.identity);

            var playerConfig = go.GetComponent<PlayerConfig>();

            if (inventory.CurrentItems[ItemType.Weapon] != default)
            {
                var item = inventory.CurrentItems[ItemType.Weapon];

                var player = WorldModels.Default.Get<Player>();
                
                player.ChangeWeapon(item);

                var key = WorldModels.Default.Get<Prefabs>().prefabMap[item.key];

                var weaponPrefab = Addressables.LoadAssetAsync<GameObject>(key).WaitForCompletion();

                var weaponObj = Instantiate(weaponPrefab, playerConfig.WeaponLeftRoot);

                var ammoRoot = weaponObj.GetComponent<WeaponMonoConfig>().ammoSpawnRoot;

                playerConfig.ammoSpawnRoot = ammoRoot;
            }

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
            
            entity.SetComponent(new PlayerComponent
            {
                speed = 5,
                stateMachine = stateMachine,
                animator = anim,
                ammoSpawnRoot = playerConfig.ammoSpawnRoot
            });
            
            entity.SetComponent(new NavMeshAgentComponent
            {
                agent = go.GetComponent<NavMeshAgent>()
            });
        }

        public override void OnUpdate(float deltaTime)
        {
            
        }
    }
}