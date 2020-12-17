using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pizza : PoolableObject
{
    private void OnTriggerEnter(Collider other)
    {
        CheckForAnimal(other);
    }

    private void Update()
    {
        CheckInBounds();
    }

    private void CheckInBounds()
    {
        if (InBoundsManager.Instance.InBounds(transform.position) == false)
        {
            PoolObject();
        }
    }

    private bool CheckForAnimal(Collider collider)
    {
        Animal animal = collider.GetComponentInParent<Animal>();
        if(animal != null)
        {
            animal.Hit();
            return true;
        }
        return false;
    }
}
