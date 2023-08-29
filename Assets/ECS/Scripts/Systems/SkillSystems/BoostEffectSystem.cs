using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;

namespace ECS.Scripts.Components
{
    public class BoostEffectSystem : UpdateSystem
    {
        private Filter effectFilter;
        
        public override void OnAwake()
        {
            this.effectFilter = this.World.Filter.With<EffectMarker>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var effectEntity in effectFilter)
            {
                ref var effectComponent = ref effectEntity.GetComponent<EffectMarker>();

                effectComponent.currentTime += deltaTime;

                if (effectComponent.currentTime > effectComponent.playTime)
                {
                    Destroy(effectComponent.effectObject);
                    this.World.RemoveEntity(effectEntity);
                }
            }
        }
    }
}