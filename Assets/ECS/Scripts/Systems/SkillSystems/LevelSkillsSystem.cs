using ECS.Scripts.Events;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ECS.Scripts.Components
{
    public class LevelSkillsSystem : UpdateSystem
    {
        private const string KEY = "Assets/Addressables/Skills/UiSkills.prefab";
        
        private GameObject prefab;

        private Event<OnLevelChanged> onLevelChanged;

        public override void OnAwake()
        {
            this.onLevelChanged = this.World.GetEvent<OnLevelChanged>();
        }

        public override void OnUpdate(float deltaTime)
        {
            if (!onLevelChanged.IsPublished)
                return;

            foreach (var evt in onLevelChanged.BatchedChanges)
            {
                Spawn();
            }
        }

        private void Spawn()
        {
            if (prefab.IsUnityNull())
                prefab = Addressables.LoadAssetAsync<GameObject>(KEY).WaitForCompletion();

            var go = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        }
    }
}