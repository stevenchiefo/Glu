using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelationShip
{
    public float Spark;

    public ILover LoverA { get; private set; }
    public ILover LoverB { get; private set; }

    public RelationShip(ILover _loverA, ILover _loverB)
    {
        LoverA = _loverA;
        LoverB = _loverB;

        Spark = _loverA.GetLoyalty() + _loverB.GetLoyalty();
    }

    public void HaveSex()
    {
        LoverA.HaveSex(LoverB);
        LoverB.HaveSex(LoverA);
    }

    public void Talk()
    {
        int _conversationPoints = LoverA.TalkWith() + LoverB.TalkWith();

        Spark += _conversationPoints;

        if (_conversationPoints < 0)
        {
            LoverA.IncreaseDesire(-2);
            LoverB.IncreaseDesire(-2);
        }
        else
        {
            LoverA.IncreaseDesire(Time.deltaTime);
            LoverB.IncreaseDesire(Time.deltaTime);
        }
    }

    public ILover GetOtherLover(ILover _lover)
    {
        if (LoverA != _lover)
        {
            return LoverB;
        }
        else
        {
            return LoverA;
        }
    }

    public void BreakRelationShip(ILover _lover)
    {
        if (LoverA != _lover)
        {
            LoverB.BreakRelationShip();
        }
        else
        {
            LoverB.BreakRelationShip();
        }
    }

    public bool BreakUp(ILover _lover)
    {
        if (LoverA != _lover)
        {
            return LoverB.BreakUp(_lover);
        }
        else
        {
            return LoverA.BreakUp(_lover);
        }
    }
}