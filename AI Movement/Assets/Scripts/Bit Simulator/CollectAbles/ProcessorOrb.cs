using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessorOrb : MonoBehaviour
{
    public void SetSize(float size)
    {
        transform.localScale = new Vector3(size, size);
    }
}