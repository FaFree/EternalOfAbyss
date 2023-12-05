using System;
using ECS.Scripts.Events;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace ECS.Scripts.Components
{
    public class RestartSystem : UpdateSystem
    {
        private const string UI_MENU = "Assets/Addresables/UIRestart.prefab";
        
        private Event<PlayerDestroyedEvent> playerDestroyedEvent;

        public override void OnAwake()
        {
            this.playerDestroyedEvent = this.World.GetEvent<PlayerDestroyedEvent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            if (!playerDestroyedEvent.IsPublished)
                return;

            foreach (var evt in playerDestroyedEvent.BatchedChanges)
            {
                var prefab = Addressables.LoadAssetAsync<GameObject>(UI_MENU).WaitForCompletion();
                Instantiate(prefab);
            }
        }
    }
}