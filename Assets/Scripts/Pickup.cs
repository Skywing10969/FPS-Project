using UnityEngine;
using UnityEngine.InputSystem;

public class Pickup : MonoBehaviour
{
    public Material highlightMaterial;
    private Material[] originalMaterials;
    private MeshRenderer[] meshRenderers;
    public GameObject weaponPrefab;
    public float lookRange = 3f;

    private bool isLookedAt = false;
    private Camera playerCam;
    private PlayerShooting player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        meshRenderers = GetComponentsInChildren<MeshRenderer>();    // gather renderers
        originalMaterials = new Material[meshRenderers.Length];     // cache originals

        for (int i = 0; i < meshRenderers.Length; i++)
        {
            originalMaterials[i] = meshRenderers[i].material;       // store material
        }

        player = FindFirstObjectByType<PlayerShooting>();           // get PlayerShooting

        playerCam = player.GetComponentInChildren<Camera>();                  // get player camera
    }

    // Update is called once per frame
    void Update()
    {
        // ray from camera forward (what the player is looking at)
        Ray ray = new Ray(playerCam.transform.position, playerCam.transform.forward);

        // check hit within lookrange
        if (Physics.Raycast(ray, out RaycastHit hit, lookRange))
        {
            if (hit.collider.GetComponentInParent<Pickup>() == this)
            {
                if (!isLookedAt) SetLookedAt(true);                     // turn highlight on
                return;                                                 // stop checking further 
            }
        }

        if (isLookedAt) SetLookedAt(false);
    }

    void SetLookedAt(bool lookedAt)
    {
        isLookedAt =lookedAt;                                       // update flag
        if (lookedAt)
        {
            foreach (MeshRenderer mr in meshRenderers)
                mr.material = highlightMaterial;                    // upply highlight
        }
        else
        {
            for(int i = 0; i < meshRenderers.Length;i++)
                meshRenderers[i].material = originalMaterials[i];      // restore originals
        }
    }

    public void OnPickUp()                                              // called by input action pickup
    {
        if (isLookedAt) return;                                         // only when aiming at item

        //if (player.gun != null)                                         // remove current gun
        player.OnDrop();

        // spawn weapon under GunHolder (view model)
        GameObject newWeapon = Instantiate(weaponPrefab, player.gunHolder);
        newWeapon.transform.localPosition = Vector3.zero;               // reset local pos
        newWeapon.transform.localRotation = Quaternion.identity;        // reset local rot

        player.gun = newWeapon.GetComponent<Gun>();                     // cache new gun
        Destroy(gameObject);                                            // remove dropped item
    }
}
