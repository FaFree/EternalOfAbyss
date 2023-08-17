using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace ECS.Scripts.Components
{
    public class JoystickSpawnSystem : UpdateSystem
    {
        private Filter filter;
        private Transform canvas;
        private const string PREFAB = "Assets/Addressables/Floating Joystick.prefab";
        public override void OnAwake()
        {
            this.filter = this.World.Filter.With<PlayerComponent>();
            
            var canvasObject = new GameObject("Canvas");
            canvasObject.AddComponent<Canvas>();
            canvasObject.AddComponent<CanvasScaler>();
            canvasObject.AddComponent<GraphicRaycaster>();

            canvas = canvasObject.GetComponent<Transform>();

            canvasObject.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;

            var prefab = Addressables.LoadAssetAsync<GameObject>(PREFAB).WaitForCompletion();
            
            var go = Object.Instantiate(prefab, Vector3.zero, Quaternion.identity);
            go.transform.SetParent(canvas, false);

            var joystickEntity = this.World.CreateEntity();

            var joystick = go.GetComponent<FloatingJoystick>();
            
            joystickEntity.SetComponent(new JoystickComponent
            {
                Joystick = joystick,
            });
        }

        public override void OnUpdate(float deltaTime)
        {
            
        }
    }
}