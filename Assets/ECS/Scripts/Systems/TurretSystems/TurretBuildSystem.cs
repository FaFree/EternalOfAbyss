using DefaultNamespace;
using DG.Tweening;
using ResourceFeature;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Scripts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Resources = ResourceFeature.Resources;

namespace ECS.Scripts.Components
{
    public class TurretBuildSystem : UpdateSystem
    {
        private const float MINIMAL_DISTANCE = 2f;
        
        private Filter turretFilter;
        private Filter playerFilter;

        private Slider progressBar;
        private GameObject progressBarObj;

        private Entity currentEntity;
        
        private float currentProgress;

        private bool isBuilding;

        private bool isInitialized;
        
        public override void OnAwake()
        {
            this.turretFilter = this.World.Filter.With<TurretComponent>().
                With<TransformComponent>();
            
            this.playerFilter = this.World.Filter.
                With<PlayerComponent>().
                With<TransformComponent>();
            
            this.isBuilding = false;
        }

        public override void OnUpdate(float deltaTime)
        {
            if (!this.isInitialized)
            {
                var config = WorldModels.Default.Get<ProgressBarConfig>();

                this.progressBar = config.progressBarSlider;
                this.progressBarObj = config.progressBarObj;

                this.isInitialized = true;
            }
            
            var playerEntity = this.playerFilter.FirstOrDefault();

            if (playerEntity == default)
                return;

            ref var playerTransform = ref playerEntity.GetComponent<TransformComponent>().transform;

            foreach (var turretEntity in this.turretFilter)
            {
                ref var turretTransform = ref turretEntity.GetComponent<TransformComponent>().transform;
                ref var turretComponent = ref turretEntity.GetComponent<TurretComponent>();

                var distance = Vector3.SqrMagnitude(playerTransform.position - turretTransform.position);

                if (distance < MINIMAL_DISTANCE && !this.isBuilding)
                {
                    this.isBuilding = true;

                    if (turretEntity.Has<ProgressMarker>())
                    {
                        ref var progressMarker = ref turretEntity.GetComponent<ProgressMarker>();

                        progressMarker.progress += deltaTime;
                        
                        this.currentEntity = turretEntity;
                        
                        this.progressBarObj.SetActive(true);

                        if (progressMarker.progress >= turretComponent.config.buildTime)
                        {
                            turretEntity.RemoveComponent<ProgressMarker>();
                            
                            turretEntity.AddComponent<ActiveMarker>();
                        
                            turretComponent.config.turretObject.SetActive(true);
                            
                            this.progressBarObj.SetActive(false);
                        }
                        
                        this.currentProgress = progressMarker.progress / turretComponent.config.buildTime;

                        this.progressBar.DOKill();
                            
                        this.progressBar.DOValue(this.currentProgress, 0.1f);
                    }
                    
                    if (!turretEntity.Has<ProgressMarker>())
                    {
                        if (!turretEntity.Has<ActiveMarker>())
                        {
                            turretEntity.AddComponent<ProgressMarker>();

                            ref var progressMarker = ref turretEntity.GetComponent<ProgressMarker>();

                            this.currentEntity = turretEntity;
                            
                            this.progressBarObj.SetActive(true);

                            progressMarker.progress += deltaTime;
                            
                            this.currentProgress = progressMarker.progress / turretComponent.config.buildTime;

                            this.progressBar.DOKill();
                            
                            this.progressBar.DOValue(this.currentProgress, 0.1f);
                        }
                    }
                }
            }

            if (!this.isBuilding)
            {
                this.currentProgress = 0;
                this.progressBar.value = 0;
                this.progressBarObj.SetActive(false);
                
                if (!this.currentEntity.IsUnityNull())
                {
                    this.currentEntity.RemoveComponent<ProgressMarker>();
                }
            }

            this.isBuilding = false;
        }
    }
}