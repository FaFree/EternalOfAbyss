using System;
using System.Collections.Generic;
using DefaultNamespace;
using Scripts;
using Scripts.BoostFeature;
using Scripts.StorageService;
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
        this.isBuilding = boost.isBuilding;
        this.ghostObj = boost.ghostObj;
        this.buildObj = boost.buildObj;
        this.buildingTag = boost.buildingTag;
    }
    
    public float damage;
    public float health;
    public float regeneration;

    public bool isMultiply;
    
    public string skillName;
    public string skillInfo;

    public Categories category;

    public bool isReboundArrow;
    public bool isTripleArrow;
    public bool isPassingArrow;
    public bool isBuilding;

    public float price;
    public float firstPrice;

    public bool isActive;
    
    public Sprite sprite;

    public string buildObj;
    public string ghostObj;
    public string buildingTag;

    public int purchaseCount;


    
    public Boost Copy()
    {
        return new Boost(this);
    }
    
    public void Multiply()
    {
        if (!this.isMultiply)
            return;

        if (this.purchaseCount == 0)
            this.firstPrice = this.price;
        
        this.purchaseCount++;

        this.price = (float) (this.firstPrice * Math.Pow(1.09, purchaseCount));
    }
    
    public void RevertMultiply()
    {
        if (!this.isMultiply || this.purchaseCount == 0)
            return;

        this.price = this.firstPrice;

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
        
        return text;
    }
}

[Serializable]
public class BoostsModel
{
    public bool isTripleArrow;
    public bool isPassingArrow;
    public bool isReboundArrow;
    
    public List<Boost> boosts;

    private IStorageService storageService;

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
        
        storageService.Save("BoostModel", this);
        
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
        
        storageService.Save("BoostModel", this);
    }
    
    public BoostsModel()
    {
        storageService = new JsonFileStorageService();
        
        boosts = new List<Boost>();
    }
}
