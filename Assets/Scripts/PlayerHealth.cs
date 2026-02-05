using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health = 100;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Damage"))
        {
            DecreaseHealth(10);
        }
    }

    private void DecreaseHealth(int amount)
    {
        health -= amount;
        CheckDeath();

        PlayerLook.instance.AddShake(0.1f, 0.25f);
        UIManager.instance.InstantiateHitUI();
    }

    private void CheckDeath()
    {
        if (health <= 0)
        {
            Die();
        }    
    }

    private void Die()
    {
        Time.timeScale = 0f;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
