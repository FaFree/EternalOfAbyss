using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class Boosts : SerializedScriptableObject
{
    [SerializeField] private List<Boost> boosts;

    public Dictionary<string, Boost> BoostsMap { get; private set; }

    public void Initialize()
    {
        this.BoostsMap = this.boosts.ToDictionary(x => x.key, x => x);
    }
}

[Serializable]
public struct Boost
{
    public string key;

    public string skillName;
    public string skillInfo;

    public float heal;

    public float health;
    
    public float damage;

    public bool isReboundBoost;

    public bool isTripleArrow;

    public bool isPassingArrow;

    public bool isMultiple;
    
    public Sprite sprite;

    public GameObject boostEffect;

    public float effectPlayTime;
}
