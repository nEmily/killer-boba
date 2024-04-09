using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

public class PlayerControllerScript : MonoBehaviour
{

    public float speed;
    public GameObject player;
    public int forceMultiplier = 1;
    public GameObject strawTip;

    //stuff for LastResort
    public bool lastResort = false;
    public float timeThreshold = .5f;
    public float lastButtonPress;
    public float firstButtonPress;
    public int buttonCount = 0;
    public int buttonCountThreshold = 5;

    private Rigidbody2D rb;
    Animator anim;
    float smooth = 10.0f;
    public GameObject strawTop;
    public GameObject straw;
    public GameObject gameController;

    private Vector2 touchOrigin = -Vector2.one; //Used to store location of screen touch origin for mobile controls.

    void Start()
    {
        rb = player.GetComponent<Rigidbody2D>();
        anim = player.GetComponent<Animator>();
    }

    void Update()
    {
        if (lastResort)
        {
            rb.transform.eulerAngles = new Vector3(0, 0, 0);
            //rb.freezeRotation = true;

#if UNITY_STANDALONE || UNITY_WEBPLAYER
            if(Input.GetKeyDown(KeyCode.DownArrow)) {
                if(Time.time - lastButtonPress < timeThreshold) {
                    lastButtonPress = Time.time;
                    buttonCount ++;
                    if(buttonCount >= buttonCountThreshold) {
                        lastResort = false;
                        rb.freezeRotation = false;

                        strawTip.GetComponent<StrawControllerScript>().PausePulling(2.0f);
                    }
                }
                else {
                    buttonCount = 0;
                }
#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
            // use mobile functions here to calculate lastResort, especially "tapCount" https://docs.unity3d.com/Manual/MobileInput.html
            if (Input.touchCount > 0 && Input.GetTouch(0).tapCount == 2)
            {
                
                lastResort = false;
                rb.freezeRotation = false;

                strawTip.GetComponent<StrawControllerScript>().PausePulling(2.0f);
                    
#endif
            }
            else
            {
                if (Time.time - firstButtonPress > timeThreshold * buttonCountThreshold)
                {
                    straw.gameObject.GetComponent<Rigidbody2D>().MoveRotation(0);
                    player.transform.position = Vector3.MoveTowards(player.transform.position, strawTop.transform.position, 10 * Time.deltaTime);
                    gameController.gameObject.GetComponent<GameControllerScript>().Lose();

                    // TODO: GAME OVER 
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (!lastResort)
        {
            if (player.GetComponent<PlayerScript>().stun)
            {
                StartCoroutine(WaitForStunToEnd());
            }

            float moveHorizontal = 0;
            float moveVertical = 0;
            //Check if we are running either in the Unity editor or in a standalone build.
#if UNITY_STANDALONE || UNITY_WEBPLAYER
            moveHorizontal = Input.GetAxis("Horizontal");
            moveVertical = Input.GetAxis("Vertical");

            //Check if we are running on iOS, Android, Windows Phone 8 or Unity iPhone
#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
            if (Input.touchCount > 0)
            {
                Touch myTouch = Input.touches[0];

                if (myTouch.phase == TouchPhase.Began)
                {
                    touchOrigin = myTouch.position;
                }
                else if (myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0)
                {
                    Vector2 touchEnd = myTouch.position;

                    moveHorizontal = touchEnd.x - touchOrigin.x;
                    moveVertical = touchEnd.y - touchOrigin.y;
                    touchOrigin = -Vector2.one;
                }
            }
#endif

            Vector2 movement = new Vector2(moveHorizontal, moveVertical);
            rb.AddForce(movement * speed);

            if (Mathf.Abs(moveHorizontal) > Mathf.Abs(moveVertical)) // going horizontally
            {
                int hor = anim.GetInteger("horizontal");
                if (hor == 0)
                {
                    if (moveHorizontal < 0) // going to the left 
                    {
                        player.transform.localScale += new Vector3(-.2f, 0, 0);
                        anim.SetInteger("horizontal", -1);
                    }
                } else
                {
                    if (moveHorizontal > 0 && hor == -1) // going to the right
                    {
                        player.transform.localScale += new Vector3(.2f, 0, 0);
                        anim.SetInteger("horizontal", 1);
                    } else if (moveHorizontal < 0 && hor == 1) // going to the left
                    {
                        player.transform.localScale += new Vector3(-.2f, 0, 0);
                        anim.SetInteger("horizontal", -1);
                    }
                }
            }
            else if (Mathf.Abs(moveHorizontal) > Mathf.Abs(moveVertical))
            {
                if (moveVertical > 0) // going up
                {
                    player.transform.rotation = new Quaternion(0, 0, 90, 0);
                }
                else if (moveVertical < 0) // going down
                {
                    player.transform.rotation = new Quaternion(0, 0, -90, 0);
                }
            }




            if ((moveHorizontal > 0 | moveHorizontal < 0) |
               (moveVertical > 0 | moveVertical < 0))
            {
                anim.SetBool("moving", true);
                anim.SetBool("bumped", false);
            }
            else
            {
                anim.SetBool("moving", false);
            }
        }
    }

    IEnumerator WaitForStunToEnd()
    {
        // Wait a frame
        //yield return null;
        // Wait 0.2 seconds
        yield return new WaitForSeconds(1.0f);
        player.GetComponent<PlayerScript>().stun = false;
    }

    public void LastResort()
    {
        if (!lastResort)
        {
            lastResort = true;
            lastButtonPress = Time.time;
            firstButtonPress = Time.time;
        }
    }

}
