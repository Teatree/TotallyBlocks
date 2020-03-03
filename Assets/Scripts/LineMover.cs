using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineMover : MonoBehaviour
{
    void Update()
    {
        if (GameManager.Instance.gameState == GameManager.GameState.Active || GameManager.Instance.gameState == GameManager.GameState.Lost)
        {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - GameManager.Instance.enemyCubeSpeed);
            foreach (Transform t in transform)
            {
                t.GetChild(0).GetComponent<Animator>().SetInteger("aniRando", 4);
            }
        }
        else {
            foreach (Transform t in transform)
            {
                int c = Random.Range(0, 3);
                t.GetChild(0).GetComponent<Animator>().SetInteger("aniRando", c);
            }
        }

        LoseCheck();
    }

    void LoseCheck()
    {
        RaycastHit hit;

        Vector3 loseRayPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        if (Physics.Raycast(loseRayPos, transform.TransformDirection(Vector3.down), out hit, 4f))
        {
            Debug.DrawRay(loseRayPos, transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);

            foreach (Transform t in transform)
            {
                t.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            }
        }
        else
        {
            Debug.DrawRay(loseRayPos, transform.TransformDirection(Vector3.down) * 4, Color.white);
        }
    }
}
