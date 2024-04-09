using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelControllerScript : MonoBehaviour {

    public GameObject StrawTip;
    public GameObject winScreen;
    public GameObject loseScreen;
    public GameObject playerController;
    public GameObject player;

	// Use this for initialization
	void Start () {
        winScreen.gameObject.SetActive(false);
        loseScreen.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update () {
		
	}

    public void Win()
    {
        StrawTip.gameObject.GetComponent<StrawControllerScript>().StopPulling();
        StrawTip.gameObject.SetActive(false);
        playerController.gameObject.SetActive(false);
        player.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        winScreen.gameObject.SetActive(true);
    }

    public void Lose()
    {
        loseScreen.gameObject.SetActive(true);
    }
}
