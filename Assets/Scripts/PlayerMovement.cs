using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{

    public AudioClip footStepSFX;

    public float moveSpeed = 5f;    //movement speed
    public float jumpForce = 5f;    //jump impulse

    //Ground check
    public Transform groundCheck; //place at player's feet
    public float groundDistance = 0.4f; //sphere radius for ground test
    public LayerMask groundMask; //set to "Ground" layer in inspector

    private Rigidbody rb;           //player rigidbody
    private Vector2 moveInput;      //WASD/Arrows as (x, y)
    private bool isGrounded;        //true while on ground

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>(); //cache Rigidbody
        StartCoroutine(PlayFootStep());
    }

    // Update is called once per frame
    void Update()
    {
        CheckGround();
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    void OnJump()
    {
        if (isGrounded)
        {
            rb.AddForce(new Vector3(0f, jumpForce, 0f), ForceMode.Impulse); //upwards impulse
        }
    }

    void OnMovement(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void MovePlayer()
    {
        //convert 2d Input to word-space using player right/forward
        Vector3 direction = (transform.right * moveInput.x) + (transform.forward * moveInput.y);
        direction = direction.normalized; //avoid faster diagonal movement
           
        //unity 6 use linearvelocity
        rb.linearVelocity = new Vector3(direction.x * moveSpeed, rb.linearVelocity.y, direction.z * moveSpeed);
    }

    //ground detection using an invisible spher at feet
    void CheckGround()
    {
        if (groundCheck == null)        //safety: require a groundcheck transform
        {
            isGrounded = false;         //not grounded if probe missing
            return;
        }

        //true if sphere overlaps with any collider or groundmask within grounddistance
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    }

    IEnumerator PlayFootStep()
    {
        while (true)
        {
            if (rb.linearVelocity.magnitude > 0.1f && isGrounded)
            {
                AudioManager.Instance.PlaySFX(footStepSFX);
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}
