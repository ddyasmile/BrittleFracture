using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Edge = EdgeNS.Edge;

/// <summary>
/// Separate nodes into different fragment with BFS
/// </summary>
public class FloodAlgorithm
{
    public static List<List<int>> floodSplit2D(ref VolumeticMesh2D mesh)
    {
        var unassignedNodes = new List<int>();
        for (int i = 0; i < mesh.nodes.Count; ++i)
        {
            unassignedNodes.Add(i);
        }
        var fragmentCount = 0;
        var nodes = new List<List<int>>();


        int ccc0 = 0;
        while (unassignedNodes.Count != 0)
        {
            if (ccc0++ > 100)
            {
                Debug.Log("Too many loops");
                break;
            }

            Debug.Log(unassignedNodes.Count);

            var newFragmentId = fragmentCount;
            fragmentCount++;

            List<int> queue = new List<int>();
            queue.Add(unassignedNodes[0]);
            unassignedNodes.RemoveAt(0);

            int targetNode = queue[0];

            int ccc1 = 0;
            while (queue.Count != 0)
            {
                if (ccc1++ > 100)
                {
                    Debug.Log("Too many loops");
                    break;
                }

                if (nodes.Count <= newFragmentId)
                    nodes.Add(new List<int>());
                nodes[newFragmentId].Add(targetNode);

                int ccc2 = 0;
                foreach (var ne in mesh.getNeighbors(targetNode))
                {
                    if (ccc2++ > 100)
                    {
                        Debug.Log("Too many loops");
                        break;
                    }

                    if (!queue.Contains(ne) && unassignedNodes.Contains(ne))
                    {
                        Edge edge = new Edge(ne, targetNode);
                        if (!mesh.isDamagedEdge(edge))
                        {
                            queue.Add(ne);
                            unassignedNodes.Remove(ne);
                        }
                    }
                }
                queue.RemoveAt(0);
            }
        }

        return nodes;
    }

    public static List<List<int>> floodSplit3D(ref VolumeticMesh3D mesh)
    {
        var unassignedNodes = new List<int>();
        for (int i = 0; i < mesh.nodes.Count; ++i)
        {
            unassignedNodes.Add(i);
        }
        var fragmentCount = 0;
        var nodes = new List<List<int>>();

        while (unassignedNodes.Count != 0)
        {
            var newFragmentId = fragmentCount;
            fragmentCount++;

            List<int> queue = new List<int>();
            queue.Add(unassignedNodes[0]);
            unassignedNodes.RemoveAt(0);

            int targetNode = queue[0];
            while (queue.Count != 0)
            {
                targetNode = queue[0];
                if (nodes.Count <= newFragmentId)
                    nodes.Add(new List<int>());
                nodes[newFragmentId].Add(targetNode);
                foreach (var ne in mesh.getNeighborNodes(targetNode))
                {
                    if (!queue.Contains(ne) && unassignedNodes.Contains(ne))
                    {
                        Edge edge = new Edge(ne, targetNode);
                        if (!mesh.isDamagedEdge(edge))
                        {
                            queue.Add(ne);
                            unassignedNodes.Remove(ne);
                        }
                    }
                }
                queue.RemoveAt(0);
            }
        }

        return nodes;
    }
}