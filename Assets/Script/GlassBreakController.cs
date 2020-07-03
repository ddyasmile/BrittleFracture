using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlassBreakController : MonoBehaviour
{
    public Slider energySlider;
    public GameObject canvas;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void startBreaking()
    {
        float energy = energySlider.value;
        Destroy(canvas);

        var mesh3d = new VolumeticMesh3D();
        var tetra = GameObject.Find("Tetra");
        var shaderBase = tetra.GetComponent<ShaderBase>();

        var tetrahedra = shaderBase.tetraPart.tetrahedra;
        for (int i = 0; i < tetrahedra.Count; i++)
        {
            var vertices = tetrahedra[i].tetra;
            mesh3d.addTetrahedron(vertices[0], vertices[1], vertices[2], vertices[3]);
        }

        mesh3d.propagatingCracks(new Vector3(0.5F, 0.5F, 0), new Vector3(0, 0, 1), 0.5F, 0.5F, 0.5F);
    }
}
