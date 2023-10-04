using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Scripts;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class SkillSpawner : MonoBehaviour
{
    private const string prefabKey = "Assets/Addressables/Skills/skill.prefab";
    private Boosts boosts;
    private Sequence sequence;

    private void Start()
    {
        this.boosts = WorldModels.Default.Get<Boosts>();

        var prefab = Addressables.LoadAssetAsync<GameObject>(prefabKey).WaitForCompletion();

        for (int i = 0; i < 3; i++)
        {
            if (boosts.GetAvaliableBoosts().Count == 0)
                return;
            
            var go = Instantiate(prefab);
            
            go.transform.SetParent(this.transform);

            var boostView = go.GetComponent<BoostView>();

            int skillNum =  Random.Range(0, boosts.GetAvaliableBoosts().Count);

            var boost = boosts.GetAvaliableBoosts().ElementAt(skillNum);
            
            boostView.Init(boost.Key);
        }
    }
}
