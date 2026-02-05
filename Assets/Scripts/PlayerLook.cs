using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    public static PlayerLook instance;

    private float shakeDuration = 0f;
    private float shakeMagnitude = 0.1f;
    private float shakeFadeSpeed = 1.5f;
    private Vector3 initialCamPos;

    public float mouseSensitivity = 50f;    //final sensitivity used
    public Transform cam;                   //camera transform (child of player)

    private float xRotation = 0f;           //vertical rotation accumulator
    private Vector2 lookInput;              //from Input system (mouse delta)


    //initialize singleton instance
    private void Awake()
    {
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //lock cursor to centre
        Cursor.visible = false;                    //hide cursor

        initialCamPos = cam.localPosition;
    }

    void OnLook(InputValue value)           //action name "Look" -> OnLook
    {
        lookInput = value.Get<Vector2>();   //read mouse delta (x, y)
    }

    // Update is called once per frame
    void Update()
    {
        HandleMouseLook();                  //apply look every frame   
        HandleShake();
    }

    public void AddShake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
    }

    private void HandleShake()
    {
        if (Time.timeScale == 0f)
        {
            cam.localPosition = initialCamPos;
            return;
        }

        if (shakeDuration > 0)
        {
            // offset camera randomly within sphere
            cam.localPosition = initialCamPos + Random.insideUnitSphere * shakeMagnitude;
            // reduce remaining shake time
            shakeDuration -= Time.deltaTime * shakeFadeSpeed;
        }
        else
        {
            // reset camera to original position
            cam.localPosition = initialCamPos;
        }
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
