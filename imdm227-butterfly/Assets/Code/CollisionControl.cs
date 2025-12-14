using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class CollisionControl : MonoBehaviour
{
    //place an invisible win cube at the end of the maze
    //make sure win cube has win tag and spike/enemy has tags
    //add rigidbody to the butterfly
    //attach this script to the butterfly

    float startX;
    float startY;
    float startZ;

    public AudioClip hit1;
    public AudioClip hit2;
    public AudioClip hit3;
    public AudioClip hit4;
    public AudioClip win;


    public GameObject canvas;
    private AudioSource source;

    void Start()
    {
        
        source = GetComponent<AudioSource>();
        canvas.SetActive(false);
        startX = transform.position.x;
        startY = transform.position.y;
        startZ = transform.position.z;

    }

    void OnCollisionEnter(Collision other)
    {
        //Debug.Log("hit");
        if (other.gameObject.tag == "Win")
        {
            source.PlayOneShot(win, 1.0f);
            if (SceneManager.GetActiveScene().buildIndex + 1 < SceneManager.sceneCountInBuildSettings)
            {
                //change scene
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                //get new starting position
                startX = transform.position.x;
                startY = transform.position.y;
                startZ = transform.position.z;
             } 
             else {
                //for this to work you need to add the scenes to the current build,
                //make sure the maze levels are in order because this triggers the 
                //scene with the next index to appear
                canvas.SetActive(true);
            }

        }

        if (other.gameObject.tag == "Spike" || other.gameObject.tag == "Enemy")
        {
            //level fail, reset position
            transform.position = new Vector3(startX, startY, startZ); 

        }

        if (other.gameObject.tag == "wall")
        {
            int rand = Random.Range(1, 5); 
            if (rand == 1)
            {
                source.PlayOneShot(hit1, 1.0f);
            } 
            else if (rand == 2)
            {
                source.PlayOneShot(hit2, 1.0f);
            }
            else if (rand == 3)
            {
                source.PlayOneShot(hit3, 1.0f);
            }
            else if (rand == 4)
            {
                source.PlayOneShot(hit4, 1.0f);
            }
        }

    }
}
