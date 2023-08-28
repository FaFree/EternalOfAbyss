using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnPlayButtonClick : MonoBehaviour
{
    public void OnClick()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
