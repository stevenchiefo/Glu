using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BulletData")]
public class BulletData : ScriptableObject
{
    public float speed;
    public float lifeTime;
    public int damage;
    public LayerMask hitLayerMask;
}