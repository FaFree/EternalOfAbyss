using System;
using System.Collections.Generic;
using System.Linq;
using ECS.Scripts.Components;
using Scripts;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class Boosts : ScriptableObject
{
    [SerializeField] private List<Boost> boosts;

    public Dictionary<string, Boost> BoostsMap { get; private set; }

    public void Initialize()
    {
        this.BoostsMap = this.boosts.ToDictionary(x => x.key, x => (Boost) x.Clone());
    }

    public Dictionary<string, Boost> GetAvaliableBoosts()
    {
        var tempBoosts = new Dictionary<string, Boost>();

        foreach (var boost in BoostsMap)
        {
            if (boost.Value.isActive == false)
            {
                tempBoosts.Add(boost.Key, boost.Value);
            }
        }

        return tempBoosts;
    }

    public void Clear()
    {
        foreach (var boostKvp in BoostsMap)
        {
            var tempValue = boostKvp.Value;
            tempValue.isActive = false;
            
            BoostsMap[boostKvp.Key] = tempValue;
        }
    }
}

[Serializable]
public struct Boost : ICloneable
{
    public string key;

    public string skillName;
    public string skillInfo;

    public float heal;

    public float health;
    
    public float damage;

    [FormerlySerializedAs("isReboundBoost")] public bool isReboundArrow;
    public bool isTripleArrow;
    public bool isPassingArrow;
    public bool isMultiple;
    public bool isActive;
    
    public Sprite sprite;

    public GameObject boostEffect;

    public float effectPlayTime;
    
    public object Clone()
    {
        return this.MemberwiseClone();
    }
}

public class BoostsModel
{
    public bool isTripleArrow;
    public bool isPassingArrow;
    public bool isReboundArrow;
    
    public List<Boost> boosts;


    public void Clear()
    {
        isTripleArrow = false;
        isPassingArrow = false;
        isReboundArrow = false;
        
        boosts.Clear();

        var boostsMap = WorldModels.Default.Get<Boosts>();

        boostsMap.Clear();
    }

    public void AddBoost(Boost boost)
    {
        boosts.Add(boost);

        if (boost.isTripleArrow)
            this.isTripleArrow = true;
        if (boost.isPassingArrow)
            this.isPassingArrow = true;
        if (boost.isReboundArrow)
            this.isReboundArrow = true;
    }
    
    public BoostsModel()
    {
        boosts = new List<Boost>();
    }
}
