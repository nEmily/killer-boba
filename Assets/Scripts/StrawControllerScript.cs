using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

public class StrawControllerScript : MonoBehaviour
{

    public float forceSpeed = 10;
    public float drinkingSpeed = .0005f;
    private bool isPulling;
    public GameObject playerController;
    public GameObject player;
    public GameObject teaLevel;
    public GameObject teaBottom;
    public bool enableTeaDecrease = true;
    public GameObject winScreen;
    public GameObject gameController;
    public GameObject straw;
    Animator anim;
    bool triggered = false;
    float timer;

    public void OnTriggerStay2D(Collider2D other) // TODO: only one boba can be suctioned up at a time
    {
        if (isPulling)
        {
            if (other.gameObject.tag == "Player")
            {
                other.transform.position = Vector2.MoveTowards(
                    other.transform.position,
                    transform.position,
                    forceSpeed * Time.deltaTime);

                if (other.transform.position == transform.position)
                {
                    playerController.GetComponent<PlayerControllerScript>().LastResort();
                    anim.SetBool("hard sucked", true);
                }
            }
            else if (other.gameObject.tag == "Boba")
            {
                // TODO: suck the boba up and delete it 
            }
        }
    }

    public void StartPulling()
    {
        isPulling = true;
    }

    public void StopPulling()
    {
        isPulling = false;

    }

    public void PausePulling(float time)
    {
        StopPulling();
        StartCoroutine(PausePullingHelper(time));
        timer = Time.time;
    }

    private IEnumerator PausePullingHelper(float time)
    {
        yield return new WaitForSeconds(time);
        StartPulling();
    }

    // Use this for initialization
    void Start()
    {
        StopPulling();
        anim = player.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPulling && enableTeaDecrease)
        {
            float step = drinkingSpeed * Time.deltaTime;
            teaLevel.transform.position = Vector2.MoveTowards(teaLevel.transform.position, teaBottom.transform.position, step);

            if (teaLevel.transform.position == teaBottom.transform.position) {
                gameController.gameObject.GetComponent<GameControllerScript>().Win();
            }
            float diff = teaLevel.GetComponent<BoxCollider2D>().bounds.max.y - transform.position.y;
            if (diff < 0) {
                straw.gameObject.GetComponent<Rigidbody2D>().MoveRotation(0);
                straw.gameObject.transform.localScale = new Vector3(straw.gameObject.transform.localScale.x, straw.gameObject.transform.localScale.y - (diff/10), straw.gameObject.transform.localScale.z);

            }

            // TODO: teaLevel.transform.position = new Vector2(teaLevel.transform.position.x, teaLevel.transform.position.y - drinkingSpeed);
        }

        if (Time.time - timer >= 2f && !triggered)
            {
                triggered = true;
                straw.gameObject.GetComponent<Rigidbody2D>().MoveRotation(.25f);
                timer = Time.time;
            }

            float rotation = straw.gameObject.GetComponent<Rigidbody2D>().rotation;
            if (Time.time - timer >= 1f && triggered)
            {
                if (rotation > 0) {
                    straw.gameObject.GetComponent<Rigidbody2D>().MoveRotation(-.5f);
                } else {
                    straw.gameObject.GetComponent<Rigidbody2D>().MoveRotation(.5f);
                }

                timer = Time.time;
            }
    }
}

