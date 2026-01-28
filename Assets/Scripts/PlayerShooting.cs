using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    public Gun gun;                 //current gun reference
    private bool isHoldingShoot;    //true while left mouse is held
    public Transform gunHolder;     

    void OnShoot()                  //called when shoot action starts
    {
        isHoldingShoot = true;
    }

    void OnShootRelease()           //called when shoot action ends
    {
        isHoldingShoot = false;  
    }
        
    void OnReload()                 //called on reload action trigger
    {
        if (gun != null)
        {
            gun.TryReload();        //reload gun
        }
    }

    public void OnDrop()
    {
        if (gun != null)
        {
            gun.Drop();
            gun = null;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isHoldingShoot && gun != null)
        {
            gun.Shoot();            //fire continuously while held
        }
    }
}
