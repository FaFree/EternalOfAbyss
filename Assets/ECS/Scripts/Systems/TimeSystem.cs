using ECS.Scripts.Events;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;

namespace ECS.Scripts.Components
{
    public class TimeSystem : UpdateSystem
    {
        private Event<BuildEndedEvent> buildEndedEvent;
        
        private bool isEnable = false;
        
        
        public override void OnAwake()
        {
            this.buildEndedEvent = this.World.GetEvent<BuildEndedEvent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            if (!this.isEnable)
                Time.timeScale = 0f;

            if (this.buildEndedEvent.IsPublished)
            {
                this.isEnable = true;
                Time.timeScale = 1f;
            }
        }
    }
}