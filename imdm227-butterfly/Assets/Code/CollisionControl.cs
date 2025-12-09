using UnityEngine;
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

    public GameObject canvas;

    void Start()
    {
        canvas.SetActive(false);
        startX = transform.position.x;
        startY = transform.position.y;
        startZ = transform.position.z;

    }
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision other)
    {
        //Debug.Log("hit");
        if (other.gameObject.tag == "Win")
        {
            if (SceneManager.GetActiveScene().buildIndex + 1 < SceneManager.sceneCountInBuildSettings)
            {
                //change scene
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                //get new starting position
                startX = transform.position.x;
                startY = transform.position.y;
                startZ = transform.position.z;
            } else {
                //Debug.Log("win");
                canvas.SetActive(true);
            }

        }

        if (other.gameObject.tag == "Spike" || other.gameObject.tag == "Enemy")
        {
            //level fail, reset position
            transform.position = new Vector3(startX, startY, startZ); 

        }
    }
}
