using DG.Tweening;
using ECS.Scripts.Events;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ECS.Scripts.Components
{
    public class LoadEndBehaviour : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        private Event<EndLoadEvent> endLoadedEvent;

        private bool isActive;

        private void Awake()
        {
            this.endLoadedEvent = World.Default.GetEvent<EndLoadEvent>();

            this.isActive = false;
        }

        private void Update()
        {
            this.slider.DOValue(0.7f, 10f);

            if (!this.endLoadedEvent.IsPublished && !this.isActive)
            {
                this.slider.DOKill();

                this.slider.DOValue(1, 2f).onComplete += LoadMenu;

                this.isActive = true;
            }
        }

        private void LoadMenu()
        {
            SceneManager.LoadSceneAsync("MenuScene");
        }
    }
}