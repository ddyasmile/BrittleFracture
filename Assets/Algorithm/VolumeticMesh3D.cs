

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
        this.edges = new List<Edge3D>();
        this.nodes = new List<Node3D>();
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
    /// find the node in current nodes.
    /// if exists, return its index
    /// if not, insert it and return its new index
    /// </summary>
    /// <param name="node">the node to insert or find</param>
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

    /// <summary>
    /// tryAddNode
    /// find the node in current nodes.
    /// if exists, return its index
    /// if not, insert it and return its new index
    /// </summary>
    /// <param name="from">one tail of the edge. choose which tail is irrelevant.</param>
    /// <param name="to">one tail of the edge. choose which tail is irrelevant.</param>
    public int tryAddEdge(int from, int to)
    {
        var edge = new Edge3D(from, to);
        return tryAddEdge(edge);
    }

    /// <summary>
    /// tryAddEdge
    /// find the edge in current edges.
    /// if exists, return its index
    /// if not, insert it and return its new index
    /// </summary>
    /// <param name="edge">the edge to insert or find</param>
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

    /// <summary>
    /// addTetrahedron
    /// add a tetrahedron into current mesh
    /// automatically adds nodes and edges
    /// and sets nodeIndexOfTetra and edgeIndexOfTetra
    /// </summary>
    /// <param name="nodeA">one node of four constructing the tetrahedron</param>
    /// <param name="nodeB">one node of four constructing the tetrahedron</param>
    /// <param name="nodeC">one node of four constructing the tetrahedron</param>
    /// <param name="nodeD">one node of four constructing the tetrahedron</param>
    public int addTetrahedron(Node3D nodeA, Node3D nodeB, Node3D nodeC, Node3D nodeD)
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

        nodeIndexOfTetra.Add(new TetrahedronNodes3D(nodeIndexA, nodeIndexB, nodeIndexC, nodeIndexD));
        edgeIndexOfTetra.Add(new TetrahedronEdges3D(edgeA, edgeB, edgeC, edgeD, edgeE, edgeF));

        if (nodeIndexOfTetra.Count != edgeIndexOfTetra.Count)
        {
            Debug.LogError("volmesh3D internal inconsistency");
        }
        return nodeIndexOfTetra.Count - 1;
    }

    /// <summary>
    /// tryAddDamage
    /// find the damage in current damages.
    /// if exists, return its index
    /// if not, insert it and return its new index
    /// </summary>
    /// <param name="damage">the damage to insert or find</param>
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

    /// <summary>
    /// isDamagedEdge
    /// judge if an edge is damaged
    /// </summary>
    /// <param name="edgeIndex">the edge index in current edges List</param>
    public bool isDamagedEdge(int edgeIndex)
    {
        var edge = edges[edgeIndex];
        foreach (var damage in damages)
        {
            if (damage.edge.Equals(edge))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// isDamagedEdge
    /// judge if an edge is damaged
    /// </summary>
    /// <param name="edge">the edge to be judged</param>
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

    /// <summary>
    /// isDamagedTetrahedron
    /// judge if an tetrahedron contains damaged edge
    /// </summary>
    /// <param name="tetraIndex">the index of tetrahedron to be judged</param>
    public bool isDamagedTetrahedron(int tetraIndex)
    {
        foreach (var edge in edgeIndexOfTetra[tetraIndex])
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


    /// <summary>
    /// getCloseNeighbors
    /// returns all nodes index that directly connects to provided tetrahedron
    /// </summary>
    /// <param name="index">the index of node to be calculated</param>
    public List<int> getNeighborNodes(int index)
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

    /// <summary>
    /// getCloseNeighbors
    /// returns all tetrahedron index that directly connects to provided tetrahedron
    /// </summary>
    /// <param name="index">the index of tetrahedron to be calculated</param>
    public List<int> getNeighborTetras(int index)
    {
        var targetEdges = new List<int>();

        foreach (var i in edgeIndexOfTetra[index])
        {
            targetEdges.Add((int)i);
        }

        var resultIndex = new List<int>();
        for (int i = 0; i < edgeIndexOfTetra.Count; ++i)
        {
            if (i == index) continue;

            int counter = 0;
            foreach (var edge in edgeIndexOfTetra[i])
            {
                if (targetEdges.Contains((int)edge)) ++counter;
            }

            if (counter > 2)
            {
                resultIndex.Add(i);
            }
        }

        return resultIndex;
    }


    /// <summary>
    /// getHitRespondingTetraIndex
    /// returns the responding tetrahedron index
    /// </summary>
    /// <param name="position">the world position where the hit happens</param>
    public int getHitRespondingTetraIndex(Vector3 position)
    {
        float minDistance = float.MaxValue;
        int minDistanceTetraIndex = -1;

        for (int i = 0; i < nodeIndexOfTetra.Count; ++i)
        {
            var nodesTuple = nodeIndexOfTetra[i];
            float distance = 0;

            foreach (var node in nodesTuple)
            {
                distance += (nodes[(int)node] - position).sqrMagnitude;
            }

            if (minDistance > distance)
            {
                minDistance = distance;
                minDistanceTetraIndex = i;
            }
        }

        return minDistanceTetraIndex;
    }


    public float implicitSurface(Node3D pos, Node3D pos0, Vector3 direction)
    {
        Vector3 up = new Vector3(0.0f, 1.0f, 0.0f);

        direction = direction.normalized;
        Vector3 front, side;
        if (direction == up)
        {
            front = new Vector3(1.0f, 0.0f, 0.0f);
            side = new Vector3(0.0f, 0.0f, 1.0f);
        }
        else if (direction == -up)
        {
            front = new Vector3(-1.0f, 0.0f, 0.0f);
            side = new Vector3(0.0f, 0.0f, -1.0f);
        }
        else
        {
            side = Vector3.Cross(up, direction).normalized;
            front = Vector3.Cross(side, direction).normalized;
        }

        return 0;
    }

    public bool isCrossed(Vector3 direction, int tetra)
    {
        List<int> nodesOfTetra = new List<int>();
        nodesOfTetra.Add(nodeIndexOfTetra[tetra].a);
        nodesOfTetra.Add(nodeIndexOfTetra[tetra].b);
        nodesOfTetra.Add(nodeIndexOfTetra[tetra].c);
        nodesOfTetra.Add(nodeIndexOfTetra[tetra].d);

        Node3D center = new Node3D(0.0f, 0.0f, 0.0f);
        foreach (var i in nodesOfTetra)
        {
            center += nodes[i];
        }
        center /= 4.0f;

        List<int> edgeOfTetra = new List<int>();
        edgeOfTetra.Add(edgeIndexOfTetra[tetra].a);
        edgeOfTetra.Add(edgeIndexOfTetra[tetra].b);
        edgeOfTetra.Add(edgeIndexOfTetra[tetra].c);
        edgeOfTetra.Add(edgeIndexOfTetra[tetra].d);
        edgeOfTetra.Add(edgeIndexOfTetra[tetra].e);
        edgeOfTetra.Add(edgeIndexOfTetra[tetra].f);

        bool crossed = false;
        foreach (var i in edgeOfTetra)
        {
            double curr = implicitSurface(pos0, center, direction);
            if (curr * last < 0) {

            }
        }

        return false;
    }


    public void propagatingCracks(Vector3 hitPos, Vector3 initialDirection, 
        double fractureToughness, 
        double constantFactor, 
        double strainEnergyDensity)
    {
        // some value that cannot caculate now
        double areaOfFractureSurfaceThatCrossesElement = 1.0;

        int hitTetra = getHitRespondingTetraIndex(hitPos);

        List<int> fracTetra = new List<int>();
        List<int> nextTetra = new List<int>();
        List<int> tmpTetra = new List<int>();

        fracTetra.Add(hitTetra);
        tmpTetra.Add(hitTetra);

        double fracEnergy = 0.0;
        double strainEnergy = 0.0;

        while (tmpTetra.Count != 0)
        {
            foreach (var tetra in tmpTetra)
            {
                fracEnergy += 
                    areaOfFractureSurfaceThatCrossesElement * 
                    fractureToughness;
                strainEnergy +=
                    areaOfFractureSurfaceThatCrossesElement *
                    constantFactor *
                    strainEnergyDensity;

                // update frac Edges

                foreach (var neTetra in getNeighborTetras(tetra))
                {
                    if (true) {

                    }
                }
            }
        }
    }
}