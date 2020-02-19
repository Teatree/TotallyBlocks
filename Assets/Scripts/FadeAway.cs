using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeAway : MonoBehaviour
{
    Material m;

    void Update()
    {
        foreach(Transform t in transform)
        {
            float decrement = 0.001f;
            transform.localScale = new Vector3(transform.localScale.x - decrement, transform.localScale.y - decrement, transform.localScale.z - decrement);
            m = t.GetComponent<MeshRenderer>().material;
            m.color = new Color(m.color.r, m.color.g, m.color.b, m.color.a-0.01f);

            if (m.color.a <= 0.2f)
            {
                Destroy(gameObject);
            }
        }
    }
}
