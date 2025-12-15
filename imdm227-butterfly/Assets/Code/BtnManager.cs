using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

//attach this to an empty gameObject

public class BtnManager : MonoBehaviour
{
    public GameObject resetBtn;
    public GameObject butterfly;
    float startX;
    float startY;
    float startZ;

    // Update is called once per frame

    void Start()
    {
        SceneManager.activeSceneChanged += ChangedActiveScene;
        //store initial butterfly position
        startX = butterfly.transform.position.x;  
        startY = butterfly.transform.position.y;
        startZ = butterfly.transform.position.z;

    }   
    void Update()
    {
        if (    Mouse.current.leftButton.wasPressedThisFrame
             && Physics.Raycast(
                    Camera.main.ScreenPointToRay(
                        Mouse.current.position.ReadValue()),
                        out RaycastHit hit))
        {
            Debug.Log(" pressed ");
            Collider collider = hit.collider;
            GameObject btn = collider.gameObject;
            Debug.Log(btn.name);

            if (btn == resetBtn) //did mouse click on button?
            {
                Debug.Log("trasnformed: " + startX + " " + startY + " " + startZ);
                butterfly.transform.position = new Vector3(startX, startY, startZ);   //reset position
            }
        }
    }
    private void ChangedActiveScene(Scene current, Scene next) //activates when scene change is detected
    {
        butterfly.transform.position = new Vector3(startX, startY, startZ); //get new position
    }
}
