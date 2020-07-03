using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Edge2D = EdgeNS.Edge;
using TriangleNodes2D = ElementNS.Tuple3D<int>;
using TriangleEdges2D = ElementNS.Tuple3D<int>;

public class VolMeshGenerator2D : MonoBehaviour
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

    public VolumeticMesh2D generateMesh() {
        var mesh = new VolumeticMesh2D();

        Mesh rawMesh = GetComponent<MeshFilter>().mesh;

        var vertices = rawMesh.vertices;

        foreach (var vertex in vertices) {
            mesh.nodes.Add(new Vector2(vertex.x, vertex.y));
        }

        var calculator = new GK.DelaunayCalculator();
        var result = calculator.CalculateTriangulation(mesh.nodes);
        var triangles = result.Triangles;

        for (int i = 0; i < triangles.Count; i += 3) {
            var edgeA = new Edge2D(triangles[i], triangles[i + 2]);
            var edgeB = new Edge2D(triangles[i + 2], triangles[i + 1]);
            var edgeC = new Edge2D(triangles[i + 1], triangles[i]);

            int edgeIndexA, edgeIndexB, edgeIndexC;

            if (!mesh.edges.Contains(edgeA)) {
                mesh.edges.Add(edgeA);
                edgeIndexA = mesh.edges.Count - 1;
            } else {
                edgeIndexA = mesh.edges.FindIndex(edge => edge == edgeA);
            }

            if (!mesh.edges.Contains(edgeB)) {
                mesh.edges.Add(edgeB);
                edgeIndexB = mesh.edges.Count - 1;
            } else {
                edgeIndexB = mesh.edges.FindIndex(edge => edge == edgeB);
            }

            if (!mesh.edges.Contains(edgeC)) {
                mesh.edges.Add(edgeC);
                edgeIndexC = mesh.edges.Count - 1;
            } else {
                edgeIndexC = mesh.edges.FindIndex(edge => edge == edgeC);
            }

            mesh.nodeJointIndexes.Add(new TriangleNodes2D(triangles[i], triangles[i + 2], triangles[i + 1]));
            mesh.edgeJointIndexes.Add(new TriangleEdges2D(edgeIndexA, edgeIndexB, edgeIndexC));
        }

        return mesh;
    }
}
