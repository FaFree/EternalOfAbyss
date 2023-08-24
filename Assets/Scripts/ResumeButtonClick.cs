using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ResumeButtonClick : MonoBehaviour
{
    public void OnClick()
    {
        this.transform.DOScale(0, 0.5f).OnComplete(() => this.gameObject.SetActive(false));
    }
}
