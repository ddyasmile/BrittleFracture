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

    // VM - N
    public List<TriangleNodes2D> nodeJointIndexes;

    // VM - L
    public List<TriangleEdges2D> edgeJointIndexes;

    // @ all lists above contains index values
    // @ all lists below contains literal values

    // VM - E
    public List<double> area;
    public List<Edge2D> edges;
    public List<Node2D> nodes;
    public List<Damage2D> damages;

    public bool isDamaged(int target) {
        foreach(var edge in edgeJointIndexes[target]) {
            foreach(var damage in damages) {
                if (damage.edge.Equals(edge)) {
                    return true;
                }
            }
        }
        return false;
    }
}

public class VolumeticMesh3D {
    public int count;

    // VM - N
    public List<TetrahedronNodes3D> nodeJointIndexes;

    // VM - L
    public List<TetrahedronEdges3D> edgeJointIndexes;

    // @ all lists above contains index values
    // @ all lists below contains literal values

    // VM - E
    public List<double> volume;
    public List<Edge3D> edges;
    public List<Node3D> nodes;
    public List<Damage3D> damages;

    public bool isDamaged(int target) {
        foreach(var edge in edgeJointIndexes[target]) {
            foreach(var damage in damages) {
                if (damage.edge.Equals(edge)) {
                    return true;
                }
            }
        }
        return false;
    }
}