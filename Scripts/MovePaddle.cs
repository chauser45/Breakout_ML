using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePaddle : MonoBehaviour
{
    public float speed = 30;
    public string axis = "Horizontal";

    void FixedUpdate(){
        float velocity = Input.GetAxisRaw(axis);
        GetComponent<Rigidbody2D>().velocity = new  Vector2(velocity,0) * speed;
    }
}
