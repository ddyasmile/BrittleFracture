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

        List<Tetrahedron> fracTetras;
        List<Plane> fracPlanes;
        List<TetraPart> tetraParts = mesh3d.getTetraParts(fragments, out fracTetras, out fracPlanes);

        int frag;
        if (tetraParts[0].tetrahedra.Count == 0)
            frag = 1;
        else
            frag = 0;
        
        Vector3 node = tetraParts[frag].tetrahedra[0].tetra[0];
        for (int i = 0; i < fracTetras.Count; i++)
        {
            TetraPart part1, part2;
            fracTetras[i].Split(fracPlanes[i], out part1, out part2);
            if (Vector3.Dot((node - fracPlanes[i].point), fracPlanes[i].normal) > 0)
            {
                tetraParts[frag].Append(part1);
                tetraParts[1 - frag].Append(part2);
            }
            else
            {
                tetraParts[frag].Append(part2);
                tetraParts[1 - frag].Append(part1);
            }
        }

        GameObject tetraGameObject = GameObject.Find("Tetra");
        var sb = tetraGameObject.GetComponent<ShaderBase>();
        sb.tetraPart = tetraParts[0];
        sb.FlushTetraPart();

        GameObject newGameObject = new GameObject();
        var sb2 = newGameObject.AddComponent<ShaderBase>();
        sb2.material = sb.material;
        sb2.tetraPart = tetraParts[1];
        sb2.FlushTetraPart();

        tetraGameObject.AddComponent<Rigidbody>();
        newGameObject.AddComponent<Rigidbody>();
    }
}
