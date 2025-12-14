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
        // GameObject start_thing = GameObject.FindGameObjectWithTag("start");
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

            if (btn == resetBtn)
            {
                Debug.Log("trasnformed: " + startX + " " + startY + " " + startZ);
                butterfly.transform.position = new Vector3(startX, startY, startZ); 
            }
        }
    }
    private void ChangedActiveScene(Scene current, Scene next)
    {
        butterfly.transform.position = new Vector3(startX, startY, startZ);
    }
}
