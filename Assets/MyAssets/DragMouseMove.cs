using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragMouseMove : MonoBehaviour {

    private Vector3 mousePosition;
    private Rigidbody2D rb;
    private Vector2 direction;
    private float moveSpeed = 100f;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(0))
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            direction = (mousePosition - transform.position).normalized;
            rb.linearVelocity = new Vector2(direction.x * moveSpeed, 0);
        }
        else {
            rb.linearVelocity = Vector2.zero;
        }
    }
}
