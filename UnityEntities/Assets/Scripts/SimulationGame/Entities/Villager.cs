using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Sex
{
    Male,
    Female,
}

public class Villager : Unit, IQuadTreeObject<Villager>, ILover
{
    public enum VillagerBehavior
    {
        RunFromEnemy,
        Heal,
        EatFood,
        SearchForFood,
        SearchForWood,
        SearchForBuildingPlace,
        RepairHouse,
        SearchForPartner,
        CheckRelationShip,
        Wander,
        Sleep,
    }

    public Vector3 Position { get; set; }

    [Header("Overall Stats")]
    public VillagerBehavior Behavior;

    public int Age;

    public float MaxHealth;
    public float Health;
    public float DetectionRange;
    public Sex Gender;

    [Header("Resources")]
    public float HungerTreshHold;

    public float Hunger;
    public int Wood;
    public int Food;

    [Header("RelationShip Stats")]
    public float SexualDisere;

    public float SexualTreshHold;

    public float Confidence;
    public float AproachAble;

    public float Apeal;
    public float ApealTreshHold;

    public float SparkTreshHold;
    public float Loyalty;

    public RelationShip CurrentRelationShip;

    [Header("House Stats")]
    public bool HasHouse;

    public House VillagerHouse;

    private delegate void BehaviorTask();

    private Dictionary<VillagerBehavior, BehaviorTask> m_Behaviors;

    private GameObject m_Target;

    public VillagerInfo GetInfo()
    {
        return new VillagerInfo(this);
    }

    public void AssignGenes(VillagerGenes _Genes)
    {
        Age = _Genes.Age;
        MaxHealth = _Genes.MaxHealth;
        Health = MaxHealth;
        DetectionRange = _Genes.DetectionRange;
        HungerTreshHold = _Genes.HungerTreshHold;
        SexualTreshHold = _Genes.SexualTreshHold;
        Loyalty = _Genes.Loyalty;
        SparkTreshHold = _Genes.SparkTreshHold;
        Apeal = _Genes.Apeal;
        ApealTreshHold = _Genes.ApealTreshHold;
        Confidence = _Genes.Confidence;
        AproachAble = _Genes.AproachAble;
        Gender = _Genes.Gender;
    }

    public void BreakRelationShip()
    {
        CurrentRelationShip = null;
    }

    public float GetApeal()
    {
        return Apeal;
    }

    public bool AskForRelationShip(ILover _lover)
    {
        return _lover.GetApeal() >= ApealTreshHold;
    }

    public bool BreakUp(ILover _lover)
    {
        int _converation = _lover.TalkWith() + TalkWith();

        return _converation > 0;
    }

    public float GetLoyalty()
    {
        return Loyalty;
    }

    public int GetAge()
    {
        return Age;
    }

    public int TalkWith()
    {
        return Random.Range(-1, 10);
    }

    public void IncreaseDesire(float _amount)
    {
        SexualDisere += _amount;
    }

    public void HaveSex(ILover _lover)
    {
        int _result = Random.Range(-1, 10);
        if (_result < 0)
            return;
        SexualDisere = 0f;
        if (Gender == Sex.Female)
        {
            _result = Random.Range(0, 100);
            if (_result > 10)
            {
                EnitiyManager.Instance.CreateVillager(EnitiyManager.Instance.MixGenes(_lover.GetObject<Villager>(), this));
            }
        }
    }

    public T GetObject<T>()
    {
        return GetComponent<T>();
    }

    public bool RequestSex()
    {
        return SexualDisere >= SexualTreshHold / 2f;
    }

    public static VillagerBehavior CaculateBehavior(VillagerInfo _info)
    {
        if (_info.Health < _info.MaxHealth * 0.30f && _info.Food >= 1)
        {
            return VillagerBehavior.Heal;
        }

        if (_info.Hunger >= _info.HungerTreshHold)
        {
            if (_info.Food > 0)
            {
                return VillagerBehavior.EatFood;
            }
            return VillagerBehavior.SearchForFood;
        }

        if (_info.HasHouse == false)
        {
            if (_info.Wood < 10)
            {
                return VillagerBehavior.SearchForWood;
            }
            return VillagerBehavior.SearchForBuildingPlace;
        }
        else if (_info.HasHouse)
        {
            if (_info.VillagerHouse != null)
            {
                if (_info.VillagerHouse.Durrabilty != 100)
                {
                    if (_info.Wood > 0)
                    {
                        return VillagerBehavior.RepairHouse;
                    }
                    else
                    {
                        return VillagerBehavior.SearchForWood;
                    }
                }
            }
        }

        if (_info.SexualDisere >= 1)
        {
            if (_info.CurrentRelationShip != null)
            {
                return VillagerBehavior.CheckRelationShip;
            }
            else
            {
                return VillagerBehavior.SearchForPartner;
            }
        }

        return VillagerBehavior.Sleep;
    }

    private void Update()
    {
        CheckBehavior();
        Position = transform.position;
    }

    public override void Load()
    {
        CreateBehaviors();

        OnPool.AddListener(() =>
        {
            if (CurrentRelationShip != null)
            {
                CurrentRelationShip.BreakRelationShip(this);
                CurrentRelationShip = null;
            }
            EnitiyManager.Instance.RemoveVillager(this);
            StopAllCoroutines();
        });
    }

    protected override void ResetObject()
    {
        base.ResetObject();
        StartCoroutine(StartAllCoroutines());
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(Position, DetectionRange);
    }

    private IEnumerator StartAllCoroutines()
    {
        yield return new WaitForSeconds(1f);
        StartCoroutine(CheckNeeds());
        StartCoroutine(CheckAge());
    }

    private void CheckBehavior()
    {
        VillagerBehavior _behavior = CaculateBehavior(GetInfo());
        if (m_Behaviors.ContainsKey(_behavior))
        {
            m_Behaviors[_behavior]?.Invoke();
        }
        Behavior = _behavior;
    }

    private IEnumerator CheckNeeds()
    {
        while (Health > 0)
        {
            yield return new WaitForSeconds(2f);
            if (Hunger >= 10)
            {
                Health--;
                if (Health <= 0)
                    PoolObject();
            }
            else
            {
                Hunger++;
            }
            SexualDisere += Time.deltaTime * 30;
        }
        PoolObject();
    }

    private IEnumerator CheckAge()
    {
        while (Age < 120)
        {
            yield return new WaitForSeconds(5f);
            Age++;
        }
        PoolObject();
    }

    private void TryToHeal()
    {
        if (Food >= 1)
        {
            float _futureHealh = Health + 1;
            if (_futureHealh >= MaxHealth)
            {
                Health = MaxHealth;
                return;
            }
            Health = _futureHealh;
        }
    }

    private void EatFood()
    {
        int _futureFood = Food - 1;
        if (_futureFood <= 0)
        {
            Food = 0;
            Hunger -= 3;

            RemoveHunger();
            return;
        }
        RemoveHunger();
        Food = _futureFood;
    }

    private void RemoveHunger()
    {
        Hunger -= 3;

        if (Hunger < 0)
        {
            Hunger = 0;
        }
    }

    private void SearchForFood()
    {
        GrowObject _object = null;
        bool DidFind = false;

        if (m_Target == null)
        {
            (_object, DidFind) = MapManager.Instance.GetClosestObject(Position, ResourceType.Weat, DetectionRange);
            if (DidFind)
                m_Target = _object.gameObject;
        }
        if (m_Target != null)
        {
            _object = m_Target.GetComponent<GrowObject>();
            if (_object == null)
            {
                m_Target = null;
                return;
            }
            else
            {
                if (_object.IsFurtilzed == false)
                {
                    m_Target = null;
                    return;
                }
            }

            if (IsNavigating == false)
            {
                NaviagateTo(_object.transform.position);
            }
            else
            {
                if (RemaingDistanceToTarget < 2.5f)
                {
                    if (IsNavigating)
                        StopNavigating();
                    _object.Harvest();
                    _object.Pool();
                    Food += 5;
                    m_Target = null;
                }
            }
        }
        else
        {
            WanderVillager();
        }
    }

    private void SearchForWood()
    {
        GrowObject _object = null;
        bool DidFind = false;

        if (m_Target == null)
        {
            (_object, DidFind) = MapManager.Instance.GetClosestObject(Position, ResourceType.Wood, DetectionRange);
            if (DidFind)
                m_Target = _object.gameObject;
        }
        if (m_Target != null)
        {
            _object = m_Target.GetComponent<GrowObject>();
            if (_object == null)
            {
                m_Target = null;
                return;
            }
            else
            {
                if (_object.IsFurtilzed == false)
                {
                    m_Target = null;
                    return;
                }
            }

            if (IsNavigating == false)
            {
                NaviagateTo(_object.transform.position);
            }
            else
            {
                if (RemaingDistanceToTarget < 2.5f)
                {
                    if (IsNavigating)
                        StopNavigating();
                    _object.Harvest();
                    _object.Pool();
                    Wood += 2;
                    m_Target = null;
                }
            }
        }
        else
        {
            WanderVillager();
        }
    }

    private void SearchForBuildingPlace()
    {
        if (m_Target == null)
        {
            Cell _cell = MapManager.Instance.GetBuildAbleCell(Position, DetectionRange);
            if (_cell != null)
            {
                StopNavigating();
                m_Target = _cell.gameObject;
            }
            else
            {
                WanderVillager();
            }
        }
        else
        {
            if (IsNavigating == false)
            {
                Cell _cell = m_Target.GetComponent<Cell>();
                NaviagateTo(_cell.WorldPosition);
            }
            else
            {
                if (RemaingDistanceToTarget < 2f)
                {
                    Cell _cell = m_Target.GetComponent<Cell>();
                    if (MapManager.Instance.BuildHouse(_cell))
                    {
                        VillagerHouse = _cell.GetHouse();

                        if (VillagerHouse != null)
                        {
                            Wood -= 10;
                            HasHouse = true;
                            VillagerHouse.OnPool.AddListener(() =>
                            {
                                HasHouse = false;
                            });

                            m_Target = null;
                            return;
                        }
                    }
                    m_Target = null;
                }
            }
        }
    }

    private void RepairHouse()
    {
        if (VillagerHouse != null)
        {
            if (Vector3.Distance(Position, VillagerHouse.transform.position) < 3f)
            {
                if (IsNavigating)
                    StopNavigating();
                int _amountNeed = 100 - VillagerHouse.Durrabilty;
                if (_amountNeed > Wood)
                {
                    VillagerHouse.Repair(Wood);
                    Wood = 0;
                    return;
                }
                else
                {
                    VillagerHouse.Repair(_amountNeed);
                    Wood -= _amountNeed;
                    return;
                }
            }
            else
            {
                if (IsNavigating == false)
                    NaviagateTo(VillagerHouse.transform.position);
            }
        }
    }

    private void SleepInHouse()
    {
        if (HasHouse && VillagerHouse != null)
        {
            if (IsNavigating == false && VillagerHouse != null)
            {
                NaviagateTo(VillagerHouse.transform.position);
            }
            else
            {
                if (RemaingDistanceToTarget < 3f)
                {
                    transform.position = new Vector3(VillagerHouse.transform.position.x, transform.position.y, VillagerHouse.transform.position.z);
                }
            }
        }
    }

    private void SearchForPartner()
    {
        if (CurrentRelationShip != null)
            return;

        if (IsNavigating == false)
            WanderVillager();

        Villager[] _nearbyVillagers = EnitiyManager.Instance.GetVillagersInRange(Position, DetectionRange);

        if (_nearbyVillagers.Length == 0)
            return;

        foreach (Villager item in _nearbyVillagers)
        {
            if (item.CurrentRelationShip == null)
            {
                if (item.AproachAble <= Confidence && item.Gender != Gender)
                {
                    if (item.AskForRelationShip(this))
                    {
                        CurrentRelationShip = new RelationShip(this, item);
                        item.CurrentRelationShip = CurrentRelationShip;
                        return;
                    }
                    else
                    {
                        SexualDisere = 0f;
                    }
                }
            }
        }
    }

    private void CheckRelationShip()
    {
        if (CurrentRelationShip == null)
            return;

        Villager _lover = CurrentRelationShip.GetOtherLover(this).GetObject<Villager>();

        if (Vector3.Distance(_lover.Position, Position) < 3f)
        {
            if (IsNavigating)
                StopNavigating();

            CurrentRelationShip.Talk();
            if (CurrentRelationShip.Spark <= SparkTreshHold)
            {
                if (CurrentRelationShip.BreakUp(this))
                {
                    CurrentRelationShip.BreakRelationShip(this);
                    CurrentRelationShip = null;
                    return;
                }
            }

            if (SexualDisere >= SexualTreshHold)
            {
                CurrentRelationShip.HaveSex();
            }
        }
        else
        {
            if (IsNavigating == false)
                NaviagateTo(_lover.Position);
        }
    }

    private void WanderVillager()
    {
        if (IsNavigating == false)
        {
            Wander();
        }
    }

    private void CreateBehaviors()
    {
        m_Behaviors = new Dictionary<VillagerBehavior, BehaviorTask>
        {
            {VillagerBehavior.Heal,TryToHeal },
            {VillagerBehavior.EatFood,EatFood },
            {VillagerBehavior.SearchForFood, SearchForFood },
            {VillagerBehavior.SearchForWood, SearchForWood },
            {VillagerBehavior.SearchForBuildingPlace, SearchForBuildingPlace },
            {VillagerBehavior.SearchForPartner,SearchForPartner },
            {VillagerBehavior.CheckRelationShip,CheckRelationShip },
            {VillagerBehavior.RepairHouse, RepairHouse },
            {VillagerBehavior.Sleep, SleepInHouse },
        };
    }
}

public struct VillagerInfo
{
    [Header("Overall Stats")]
    public Villager.VillagerBehavior Behavior;

    public int Age;

    public float MaxHealth;
    public float Health;
    public float DetectionRange;
    public Sex Gender;

    [Header("Resources")]
    public float HungerTreshHold;

    public float Hunger;
    public int Wood;
    public int Food;

    [Header("RelationShip Stats")]
    public float SexualDisere;

    public float SexualTreshHold;

    public float Confidence;
    public float AproachAble;

    public float Apeal;
    public float ApealTreshHold;

    public float SparkTreshHold;
    public float Loyalty;

    public RelationShip CurrentRelationShip;

    [Header("House Stats")]
    public bool HasHouse;

    public House VillagerHouse;

    public Vector3 WorldPosition;

    public VillagerInfo(Villager _villager)
    {
        Behavior = _villager.Behavior;

        Age = _villager.Age;
        MaxHealth = _villager.MaxHealth;
        Health = _villager.Health;
        DetectionRange = _villager.DetectionRange;
        Gender = _villager.Gender;

        HungerTreshHold = _villager.HungerTreshHold;
        Hunger = _villager.Hunger;
        Wood = _villager.Wood;
        Food = _villager.Food;

        SexualDisere = _villager.SexualDisere;
        SexualTreshHold = _villager.SexualTreshHold;

        Confidence = _villager.Confidence;
        AproachAble = _villager.AproachAble;

        Apeal = _villager.Apeal;
        ApealTreshHold = _villager.ApealTreshHold;

        SparkTreshHold = _villager.SparkTreshHold;
        Loyalty = _villager.Loyalty;

        CurrentRelationShip = _villager.CurrentRelationShip;

        HasHouse = _villager.HasHouse;
        VillagerHouse = _villager.VillagerHouse;

        WorldPosition = _villager.Position;
    }
}