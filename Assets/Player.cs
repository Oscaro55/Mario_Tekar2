using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public Rigidbody rb;
    public Animator anim;
    public float Accel ;
    public float Break ;
    private float drift;
    public float driftValue;
    public float Turn;
    public float MaxSpeed;
    public bool forward = false;
    private Vector3 VelocityUp;
    public RaycastHit suspension;
    private float distance;
    public float elevation;
    private Vector3 Tup;
    private Vector3 Smooth;
    public GameObject RespawnPoint;
    private float gravity = -20;
    private float t = 0;
    private int check = 0;
    public bool end = false;
    public AudioSource source;
    public Transform Respawn;
    private float coeff = 1;
    public float coeffValue;
    void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Z)) forward = true;
        else forward = false;

        // Speed
        if (Input.GetKey(KeyCode.Z) && rb.velocity.magnitude < MaxSpeed * coeff)
        {

            rb.AddRelativeForce(Vector3.forward * Accel * coeff, ForceMode.Force);
            if (Accel < 425) Accel += 1 * coeff;
          
        }
        
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.S))
        {
            rb.AddRelativeForce(Vector3.forward * Break, ForceMode.Force);
            if ( Accel > 200) Accel -= 3.5f;
          
        }

        if (!forward && Accel > 200)
        {
            Accel -= 2;
            rb.AddRelativeForce(Vector3.forward * Accel, ForceMode.Force);
        }
       
        //Turn

        var sideSpeed = Vector3.Project(rb.GetPointVelocity(transform.position), transform.right).magnitude * Vector3.Dot(rb.GetPointVelocity(transform.position).normalized, transform.right);
        if (Input.GetKey(KeyCode.Q))
        {
            anim.SetBool("Turning_left", true);
            drift = -driftValue;
            rb.AddForce(transform.right * -Turn *sideSpeed, ForceMode.Force);
        }

        if (!Input.GetKey(KeyCode.Q))
        {
            anim.SetBool("Turning_left", false);
        }

        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(transform.right * Turn * sideSpeed, ForceMode.Force);
            anim.SetBool("Turning_right", true);
            drift = driftValue;
        }

        if (!Input.GetKey(KeyCode.D))
        {
            anim.SetBool("Turning_right", false);
        }

        if (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.Q)) drift = 0;
        if (drift != 0)
        {
            MaxSpeed = 50;
            if (Accel > 250)Accel -= 2;
        }
        else MaxSpeed = 55;

        transform.Rotate(0, drift, 0);

        // Suspensions
        SuspensionDetect();

        //Reload
        if (Input.GetKey(KeyCode.Space))
        {
            transform.position = RespawnPoint.transform.position;
            transform.rotation = RespawnPoint.transform.rotation;
            Accel = 200;
            rb.velocity = new Vector3(0,0,0);
        }
        if (Input.GetKey(KeyCode.R))
        {
            transform.position = Respawn.position;
            transform.rotation = Respawn.rotation;
            Accel = 200;
            rb.velocity = new Vector3(0, 0, 0);
            GameObject.Find("Scrollbar").GetComponent<SpeedCounter>().timer = 0;
            GameObject.Find("Scrollbar").GetComponent<SpeedCounter>().StartTime = false;
        }

        // Audio

        source.pitch = rb.velocity.magnitude / 25;
    }

    void SuspensionDetect()
    {
        VelocityUp = new Vector3(0, rb.velocity.y, 0);

        if (Physics.Raycast(transform.position + new Vector3(0, 0, 0.25f), transform.up * -1, out suspension, 3, 3))
        {
           
            if (suspension.transform.CompareTag("Road"))
            {
                // normal
                MeshCollider meshCollider = suspension.collider as MeshCollider;
                Mesh mesh = meshCollider.sharedMesh;
                Vector3[] normals = mesh.normals;
                int[] triangles = mesh.triangles;
                Vector3 n0 = normals[triangles[suspension.triangleIndex * 3 + 0]];
                Vector3 n1 = normals[triangles[suspension.triangleIndex * 3 + 1]];
                Vector3 n2 = normals[triangles[suspension.triangleIndex * 3 + 2]];
                Vector3 baryCenter = suspension.barycentricCoordinate;
                Vector3 interpolatedNormal = n0 * baryCenter.x + n1 * baryCenter.y + n2 * baryCenter.z;
                interpolatedNormal = interpolatedNormal.normalized;
                Tup = transform.up;
                if (Vector3.Angle(Tup, interpolatedNormal) >1)
                {
                    if (t <= 1) t += Time.deltaTime;
                    Smooth = Vector3.Lerp(Tup, interpolatedNormal, t);
                    transform.rotation = Quaternion.FromToRotation(Tup, Smooth) * transform.rotation;
                }
                else t = 0; 
                rb.AddRelativeForce (interpolatedNormal * gravity);
                distance = suspension.distance;
                rb.AddRelativeForce((Vector3.up * ((elevation - distance) * 8)) - (VelocityUp /3), ForceMode.Impulse);
            }
        }
        else
        {
            rb.AddForce(transform.up * gravity * 3);
            transform.Rotate(0.65f, 0, 0);
            MaxSpeed = 50;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Checkpoint"))
        {
            check++;
            var audio = other.gameObject.GetComponent<AudioSource>();
            audio.Play();
            RespawnPoint.transform.position = other.transform.position;
            RespawnPoint.transform.rotation = other.transform.localRotation;
        }
        if (other.gameObject.CompareTag("End"))
        {
            GameObject.Find("Scrollbar").GetComponent<SpeedCounter>().StartTime = true;
            var audio = other.gameObject.GetComponent<AudioSource>();
            RespawnPoint.transform.position = other.transform.position;
            RespawnPoint.transform.rotation = other.transform.localRotation;
            audio.Play();
            if (check >= 5)
            {
                StartCoroutine(finish());
                end = true;
                check = 0;
            }
        }
        if (other.gameObject.CompareTag("Boost"))
        {
            coeff = coeffValue;
            StartCoroutine(Boost());
            print("Bosst");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Accel = 200;
            rb.velocity /= 1.2f;
            var audio = collision.gameObject.GetComponent<AudioSource>();
            audio.Play();
        }
    }

    IEnumerator finish()
    {
        yield return new WaitForSeconds(0.5f);
        end = false;
    }

    IEnumerator Boost()
    {
        yield return new WaitForSeconds(1.5f);
        coeff = 1;
    }
}
