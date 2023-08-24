using Scellecs.Morpeh;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelRestart : MonoBehaviour
{
    private string sceneName;

    private void Start()
    {
        this.sceneName = SceneManager.GetActiveScene().name;
    }

    public void OnClick()
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
