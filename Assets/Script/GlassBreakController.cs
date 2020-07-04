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
        // var mat = Matrix3D.initMatrixWithValues(1, 0, 0, 0, 1, 0, 0, 0, 1);

        // Debug.Log(mat.getDeterminant());

        // var newMat = mat.getInverseMatrix().getInverseMatrix();

        // foreach (var value in newMat.values)
        // {
        //     Debug.Log(value);
        // }
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

        for (int i = 0; i < 1; i++)
        {
            float x = Random.Range(0, 1);
            float y = Random.Range(0, 1);
            mesh3d.propagatingCracks(new Vector3(1F, 1F, 0), new Vector3(x, y, 1), 0.5F, 0.9F, 0.9F);
        }

        List<List<int>> fragments = new List<List<int>>();
        fragments = FloodAlgorithm.floodSplit3D(ref mesh3d);

        foreach (int i in mesh3d.damagedNodes)
        {
            Tetrahedron fracTetra = mesh3d.getFractureTetra(i);
            Plane fracPlane = mesh3d.getFracturePlane(i);
            TetraPart part1, part2;
            fracTetra.Split(fracPlane, out part1, out part2);

            int index = tetrahedra.FindIndex(tetr => tetr.Equals(fracTetra));
            /// remove fracTetra from tetrahedra
            /// then append part1 and part2 into tetrahedra
        }
    }
}
