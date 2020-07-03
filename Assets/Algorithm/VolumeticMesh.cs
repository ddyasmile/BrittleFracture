using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TriangleNodes2D = ElementNS.Tuple3D<int>;
using TetrahedronNodes3D = ElementNS.Tuple4D<int>;

using TriangleEdges2D = ElementNS.Tuple3D<int>;
using TetrahedronEdges3D = ElementNS.Tuple6D<int>;

using Node2D = UnityEngine.Vector2;
using Node3D = UnityEngine.Vector3;

using Edge2D = EdgeNS.Edge;
using Edge3D = EdgeNS.Edge;

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

public class VolumeticMesh3D
{

    public VolumeticMesh3D()
    {
        this.nodeJointIndexes = new List<TetrahedronNodes3D>();
        this.edgeJointIndexes = new List<TetrahedronEdges3D>();

        // this.volume = new List<double>();
        this.edges = new List<Edge3D>();
        this.nodes = new List<Node3D>();
        this.damages = new List<Damage3D>();

        // this.tetrahedronCount = 0;
    }

    // public int tetrahedronCount;

    // VM - N
    public List<TetrahedronNodes3D> nodeJointIndexes;

    // VM - L
    public List<TetrahedronEdges3D> edgeJointIndexes;

    // @ all lists above contains index values
    // @ all lists below contains literal values

    // VM - E
    public double getVolume(int index)
    {
        var points = nodeJointIndexes[index];
        var edge1 = nodes[points.b] - nodes[points.a];
        var edge2 = nodes[points.c] - nodes[points.a];
        var edge3 = nodes[points.d] - nodes[points.a];

        return Vector3.Dot(edge1, Vector3.Cross(edge2, edge3));
    }

    public int tryAddEdge(int from, int to)
    {
        var edge = new Edge3D(from, to);
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

    public List<Edge3D> edges;
    public List<Node3D> nodes;
    public List<Damage3D> damages;

    public void calculateVolumes()
    {
        // TODO: fill in volume List
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

    public int getHitRespondingTetraIndex(Vector3 position)
    {
        return 0;
    }

    public double implicitSurface(Node3D pos, Node3D pos0, Vector3 direction)
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

        return 0.0;
    }

    public bool isCrossed(Node3D pos0, Vector3 direction, int tetra)
    {
        List<int> edgeOfTetra = new List<int>();
        edgeOfTetra.Add(edgeJointIndexes[tetra].a);
        edgeOfTetra.Add(edgeJointIndexes[tetra].b);
        edgeOfTetra.Add(edgeJointIndexes[tetra].c);
        edgeOfTetra.Add(edgeJointIndexes[tetra].d);
        edgeOfTetra.Add(edgeJointIndexes[tetra].e);
        edgeOfTetra.Add(edgeJointIndexes[tetra].f);

        bool crossed = false;
        foreach (var i in edgeOfTetra)
        {
            double fromPos = implicitSurface(nodes[edges[i].from], pos0, direction);
            double toPos = implicitSurface(nodes[edges[i].to], pos0, direction);
            if (fromPos * toPos <= 0)
            {
                double cutPos = Mathf.Abs((float)fromPos) / (Mathf.Abs((float)fromPos) + Mathf.Abs((float)toPos));
                damages.Add(new Damage3D(edges[i], cutPos));
                crossed = true;
            }
        }

        return crossed;
    }


    public void propagatingCracks(Vector3 hitPos, Vector3 initialDirection, 
        double fractureToughness, 
        double constantFactor, 
        double strainEnergyDensity)
    {
        // some value that cannot caculate now
        double areaOfFractureSurfaceThatCrossesElement = 1.0;

        int hitTetra = getHitRespondingTetraIndex(hitPos);

        List<int> nodesOfTetra = new List<int>();
        nodesOfTetra.Add(nodeJointIndexes[hitTetra].a);
        nodesOfTetra.Add(nodeJointIndexes[hitTetra].b);
        nodesOfTetra.Add(nodeJointIndexes[hitTetra].c);
        nodesOfTetra.Add(nodeJointIndexes[hitTetra].d);

        Node3D center0 = new Node3D(0.0f, 0.0f, 0.0f);
        foreach (var i in nodesOfTetra)
        {
            center0 += nodes[i];
        }
        center0 /= 4.0f;

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

                foreach (var neTetra in getNeighborsTetra(tetra))
                {
                    if ()
                }
            }
        }
    }
}