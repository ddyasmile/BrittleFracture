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
                if (nodes[newFragmentId] == null)
                    nodes.Add(new List<int>());
                nodes[newFragmentId].Add(targetNode);
                foreach (var ne in mesh.getNeighbors(targetNode))
                {
                    if (!queue.Contains(ne))
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
                if (nodes[newFragmentId] == null)
                    nodes.Add(new List<int>());
                nodes[newFragmentId].Add(targetNode);
                foreach (var ne in mesh.getNeighborNodes(targetNode))
                {
                    if (!queue.Contains(ne))
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