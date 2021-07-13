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

    private QuadTree<Point> m_Tree;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
    }

    public void SetTree(QuadTree<Point> _tree)
    {
        m_Tree = _tree;
    }

    private void Start()
    {
        m_Cam = Camera.main;
    }

    // Update is called once per frame
    private void Update()
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

    public Vector3[] GetCorners(Vector3 _pos1, Vector3 _pos2)
    {
        Vector3 newP1;
        Vector3 newP2;
        Vector3 newP3;
        Vector3 newP4;

        if (_pos1.x > _pos2.x)
        {
            if (_pos1.z > _pos2.z)
            {
                newP1 = _pos1;
                newP2 = new Vector3(_pos2.x, 0f, _pos1.z);
                newP3 = new Vector3(_pos1.x, 0f, _pos2.z);
                newP4 = _pos2;
            }
            else
            {
                newP1 = _pos1;
                newP2 = new Vector3(_pos2.x, 0f, _pos1.z);
                newP3 = new Vector3(_pos1.x, 0f, _pos2.z);
                newP4 = _pos2;
            }
        }
        else
        {
            if (_pos1.z > _pos2.z)
            {
                newP1 = new Vector3(_pos2.x, 0f, _pos1.z);
                newP2 = _pos1;
                newP3 = _pos2;
                newP4 = new Vector3(_pos1.x, 0f, _pos2.z);
            }
            else
            {
                newP1 = _pos2;
                newP2 = new Vector3(_pos1.x, 0f, _pos2.z);
                newP3 = new Vector3(_pos1.x, 0f, _pos2.z);
                newP4 = _pos1;
            }
        }

        return new Vector3[]
        {
            newP1,
            newP2,
            newP3,
            newP4,
        };
    }

    public bool HasASelection()
    {
        return HasSelection;
    }

    public void ResetSelection()
    {
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
            HasSelection = true;
        }
    }

    private Vector3 GetMousePosition()
    {
        RaycastHit hit = new RaycastHit();
        Ray _ray = m_Cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(_ray, out RaycastHit _hit))
        {
            if (_hit.collider != null)
            {
                hit = _hit;
            }
        }
        return hit.point;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(SelectionPosition_1, SelectionPosition_2);

        if (m_Tree != null)
            m_Tree.OnDrawGizmos();
    }
}