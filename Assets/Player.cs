using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody rb;
    Vector3 Accel = new Vector3(0, 0, 1f);
    Vector3 Break = new Vector3(0, 0, -.5f);
    Vector3 TurnLeft = new Vector3(-2f, 0, 0);
    Vector3 TurnRight = new Vector3(2f, 0, 0);
    Vector3 Velocity;
    void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {

        // Speed
        if (Input.GetKey(KeyCode.Z) && rb.velocity.z < 50)
        {
            rb.AddForce(Accel, ForceMode.Impulse);
        }

        if (Input.GetKey(KeyCode.Z) && rb.velocity.z >= 50)
        {
            rb.velocity = Velocity ;
        }

        if (Input.GetKey(KeyCode.S) && rb.velocity.z > 2)
        {
            rb.AddForce(Break, ForceMode.Impulse);
        }

        Velocity = rb.velocity;

        //Turn
        if (Input.GetKey(KeyCode.Q) && rb.velocity.z > 4)
        {
            transform.Translate(Vector3.right * Time.deltaTime * -13);
        }

        if (Input.GetKey(KeyCode.D) && rb.velocity.z > 4)
        {
            transform.Translate(Vector3.right * Time.deltaTime * 13);
        }
    }
}
