using DG.Tweening;
using ECS.Scripts.Events;
using Scripts;
using Scellecs.Morpeh;
using Scripts.LevelFeature;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoostView : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillInfo;
 
    private Event<BoostRequest> boostRequest;

    private Transform parent;

    private string boostKey;

    private Boosts boosts;

    private Boost Boost => this.boosts.BoostsMap[this.boostKey];

    public void Init(string boostKey)
    {
        this.boostKey = boostKey;
        
        this.image.sprite = this.Boost.sprite;
        this.skillName.text = this.Boost.skillName;
        this.skillInfo.text = this.Boost.skillInfo;
    }
    
    private void Awake()
    {
        this.boosts = WorldModels.Default.Get<Boosts>();

        this.boostRequest = World.Default.GetEvent<BoostRequest>();
        
        button.interactable = true;
    }

    public void OnClick()
    {
        boostRequest.NextFrame(new BoostRequest
        {
            boostKey = boostKey,
        });
        
        button.interactable = false;
        
        parent = this.gameObject.transform.parent;

        Time.timeScale = 1f;

        parent.DOMoveY(-1000, 1).OnComplete(() =>
        {
            parent.DOKill();
            
            if (!Boost.isMultiple)
            {
                boosts.BoostsMap.Remove(Boost.key);
            }
            
            Destroy(parent.parent.gameObject);
            LevelManager.isView = false;
        });
    }
}
