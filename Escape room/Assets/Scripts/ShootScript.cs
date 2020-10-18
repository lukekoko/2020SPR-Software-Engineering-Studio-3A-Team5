using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootScript : MonoBehaviour
{
    public GameObject arCamera;
    public GameObject smoke;
    public int score = 0;
    public int shots = 12;
    public GameObject scoreText, shotsText;
    public bool done = false;

    public GameObject balloonGameUI;

    void Start()
    {
        scoreText.GetComponent<Text>().text = score.ToString();
        shotsText.GetComponent<Text>().text = shots.ToString();
    }
    public void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(arCamera.transform.position, arCamera.transform.forward, out hit) && shots <= 12 && shots > 0 && !done)
        {
            if (hit.transform.name == "balloon1(Clone)" || hit.transform.name == "balloon2(Clone)" || hit.transform.name == "balloon3(Clone)")
            {
                Destroy(hit.transform.gameObject);

                //Instantiates a smoke effect
                Instantiate(smoke, hit.point, Quaternion.LookRotation(hit.normal));
                score++;
                scoreText.GetComponent<Text>().text = score.ToString();
                shots--;
                shotsText.GetComponent<Text>().text = shots.ToString();
            }
        }
        else
        {
            shots--;
            shotsText.GetComponent<Text>().text = shots.ToString();
        }
        if (shots <= 0 || score >= 12)
        {
            // end game
            done = true;
            balloonGameUI.SetActive(false);
        }
    }
}
