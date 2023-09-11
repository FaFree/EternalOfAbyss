using DefaultNamespace;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Scripts;
using UnityEngine;

namespace ECS.Scripts.Components
{
    public class HealthBarSystem : UpdateSystem
    {
        private Filter playerFilter;
        private Filter unitFilter;
        public override void OnAwake()
        {
            this.playerFilter = this.World.Filter.With<PlayerComponent>();
            this.unitFilter = this.World.Filter.With<UnitComponent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var playerEntity in playerFilter)
            {
                ref var healthBarComponent = ref playerEntity.GetComponent<HealthBarComponent>();
                ref var canvasTransform = ref healthBarComponent.canvas;
                canvasTransform.rotation = Quaternion.Euler(0, 0,0);
                
                var playerModel = WorldModels.Default.Get<UnitPlayer>();
                ref var healthBar = ref playerEntity.GetComponent<HealthBarComponent>().healthBar;
                ref var playerHealth = ref playerEntity.GetComponent<HealthComponent>().health;
                healthBar.fillAmount = GetHealthOnPercent(playerHealth, playerModel.MaxHealth);
            }

            foreach (var unitEntity in unitFilter)
            {
                ref var healthBarComponent = ref unitEntity.GetComponent<HealthBarComponent>();
                ref var canvasTransform = ref healthBarComponent.canvas;
                canvasTransform.rotation = Quaternion.Euler(0, 0,0);
                
                ref var unitModel = ref unitEntity.GetComponent<UnitComponent>().unit;
                ref var healthBar = ref unitEntity.GetComponent<HealthBarComponent>().healthBar;
                ref var unitHealth = ref unitEntity.GetComponent<HealthComponent>().health;
                healthBar.fillAmount = GetHealthOnPercent(unitHealth, unitModel.MaxHealth);
            }
        }

        private float GetHealthOnPercent(float currentHealth, float maxHealth)
        {
            return currentHealth / maxHealth;
        }
    }
}