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

public class VolumeticMesh2D {
    public int count;

    // VM - E
    public List<double> area;

    // VM - N
    public List<TriangleNodes2D> nodeJointIndexes;

    // VM - L
    public List<TriangleEdges2D> edgeJointIndexes;

    // prepared
    public List<Edge2D> edges;
    public List<Node2D> nodes;
}

public class VolumeticMesh3D {
    public int count;

    // VM - E
    public List<double> volume;

    // VM - N
    public List<TetrahedronNodes3D> nodeJointIndexes;

    // VM - L
    public List<TetrahedronEdges3D> edgeJointIndexes;

    // prepared
    public List<Edge3D> edges;
    public List<Node3D> nodes;
}