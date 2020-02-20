using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    public PlayerState playerState;
    public GameObject cubeReplace;

    public int coolDownCounter;
    public int coolDownTime;

    FrontTargetGameObject _temp;

    int colorTick;

    int killedLines;

    public void Awake()
    {
        colorTick = 0;
    }

    private void Start()
    {
        transform.GetComponent<MeshRenderer>().material = GameManager.Instance.PickRandomColour();

        playerState = PlayerState.Passive;
    }

    void Update()
    {
        if (GameManager.Instance.gameState != GameManager.GameState.Active)
            return;

        ButtonDetector();
        LoseCheck();
        MovePlayer();
        StateUpdate();

        FrontTargetGameObject cubes = CubesInFront();

        if (cubes.leftObj != null)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, cubes.leftObj.transform.position.z - 1.05f);
        }
        if (cubes.rightObj != null)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, cubes.rightObj.transform.position.z - 1.05f);
        }

        if (cubes.rightObj != null && cubes.leftObj != null)
        {
            if (cubes.leftObj.transform.GetComponent<MeshRenderer>().material.name == transform.GetComponent<MeshRenderer>().material.name
                && cubes.rightObj.transform.GetComponent<MeshRenderer>().material.name == transform.GetComponent<MeshRenderer>().material.name)
            {
                Debug.Log("It's my brother!");
                
                playerState = PlayerState.Pushing;

                _temp = cubes;

                
                //cubes.rightObj.GetComponent<Collider>().enabled = false;
            }
            else
            {
                playerState = PlayerState.Passive;
            }
        }
    }

    private IEnumerator Blink()
    {
        Color c = transform.GetComponent<MeshRenderer>().material.color;

        transform.GetComponent<MeshRenderer>().material.color = Color.white;
        yield return new WaitForSeconds(0.2f);
        transform.GetComponent<MeshRenderer>().material.color = c;
        yield return new WaitForSeconds(0.2f);
        transform.GetComponent<MeshRenderer>().material.color = Color.white;
        yield return new WaitForSeconds(0.2f);
        transform.GetComponent<MeshRenderer>().material.color = c;
        yield return new WaitForSeconds(0.2f);
        transform.GetComponent<MeshRenderer>().material.color = Color.white;
        yield return new WaitForSeconds(0.2f);
        transform.GetComponent<MeshRenderer>().material.color = c;
        yield return null;
    }

    void RemoveLine()
    {
        Debug.Log("RemoveLine");
        GameManager.Instance.IncreaseSpeed();
        GameManager.Instance.MoveCubeLine();

        if (_temp.rightObj != null)
        {
            GameObject c = Instantiate(cubeReplace, _temp.rightObj.transform.position, _temp.rightObj.transform.rotation);
            foreach (Transform t in c.transform)
            {
                t.GetComponent<MeshRenderer>().material = _temp.rightObj.GetComponent<MeshRenderer>().material;
                Rigidbody r = t.GetComponent<Rigidbody>();
                r.AddForce(Vector3.up * 200, ForceMode.Force);
                r.AddForce(Vector3.forward * 800, ForceMode.Force);

            }

            CubeSpawner.removeFirstLine();
            killedLines++;
            transform.GetChild(0).GetComponent<TextMesh>().text = "" + killedLines;
        }

        //Debug.LogError("you killed the line! space " + killedLines); 
        PickRandomColour();
    }

    void PickRandomColour()
    {
        colorTick++;
        //Debug.Log("colorTick: " + colorTick);
        if (colorTick > 6)
        {
            Material m = transform.GetComponent<MeshRenderer>().material;
            transform.GetComponent<MeshRenderer>().material = GameManager.Instance.PickRandomColour();
            if (m.name != transform.GetComponent<MeshRenderer>().material.name)
            {
                Debug.Log("COROUTINE!");
                StartCoroutine(Blink());
            }
            colorTick = 0;
        }
    }

    void MovePlayer()
    {
        float posX = transform.position.x;
        if (Input.GetButtonDown("LEFT"))
        {
            if (posX > -2)
                posX -= GameManager.Instance.playerCubeSpeed;
            transform.position = new Vector3(posX, transform.position.y, transform.position.z);
        }
        if (Input.GetButtonDown("RIGHT"))
        {
            if(posX < 2)
                posX += GameManager.Instance.playerCubeSpeed;
            transform.position = new Vector3(posX, transform.position.y, transform.position.z);
        }
    }

    void StateUpdate()
    {
        if (playerState == PlayerState.Pushing) 
        {
            Debug.Log("pushing");
            coolDownCounter++;
        }

        if(playerState == PlayerState.Passive)
        {
            Debug.Log("passive");
            coolDownCounter = 0;
        }

        if(coolDownCounter > coolDownTime)
        {
            RemoveLine();
        }
    }

    void ButtonDetector()
    {
        if (Input.GetButtonDown("ENTER"))
        {
            Debug.Log("Enter");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

    }

    void LoseCheck()
    {
        RaycastHit hit;

        Vector3 loseRayPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        if (Physics.Raycast(loseRayPos, transform.TransformDirection(Vector3.down), out hit, 4f))
        {
            Debug.DrawRay(loseRayPos, transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);
            hit.transform.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
            GameManager.Instance.Lose();
        }
        else
        {
            Debug.DrawRay(loseRayPos, transform.TransformDirection(Vector3.down) * 4, Color.white);
        }
    }

    FrontTargetGameObject CubesInFront()
    {
        FrontTargetGameObject targets = new FrontTargetGameObject();

        RaycastHit hit;
        RaycastHit hit2;

        Vector3 rayPosLeft = new Vector3(transform.position.x - 0.4f, transform.position.y, transform.position.z);
        Vector3 rayPosRight = new Vector3(transform.position.x + 0.4f, transform.position.y, transform.position.z);

        if (Physics.Raycast(rayPosLeft, transform.TransformDirection(Vector3.forward), out hit, 4f))
        {
            Debug.DrawRay(rayPosLeft, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            targets.leftObj = hit.transform.gameObject;
        }
        else
        {
            Debug.DrawRay(rayPosLeft, transform.TransformDirection(Vector3.forward) * 4, Color.white);
            targets.leftObj = null;
        }

        if (Physics.Raycast(rayPosRight, transform.TransformDirection(Vector3.forward), out hit2, 4f))
        {
            Debug.DrawRay(rayPosRight, transform.TransformDirection(Vector3.forward) * hit2.distance, Color.yellow);
            targets.rightObj = hit2.transform.gameObject;
        }
        else
        {
            Debug.DrawRay(rayPosRight, transform.TransformDirection(Vector3.forward) * 4, Color.white);
            targets.rightObj = null;
        }

        return targets;
    }

    public enum PlayerState {
        Passive,
        Pushing
    }

    private class FrontTargetGameObject {
        public GameObject leftObj;
        public GameObject rightObj;
    }
}
