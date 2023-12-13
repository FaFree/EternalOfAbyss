using System;
using System.Collections.Generic;
using Configs;
using Configs.BoostsConfig;
using DefaultNamespace;
using ECS.Scripts.Events;
using Models;
using Scellecs.Morpeh;
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

        this.barrierBoostConfig = boost.barrierBoostConfig;
        this.baseBoostConfig = boost.baseBoostConfig;
        this.playerBoostConfig = boost.playerBoostConfig;
        this.turretBoostConfig = boost.turretBoostConfig;
    }
    
    public bool isMultiply;
    
    public string skillName;
    public string skillInfo;

    public Categories category;

    public BarrierBoostConfig barrierBoostConfig;
    public BaseBoostConfig baseBoostConfig;
    public PlayerBoostConfig playerBoostConfig;
    public TurretBoostConfig turretBoostConfig;
    public WeaponConfig weaponConfig;
    

    public bool isReboundArrow;
    public bool isTripleArrow;
    public bool isPassingArrow;
    public bool isBuilding;

    public float price;
    public float firstPrice;

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
        switch (category)
        {
            case Categories.Barriers: return barrierBoostConfig.ToString();
            case Categories.Base: return baseBoostConfig.ToString();
            case Categories.Player: return playerBoostConfig.ToString();
            case Categories.Turrets: return turretBoostConfig.ToString();
        }

        return "";
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

    private Event<BoostRequest> boostRequest;

    public void Clear()
    {
        isTripleArrow = false;
        isPassingArrow = false;
        isReboundArrow = false;

        var playerModel = WorldModels.Default.Get<Player>();

        foreach (var boost in this.boosts)
        {
            switch (boost.category)
            {
                case  Categories.Base:
                    var baseStats = WorldModels.Default.Get<BaseStatConfig>();
                    baseStats.regeneration -= boost.baseBoostConfig.regeneration;
                    baseStats.maxHealth -= boost.baseBoostConfig.health;
                    break;
                  
                case Categories.Barriers:
                    var barrierStats = WorldModels.Default.Get<BarrierStatConfig>();
                    barrierStats.health -= boost.barrierBoostConfig.health;
                    break;
                  
                case Categories.Turrets:
                    var turretStats = WorldModels.Default.Get<TurretStatConfig>();
                    turretStats.damage -= boost.turretBoostConfig.damage;
                    break;
                
                case Categories.Player:
                    playerModel.RemoveBoosts();
                    break;
            }
        }
        
        this.boosts.Clear();
        
        storageService.Save("BoostModel", this);
        
        WorldModels.Default.Get<Boosts>().Clear();
    }

    public void Initialize()
    {
        this.boostRequest = World.Default.GetEvent<BoostRequest>();
        
        foreach (var boost in this.boosts)
        {
            this.boostRequest.NextFrame(new BoostRequest
            {
                boost = boost
            });
        }
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
