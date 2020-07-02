using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        return mesh;
    }
}
