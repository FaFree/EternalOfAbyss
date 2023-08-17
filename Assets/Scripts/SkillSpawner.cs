using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Factory;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class SkillSpawner : MonoBehaviour
{
    private const string prefabKey = "Assets/Addressables/Skills/skill.prefab";
    private Boosts boosts;

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

            var boost = boosts.BoostsMap.ElementAt(Random.Range(0, boosts.BoostsMap.Count));
            boostView.Init(boost.Key);
        }
    }
}
