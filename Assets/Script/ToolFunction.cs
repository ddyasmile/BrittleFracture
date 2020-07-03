using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Plane
{
    public Vector3 normal; // Normal of this plane
    public Vector3 point; // Any point on this plane
    public Plane(Vector3 normal, Vector3 point)
    {
        this.normal = normal;
        this.point = point;
    }
    public Plane(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        normal = Vector3.Cross(p2 - p1, p3 - p1);
        point = p1;
    }
}

public class Tetrahedron
{
    public List<Vector3> tetra;
    public Tetrahedron(List<Vector3> vertices)
    {
        tetra = new List<Vector3>();
        for (int i = 0; i < 4; i++) tetra.Add(vertices[i]);
    }
    public void GetMeshData(out List<Vector3> vertices, out List<int> triangles, out List<Vector2> uvs)
    {
        vertices = new List<Vector3>();
        triangles = new List<int>();
        uvs = new List<Vector2>();
        for (int i = 0; i < 2; i++)
        {
            for (int j = i + 1; j < 3; j++)
            {
                for (int k = j + 1; k < 4; k++)
                {
                    int triangleStartIndex = triangles.Count;
                    int remain = 6 - i - j - k;
                    vertices.Add(tetra[i]);
                    vertices.Add(tetra[j]);
                    vertices.Add(tetra[k]);
                    triangles.Add(triangleStartIndex);
                    if (Vector3.Dot(Vector3.Cross(tetra[k] - tetra[i], tetra[j] - tetra[i]), tetra[remain] - tetra[i]) > 0)
                    {
                        triangles.Add(triangleStartIndex + 1);
                        triangles.Add(triangleStartIndex + 2);
                    }
                    else
                    {
                        triangles.Add(triangleStartIndex + 2);
                        triangles.Add(triangleStartIndex + 1);
                    }
                }
            }
        }
        for (int i = 0; i < 12; i++)
        {
            // TODO: support uv
            uvs.Add(new Vector2(0.0f, 0.0f));
        }
    }
    /// <summary>
    /// Split a tetrahedron to two tetra parts (fragments). You can invoke TetraPart.Append to merge these fragments.
    /// </summary>
    /// <param name="plane"></param>
    /// <param name="part1"></param>
    /// <param name="part2"></param>
    public void Split(Plane plane, out TetraPart part1, out TetraPart part2)
    {
        part1 = new TetraPart();
        part2 = new TetraPart();
        Vector3 P = plane.point;
        Vector3 normal = plane.normal;
        List<Vector3> intersections = new List<Vector3>();
        Vector3 intersection;
        int[] segments = { 0, 1, 0, 2, 0, 3, 1, 2, 1, 3, 2, 3 };
        for (int j = 0; j < 6; j++)
        {
            Vector3 vertex1 = tetra[segments[2 * j]];
            Vector3 vertex2 = tetra[segments[2 * j + 1]];
            if (ToolFunction.IntersectionForSegmentWithPlane(vertex1, vertex2, P, normal, out intersection))
            {
                intersections.Add(intersection);
            }
        }
        switch (intersections.Count)
        {
            case 0:
                // no intersections. Ignore.
                part1.PushBackTetra(tetra);
                return;
            case 1:
                intersection = intersections[0];
                // 1 intersection (not including two vertices)
                int pindex = 0, nindex = 0;
                List<int> remains = new List<int>();
                for (int i = 0; i < 4; i++)
                {
                    Vector3 v = tetra[i];
                    if (Vector3.Dot(v - intersection, normal) > 0)
                    {
                        pindex = i;
                    }
                    else if (Vector3.Dot(v - intersection, normal) < 0)
                    {
                        nindex = i;
                    }
                    else
                    {
                        remains.Add(i);
                    }
                }
                part1.PushBackTetra(new List<Vector3>{
                        tetra[pindex], tetra[remains[0]], tetra[remains[1]], intersection
                    });
                part2.PushBackTetra(new List<Vector3>{
                        tetra[nindex], tetra[remains[0]], tetra[remains[1]], intersection
                    });
                break;
            case 2:
                intersection = intersections[0];
                List<int> nindices = new List<int>();
                List<int> pindices = new List<int>();
                int remainIndex = 0;
                for (int i = 0; i < 4; i++)
                {
                    Vector3 v = tetra[i];
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
                    part1.PushBackPyramid(new List<Vector3>{
                            tetra[remainIndex], intersections[0], intersections[1], tetra[pindices[0]], tetra[pindices[1]]
                        });
                    part2.PushBackTetra(new List<Vector3>{
                            tetra[remainIndex], intersections[0], intersections[1], tetra[nindices[0]]
                        });
                }
                else
                {
                    part2.PushBackPyramid(new List<Vector3>{
                            tetra[remainIndex], intersections[0], intersections[1], tetra[nindices[0]], tetra[nindices[1]]
                        });
                    part1.PushBackTetra(new List<Vector3>{
                            tetra[remainIndex], intersections[0], intersections[1], tetra[pindices[0]]
                        });
                }
                break;
            case 3:
                intersection = intersections[0];
                nindices = new List<int>();
                pindices = new List<int>();
                for (int i = 0; i < 4; i++)
                {
                    Vector3 v = tetra[i];
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
                    part1.PushBackTriPrism(new List<Vector3>{
                            intersections[0], intersections[1], intersections[2], tetra[pindices[0]], tetra[pindices[1]], tetra[pindices[2]]
                        });
                    part2.PushBackTetra(new List<Vector3>{
                            intersections[0], intersections[1], intersections[2], tetra[nindices[0]]
                        });
                }
                else
                {
                    part2.PushBackTriPrism(new List<Vector3>{
                            intersections[0], intersections[1], intersections[2], tetra[nindices[0]], tetra[nindices[1]], tetra[nindices[2]]
                        });
                    part1.PushBackTetra(new List<Vector3>{
                            intersections[0], intersections[1], intersections[2], tetra[pindices[0]]
                        });
                }
                break;
            case 4:
                intersection = intersections[0];
                nindices = new List<int>();
                pindices = new List<int>();
                for (int i = 0; i < 4; i++)
                {
                    Vector3 v = tetra[i];
                    if (Vector3.Dot(v - intersection, normal) > 0)
                    {
                        pindices.Add(i);
                    }
                    else if (Vector3.Dot(v - intersection, normal) < 0)
                    {
                        nindices.Add(i);
                    }
                }
                part1.PushBackTriPrism(new List<Vector3>{
                        intersections[0], intersections[1], intersections[2], intersections[3], tetra[pindices[0]], tetra[pindices[1]]
                    });
                part2.PushBackTriPrism(new List<Vector3>{
                        intersections[0], intersections[1], intersections[2], intersections[3], tetra[nindices[0]], tetra[nindices[1]]
                    });
                break;
        }
    }
}

public class TetraPart
{
    /// <summary>
    /// A fragment combined by tetrahedra.
    /// </summary>
    public List<Vector3> vertices;
    public List<int> triangles;
    public List<Vector2> uvs;
    public List<Tetrahedron> tetrahedra;
    public TetraPart()
    {
        tetrahedra = new List<Tetrahedron>();
        vertices = new List<Vector3>();
        triangles = new List<int>();
        uvs = new List<Vector2>();
    }
    /// <summary>
    /// Add a tetrahedron to this fragment.
    /// </summary>
    /// <param name="tetra">Tetrahedron given.</param>
    public void PushBackTetra(List<Vector3> tetra)
    {
        if (tetra.Count != 4) return;
        var obj = new Tetrahedron(tetra);
        tetrahedra.Add(obj);
        obj.GetMeshData(out List<Vector3> v, out List<int> t, out List<Vector2> u);
        vertices.AddRange(v);
        for (int i = 0; i < t.Count; i++)
        {
            t[i] += triangles.Count;
        }
        triangles.AddRange(t);
        uvs.AddRange(u);
    }
    /// <summary>
    /// Add a pyramid (5 vertices and 5 planes) to this fragment.
    /// The pyramid should be given as (topVertex, bottomVertex1, bottomVertex2, bottomVertex3, bottomVertex4).
    /// </summary>
    /// <param name="pyramid">Pyramid given.</param>
    public void PushBackPyramid(List<Vector3> pyramid)
    {
        if (pyramid.Count != 5) return;
        Vector3 top = pyramid[0];
        pyramid.RemoveAt(0);
        pyramid = ToolFunction.SortConvexPolygon(pyramid);
        PushBackTetra(new List<Vector3>{
            top, pyramid[0], pyramid[1], pyramid[2]
        });
        PushBackTetra(new List<Vector3>{
            top, pyramid[0], pyramid[3], pyramid[2]
        });
    }
    /// <summary>
    /// Add a triangular prism (6 vertices and 5 planes) to this fragment.
    /// </summary>
    /// <param name="triPrism">Triangular prism given.</param>
    public void PushBackTriPrism(List<Vector3> triPrism)
    {
        if (triPrism.Count != 6) return;
        List<int> plane1 = new List<int>(), plane2 = new List<int>();
        int times = 0;
        for (int i1 = 0; i1 < 6; i1++)
            for (int i2 = i1 + 1; i2 < 6; i2++)
                for (int i3 = i2 + 1; i3 < 6; i3++)
                    for (int i4 = i3 + 1; i4 < 6; i4++)
                        if (ToolFunction.IsCoplanar(triPrism[i1], triPrism[i2], triPrism[i3], triPrism[i4]))
                        {
                            times++;
                            if (times == 1)
                            {
                                plane1.AddRange(new int[] { i1, i2, i3, i4 });
                            } else
                            {
                                plane2.AddRange(new int[] { i1, i2, i3, i4 });
                                goto out_loop;
                            }
                        }
        out_loop:
        List<int> intersect = new List<int>();
        foreach (int i in plane1)
        {
            if (plane2.Contains(i))
            {
                intersect.Add(i);
                plane2.Remove(i);
            }
        }
        foreach (int i in intersect)
        {
            plane1.Remove(i);
        }
        // For ABC-DEF
        // vertices1: ADEB
        List<Vector3> vertices1 = ToolFunction.SortConvexPolygon(new List<Vector3>{
            triPrism[intersect[0]], triPrism[intersect[1]], triPrism[plane1[0]], triPrism[plane1[1]]
        });
        // vertices2: ADFC
        List<Vector3> vertices2 = ToolFunction.SortConvexPolygon(new List<Vector3>{
            triPrism[intersect[0]], triPrism[intersect[1]], triPrism[plane2[0]], triPrism[plane2[1]]
        });
        // tetra F-ABC
        PushBackTetra(new List<Vector3>{
            vertices2[2], vertices1[0], vertices1[3], vertices2[3]
        });
        // tetra F-ABE
        PushBackTetra(new List<Vector3>{
            vertices2[2], vertices1[0], vertices1[3], vertices1[2]
        });
        // tetra F-ADE
        PushBackTetra(new List<Vector3>{
            vertices2[2], vertices1[0], vertices1[1], vertices1[2]
        });
    }
    /// <summary>
    /// Append an tetra part to this.
    /// </summary>
    public void Append(TetraPart t)
    {
        tetrahedra.AddRange(t.tetrahedra);
        int i = triangles.Count;
        triangles.AddRange(t.triangles);
        vertices.AddRange(t.vertices);
        uvs.AddRange(t.uvs);
        for (int j = i; j < triangles.Count; j++)
        {
            triangles[j] += i;
        }
    }
    public void SplitSelf()
    {
        TetraPart newPart = new TetraPart();
        foreach (var t in tetrahedra)
        {
            var v1 = t.tetra[0];
            var v2 = t.tetra[1];
            var v3 = t.tetra[2];
            var v4 = t.tetra[3];
            var mid12 = (v1 + v2) / 2;
            var mid13 = (v1 + v3) / 2;
            var mid14 = (v1 + v4) / 2;
            var mid23 = (v2 + v3) / 2;
            var mid24 = (v2 + v4) / 2;
            var mid34 = (v3 + v4) / 2;
            newPart.PushBackTetra(new List<Vector3>
            {
                v1, mid12, mid13, mid14
            });
            newPart.PushBackTetra(new List<Vector3>
            {
                v2, mid12, mid23, mid24
            });
            newPart.PushBackTetra(new List<Vector3>
            {
                v3, mid13, mid34, mid23
            });
            newPart.PushBackPyramid(new List<Vector3>
            {
                mid13, mid12, mid23, mid14, mid34
            });
            newPart.PushBackTriPrism(new List<Vector3>
            {
                mid14, mid12, mid34, mid23, mid24, v4
            });
        }
        tetrahedra = newPart.tetrahedra;
        triangles = newPart.triangles;
        uvs = newPart.uvs;
        vertices = newPart.vertices;
    }
}

public class ToolFunction : MonoBehaviour
{
    /// <summary>
    /// Given a segment and a plane, calculate the intersection of them. If the segment does not intersect with the plane, return false.
    /// </summary>
    /// <param name="vertex1">Vertex of segment.</param>
    /// <param name="vertex2">Vertex of segment.</param>
    /// <param name="P">A point on the plane.</param>
    /// <param name="normal">Normal of the plane.</param>
    public static bool IntersectionForSegmentWithPlane(Vector3 vertex1, Vector3 vertex2, Vector3 P, Vector3 normal, out Vector3 intersection)
    {
        Vector3 line = vertex2 - vertex1;
        float k = Vector3.Dot(P - vertex1, normal) / Vector3.Dot(line, normal);
        intersection = vertex1 + k * line;
        return (k > 0 && k < 1);
    }
    /// <summary>
    /// Return whether 4 vertices are coplanar.
    /// </summary>
    public static bool IsCoplanar(Vector3 vertex1, Vector3 vertex2, Vector3 vertex3, Vector3 vertex4)
    {
        return Mathf.Abs(Vector3.Dot(Vector3.Cross(vertex3 - vertex1, vertex2 - vertex1), vertex4 - vertex1)) < 1e-5;
    }
    /// <summary>
    /// Return whether vertices are coplanar.
    /// </summary>
    public static bool IsCoplanar(List<Vector3> vertices)
    {
        if (vertices.Count <= 3) return true;
        Vector3 normal = Vector3.Cross(vertices[2] - vertices[0], vertices[1] - vertices[0]);
        for (int i = 3; i < vertices.Count; i++)
        {
            if (Mathf.Abs(Vector3.Dot(vertices[i] - vertices[0], normal)) >= 1e-5) return false;
        }
        return true;
    }
    /// <summary>
    /// Sort an unsorted vertices to a convex polygon (edges are combined by adjacent vertices). 
    /// If the vertices can't build a convex polygon, return an empty list.
    /// Assuming given vertices are coplanar.
    /// </summary>
    public static List<Vector3> SortConvexPolygon(List<Vector3> vertices)
    {
        if (vertices.Count <= 3) return vertices;
        List<int> ans = new List<int>();
        List<Vector3> output = new List<Vector3>();
        int current = 0;
        ans.Add(0);
        while (ans.Count < vertices.Count)
        {
            bool interrupt = true;
            for (int i = 0; i < vertices.Count; i++)
            {
                if (ans.Contains(i)) continue;
                int next = i;
                Vector3 value = new Vector3();
                bool flag = true;
                for (int j = 0; j < vertices.Count; j++)
                {
                    if (j == next || j == current) continue;
                    Vector3 temp = Vector3.Cross(vertices[next] - vertices[current], vertices[j] - vertices[current]);
                    if (Vector3.Dot(value, temp) < 0)
                    {
                        flag = false;
                        break;
                    }
                    value = temp;
                }
                if (flag)
                {
                    ans.Add(next);
                    current = next;
                    interrupt = false;
                }
            }
            if (interrupt)
            {
                return output;
            }
        }
        foreach (int i in ans)
        {
            output.Add(vertices[i]);
        }
        return output;
    }
    public static List<Vector3> MergeVertices(Vector3[] arr, int start, int end)
    {
        List<Vector3> l = new List<Vector3>();
        for (int i = start; i < end; i++)
        {
            if (!l.Contains(arr[i])) l.Add(arr[i]);
        }
        return l;
    }
}
