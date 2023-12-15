using System.Collections;
using UnityEngine;
using TMPro;

public class Weapon : MonoBehaviour
{
    [Header("Shooting Settings")]
    public Transform firePoint; // The position where bullets are spawned
    public LayerMask shootableLayer; // The layer that can be shot

    [Header("Damage Settings")]
    [Range(1f, 100f)]
    public float damage = 10f; // Damage per shot

    [Header("Rate of Fire")]
    [Range(1f, 30f)]
    public float fireRate = 10f; // Shots per second

    [Header("Ammo Settings")]
    [Range(1, 100)]
    public int magazineSize = 30; // Number of bullets in a magazine

    [Header("Reload Settings")]
    [Range(1f, 5f)]
    public float reloadTime = 2f; // Time to reload in seconds

    [Header("UI Elements")]
    public TMP_Text bulletText;

    [Header("Input Settings")]
    public KeyCode reloadKey = KeyCode.R;

    private float timeToFire = 0f;
    private int currentAmmo;
    [HideInInspector] public bool isReloading = false;

    void Start()
    {
        currentAmmo = magazineSize;
    }

    void Update()
    {
        // Display current ammo count or reloading status in UI
        bulletText.text = !isReloading ? (magazineSize + "/" + currentAmmo) : "Reloading";

        // If reloading, do not allow shooting until reload is complete
        if (isReloading)
        {
            return;
        }

        // Check for out-of-ammo or manual reload trigger
        if (currentAmmo <= 0 || (Input.GetKeyDown(reloadKey) && currentAmmo < magazineSize))
        {
            StartCoroutine(Reload());
            return; // Out of ammo or manual reload triggered, start reload
        }

        // Check for shooting input
        if (Input.GetButton("Fire1") && Time.time >= timeToFire)
        {
            timeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        // Raycast to check if we hit something
        RaycastHit hit;
        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, Mathf.Infinity, shootableLayer))
        {
            // Deal damage or apply other effects to the hit object
            Debug.Log("Hit: " + hit.transform.name);
        }

        // Reduce ammo
        currentAmmo--;

        // Add recoil or other shooting effects if needed

        // Play shooting sound or particle effects if needed
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        // Play reload animation or other visual/audio effects

        // Wait for the specified reload time
        yield return new WaitForSeconds(reloadTime);

        // Reset ammo to full and finish reloading
        currentAmmo = magazineSize;
        isReloading = false;
    }
}
