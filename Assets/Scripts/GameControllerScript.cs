using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerScript : MonoBehaviour {

    public GameObject StrawTip;
    public GameObject winScreen;
    public GameObject loseScreen;
    public GameObject playerController;
    public GameObject player;

    public GameObject iceCube;
    public GameObject teaLevel;
    public Vector2 spawnPoint;
    public int numIceCubes;


	// Use this for initialization
	void Start () {
        winScreen.gameObject.SetActive(false);
        loseScreen.gameObject.SetActive(false);

        StartCoroutine(WaitForGameStart());
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void Win()
    {
        StrawTip.gameObject.GetComponent<StrawControllerScript>().StopPulling();
        StrawTip.gameObject.SetActive(false);
        playerController.gameObject.SetActive(false);
        winScreen.gameObject.SetActive(true);
    }

    public void Lose()
    {
        loseScreen.gameObject.SetActive(true);

        Time.timeScale = 0;

    }


    private IEnumerator WaitForGameStart()
    {
        yield return new WaitForSeconds(3f);
        teaLevel.gameObject.SetActive(true);
        playerController.gameObject.SetActive(true);

        StrawTip.GetComponent<StrawControllerScript>().StartPulling();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
