using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.PlayerUpgradeFeature;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeIconView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textPercent;
    [SerializeField] private TextMeshProUGUI textName;
    [SerializeField] private TextMeshProUGUI textButton;
    [SerializeField] private Image spriteRoot;

    public Upgrade currentUpgrade;

    public Action onClick;

    public void OnClick()
    {
        onClick?.Invoke();
    }

    public void Initialize(Upgrade upgrade)
    {
        this.currentUpgrade = upgrade;
        
        this.textPercent.text = upgrade.GetPercentToString();

        this.textName.text = upgrade.upgradeType.ToString();

        this.textButton.text = "Buy " + (int)upgrade.upgradePrice;

        this.spriteRoot.sprite = upgrade.upgradeSprite;
    }
}
