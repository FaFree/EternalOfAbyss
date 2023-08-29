using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtonManager : MonoBehaviour
{
    [SerializeField] private List<Button> buttons;

    public GameObject currentWindow;

    public void OnClick(Button clickedButton)
    {
        foreach (var button in buttons)
        {
            button.interactable = button != clickedButton;
        }
        
        currentWindow.SetActive(false);
    }

    public void OnClickActive(GameObject newWindow)
    {
        newWindow.SetActive(true);
        currentWindow = newWindow;
    }
}
