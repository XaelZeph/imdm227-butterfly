using UnityEngine.InputSystem;
using UnityEngine;

public class Mover : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Keyboard.current.leftArrowKey.isPressed)
        {
            transform.position += (Time.deltaTime * new Vector3(-1, 0, 0));
        }

        if (Keyboard.current.rightArrowKey.isPressed)
        {
            transform.position += (Time.deltaTime * new Vector3(1, 0, 0));
        }

        if (Keyboard.current.upArrowKey.isPressed)
        {
            transform.position += (Time.deltaTime * new Vector3(0, 1, 0));
        }
        
        if (Keyboard.current.downArrowKey.isPressed)
        {
            transform.position += (Time.deltaTime * new Vector3(0,-1,0));
        }
    }
}
