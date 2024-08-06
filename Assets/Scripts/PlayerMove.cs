using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 10f;

    [SerializeField] private Rigidbody2D rb;

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        Vector2 movement = new Vector2(moveHorizontal, moveVertical).normalized;

        if (rb != null)
        {
            rb.velocity = movement * speed;
        }
        else
        {
            Matrix.Matrix4 trans = new Matrix.Matrix4().Translation2D(speed * moveHorizontal, speed * moveVertical);
            transform.position = new Vector3(transform.position.x + trans.GetTranslation().x * Time.deltaTime, transform.position.y + trans.GetTranslation().y * Time.deltaTime, transform.position.z);
        }
    }
}
