using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeAway : MonoBehaviour
{
    Material m;
    public bool d;

    void Update()
    {
        if (d)
        {
            foreach (Transform t in transform)
            {
                float decrement = 0.001f;
                transform.localScale = new Vector3(transform.localScale.x - decrement, transform.localScale.y - decrement, transform.localScale.z - decrement);
                m = t.GetComponent<MeshRenderer>().material;
                m.color = new Color(m.color.r, m.color.g, m.color.b, m.color.a - 0.01f);

                if (m.color.a <= 0.2f)
                {
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            Color c = transform.GetComponent<Image>().color;
            if (c.a >= 0.05f)
            {
                c = new Color(c.r, c.g, c.b, c.a - 0.01f);
                transform.GetComponent<Image>().color = c;
            }
            else
            {
                c = new Color(c.r, c.g, c.b, 0);
                transform.GetComponent<Image>().color = c;
                transform.GetChild(0).GetComponent<Text>().text = "";

                transform.gameObject.SetActive(false);
            }
        }
    }
}
