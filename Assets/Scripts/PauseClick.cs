using DG.Tweening;
using UnityEngine;

public class PauseClick : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;

    public void OnClick()
    {
        pauseMenu.SetActive(true);
        var groupCanvas = pauseMenu.GetComponent<CanvasGroup>();

        Time.timeScale = 0f;
        
        groupCanvas.DOFade(1, 1);
    }
}
