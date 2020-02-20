using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    public GameObject cubeLine;

    public static Dictionary<int, GameObject> cubeLineByIndex = new Dictionary<int, GameObject>();
    int globalIndexLast = 0;
    public static int firstIndexAlive = 0;

    public void Start()
    {
        for (int i = 0; i <= 20; i++)
        {
            globalIndexLast = i;
            cubeLineByIndex.Add(globalIndexLast, GenerateCubeLine(i));
        }
    }

    public void SpawnLine()
    {
        //Debug.Log("gloabal index: " + globalIndexLast);
        cubeLineByIndex.Add(globalIndexLast + 1, GenerateCubeLine(cubeLineByIndex[globalIndexLast].transform.position.z + 1.05f));
        globalIndexLast++;
    }

    GameObject GenerateCubeLine(int index)
    {
        List<GameObject> res = new List<GameObject>();
        List<int> existingCols = new List<int>();

        GameObject line = Instantiate(cubeLine, new Vector3(-1.05f, 0, index * 1.05f), Quaternion.identity);

        for (int i = 0; i < 5; i++)
        {
            int materialNumber = Random.Range(0, 5);
            while (existingCols.Contains(materialNumber)){
                materialNumber = Random.Range(0, 5);
            }
            existingCols.Add(materialNumber);

            GameObject cube = line.transform.GetChild(i).gameObject;
            cube.GetComponent<MeshRenderer>().material = GameManager.Instance.materials[materialNumber];
            
            res.Add(cube);
        }

        return line;
    }

    GameObject GenerateCubeLine(float posZ)
    {
        List<int> existingCols = new List<int>();

        GameObject line = Instantiate(cubeLine, new Vector3(-1.05f, 0, posZ), Quaternion.identity);

        for (int i = 0; i < 5; i++)
        {
            int materialNumber = Random.Range(0, 5);
            while (existingCols.Contains(materialNumber))
            {
                materialNumber = Random.Range(0, 5);
            }
            existingCols.Add(materialNumber);

            GameObject cube = line.transform.GetChild(i).gameObject;
            cube.GetComponent<MeshRenderer>().material = GameManager.Instance.materials[materialNumber];
        }

        return line;
    }

    public static void removeFirstLine()
    {
        Destroy(cubeLineByIndex[firstIndexAlive]);
        cubeLineByIndex.Remove(firstIndexAlive);
        firstIndexAlive++;
    }
}
