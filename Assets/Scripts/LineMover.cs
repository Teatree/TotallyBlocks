using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineMover : MonoBehaviour
{
    void Update()
    {
        if(GameManager.Instance.gameState == GameManager.GameState.Active)
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - GameManager.Instance.enemyCubeSpeed);
    }
}
