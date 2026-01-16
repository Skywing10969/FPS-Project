using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 15f;   //bullet speed
    public float lifetime = 3f; //destroy after this time

    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.linearVelocity = -transform.right * speed;   //set forward motion

        Destroy(gameObject, lifetime);                  //auto-destroy after lifetime
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);    //destroy on collision
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
