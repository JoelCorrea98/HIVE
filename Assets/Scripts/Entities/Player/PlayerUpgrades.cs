using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
public class PlayerUpgrades : MonoBehaviour
{
    public class UpgradePool
    {
        public List<Upgrade> Attack = new List<Upgrade>();
        public List<Upgrade> Mobility = new List<Upgrade>();
        public List<Upgrade> Resistance = new List<Upgrade>();

        public List<Upgrade> ChooseUpgradeList(PlayerLevel.ExpType type)
        {
            switch (type)
            {
                case PlayerLevel.ExpType.Attack:
                    return Attack;
                case PlayerLevel.ExpType.Defense:
                    return Resistance;
                case PlayerLevel.ExpType.Mobility:
                    return Mobility;
                
                default:
                    return Attack;
            }
        }

        public void SetTypeOfUpgrades()
        {
            foreach (var upgrade in Attack)
            {
                upgrade.type = PlayerLevel.ExpType.Attack;
            }
            
            foreach (var upgrade in Mobility)
            {
                upgrade.type = PlayerLevel.ExpType.Mobility;
            }
            
            foreach (var upgrade in Resistance)
            {
                upgrade.type = PlayerLevel.ExpType.Defense;
            }
        }
    }
    public class Upgrade
    {
        public string name;
        public string description;
        public Action action = delegate { };
        public bool oneTimeOnly = true;
        public PlayerLevel.ExpType type;
    }

    public enum UpgradeType
    {
        Small, Big
    }

    public Player player;

    public UpgradePool Small;
    public UpgradePool Big;

    private void Start()
    {
        GeneratePools();
    }

    private void GeneratePools()
    {
        Small = new UpgradePool()
        {
            Attack = new List<Upgrade>()
            {
                new Upgrade()
                {
                    name = "DMG Buff",
                    description = "+20% DMG",
                    action = DamagePercentBuff,
                    oneTimeOnly = false
                }
            },

            Mobility = new List<Upgrade>()
            {
                new Upgrade()
                {
                    name = "Speed Buff",
                    description = "+5% Mobility",
                    action = MobilityPercentBuff,
                    oneTimeOnly = false
                }
            },

            Resistance = new List<Upgrade>()
            {
                new Upgrade()
                {
                    name = "HP Buff",
                    description = "+5 HP",
                    action = HPFlatUpgrade,
                    oneTimeOnly = false
                }
            },
        };
        Small.SetTypeOfUpgrades();

        Big = new UpgradePool()
        {
            Attack = new List<Upgrade>()
            {
                new Upgrade()
                {
                    name = "Grenade Throw",
                    description = "Unlocks Grenade Throw",
                    action = ActivateGrenadeThrow
                }
            },

            Mobility = new List<Upgrade>()
            {
                new Upgrade()
                {
                    name = "Double Jump",
                    description = "Unlocks Double Jump",
                    action = ActivateDoubleJump
                }
            },

            Resistance = new List<Upgrade>()
            {

            },
        };
        Big.SetTypeOfUpgrades();
    }

    public List<Upgrade> GachaPull(UpgradeType type, List<PlayerLevel.Exp> exp)
    {
        List<Upgrade> upgradeOptions = new List<Upgrade>();
        
        UpgradePool pool = new UpgradePool();
        switch (type)
        {
            case UpgradeType.Small:
                pool = Small;
                break;
            case UpgradeType.Big:
                pool = Big;
                break;
        }

        #region Get Top Type exp wise
        PlayerLevel.ExpType topType;
        List<PlayerLevel.ExpType> ties = new List<PlayerLevel.ExpType>();
        int currentMax = 0;
        foreach (PlayerLevel.Exp expType in exp)
        {
            if (expType.ThisLevel > currentMax)
            {
                currentMax = expType.ThisLevel;
                ties.Clear();
                ties.Add(expType.type);
            }
            else if (expType.ThisLevel == currentMax)
            {
                ties.Add(expType.type);
            }
        }
        topType = ties.ChooseRandom();
        #endregion
        
        #region Get top 2 type exp wise
        PlayerLevel.ExpType top2Type = PlayerLevel.ExpType.Attack;
        List<PlayerLevel.ExpType> ties2 = new List<PlayerLevel.ExpType>();
        int currentMax2 = 0;
        foreach (PlayerLevel.Exp expType in exp)
        {
            if(expType.type != topType)
            {
                if (expType.ThisLevel > currentMax2)
                {
                    currentMax2 = expType.ThisLevel;
                    ties2.Clear();
                    ties2.Add(expType.type);
                }
                else if (expType.ThisLevel == currentMax2)
                {
                    ties2.Add(expType.type);
                }
            }
        }

        top2Type = ties2.ChooseRandom();
        #endregion

        List<Upgrade> upgradesOfTopType = pool.ChooseUpgradeList(topType);
        List<Upgrade> upgradesOfTop2Type = pool.ChooseUpgradeList(top2Type);

        Upgrade empty = new Upgrade();
        empty.type = topType;
        if (upgradesOfTopType.Count > 0)
        {
            Upgrade one = upgradesOfTopType.ChooseRandom();
            upgradeOptions.Add(one);

            if (one.oneTimeOnly)
            {
                upgradesOfTopType.Remove(one);
            }
            

            
            List<Upgrade> newList = upgradesOfTopType.Where(x => x != one).ToList();
            if (newList.Count > 0)
            {
                Upgrade two = newList.ChooseRandom();
                upgradeOptions.Add(two);
                if (two.oneTimeOnly)
                {
                    upgradesOfTopType.Remove(two);
                }
            }
            else
            {
                
                upgradeOptions.Add(empty);
            }
        }
        else
        {
            upgradeOptions.Add(empty);
            upgradeOptions.Add(empty);
        }

        if (upgradesOfTop2Type.Count > 0)
        {
            Upgrade one = upgradesOfTop2Type.ChooseRandom();
            upgradeOptions.Add(one);
            
            if (one.oneTimeOnly)
            {
                upgradesOfTop2Type.Remove(one);
            }
        }
        else
        {
            Upgrade empty2 = new Upgrade();
            empty2.type = top2Type;
            upgradeOptions.Add(empty);
        }


        //DEBUG TEST
        bool first = true;
        PlayerLevel.ExpType checktype = PlayerLevel.ExpType.Attack;
        int count = 0;
        foreach (var item in upgradeOptions)
        {
            if (first)
            {
                checktype = item.type;
            }

            if(item.type == checktype)
            {
                count++;
            }
            
        }

        if(count > 2)
        {
            Debug.Log("Bug");
        }

        return upgradeOptions;
    }

    #region UpgradesSmall
    void DamagePercentBuff()
    {
        float percentageUpgrade = 20;
        player.shoot.damage += (player.shoot.damage * percentageUpgrade)/100;
    }

    void HPFlatUpgrade()
    {
        int buff = 5;
        player.MaxHP += buff;
        player.CurrentHealth += buff;
    }

    void MobilityPercentBuff()
    {
        float percentageUpgrade = 5;
        player.movement.maxSpeed += (player.movement.maxSpeed * percentageUpgrade)/100;
    }

    #endregion

    #region UpgradesBig
    void ActivateDoubleJump()
    {
        player.jump.amountOfJumps = 2;
    }

    void ActivateGrenadeThrow()
    {
        player.grenadeThrow.MechanicActivated = true;
    }
    #endregion
}