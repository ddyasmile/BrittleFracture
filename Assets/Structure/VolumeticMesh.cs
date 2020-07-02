using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TriangleNodes2D = Element.Tuple3D<int>;
using TetrahedronNodes3D = Element.Tuple4D<int>;

using TriangleEdges2D = Element.Tuple3D<int>;
using TetrahedronEdges3D = Element.Tuple6D<int>;

using Node2D = UnityEngine.Vector2;
using Node3D = UnityEngine.Vector3;

public class VolumeticMesh2D {
    public int count;

    // VM - E
    public List<double> area;

    // VM - N
    public List<TetrahedronNodes2D> nodeJointIndexes;

    // VM - L
    public List<TetrahedronEdges2D> edgeJointIndexes;

    // prepared
    public List<Edge> edges;
    public List<Vector2> nodes;
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
    public List<Edge> edges;
    public List<Vector3> nodes;
}