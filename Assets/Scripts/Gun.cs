using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
    //Firing and reload settings
    public float reloadTime = 1f;
    public float fireRate = 0.15f;
    public int maxSize = 20;        //magazine size

    //Bullet and spawn point
    public GameObject bullet;           //bullet prefab
    public Transform bulletSpawnPoint;  //muzzle / spawn position

    //State
    private int currentAmmo;
    private bool isReloading = false;
    private float nextTimeToFire = 0f;

    //Reload animation (rotation)
    private Quaternion initialRotation;                                 //starting local rotation
    private Vector3 initialPosition;                                    //starting local position
    private Vector3 reloadRotationOffset = new Vector3(66f, 50f, 50f);  //reload tilt

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentAmmo = maxSize;
        initialRotation = transform.localRotation;
        initialPosition = transform.localPosition;
    }

    public void Shoot()
    {
        if (isReloading) return;                //block during reload
        if (Time.time < nextTimeToFire) return; //respect firerate

        if (currentAmmo <= 0)                   //empty mag -> reload
        {
            StartCoroutine(Reload());
            return;
        }

        nextTimeToFire = Time.time + fireRate;  //schedule next shot
        currentAmmo--;

        //spawn bullet at muzzle
        Instantiate(bullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
    }

    IEnumerator Reload()        //simple reload animation
    {
        isReloading = true;

        //tilt from initial -> target -> back to initial
        Quaternion targetRotation = Quaternion.Euler(initialRotation.eulerAngles + reloadRotationOffset);
        float halfReload = reloadTime / 2f;
        float t = 0f;

        while (t < halfReload)      //initial -> target
        {
            t += Time.deltaTime;
            transform.localRotation = Quaternion.Slerp(initialRotation, targetRotation, t / halfReload);
            yield return null;
        }

        t = 0f;
        while (t < halfReload)
        {
            t += Time.deltaTime;
            transform.localRotation = Quaternion.Slerp(targetRotation, initialRotation, t / halfReload);
            yield return null;
        }

        currentAmmo = maxSize;
        isReloading = false;
    }

    public void TryReload()                                     //called on R
    {
        if (isReloading || currentAmmo == maxSize) return;      //skip if busy/full
        StartCoroutine(Reload());                               //start reload
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
