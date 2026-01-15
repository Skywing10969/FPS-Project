using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    public float mouseSensitivity = 50f;    //final sensitivity used
    public Transform cam;                   //camera transform (child of player)

    private float xRotation = 0f;           //vertical rotation accumulator
    private Vector2 lookInput;              //from Input system (mouse delta)

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //lock cursor to centre
        Cursor.visible = false;                    //hide cursor
    }

    void OnLook(InputValue value)           //action name "Look" -> OnLook
    {
        lookInput = value.Get<Vector2>();   //read mouse delta (x, y)
    }

    // Update is called once per frame
    void Update()
    {
        HandleMouseLook();                  //apply look every frame   
    }

    void HandleMouseLook()
    {
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;     //yaw
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;     //pitch

        xRotation -= mouseY;        //invert for natural pitch
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); //limit vertical look
        cam.localRotation = Quaternion.Euler(xRotation, 0f, 0f); //pitch on cam

        transform.Rotate(Vector3.up * mouseX); // yaw on player body (Y axis)
    }
}
