using DG.Tweening;
using ECS.Scripts.Events;
using Scripts;
using Scellecs.Morpeh;
using UnityEngine;
using UnityEngine.UI;

public class BoostView : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Button button;

    private Event<BoostRequest> boostRequest;

    private Transform parent;

    private string boostKey;

    private Boosts boosts;

    private Boost Boost => this.boosts.BoostsMap[this.boostKey];

    public void Init(string boostKey)
    {
        this.boostKey = boostKey;
        
        this.image.sprite = this.Boost.sprite;
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

        var seq = DOTween.Sequence();
        
        seq.Append(parent.DOMoveY(-600, 2, false));

        seq.OnComplete(() =>
        {
            Destroy(parent.parent.gameObject);
        });
    }
}
