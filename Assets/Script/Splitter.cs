using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splitter : MonoBehaviour
{
    public Material m;
    // Start is called before the first frame update
    void Start()
    {
        List<Vector3> vectors = new List<Vector3>
        {
            new Vector3(0.0f, 0.0f, 0.0f),
            new Vector3(1.0f, 0.0f, 0.0f),
            new Vector3(0.0f, 1.0f, 0.0f),
            new Vector3(0.0f, 0.0f, 1.0f)
        };
        var g = new GameObject("Tetra");
        var sb = g.AddComponent<ShaderBase>();
        sb.tetraPart.PushBackTetra(vectors);
        var t = sb.tetraPart.tetrahedra[0];
        t.Split(new Plane(new Vector3(2.0f, 1.0f, -1.0f), new Vector3(0.0f, 0.5f, 0.0f)), out TetraPart part1, out TetraPart part2);
        GameObject g2 = new GameObject();
        var sb2 = g2.AddComponent<ShaderBase>();
        sb.tetraPart = part1;
        sb.material = m;
        sb.FlushTetraPart();
        g.AddComponent<Rigidbody>();
        sb2.tetraPart = part2;
        // sb2.tetraPart.Append(part1);
        sb2.material = m;
        sb2.FlushTetraPart();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
