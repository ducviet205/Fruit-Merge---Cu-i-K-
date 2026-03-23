using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragFingerMove : MonoBehaviour
{
    public bool held;
    public float startPox;
    public float startPoy;
    public Vector2 b;
    private void Update()
    {
        
            if (held)
            {
                Vector3 mousePos;
                mousePos = Input.mousePosition;
                mousePos = Camera.main.ScreenToWorldPoint(mousePos);

                this.gameObject.transform.localPosition = new Vector3(mousePos.x - startPox, 3, 0);
            }
        
    }


    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            startPox = mousePos.x  - this.transform.localPosition.x;
            //startPoy = mousePos.y - this.transform.localPosition.y;

            held = true;
        }
    }

    private void OnMouseUp()
    {
        held = false;
    }
}

