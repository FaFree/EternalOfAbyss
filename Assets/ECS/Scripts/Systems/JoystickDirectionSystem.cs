using System.Linq;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace ECS.Scripts.Components
{
    public class JoystickDirectionSystem : UpdateSystem
    {
        private Filter joyFilter;
        private Filter playerFilter;
        
        public override void OnAwake()
        {
            this.joyFilter = this.World.Filter.With<JoystickComponent>();
            this.playerFilter = this.World.Filter.With<PlayerComponent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            var joystickEntity = this.joyFilter.FirstOrDefault();
            var playerEntity = this.playerFilter.FirstOrDefault();

            if (joystickEntity == default || playerEntity == default)
                return;

            var direction = joystickEntity.GetComponent<JoystickComponent>().Joystick.Direction;

            playerEntity.GetComponent<PlayerComponent>().direction = new Vector3(direction.x, 0, direction.y);
        }
    }
}