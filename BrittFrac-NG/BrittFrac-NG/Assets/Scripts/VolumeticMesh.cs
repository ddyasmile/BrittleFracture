using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeticMesh3D : MonoBehaviour
{
    public List<Vector3> nodes;
    public int PointCount = 1000000;

    public void InitNodes()
    {
        FillInternalPoints();
    }

    public void InitNodesWith(List<Vector3> nodes)
    {
        this.nodes = nodes;
        UpdateConvexMesh();
        FillInternalPoints();
    }

    public void FillInternalPoints()
    {
        var points = new List<Vector3>(PointCount);
        var mf = GetComponent<MeshFilter>();
        var mesh = mf.sharedMesh;
        var bounds = mesh.bounds;
        var calc = new GK.InsideMeshCalculator(mesh);

        var verts = new List<Vector3>();
        var tris = new List<int>();

        mesh.GetVertices(verts);
        mesh.GetTriangles(tris, 0);

        points.AddRange(verts);

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

        while (points.Count < PointCount)
        {
            var point = new Vector3(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y),
                Random.Range(bounds.min.z, bounds.max.z));

            if (calc.IsInside(point))
            {
                points.Add(point);
            }
        }

        this.nodes = points;
    }

    public void UpdateConvexMesh()
    {
        if (nodes.Count < 4)
        {
            return;
        }
        else
        {
            var mesh = new Mesh();
            mesh.name = "Shard";

            var calc = new GK.ConvexHullCalculator();
            var verts = new List<Vector3>();
            var tris = new List<int>();
            var normals = new List<Vector3>();

            calc.GenerateHull(nodes, ref verts, ref tris, ref normals);

            this.nodes = verts;
            mesh.SetVertices(verts);
            mesh.SetNormals(normals);
            mesh.SetTriangles(tris, 0);

            GetComponent<MeshFilter>().mesh = mesh;
        }
    }

    public float calculateVolumes()
    {
        // TODO: fill in volume List
        var meshFilter = GetComponent<MeshFilter>();
        Vector3[] arrVertices = meshFilter.mesh.vertices;
        int[] arrTriangles = meshFilter.mesh.triangles;
        float sum = 0.0f;
        for (int i = 0; i < meshFilter.mesh.subMeshCount; i++)
        {
            int[] arrIndices = meshFilter.mesh.GetTriangles(i);
            for (int j = 0; j < arrIndices.Length; j += 3)
                sum += this.CalculateVolume(arrVertices[arrIndices[j]]
                            , arrVertices[arrIndices[j + 1]]
                            , arrVertices[arrIndices[j + 2]]);
        }

        return sum;
    }

    private float CalculateVolume(Vector3 pt0, Vector3 pt1, Vector3 pt2)
    {
        return Vector3.Dot(Vector3.Cross(pt0, pt1), pt2);
    }
}