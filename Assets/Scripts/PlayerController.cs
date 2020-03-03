using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class PlayerController : MonoBehaviour {

    public static DataController.PlayerData playerData;

    public PlayerState playerState;
    public GameObject cubeReplace;

    public int coolDownCounter;
    public int coolDownTime;
    
    public float feedbackTrickleValue;
    public float feedbackEatValue;
    public List<string> feedbackWords;
    float currentFeedbackVal;

    public Text scoreText;
    public Text m_Text;

    float[] playerPoses = { -2.1f, -1.05f, 0, 1.05f, 2.1f }; 

    FrontTargetGameObject _temp;
    Vector3 clickPos = new Vector3();

    int colorTick;

    int killedLines;

    public void Awake()
    {
        playerData = DataController.LoadPlayer();

        colorTick = 0;
    }

    private void Start()
    {
        currentFeedbackVal = 0f;
        transform.GetComponent<MeshRenderer>().material = GameManager.Instance.PickRandomColour();

        playerState = PlayerState.Passive;
    }

    void Update()
    {
        if (GameManager.Instance.gameState != GameManager.GameState.Active)
            return;

        LoseCheck();
        StateUpdate();
        MoveControls();

        FeedbackTrickleDown();

        //if (posX > -2 && diff < 0.1f)
        //{
        //    posX -= GameManager.Instance.playerCubeSpeed / 100;
        //    transform.position = new Vector3(posX, transform.position.y, transform.position.z);
        //}

        //if (posX < 2 && diff > 0.1f)
        //{
        //    posX += GameManager.Instance.playerCubeSpeed/100;
        //    transform.position = new Vector3(posX, transform.position.y, transform.position.z);
        //}

        //Update the Text on the screen depending on current position of the touch each frame

        FrontTargetGameObject cubes = CubesInFront();

        if (cubes.leftObj != null || cubes.rightObj != null)
        {
            //transform.position = new Vector3(transform.position.x, transform.position.y, cubes.leftObj.transform.position.z - 1.05f);
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y, cubes.leftObj.transform.position.z - 1.05f), 0.1f);
        }

        if (cubes.rightObj != null && cubes.leftObj != null)
        {
            if (cubes.leftObj.transform.GetChild(0).transform.GetComponent<MeshRenderer>().material.name == transform.GetComponent<MeshRenderer>().material.name
                && cubes.rightObj.transform.GetChild(0).transform.GetComponent<MeshRenderer>().material.name == transform.GetComponent<MeshRenderer>().material.name)
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

    private void FeedbackTrickleDown()
    {
        if(currentFeedbackVal>0)
            currentFeedbackVal -= feedbackTrickleValue;

        UIManager.Instance.UpdateFeedbackSliderUI(currentFeedbackVal);
    }

    private void AddFeedbackVal(float val)
    {
        currentFeedbackVal += val;

        if(currentFeedbackVal >= 1)
        {
            currentFeedbackVal = 0;
            UIManager.Instance.DisplayFeedbackMessage(feedbackWords[UnityEngine.Random.Range(0, feedbackWords.Count)]);
        }

        UIManager.Instance.UpdateFeedbackSliderUI(currentFeedbackVal);
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
                t.GetComponent<MeshRenderer>().material = _temp.rightObj.transform.GetChild(0).GetComponent<MeshRenderer>().material;
                Rigidbody r = t.GetComponent<Rigidbody>();
                r.AddForce(Vector3.up * 200, ForceMode.Force);
                r.AddForce(Vector3.forward * 800, ForceMode.Force);

            }

            CubeSpawner.removeFirstLine();
            killedLines++;
            AddFeedbackVal(feedbackEatValue);
            scoreText.text = "" + killedLines;
        }

        //Debug.LogError("you killed the line! space " + killedLines); 
        PickRandomColour();
    }

    void MoveControls()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            if (Input.touchCount > 0)
            {
                Vector3 touch = Input.mousePosition;

                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    clickPos = Input.GetTouch(0).position;
                }
                if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    float per = touch.x / Screen.width * 200;
                    float perC = clickPos.x / Screen.width * 200;
                    float diff = per - perC;
                    m_Text.text = "x : " + diff + "%";

                    float posX = transform.position.x;
                    if (diff > 50)
                        posX = playerPoses[4];
                    if (diff > 10 && diff < 50)
                        posX = playerPoses[3];
                    if (diff > -10 && diff < 10)
                        posX = playerPoses[2];
                    if (diff > -50 && diff < -10)
                        posX = playerPoses[1];
                    if (diff < -50)
                        posX = playerPoses[0];

                    transform.position = new Vector3(posX, transform.position.y, transform.position.z);
                }
            }
        }
        else
        {
            Vector3 mouse = Input.mousePosition;

            if (Input.GetMouseButtonDown(0))
            {
                clickPos = Input.mousePosition;
            }

            if (Input.GetMouseButton(0))
            {
                float per = mouse.x / Screen.width * 200;
                float perC = clickPos.x / Screen.width * 200;
                float diff = per - perC;
                m_Text.text = "x : " + diff + "%";

                float posX = transform.position.x;
                if (diff > 50)
                    posX = playerPoses[4];
                if (diff > 10 && diff < 50)
                    posX = playerPoses[3];
                if (diff > -10 && diff < 10)
                    posX = playerPoses[2];
                if (diff > -50 && diff < -10)
                    posX = playerPoses[1];
                if (diff < -50)
                    posX = playerPoses[0];

                transform.position = new Vector3(posX, transform.position.y, transform.position.z);
            }
        }
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

    void StateUpdate()
    {
        if (playerState == PlayerState.Pushing) 
        {
            //Debug.Log("pushing");
            coolDownCounter++;
        }

        if(playerState == PlayerState.Passive)
        {
            //Debug.Log("passive");
            coolDownCounter = 0;
        }

        if(coolDownCounter > coolDownTime)
        {
            RemoveLine();
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

            SetHighScore();
            GameManager.Instance.Lose(killedLines);

            transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
        else
        {
            Debug.DrawRay(loseRayPos, transform.TransformDirection(Vector3.down) * 4, Color.white);
        }
    }

    public void SetHighScore()
    {
        playerData.highestScore = playerData.highestScore < killedLines ? killedLines : playerData.highestScore;
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

    public static string GenerateID()
    {
        return System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("Player" + UnityEngine.Random.Range(0, 100000000) + DateTime.Now));
    }
}
