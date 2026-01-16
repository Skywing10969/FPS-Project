using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float time = 0.1f; //lifetime in seconds

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, time); //auto destroy after 'time'
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
