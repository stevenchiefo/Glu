using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderActive : MonoBehaviour
{
    public Material m_Mat;
    public float Size;
    private float m_Timer = 0f;

    private void Start()
    {
        m_Mat = GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    private void Update()
    {
        m_Timer += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.E))
        {
            Size = 2f;
        }
        if (m_Timer >= 1f / 10f)
        {
            m_Timer = 0f;
            if (Size >= 1f)
            {
                Size = Size - 1f * Time.deltaTime;
            }
        }
        m_Mat.SetFloat("_Size", Size);
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