using System.Collections;
using System.Collections.Generic;
using CoinScaleSystem;
using Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour
{
    public void OnClick()
    {
        WorldModels.Default.Get<BoostsModel>().Clear();

        RewardCoinScaler.UpdateScale();

        SceneManager.LoadSceneAsync("MenuScene");
    }
}
