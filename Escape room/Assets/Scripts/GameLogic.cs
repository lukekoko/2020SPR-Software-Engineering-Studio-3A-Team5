using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    private ShootScript shootScript;

    public GameObject endScreen;

    public GameObject scoreText, missedText;

    // Start is called before the first frame update
    void Start()
    {
        shootScript = GameObject.Find("ShootScript").GetComponent<ShootScript>();
    }

    // Update is called once per frame
    void Update()
    {
        // check if game can end
        if (shootScript.done)
        {
            // display end screen
            endScreen.SetActive(true);
            scoreText.GetComponent<Text>().text = shootScript.score.ToString();
            missedText.GetComponent<Text>().text = (12 - shootScript.score).ToString();
        }
    }
}
