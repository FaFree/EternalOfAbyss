using System;
using System.Collections.Generic;
using System.Linq;
using Scripts;
using Scripts.BoostFeature;
using UnityEngine;

[Serializable]
public class Boosts : ScriptableObject
{
    [SerializeField] private List<Boost> boosts;

    public Dictionary<string, Boost> BoostsMap { get; private set; }

    public void Initialize()
    {
        this.BoostsMap = this.boosts.ToDictionary(x => x.key, x => x);
    }

    public Dictionary<string, Boost> GetAvailableBoosts()
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
            boostKvp.Value.Deactivate();
        }
    }
}

[Serializable]
public class Boost
{
    public float damage;
    public float health;
    public float regeneration;
    
    public string key;

    public string skillName;
    public string skillInfo;

    public Categories category;

    public bool isReboundArrow;
    public bool isTripleArrow;
    public bool isPassingArrow;

    public float price;

    public bool isActive;
    
    public Sprite sprite;

    public void Deactivate()
    {
        this.isActive = false;
    }

    public void Activate()
    {
        this.isActive = true;
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
        
        WorldModels.Default.Get<Boosts>().Clear();
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

        foreach (var kvp in WorldModels.Default.Get<Boosts>().BoostsMap)
        {
            if (kvp.Value.isActive)
                this.AddBoost(kvp.Value);
        }
    }
}
