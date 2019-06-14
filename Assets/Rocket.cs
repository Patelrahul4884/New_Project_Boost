
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip Death;
    [SerializeField] AudioClip nextLevel;

    [SerializeField] ParticleSystem mainEngineParticle;
    [SerializeField] ParticleSystem DeathParticle;
    [SerializeField] ParticleSystem nextLevelParticle;

    Rigidbody rigidBody;
    AudioSource audioSource;
    

	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
	}
    enum State { Alive, Dead, Transanding }
    State state = State.Alive;
    // Update is called once per frame
    void Update () {
        if (state == State.Alive)
        {
            Thrust();
            Rotate();
        }
	}
   
    void OnCollisionEnter(Collision collision)
    {
        if(state!=State.Alive)
        {
           
            return;
        }
        switch (collision.gameObject.tag)
        {
            case "Friendly":
              //  Scene scene = SceneManager.GetActiveScene();
              //  SceneManager.LoadScene(scene.name);
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }
    private void StartSuccessSequence()
    {
        audioSource.PlayOneShot(nextLevel);
        audioSource.Stop();
        nextLevelParticle.Play();
        state = State.Transanding;
        Invoke("LoadNextLevel", 1f);
    }
    private void StartDeathSequence()
    {
        state = State.Dead;
        audioSource.Stop();
        DeathParticle.Play();
        mainEngineParticle.Stop();
        audioSource.Stop();
        audioSource.PlayOneShot(Death);
        Invoke("LoadFirstLevel", 1f);
        // kill player
    }

   

    private  void LoadNextLevel()
    {
        SceneManager.LoadScene(1);
    }
    private  void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space)) // can thrust while rotating
        {
            rigidBody.AddRelativeForce(Vector3.up * mainThrust);
            if (!audioSource.isPlaying) // so it doesn't layer
            {
                audioSource.PlayOneShot(mainEngine);
                mainEngineParticle.Play();
                
            }
        }
        else
        {
            audioSource.Stop();
            mainEngineParticle.Stop();
        }
    }

    private void Rotate()
    {
        rigidBody.freezeRotation = true; // take manual control of rotation
       
        float rotationThisFrame = rcsThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false; // resume physics control of rotation
    }
}