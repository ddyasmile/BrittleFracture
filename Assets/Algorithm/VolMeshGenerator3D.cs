using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Edge3D = EdgeNS.Edge;
using TetrahedronNodes3D = ElementNS.Tuple4D<int>;
using TetrahedronEdges3D = ElementNS.Tuple6D<int>;

public class VolMeshGenerator3D : MonoBehaviour
{

    public int PointCountLimit = 50;
    public Material tetraMaterial;

    // Start is called before the first frame update
    void Start()
    {
        var mesh3d = generateMesh();
        // drawMesh(mesh3d);
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
        Debug.Log(string.Format("internal {0} points", points.Count));

        // foreach (var point in points)
        // {
        // Debug.Log(point);
        // }

        var returnMesh = new VolumeticMesh3D();

        returnMesh.vertices = points;

        // write a rubbish O(N^4) algorithm
        for (int i = 0; i < points.Count - 3; i++)
        {
            for (int j = i + 1; j < points.Count - 2; j++)
            {
                for (int k = j + 1; k < points.Count - 1; k++)
                {
                    for (int l = k + 1; l < points.Count; l++)
                    {
                        // Debug.Log(triangleCount);
                        // Debug.Log(i);
                        // Debug.Log(j);
                        // Debug.Log(k);
                        // Debug.Log(l);
                        // Debug.Log("===")

                        returnMesh.nodeJointIndexes.Add(new TetrahedronNodes3D(i, j, k, l));

                        var edge1 = returnMesh.tryAddEdge(i, j);
                        var edge2 = returnMesh.tryAddEdge(i, k);
                        var edge3 = returnMesh.tryAddEdge(i, l);
                        var edge4 = returnMesh.tryAddEdge(j, k);
                        var edge5 = returnMesh.tryAddEdge(j, l);
                        var edge6 = returnMesh.tryAddEdge(k, l);

                        returnMesh.edgeJointIndexes.Add(new TetrahedronEdges3D(edge1, edge2, edge3, edge4, edge5, edge6));
                    }
                }
            }
        }

        Debug.Log(string.Format("splited it into {0} tetras", returnMesh.edgeJointIndexes.Count));

        return returnMesh;
    }

    public void drawSubElement(List<int> vertices)
    {
        var vertices;
    }

    public void drawMesh(VolumeticMesh3D toDrawMesh)
    {
        Destroy(gameObject);
        for (int i = 0; i < toDrawMesh.edgeJointIndexes.Count; ++i)
        {
            var vectors = new List<Vector3>
            {
                toDrawMesh.nodes[toDrawMesh.nodeJointIndexes[i].a],
                toDrawMesh.nodes[toDrawMesh.nodeJointIndexes[i].b],
                toDrawMesh.nodes[toDrawMesh.nodeJointIndexes[i].c],
                toDrawMesh.nodes[toDrawMesh.nodeJointIndexes[i].d]
            };
            var g = new GameObject("Tetra");
            var sb = g.AddComponent<ShaderBase>();
            sb.tetraPart.PushBackTetra(vectors);
            sb.material = tetraMaterial;
            var rb = g.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.drag = 0.3f;
            rb.angularDrag = 0.3f;
            var random = new System.Random();
            rb.AddForce(random.Next(-5, 5), random.Next(-5, 5), random.Next(-5, 5));
        }
    }
}
