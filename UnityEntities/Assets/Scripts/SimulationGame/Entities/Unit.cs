using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChiefoUtilities.PathFinding;

public class Unit : PoolableObject
{
    public const float StopDistance = 2f;

    public float Speed = 1f;
    public bool IsNavigating { get; private set; }

    public float RemaingDistanceToTarget;

    private Rigidbody m_Rb;

    private bool m_Stop;

    public void StopNavigating()
    {
        if (m_Stop == false)
            m_Stop = true;
    }

    public void NaviagateTo(Vector3 _target)
    {
        StartCoroutine(MoveTowards(_target));
    }

    public void Wander()
    {
        StartCoroutine(FollowPath(PathFinder.Instance.GetRandomPath(transform.position)));
    }

    private IEnumerator MoveTowards(Vector3 _target)
    {
        if (m_Rb == null)
        {
            m_Rb = GetComponent<Rigidbody>();
        }
        IsNavigating = true;
        List<PathNode> _path = PathFinder.Instance.FindPath(transform.position, _target);
        if (_path != null)
        {
            int _targetIndex = 0;
            while (_targetIndex <= _path.Count - 1)
            {
                PathNode _currentNode = _path[_targetIndex];
                float _Remainingdistance = Vector3.Distance(m_Rb.position, _currentNode.WorldPosition);
                while (_Remainingdistance >= StopDistance)
                {
                    RemaingDistanceToTarget = Vector3.Distance(transform.position, _target);
                    if (m_Stop)
                    {
                        break;
                    }

                    _Remainingdistance = Vector3.Distance(m_Rb.position, _currentNode.WorldPosition);
                    Vector3 _dir = _currentNode.WorldPosition - m_Rb.position;
                    _dir = _dir.normalized;
                    _dir.y = 0f;

                    m_Rb.MovePosition(m_Rb.position + _dir * Speed * Time.deltaTime);

                    Vector3 LookatPos = m_Rb.position + _dir;
                    transform.LookAt(LookatPos);

                    Debug.DrawLine(transform.position, _currentNode.WorldPosition);
                    yield return null;
                }
                _targetIndex++;
                if (m_Stop)
                {
                    break;
                }
                yield return null;
                RemaingDistanceToTarget = Vector3.Distance(transform.position, _path[_path.Count - 1].WorldPosition);
            }
        }
        m_Stop = false;
        IsNavigating = false;
    }

    private IEnumerator FollowPath(List<PathNode> _path)
    {
        if (m_Rb == null)
        {
            m_Rb = GetComponent<Rigidbody>();
        }
        IsNavigating = true;
        PathNode _currentNode = null;
        float _Remainingdistance = 1000;
        if (_path != null)
        {
            int _targetIndex = 0;
            while (_targetIndex <= _path.Count - 1)
            {
                _currentNode = _path[_targetIndex];
                _Remainingdistance = Vector3.Distance(m_Rb.position, _currentNode.WorldPosition);
                while (_Remainingdistance >= StopDistance)
                {
                    RemaingDistanceToTarget = Vector3.Distance(transform.position, _path[_path.Count - 1].WorldPosition);
                    if (m_Stop)
                    {
                        break;
                    }

                    _Remainingdistance = Vector3.Distance(m_Rb.position, _currentNode.WorldPosition);
                    Vector3 _dir = _currentNode.WorldPosition - m_Rb.position;
                    _dir = _dir.normalized;
                    _dir.y = 0f;

                    m_Rb.MovePosition(m_Rb.position + _dir * Speed * Time.deltaTime);

                    Vector3 LookatPos = m_Rb.position + _dir;
                    transform.LookAt(LookatPos);

                    Debug.DrawLine(transform.position, _currentNode.WorldPosition);
                    yield return null;
                }
                _targetIndex++;
                if (m_Stop)
                {
                    break;
                }
                yield return null;
                RemaingDistanceToTarget = Vector3.Distance(transform.position, _path[_path.Count - 1].WorldPosition);
            }
        }
        m_Stop = false;
        IsNavigating = false;
    }
}