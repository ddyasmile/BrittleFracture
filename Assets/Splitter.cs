using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splitter : MonoBehaviour
{
    private bool cutted = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!cutted && Time.time > 4.0f)
        {
            List<GameObject> gameObjects = new List<GameObject>
            {
                GameObject.Find("Tetra")
            };
            int count = gameObjects.Count;
            for (int i = 0; i < count; i++)
            {
                var o = gameObjects[i];
                GameObject n = o.GetComponent<ShaderBase>().SplitTetrahedron(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(-1.0f, 1.0f, -1.0f));
                if (n != null) gameObjects.Add(n);
            }
            count = gameObjects.Count;
            for (int i = 0; i < count; i++)
            {
                var o = gameObjects[i];
                GameObject n = o.GetComponent<ShaderBase>().SplitTetrahedron(new Vector3(0.0f, 0.0f, 0.5f), new Vector3(0.0f, 1.0f, 1.0f));
                if (n != null) gameObjects.Add(n);
            }
            count = gameObjects.Count;
            for (int i = 0; i < count; i++)
            {
                var o = gameObjects[i];
                GameObject n = o.GetComponent<ShaderBase>().SplitTetrahedron(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(-1.0f, 1.0f, -1.0f));
                if (n != null) gameObjects.Add(n);
            }
            foreach (GameObject n in gameObjects) {
                n.AddComponent<Rigidbody>().AddForce(new Vector3(
                    Random.Range(-50.0f, 50.0f), Random.Range(-50.0f, 50.0f), Random.Range(-50.0f, 50.0f)
                ));
            }
            cutted = true;
        }
    }
}
