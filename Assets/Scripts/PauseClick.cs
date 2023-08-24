using DG.Tweening;
using UnityEngine;

public class PauseClick : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;

    public void OnClick()
    {
        pauseMenu.SetActive(true);
        pauseMenu.transform.DOScale(1, 1);
    }
}
