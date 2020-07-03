using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Edge3D = EdgeNS.Edge;
using TetrahedronNodes3D = ElementNS.Tuple4D<int>;
using TetrahedronEdges3D = ElementNS.Tuple6D<int>;

public class VolMeshGenerator3D : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        generateMesh();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public VolumeticMesh3D generateMesh() {
        var mesh = new VolumeticMesh3D();

        Mesh rawMesh = GetComponent<MeshFilter>().mesh;

        var vertices = rawMesh.vertices;
        var triangles = rawMesh.triangles;

        foreach (var vertex in vertices) {
            mesh.nodes.Add(vertex);
        }

        var convexGen = new GK.ConvexHullCalculator();
        var verts = new List<Vector3>();
		var tris = new List<int>();
		var normals = new List<Vector3>();

        convexGen.GenerateHull(new List<Vector3>(rawMesh.vertices), ref verts, ref tris, ref normals);
        rawMesh.Clear();
        rawMesh.SetVertices(verts);
        rawMesh.SetTriangles(tris, 0);
        rawMesh.SetNormals(normals);
        return mesh;
    }
}
