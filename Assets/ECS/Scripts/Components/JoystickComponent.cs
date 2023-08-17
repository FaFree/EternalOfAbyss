using System;
using Scellecs.Morpeh;

namespace ECS.Scripts.Components
{
    [Serializable]
    public struct JoystickComponent : IComponent
    {
        public FloatingJoystick Joystick;
    }
}