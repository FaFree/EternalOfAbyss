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
    private int previousSkill = -1;

    // Start is called before the first frame update
    private void Start()
    {
        this.boosts = WorldModels.Default.Get<Boosts>();

        var prefab = Addressables.LoadAssetAsync<GameObject>(prefabKey).WaitForCompletion();

        for (int i = 0; i < 2; i++)
        {
            var go = Instantiate(prefab);
            go.transform.SetParent(this.transform);

            var boostView = go.GetComponent<BoostView>();

            int skillNum;

            do
            {
                skillNum = Random.Range(0, boosts.BoostsMap.Count);
            } 
            while (skillNum == previousSkill);

            previousSkill = skillNum;
            
            var boost = boosts.BoostsMap.ElementAt(skillNum);
            boostView.Init(boost.Key);

            var seq = DOTween.Sequence();

            seq.Append(go.transform.DOScale(Vector3.one * 1.2f, 2));
            seq.Append(go.transform.DOScale(Vector3.one * 0.8f, 2));

            seq.SetLoops(-1);
        }
    }
}
