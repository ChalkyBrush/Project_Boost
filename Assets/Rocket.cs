using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 175f;
    [SerializeField] float primaryThrust = 15f;

    Rigidbody rigidBody;
    AudioSource thrusterAudio;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        thrusterAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {

        Thrust();
        Rotate();
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * primaryThrust);
            if (!thrusterAudio.isPlaying)
            {
                thrusterAudio.Play();
            }
        }
        else
        {
            thrusterAudio.Stop();
        }
    }

    private void Rotate()
    {
        rigidBody.freezeRotation = true;

        float rotationThisFrame = rcsThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.A) && !(Input.GetKey(KeyCode.D)))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D) && !(Input.GetKey(KeyCode.A)))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
        rigidBody.freezeRotation = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        print("collided");
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("collision OK");
                break;
            case "Fuel":
                print("collision OK");
                break;
            default:
                print("dead");
                break;
        }
    }
}
