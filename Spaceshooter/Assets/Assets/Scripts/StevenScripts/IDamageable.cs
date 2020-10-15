using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

interface IDamageable 
{
    bool IsAlive { get; set; }
    void TakeDamage(int damage);
    
    
}
