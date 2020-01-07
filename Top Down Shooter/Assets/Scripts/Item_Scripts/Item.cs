using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Item
{
    public enum KindItem
    {
        Potion = 0,
        Loot,
        Weapon
    };

    public Sprite Sprite { get; protected set; }
    public string Naam { get; protected set; }

    public KindItem Kind { get; protected set; }

    public float Value { get; protected set; }

    public float UseItem()
    {
        switch (Kind)
        {
            case KindItem.Potion:
                return UsePotion();

            case KindItem.Loot:
                break;

            case KindItem.Weapon:
                return UseWeapon();
        }

        return 0;
    }

    public virtual float UsePotion()
    {
        float health = 20;
        return health;
    }

    public virtual float UseWeapon()
    {
        float Damage = 25;
        return Damage;
    }

    public Tuple<string, KindItem> GetInfo()
    {
        return Tuple.Create(Naam, Kind);
    }

    public void Prints(string sentence)
    {
        Debug.Log(sentence);
    }
}