using Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour
{
    public void OnClick()
    {
        WorldModels.Default.Get<BoostsModel>().Clear();
        
        SceneManager.LoadSceneAsync("MenuScene");
    }
}
