using Scellecs.Morpeh;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelRestart : MonoBehaviour
{
    private const string EMPTY_SCENE = "Empty Scene";

    private string sceneName;

    private void Start()
    {
        this.sceneName = SceneManager.GetActiveScene().name;
    }

    public void OnClick()
    {
        SceneManager.LoadScene(EMPTY_SCENE, LoadSceneMode.Single);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
