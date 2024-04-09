using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceCubeScript : MonoBehaviour {

    public float floatTime = 5f;
    public float scaleSize = -.001f;
    public float normGravScale = -0.5f;
	// Use this for initialization
	void Start () {
        StartCoroutine(StartFloating());
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.localScale.x > 0) 
		{
			transform.localScale += new Vector3(scaleSize, scaleSize, scaleSize);
		}
		else
		{
			gameObject.SetActive(false);
		}
	}

    IEnumerator StartFloating()
    {
        yield return new WaitForSeconds(floatTime);
        this.GetComponent<Rigidbody2D>().gravityScale = normGravScale;
    }
}
