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

        for (int i = 0; i < triangles.Length; i += 3) {
            var edgeA = new Edge3D(triangles[i], triangles[i + 2]);
            var edgeB = new Edge3D(triangles[i + 2], triangles[i + 1]);
            var edgeC = new Edge3D(triangles[i + 1], triangles[i]);

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

            // mesh.nodeJointIndexes.Add(new TriangleNodes3D(triangles[i], triangles[i + 2], triangles[i + 1]));
            // mesh.edgeJointIndexes.Add(new TriangleEdges2D(edgeIndexA, edgeIndexB, edgeIndexC));
        }

        return mesh;
    }
}
