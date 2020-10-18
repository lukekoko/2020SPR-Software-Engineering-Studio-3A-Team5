using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject[] balloons;
    public bool active = false;
    private int count = 0;

    private PlaneDetection planeDetection;

    private ShootScript shootScript;


    // Start is called before the first frame update
    void Start()
    {
        planeDetection = GameObject.Find("AR Session Origin").GetComponent<PlaneDetection>();
        shootScript = GameObject.Find("ShootScript").GetComponent<ShootScript>();
    }

    void Update()
    {
        if (planeDetection.startupDone && !active)
        {   
            active = true;
            StartCoroutine(StartSpawning());
        }
    }

    IEnumerator StartSpawning()
    {
        while (count < 4)
        {
            for (int i = 0; i < 3; i++)
            {
                if (shootScript.done)
                    break;
                Instantiate(balloons[i], spawnPoints[i].position, Quaternion.identity);
            }
            if (shootScript.done)
                break;
            yield return new WaitForSeconds(4);
            count++;
        }
    }

}
