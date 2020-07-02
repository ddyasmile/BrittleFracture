using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ShaderBase : MonoBehaviour
{
    public Material material;

    public TetraPart tetraPart;

    public void FlushTetraPart()
    {
        if (!gameObject.GetComponent<MeshFilter>())
        {
            gameObject.AddComponent<MeshFilter>();
        }
        if (!gameObject.GetComponent<MeshRenderer>())
        {
            gameObject.AddComponent<MeshRenderer>().material = material;
        }
        if (!gameObject.GetComponent<MeshCollider>())
        {
            gameObject.AddComponent<MeshCollider>().convex = true;
        }
        Mesh mesh = gameObject.GetComponent<MeshFilter>().mesh;
        mesh.vertices = tetraPart.vertices.ToArray();
        mesh.uv = tetraPart.uvs.ToArray();
        mesh.triangles = tetraPart.triangles.ToArray();
        mesh.RecalculateNormals();
        gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;
    }
    private void Awake()
    {
        tetraPart = new TetraPart();
    }
    void Start()
    {
        if (GetComponent<MeshRenderer>() != null) return;
        //GameObject gameObject = new GameObject("Tetrahedron");
        //gameObject.transform.position = Vector3.zero;
        FlushTetraPart();
    }
    void Update()
    {
        
    }
}