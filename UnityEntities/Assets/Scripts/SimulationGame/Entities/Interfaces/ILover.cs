using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILover
{
    void BreakRelationShip();

    float GetApeal();

    bool AskForRelationShip(ILover _lover);

    bool BreakUp(ILover _lover);

    float GetLoyalty();

    int GetAge();

    int TalkWith();

    void IncreaseDesire(float _amount);

    void HaveSex(ILover _lover);

    bool RequestSex();

    T GetObject<T>();
}