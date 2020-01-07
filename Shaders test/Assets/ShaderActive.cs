using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderActive : MonoBehaviour
{
    public Material m_Mat;

    private void Start()
    {
        m_Mat = gameObject.GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            m_Mat.SetFloat("Seeable", 1f);
        }
    }

    private void OnMouseOver()
    {
        m_Mat.SetColor("FullColor", Color.blue);
    }

    private void OnMouseExit()
    {
        m_Mat.color = default;
    }
}