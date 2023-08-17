using System;
using Scellecs.Morpeh;

namespace ECS.Scripts.Components
{
    [Serializable]
    public struct PrefabsComponent : IComponent
    {
        public const string PLAYER_PREFAB = "Assets/Addressables/Player.prefab";
        public const string JOYSTICK_PREFAB = "Assets/Addressables/Floating Joystick.prefab";
    }
}