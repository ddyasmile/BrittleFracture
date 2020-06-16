using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ShaderBase : MonoBehaviour
{
    public Material material;
    /// <summary>
    /// Given a plane, split this mesh by it, and creates a new mesh object.
    /// </summary>
    /// <param name="P">A point on the plane.</param>
    /// <param name="normal">Normal of the plane.</param>
    public GameObject SplitTetrahedron(Vector3 P, Vector3 normal)
    {
        int[] segments = { 0, 1, 0, 2, 0, 3, 1, 2, 1, 3, 2, 3 };
        var mesh = gameObject.GetComponent<MeshFilter>().mesh;
        TetraPart positive = new TetraPart();
        TetraPart negative = new TetraPart();
        for (int tetraIndex = 0; tetraIndex < mesh.triangles.Length; tetraIndex += 12)
        {
            List<Vector3> vertices = ToolFunction.MergeVertices(mesh.vertices, tetraIndex, tetraIndex + 12);
            List<Vector3> intersections = new List<Vector3>();
            Vector3 intersection;
            for (int j = 0; j < 6; j++)
            {
                Vector3 vertex1 = vertices[segments[2 * j]];
                Vector3 vertex2 = vertices[segments[2 * j + 1]];
                if (ToolFunction.IntersectionForSegmentWithPlane(vertex1, vertex2, P, normal, out intersection))
                {
                    intersections.Add(intersection);
                }
            }
            switch (intersections.Count)
            {
                case 0:
                    // no intersections. Ignore.
                    return null;
                case 1:
                    intersection = intersections[0];
                    // 1 intersection (not including two vertexs)
                    int pindex = 0, nindex = 0;
                    List<int> remains = new List<int>();
                    for (int i = 0; i < 4; i++)
                    {
                        Vector3 v = vertices[i];
                        if (Vector3.Dot(v - intersection, normal) > 0)
                        {
                            pindex = i;
                        }
                        else if (Vector3.Dot(v - intersection, normal) < 0)
                        {
                            nindex = i;
                        } else
                        {
                            remains.Add(i);
                        }
                    }
                    positive.PushBackTetra(new List<Vector3>{
                        vertices[pindex], vertices[remains[0]], vertices[remains[1]], intersection
                    });
                    negative.PushBackTetra(new List<Vector3>{
                        vertices[nindex], vertices[remains[0]], vertices[remains[1]], intersection
                    });
                    break;
                case 2:
                    intersection = intersections[0];
                    List<int> nindices = new List<int>();
                    List<int> pindices = new List<int>();
                    int remainIndex = 0;
                    for (int i = 0; i < 4; i++)
                    {
                        Vector3 v = vertices[i];
                        if (Vector3.Dot(v - intersection, normal) > 0)
                        {
                            pindices.Add(i);
                        }
                        else if (Vector3.Dot(v - intersection, normal) < 0)
                        {
                            nindices.Add(i);
                        }
                        else
                        {
                            remainIndex = i;
                        }
                    }
                    if (pindices.Count == 2)
                    {
                        positive.PushBackPyramid(new List<Vector3>{
                            vertices[remainIndex], intersections[0], intersections[1], vertices[pindices[0]], vertices[pindices[1]]
                        });
                        negative.PushBackTetra(new List<Vector3>{
                            vertices[remainIndex], intersections[0], intersections[1], vertices[nindices[0]]
                        });
                    } else
                    {
                        negative.PushBackPyramid(new List<Vector3>{
                            vertices[remainIndex], intersections[0], intersections[1], vertices[nindices[0]], vertices[nindices[1]]
                        });
                        positive.PushBackTetra(new List<Vector3>{
                            vertices[remainIndex], intersections[0], intersections[1], vertices[pindices[0]]
                        });
                    }
                    break;
                case 3:
                    intersection = intersections[0];
                    nindices = new List<int>();
                    pindices = new List<int>();
                    for (int i = 0; i < 4; i++)
                    {
                        Vector3 v = vertices[i];
                        if (Vector3.Dot(v - intersection, normal) > 0)
                        {
                            pindices.Add(i);
                        }
                        else if (Vector3.Dot(v - intersection, normal) < 0)
                        {
                            nindices.Add(i);
                        }
                    }
                    if (pindices.Count == 3)
                    {
                        positive.PushBackTriPrism(new List<Vector3>{
                            intersections[0], intersections[1], intersections[2], vertices[pindices[0]], vertices[pindices[1]], vertices[pindices[2]]
                        });
                        negative.PushBackTetra(new List<Vector3>{
                            intersections[0], intersections[1], intersections[2], vertices[nindices[0]]
                        });
                    } else
                    {
                        negative.PushBackTriPrism(new List<Vector3>{
                            intersections[0], intersections[1], intersections[2], vertices[nindices[0]], vertices[nindices[1]], vertices[nindices[2]]
                        });
                        positive.PushBackTetra(new List<Vector3>{
                            intersections[0], intersections[1], intersections[2], vertices[pindices[0]]
                        });
                    }
                    break;
                case 4:
                    intersection = intersections[0];
                    nindices = new List<int>();
                    pindices = new List<int>();
                    for (int i = 0; i < 4; i++)
                    {
                        Vector3 v = vertices[i];
                        if (Vector3.Dot(v - intersection, normal) > 0)
                        {
                            pindices.Add(i);
                        }
                        else if (Vector3.Dot(v - intersection, normal) < 0)
                        {
                            nindices.Add(i);
                        }
                    }
                    positive.PushBackTriPrism(new List<Vector3>{
                        intersections[0], intersections[1], intersections[2], intersections[3], vertices[pindices[0]], vertices[pindices[1]]
                    });
                    negative.PushBackTriPrism(new List<Vector3>{
                        intersections[0], intersections[1], intersections[2], intersections[3], vertices[nindices[0]], vertices[nindices[1]]
                    });
                    break;
            }
        }
        mesh.vertices = positive.vertices.ToArray();
        mesh.uv = positive.uvs.ToArray();
        mesh.triangles = positive.triangles.ToArray();
        mesh.RecalculateNormals();
        gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;

        // new Object
        GameObject fragment = new GameObject("Fragment");
        fragment.AddComponent<MeshFilter>().mesh = new Mesh()
        {
            vertices = negative.vertices.ToArray(),
            uv = negative.uvs.ToArray(),
            triangles = negative.triangles.ToArray(),
        };
        fragment.AddComponent<MeshRenderer>().material = material;
        fragment.AddComponent<MeshCollider>().convex = true;
        fragment.GetComponent<MeshFilter>().mesh.RecalculateNormals();
        fragment.AddComponent<ShaderBase>().material = material;
        return fragment;
    }
    void Start()
    {
        if (GetComponent<MeshRenderer>() != null) return;
        //GameObject gameObject = new GameObject("Tetrahedron");
        gameObject.transform.position = Vector3.zero;

        //顶点数组
        List<Vector3> vectors = new List<Vector3>
        {
            new Vector3(0.0f, 0.0f, 0.0f),
            new Vector3(1.0f, 0.0f, 0.0f),
            new Vector3(0.0f, 1.0f, 0.0f),
            new Vector3(0.0f, 0.0f, 1.0f)
        };
        TetraPart tetraPart = new TetraPart();
        tetraPart.PushBackTetra(vectors);

        Mesh mesh = new Mesh()
        {
            vertices = tetraPart.vertices.ToArray(),
            uv = tetraPart.uvs.ToArray(),
            triangles = tetraPart.triangles.ToArray(),
        };

        //重新计算网格的法线
        //在修改完顶点后，通常会更新法线来反映新的变化。法线是根据共享的顶点计算出来的。
        //导入到网格有时不共享所有的顶点。例如：一个顶点在一个纹理坐标的接缝处将会被分成两个顶点。
        //因此这个RecalculateNormals函数将会在纹理坐标接缝处创建一个不光滑的法线。
        //RecalculateNormals不会自动产生切线，因此bumpmap着色器在调用RecalculateNormals之后不会工作。然而你可以提取你自己的切线。
        
        gameObject.AddComponent<MeshFilter>().mesh = mesh;
        //Material/New Material 1
        gameObject.AddComponent<MeshRenderer>().material = material;
        gameObject.AddComponent<MeshCollider>().convex = true;
        mesh.RecalculateNormals();
    }
    void Update()
    {
    }

}