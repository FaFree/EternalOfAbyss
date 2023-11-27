using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using Scripts;
using Scripts.BoostFeature;
using UnityEngine;

[Serializable]
public class Boosts : ScriptableObject
{
    [SerializeField] private List<Boost> boosts;

    public List<Boost> BoostsList { get; private set; }

    public void Initialize()
    {
        this.BoostsList = this.boosts;
    }

    public List<Boost> GetAvailableBoosts()
    {
        var tempBoosts = new List<Boost>();

        foreach (var boost in BoostsList)
        {
            if (!boost.isActive)
            {
                tempBoosts.Add(boost);
            }
        }

        return tempBoosts;
    }

    public void Clear()
    {
        foreach (var boost in BoostsList)
        {
            boost.Deactivate();
            boost.RevertMultiply();
        }
    }
}

[Serializable]
public class Boost
{
    public Boost(Boost boost)
    {
        this.damage = boost.damage;
        this.health = boost.health;
        this.regeneration = boost.regeneration;
        this.isMultiply = boost.isMultiply;
        this.skillName = boost.skillName;
        this.skillInfo = boost.skillInfo;
        this.category = boost.category;
        this.isPassingArrow = boost.isPassingArrow;
        this.isReboundArrow = boost.isReboundArrow;
        this.isTripleArrow = boost.isTripleArrow;
        this.price = boost.price;
        this.sprite = boost.sprite;
    }
    
    public float damage;
    public float health;
    public float regeneration;

    public bool isMultiply;

    public string statInfo;
    public string skillName;
    public string skillInfo;

    public Categories category;

    public bool isReboundArrow;
    public bool isTripleArrow;
    public bool isPassingArrow;

    public float price;

    public bool isActive;
    
    public Sprite sprite;

    public int purchaseCount;


    
    public Boost Copy()
    {
        return new Boost(this);
    }
    public void Multiply()
    {
        if (!this.isMultiply)
            return;

        this.damage *= 1.2f;
        this.health *= 1.2f;
        this.regeneration *= 1.2f;
        this.price *= 1.5f;

        this.purchaseCount++;
    }
    
    public void RevertMultiply()
    {
        if (!this.isMultiply || this.purchaseCount == 0)
            return;

        float revertMultiplier = Mathf.Pow(1.2f, this.purchaseCount);
        this.damage /= revertMultiplier;
        this.health /= revertMultiplier;
        this.regeneration /= revertMultiplier;
        
        float revertPriceMultiplier = Mathf.Pow(1.5f, this.purchaseCount);
        this.price /= revertPriceMultiplier;

        this.purchaseCount = 0;
    }

    public void Deactivate()
    {
        this.isActive = false;
    }

    public void Activate()
    {
        this.isActive = true;
    }

    public override string ToString()
    {
        string text = "";

        if (this.damage > 0)
            text += $"Damage: {(int)this.damage} \n";
        if (this.health > 0)
            text += $"Health: {(int)this.health} \n";
        if (this.regeneration > 0)
            text += $"Regeneration: {(int)this.regeneration} \n";

        this.statInfo = text;
        
        return text;
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

        var baseConfig = WorldModels.Default.Get<BaseStatConfig>();
        var playerModel = WorldModels.Default.Get<Player>();

        foreach (var boost in this.boosts)
        {
            if (boost.category == Categories.Base)
            {
                baseConfig.maxHealth -= boost.health;
                baseConfig.regeneration -= boost.regeneration;
            }

            if (boost.category == Categories.Player)
            {
                playerModel.RemoveBoosts(); 
            }
        }
        
        this.boosts.Clear();
        
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

        foreach (var boost in WorldModels.Default.Get<Boosts>().BoostsList)
        {
            if (boost.isActive)
                this.AddBoost(boost);
        }
    }
}
