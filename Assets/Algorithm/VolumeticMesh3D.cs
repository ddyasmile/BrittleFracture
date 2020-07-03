

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TetrahedronNodes3D = ElementNS.Tuple4D<int>;
using TetrahedronEdges3D = ElementNS.Tuple6D<int>;
using Node3D = UnityEngine.Vector3;
using Edge3D = EdgeNS.Edge;

public class VolumeticMesh3D
{
    /// <summary>
    /// default constructor
    /// </summary>
    public VolumeticMesh3D()
    {
        // this.volume = new List<double>();
        this.edges = new List<Node3D>();
        this.nodes = new List<Edge3D>();
        this.damages = new List<Damage3D>();

        this.nodeIndexOfTetra = new List<TetrahedronNodes3D>();
        this.edgeIndexOfTetra = new List<TetrahedronEdges3D>();
    }

    /// <summary>
    /// VM - N
    /// containing 4 node indexes of a tetra.
    /// </summary>
    public List<TetrahedronNodes3D> nodeIndexOfTetra;

    /// <summary>
    /// VM - L
    /// containing 6 edge indexes of a tetra.
    /// </summary>
    public List<TetrahedronEdges3D> edgeIndexOfTetra;

    // @ all lists above contains index values
    // @ all lists below contains literal values

    /// <summary>
    /// VM - E
    /// calculate the volume of a specific tetra
    /// </summary>
    /// <param name="index">calculate which tetra's volume?</param>
    public float getVolumeOfTetraAtIndex(int index)
    {
        var points = nodeIndexOfTetra[index];
        var edge1 = nodes[points.b] - nodes[points.a];
        var edge2 = nodes[points.c] - nodes[points.a];
        var edge3 = nodes[points.d] - nodes[points.a];

        return Vector3.Dot(edge1, Vector3.Cross(edge2, edge3));
    }

    /// <summary>
    /// nodes
    /// do not Add to nodes directly
    /// use wrapped function `tryAddNode` instead
    /// </summary>
    public List<Node3D> nodes;

    /// <summary>
    /// edges
    /// do not Add to edges directly
    /// use wrapped function `tryAddEdge` instead
    /// </summary>
    public List<Edge3D> edges;


    /// <summary>
    /// damages
    /// do not Add to damages directly
    /// use wrapped function `tryAddDamage` instead
    /// </summary>
    public List<Damage3D> damages;


    /// <summary>
    /// calculateVolumes
    /// calculate the whole volume of all tetras
    /// </summary>
    public float calculateVolumes()
    {
        float sumVolume = 0;
        for (int i = 0; i < nodeIndexOfTetra.Count; ++i)
        {
            sumVolume += getVolumeOfTetraAtIndex(i);
        }
        return sumVolume;
    }

    /// <summary>
    /// tryAddNode
    /// calculate the volume of a specific tetra
    /// </summary>
    /// <param name="node">the node to insert of find</param>
    public int tryAddNode(Node3D node)
    {
        if (nodes.Contains(node))
        {
            return nodes.FindIndex(nod => nod == node);
        }
        else
        {
            nodes.Add(node);
            return nodes.Count - 1;
        }
    }

    public int tryAddEdge(int from, int to)
    {
        var edge = new Edge3D(from, to);
        return tryAddEdge(edge);
    }

    public int tryAddEdge(Edge3D edge)
    {
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

    public int tryAddTetrahedron(Node3D nodeA, Node3D nodeB, Node3D nodeC, Node3D nodeD)
    {
        var nodeIndexA = tryAddNode(nodeA);
        var nodeIndexB = tryAddNode(nodeB);
        var nodeIndexC = tryAddNode(nodeC);
        var nodeIndexD = tryAddNode(nodeD);

        var edgeA = tryAddEdge(nodeIndexA, nodeIndexB);
        var edgeB = tryAddEdge(nodeIndexA, nodeIndexC);
        var edgeC = tryAddEdge(nodeIndexA, nodeIndexD);
        var edgeD = tryAddEdge(nodeIndexB, nodeIndexC);
        var edgeE = tryAddEdge(nodeIndexB, nodeIndexD);
        var edgeF = tryAddEdge(nodeIndexC, nodeIndexD);

        this.nodeIndexOfTetra.Add(new TetrahedronNodes3D(nodeIndexA, nodeIndexB, nodeIndexC, nodeIndexD));
        this.edgeIndexOfTetra.Add(new TetrahedronEdges3D(edgeA, edgeB, edgeC, edgeD, edgeE, edgeF));
    }

    public int tryAddDamage(Damage3D damage)
    {
        if (damages.Contains(damage))
        {
            return damages.FindIndex(damag => damag == damage);
        }
        else
        {
            damages.Add(damage);
            return damages.Count - 1;
        }
    }

    public bool isDamagedEdge(Edge3D edge)
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

    public bool isDamagedTetrahedron(int target)
    {
        foreach (var edge in edgeJointIndexes[target])
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

    public List<int> getNearestNeighbors(int index)
    {
        var neighbors = new HashSet<int>();
        foreach (var edge in edges)
        {
            if (edge.from == index)
            {
                neighbors.Add(edge.to);
            }
            else if (edge.to == index)
            {
                neighbors.Add(edge.from);
            }
        }
        return new List<int>(neighbors);
    }
}