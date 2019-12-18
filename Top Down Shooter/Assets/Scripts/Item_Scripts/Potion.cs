using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Potion : Item
{
    public float HealthAmount;

    public Potion(string naam, Item.KindItem kindItem, float healthamount)
    {
        base.Naam = naam;
        base.Kind = kindItem;
        HealthAmount = healthamount;
    }

    public override float UsePotion()
    {
        Prints("You used " + Naam);
        return HealthAmount;
    }
}