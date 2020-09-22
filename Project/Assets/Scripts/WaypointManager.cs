using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    public static WaypointManager instance;

    [SerializeField] private Transform[] waypoints;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(instance);
        }
    }

    public Transform GetWaypoint(int index)
    {
        return waypoints[index];
    }

    public Transform GetRandomWaypoint()
    {
        return waypoints[UnityEngine.Random.Range(0, waypoints.Length)];
    }

    public Transform GetNextWaypoint(Transform currentWaypoint)
    {
        int waypointIndex = Array.IndexOf(waypoints, currentWaypoint);
        waypointIndex++;
        if (waypointIndex >= waypoints.Length)
        {
            waypointIndex = 0;
        }
        return waypoints[waypointIndex];
    }
}