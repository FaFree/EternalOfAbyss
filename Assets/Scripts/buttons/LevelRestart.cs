using ECS.Scripts.Events;
using Scellecs.Morpeh;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelRestart : MonoBehaviour
{
    [SerializeField] private Transform root;
    private Event<LevelRestartRequest> restartRequest;

    private void Start()
    {
        this.restartRequest = World.Default.GetEvent<LevelRestartRequest>();
        Time.timeScale = 0f;
    }

    public void OnClick()
    {
        Destroy(this.root.gameObject);
        restartRequest.NextFrame(new LevelRestartRequest());
        Time.timeScale = 1f;
    }
}
