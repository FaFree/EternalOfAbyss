using DG.Tweening;
using ECS.Scripts.Events;
using Scellecs.Morpeh;
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
            if (!this.isActive)
            {
                this.slider.DOValue(0.7f, 5f);
                this.isActive = true;
            }

            if (!this.endLoadedEvent.IsPublished)
            {
                this.slider.DOKill();

                this.slider.DOValue(1, 0.1f).OnComplete(() =>
                {
                    SceneManager.LoadSceneAsync("MenuScene");
                });
            }
        }
    }
}