using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Edge3D = EdgeNS.Edge;
using TetrahedronNodes3D = ElementNS.Tuple4D<int>;
using TetrahedronEdges3D = ElementNS.Tuple6D<int>;

public class VolMeshGenerator3D : MonoBehaviour
{

    public int PointCountLimit = 500;

    // Start is called before the first frame update
    void Start()
    {
        generateMesh();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public VolumeticMesh3D generateMesh()
    {
        Mesh rawMesh = GetComponent<MeshFilter>().mesh;

        var points = new List<Vector3>(rawMesh.vertices);

        var mf = GetComponent<MeshFilter>();
        var mesh = mf.sharedMesh;
        var bounds = mesh.bounds;
        var insideMeshCalc = new GK.InsideMeshCalculator(mesh);

        var verts = new List<Vector3>();
        var tris = new List<int>();
        var normals = new List<Vector3>();

        for (int i = 0; i < points.Count - 1; i++)
        {
            var p0 = points[i];

            for (int j = i + 1; j < points.Count; j++)
            {
                var p1 = points[j];

                if ((p1 - p0).magnitude <= 0.00001f)
                {
                    points.RemoveAt(j--);
                }
            }
        }

        while (points.Count < PointCountLimit)
        {
            var point = new Vector3(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y),
                Random.Range(bounds.min.z, bounds.max.z));

            if (insideMeshCalc.IsInside(point))
            {
                points.Add(point);
            }
        }

        if (points.Count < 4)
        {
            Debug.LogError("no enough points to generate volmesh");
            return null;
        }

        var returnMesh = new VolumeticMesh3D();

        var scale = transform.lossyScale.x;

        var hullCalc = new GK.ConvexHullCalculator();
        hullCalc.GenerateHull(points, ref verts, ref tris, ref normals);

        returnMesh.nodes = verts;

        var triangleIndexes = new List<List<int>>();

        for (int i = 0; i < tris.Count; i += 3)
        {
            triangleIndexes.Add(new List<int>() { tris[i], tris[i + 1], tris[i + 2] });
        }

        var triangleCount = triangleIndexes.Count;
        if (triangleCount < 4)
        {
            Debug.LogError("no enough triangles to generate volmesh");
            return returnMesh;
        }
        

        // write a rubbish O(N^4) algorithm
        for (int i = 0; i < triangleCount - 3; i++)
        {
            for (int j = i + 1; j < triangleCount - 2; j++)
            {
                for (int k = j + 1; k < triangleCount - 1; k++)
                {
                    for (int l = k + 1; l < triangleCount; l++)
                    {
                        // Debug.Log(triangleCount);
                        // Debug.Log(i);
                        // Debug.Log(j);
                        // Debug.Log(k);
                        // Debug.Log(l);
                        // Debug.Log("===");
                        var uniqueNodes = new List<int>();
                        uniqueNodes.AddRange(triangleIndexes[i]);
                        uniqueNodes.AddRange(triangleIndexes[j]);
                        uniqueNodes.AddRange(triangleIndexes[k]);
                        uniqueNodes.AddRange(triangleIndexes[l]);
                        if (uniqueNodes.Count < 4)
                        {
                            Debug.LogError("no ... that's impossible!");
                        }
                        else if (uniqueNodes.Count == 4)
                        {
                            var node1 = uniqueNodes[0];
                            var node2 = uniqueNodes[1];
                            var node3 = uniqueNodes[2];
                            var node4 = uniqueNodes[3];
                            returnMesh.nodeJointIndexes.Add(new TetrahedronNodes3D(node1, node2, node3, node4));

                            var edge1 = returnMesh.tryAddEdge(node1, node2);
                            var edge2 = returnMesh.tryAddEdge(node1, node3);
                            var edge3 = returnMesh.tryAddEdge(node1, node4);
                            var edge4 = returnMesh.tryAddEdge(node2, node3);
                            var edge5 = returnMesh.tryAddEdge(node2, node4);
                            var edge6 = returnMesh.tryAddEdge(node3, node4);

                            returnMesh.edgeJointIndexes.Add(new TetrahedronEdges3D(edge1, edge2, edge3, edge4, edge5, edge6));
                        }
                    }
                }
            }
        }

        return returnMesh;
    }
}
