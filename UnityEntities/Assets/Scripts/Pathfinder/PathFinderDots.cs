using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Burst;
using Unity.Collections;
using System;
using System.Diagnostics;

namespace PathFinderDots
{
    public class PathFinderDots : MonoBehaviour
    {
        private const int MOVE_STRAIGHT_COST = 10;
        private const int MOVE_Diagnol_COST = 15;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                FindPath();
            }
        }

        private void FindPath()
        {
            NativeList<JobHandle> Handles = new NativeList<JobHandle>(Allocator.TempJob);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            for (int i = 0; i < 5; i++)
            {
                FindPathJob findPathJob = new FindPathJob
                {
                    startPoint = new int2(0, 0),
                    endPosition = new int2(19, 19),
                };
                Handles.Add(findPathJob.Schedule());
            }


            JobHandle.CompleteAll(Handles);
            stopwatch.Stop();
            Handles.Dispose();
            UnityEngine.Debug.Log($"MS:{stopwatch.ElapsedTicks}");
            stopwatch = null;
        }
        [BurstCompile]
        private struct FindPathJob : IJob
        {
            public int2 startPoint;
            public int2 endPosition;
            public void Execute()
            {
                int2 gridSize = new int2(20, 20);

                NativeArray<PathNode> pathNodeArray = new NativeArray<PathNode>(gridSize.x * gridSize.y, Allocator.Temp);

                for (int x = 0; x < gridSize.x; x++)
                {
                    for (int y = 0; y < gridSize.y; y++)
                    {

                        PathNode pathNode = new PathNode();
                        pathNode.x = x;
                        pathNode.y = y;
                        pathNode.index = CaculateIndex(x, y, gridSize.x);

                        pathNode.gCost = int.MaxValue;
                        pathNode.hCost = CaculateDistanceCost(new int2(x, y), endPosition);
                        pathNode.CaculateFcost();

                        pathNode.IsWalkable = true;
                        pathNode.cameFromNodeIndex = -1;

                        pathNodeArray[pathNode.index] = pathNode;
                    }
                }

                PathNode node = pathNodeArray[CaculateIndex(0, 1, gridSize.x)];
                node.IsWalkable = false;
                pathNodeArray[node.index] = node;


                NativeArray<int2> neighboursOffsetArray = new NativeArray<int2>(8, Allocator.Temp);
                neighboursOffsetArray[0] = new int2(-1, 0);
                neighboursOffsetArray[1] = new int2(1, 0);
                neighboursOffsetArray[2] = new int2(0, 1);
                neighboursOffsetArray[3] = new int2(0, 1);
                neighboursOffsetArray[4] = new int2(-1, -1);
                neighboursOffsetArray[5] = new int2(-1, 1);
                neighboursOffsetArray[6] = new int2(1, -1);
                neighboursOffsetArray[7] = new int2(1, 1);
            

                int endNodeIndex = CaculateIndex(endPosition.x, endPosition.y, gridSize.x);
                PathNode startNode = pathNodeArray[CaculateIndex(startPoint.x, startPoint.y, gridSize.x)];

                startNode.gCost = 0;
                startNode.CaculateFcost();

                pathNodeArray[startNode.index] = startNode;

                NativeList<int> OpenList = new NativeList<int>(Allocator.Temp);
                NativeList<int> ClosedList = new NativeList<int>(Allocator.Temp);

                OpenList.Add(startNode.index);

                while (OpenList.Length > 0)
                {
                    int currentIndex = GetLowestCost(OpenList, pathNodeArray);

                    PathNode currentNode = pathNodeArray[currentIndex];

                    if (currentIndex == endNodeIndex)
                    {
                        break;
                    }

                    for (int i = 0; i < OpenList.Length; i++)
                    {
                        if (OpenList[i] == currentIndex)
                        {
                            OpenList.RemoveAtSwapBack(i);
                            break;
                        }
                    }

                    ClosedList.Add(currentIndex);

                    for (int i = 0; i < neighboursOffsetArray.Length; i++)
                    {
                        int2 neighbourOffset = neighboursOffsetArray[i];
                        int2 neighbourposition = new int2(currentNode.x + neighbourOffset.x, currentNode.y + neighbourOffset.y);
                        if (!isWithinGrid(neighbourposition, gridSize))
                        {
                            continue;
                        }

                        int neighbourindex = CaculateIndex(neighbourposition.x, neighbourposition.y, gridSize.x);
                        if (ClosedList.Contains(neighbourindex))
                        {
                            continue;
                        }

                        PathNode neigbour = pathNodeArray[neighbourindex];
                        if (!neigbour.IsWalkable)
                        {
                            continue;
                        }
                        int2 currentNodePosition = new int2(currentNode.x, currentNode.y);
                        int tenativeGCost = currentNode.gCost + CaculateDistanceCost(currentNodePosition, neighbourposition);
                        if (tenativeGCost < neigbour.gCost)
                        {
                            neigbour.cameFromNodeIndex = currentIndex;
                            neigbour.gCost = tenativeGCost;
                            neigbour.CaculateFcost();
                            pathNodeArray[neighbourindex] = neigbour;

                            if (!OpenList.Contains(neighbourindex))
                            {
                                OpenList.Add(neighbourindex);
                            }
                        }
                    }
                }

                PathNode endnode = pathNodeArray[endNodeIndex];
                if (endnode.cameFromNodeIndex == -1)
                {

                }
                else
                {
                    //NativeList<int2> _path = CaculatePath(pathNodeArray, endnode);
                    //for (int i = _path.Length -1 ; i >= 0; i--)
                    //{
                    //    Path.Add(_path[i]);
                    //}

                }

                neighboursOffsetArray.Dispose();
                OpenList.Dispose();
                ClosedList.Dispose();
                pathNodeArray.Dispose();
            }

            private NativeList<int2> CaculatePath(NativeArray<PathNode> PathnodeArray, PathNode endnode)
            {
                if (endnode.cameFromNodeIndex == -1)
                {
                    return new NativeList<int2>(Allocator.Temp);
                }
                else
                {
                    NativeList<int2> path = new NativeList<int2>(Allocator.Temp);
                    path.Add(new int2(endnode.x, endnode.y));

                    PathNode currentNode = endnode;
                    while (currentNode.cameFromNodeIndex != -1)
                    {
                        PathNode camefromNode = PathnodeArray[currentNode.cameFromNodeIndex];
                        path.Add(new int2(camefromNode.x, camefromNode.y));
                        currentNode = camefromNode;

                    }
                    return path;
                }
            }

            private bool isWithinGrid(int2 gridposition, int2 gridsize)
            {
                return
                    gridposition.x >= 0 &&
                    gridposition.y >= 0 &&
                    gridposition.x < gridsize.x &&
                    gridposition.y < gridsize.y;
            }

            private int GetLowestCost(NativeList<int> openlist, NativeArray<PathNode> pathNodeArray)
            {
                PathNode lowestPathnode = pathNodeArray[openlist[0]];
                for (int i = 1; i < openlist.Length; i++)
                {
                    PathNode testPathNode = pathNodeArray[openlist[i]];
                    if (testPathNode.fCost < lowestPathnode.fCost)
                    {
                        lowestPathnode = testPathNode;
                    }
                }
                return lowestPathnode.index;
            }

            private int CaculateIndex(int x, int y, int gridwith)
            {
                return x + y * gridwith;
            }

            private int CaculateDistanceCost(int2 aPos, int2 bPos)
            {
                int xDistance = math.abs(aPos.x - bPos.x);
                int yDistance = math.abs(aPos.y - bPos.y);
                int remaining = math.abs(aPos.x - bPos.y);
                return MOVE_Diagnol_COST * math.min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
            }
        }

        

    }

    public struct PathNode
    {
        public int x;
        public int y;

        public int index;

        public int gCost;
        public int hCost;
        public int fCost;

        public bool IsWalkable;

        public int cameFromNodeIndex;

        public void CaculateFcost()
        {
            fCost = gCost + hCost;
        }
    }
}

