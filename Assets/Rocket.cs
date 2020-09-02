using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 175f;
    [SerializeField] float primaryThrust = 15f;
    [SerializeField] AudioClip mainEngineSound = null;
    [SerializeField] AudioClip deathSound = null;
    [SerializeField] AudioClip deathSoundHighlight = null;
    [SerializeField] AudioClip winSound = null;

    [SerializeField] ParticleSystem rocketThrustParticle = null;
    [SerializeField] ParticleSystem successParticle = null;
    [SerializeField] ParticleSystem deathParticle = null;

    Rigidbody rigidBody;
    AudioSource thrusterAudio;

    enum State { Alive, Dying, Transcending }
    [SerializeField] State state = State.Alive;

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
        if (state == State.Alive)
        {
            Thrust();
            Rotate();
        }
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            thrusterAudio.Stop();
            rocketThrustParticle.Stop();
        }
    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * primaryThrust);
        if (!thrusterAudio.isPlaying)
        {
            thrusterAudio.PlayOneShot(mainEngineSound);
        }
        rocketThrustParticle.Play();
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

    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive)
        {
            return;
        }
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("collision OK");
                break;
            case "Finish":
                state = State.Transcending;
                PlayWinEffects();
                Invoke("LoadNextLevel", 1.5f);
                break;
            default:
                state = State.Dying;
                PlayDeathEffects();
                Invoke("RestartLevel", 6f);
                break;
        }
    }

    private void PlayDeathEffects()
    {
        thrusterAudio.Stop();
        thrusterAudio.PlayOneShot(deathSound);
        thrusterAudio.PlayOneShot(deathSoundHighlight);
        deathParticle.Play();
    }

    private void PlayWinEffects()
    {
        thrusterAudio.Stop();
        thrusterAudio.PlayOneShot(winSound);
        successParticle.Play();
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1);
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }
}
