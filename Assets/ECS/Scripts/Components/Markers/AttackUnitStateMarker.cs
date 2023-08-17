using Scellecs.Morpeh;

namespace ECS.Scripts.Components
{
    public struct AttackUnitStateMarker : IComponent
    {
        public bool isFirstAttack;
        public float timer;
    }
}