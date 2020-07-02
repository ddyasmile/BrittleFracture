using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Edge = EdgeNS.Edge;

public class FloodAlgorithm {
    public static List<List<int>> floodSplit2D(ref VolumeticMesh2D mesh) {
        var unassignedNodes = new List<int>();
        for (int i = 0; i < mesh.triangleCount; ++i) {
            unassignedNodes.Add(i);
        }
        var fragmentCount = 0;
        var nodes = new List<List<int>>();

        while (unassignedNodes.Count != 0) {
            var newFragmentId = fragmentCount;
            fragmentCount++;
            
            var targetNode = unassignedNodes[0];
            unassignedNodes.RemoveAt(0);
            foreach (var ne in mesh.getNeighbors(targetNode)) {
                if (targetNode != ne) {
                    var edge = new Edge(ne, targetNode);
                    if (!mesh.isDamagedEdge(edge)) {
                        if (nodes[newFragmentId] == null) {
                            nodes[newFragmentId] = new List<int>() {ne};
                        } else {
                            nodes[newFragmentId].Add(ne);
                        }
                    }
                    unassignedNodes.Remove(ne);
                }
            }
        }

        return nodes;
    }

    public static List<List<int>> floodSplit3D(ref VolumeticMesh3D mesh) {
        var unassignedNodes = new List<int>();
        for (int i = 0; i < mesh.tetrahedronCount; ++i) {
            unassignedNodes.Add(i);
        }
        var fragmentCount = 0;
        var nodes = new List<List<int>>();

        while (unassignedNodes.Count != 0) {
            var newFragmentId = fragmentCount;
            fragmentCount++;
            
            var targetNode = unassignedNodes[0];
            unassignedNodes.RemoveAt(0);
            foreach (var ne in mesh.getNeighbors(targetNode)) {
                if (targetNode != ne) {
                    var edge = new Edge(ne, targetNode);
                    if (!mesh.isDamagedEdge(edge)) {
                        if (nodes[newFragmentId] == null) {
                            nodes[newFragmentId] = new List<int>() {ne};
                        } else {
                            nodes[newFragmentId].Add(ne);
                        }
                    }
                    unassignedNodes.Remove(ne);
                }
            }
        }

        return nodes;
    }
}