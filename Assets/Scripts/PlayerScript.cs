using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

public class PlayerScript : MonoBehaviour {
	private Rigidbody2D rb;
	public int sink;
	public bool stun = false;
	public GameObject teaLevel;
    Animator anim;
    // Use this for initialization
    void Start () {
		rb = this.GetComponent<Rigidbody2D>();
        anim = this.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
		/*float diff = teaLevel.GetComponent<BoxCollider2D>().bounds.max.y - transform.position.y;
        if (diff < 0) {
            rb.velocity = Vector2.zero;
        }*/

	}

	public void OnCollisionEnter2D (Collision2D other)
	{
		if (other.gameObject.tag == "Ice") 
		{
			Vector2 knockbackVelocity = new Vector2(0, sink);
 			rb.velocity = -knockbackVelocity;
 			stun = true;
 		} 

        anim.SetBool("bumped", true);
    }
}
