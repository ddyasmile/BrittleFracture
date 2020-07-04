using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDebug : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        Vector3[] vertices = mesh.vertices;

        foreach (Vector3 vertex in vertices)
        {
            // Debug.Log(vertex);
            // Debug.Log("to center distance:");
            Debug.Log(Vector3.Magnitude(vertex));
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
