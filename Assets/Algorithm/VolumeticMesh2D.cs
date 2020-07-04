using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TriangleNodes2D = ElementNS.Tuple3D<int>;
using TriangleEdges2D = ElementNS.Tuple3D<int>;
using Node2D = UnityEngine.Vector2;
using Edge2D = EdgeNS.Edge;

public class VolumeticMesh2D
{

    public VolumeticMesh2D()
    {
        this.nodeJointIndexes = new List<TriangleNodes2D>();
        this.edgeJointIndexes = new List<TriangleEdges2D>();

        // this.area = new List<double>();
        this.edges = new List<Edge2D>();
        this.nodes = new List<Node2D>();
        this.damages = new List<Damage2D>();

        // this.triangleCount = 0;
    }

    // public int triangleCount;

    // VM - N
    public List<TriangleNodes2D> nodeJointIndexes;

    // VM - L
    public List<TriangleEdges2D> edgeJointIndexes;

    // @ all lists above contains index values
    // @ all lists below contains literal values

    // VM - E
    // public List<double> area;
    public double getArea(int index)
    {
        var points = nodeJointIndexes[index];
        var edge1 = nodes[points.b] - nodes[points.a];
        var edge2 = nodes[points.c] - nodes[points.a];

        return Vector2.Dot(edge1, edge2);
    }

    public int tryAddEdge(int from, int to)
    {
        var edge = new Edge2D(from, to);
        if (edges.Contains(edge))
        {
            return edges.FindIndex(edg => edg == edge);
        }
        else
        {
            edges.Add(edge);
            return edges.Count - 1;
        }
    }

    public List<Edge2D> edges;
    public List<Node2D> nodes;
    public List<Damage2D> damages;

    public void calculateAreas()
    {
        // TODO: fill in area List
    }

    public bool isDamagedEdge(Edge2D edge)
    {
        foreach (var damage in damages)
        {
            if (damage.edge.Equals(edge))
            {
                return true;
            }
        }
        return false;
    }

    public bool isDamagedTriangle(int target)
    {
        foreach (var edge in edgeJointIndexes[target].flatten())
        {
            foreach (var damage in damages)
            {
                if (damage.edge.Equals(edge))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public List<int> getNeighbors(int i)
    {
        var neighbors = new HashSet<int>();
        foreach (var edge in edges)
        {
            if (edge.from == i)
            {
                neighbors.Add(edge.to);
            }
            else if (edge.to == i)
            {
                neighbors.Add(edge.from);
            }
        }
        return new List<int>(neighbors);
    }
}
