using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseSelecter : MonoBehaviour
{
    public static MouseSelecter Instance;

    private Camera m_Cam;
    private Vector3 SelectionPosition_1;
    private Vector3 SelectionPosition_2;
    private bool HasSelection;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        m_Cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        CheckMouseSelection();
    }

    public Vector3 GetTargetPosition()
    {
        Vector3 _targetpos = Vector3.zero;
        if (Input.GetMouseButtonDown(1))
        {
            _targetpos = GetMousePosition();
        }
        return _targetpos;
    }

    public bool DidSelectTarget() => Input.GetMouseButtonDown(1);
    public Vector3[] CheckForSelection()
    {
        CheckMouseSelection();
        return new Vector3[]
        {
            SelectionPosition_1,
            SelectionPosition_2,
        };
    }

    public bool HasASelection()
    {
        return HasSelection;
    }

    public void ResetSelection()
    {
        SelectionPosition_1 = Vector3.zero;
        SelectionPosition_2 = Vector3.zero;
        HasSelection = false;
    }

    private void CheckMouseSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SelectionPosition_1 = GetMousePosition();
        }

        if (Input.GetMouseButtonUp(0))
        {
            SelectionPosition_2 = GetMousePosition();
            HasSelection = false;
        }
    }

    private Vector3 GetMousePosition()
    {
        RaycastHit hit = new RaycastHit();
        Ray _ray = m_Cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(_ray, out RaycastHit _hit))
        {
            if(_hit.collider != null)
            {
                hit = _hit;
            }
        }
        return hit.point;
    }
}
