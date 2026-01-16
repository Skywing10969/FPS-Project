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

    //Recoil and flash
    public float recoilDistance = 0.1f;
    public float recoilSpeed = 15f;
    public GameObject weaponFlash;

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

        //spawn brief muzzle flash
        Instantiate(weaponFlash, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

        StopCoroutine(nameof(Recoil));          //ensure single recoil
        StartCoroutine(nameof(Recoil));         //run recoil animation
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
        while (t < halfReload)      //target -> initial
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

    IEnumerator Recoil()            //position-based recoil
    {
        Vector3 recoilTarget = initialPosition + new Vector3(0f, 0f, recoilDistance);
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * recoilSpeed;
            transform.localPosition = Vector3.Lerp(initialPosition, recoilTarget, t);
            yield return null;
        }

        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * recoilSpeed;
            transform.localPosition = Vector3.Lerp(initialPosition, recoilTarget, t);
            yield return null;
        }

        transform.localPosition = initialPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
