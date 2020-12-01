using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float firerate = 0.01f;

    public int maxAmmo = 30;
    private int currentAmmo = -1;
    public float reloadTime = 1f;
    private float nextFireTime = 0f;


    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    public Text txtAmmoCount;



    void Start()
    {
        nextFireTime = nextFireTime = 1f / firerate;
        muzzleFlash = gameObject.GetComponentInChildren<ParticleSystem>();
        fpsCam = gameObject.GetComponentInParent<Camera>();
        if (currentAmmo == -1)
            currentAmmo = maxAmmo;
    }
    // Update is called once per frame
    void Update()
    {
        txtAmmoCount.text = currentAmmo.ToString();
        if (currentAmmo <= 0 || Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
            return;
        }
        if (Input.GetButton("Fire1") && (Time.time >= nextFireTime))
        {
            nextFireTime = Time.time + (1f / firerate);
            Shoot();
        }
    }

    void Shoot()
    {
        muzzleFlash.Play();

        currentAmmo--;

        RaycastHit hitInfo;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hitInfo, range))
        {
            target Target = hitInfo.transform.GetComponent<target>();
            if (Target != null)
            {
                Target.TakeDamage(damage);
            }

            GameObject impactParticle = Instantiate( impactEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            Destroy(impactParticle, 1f);
        }
    }

    IEnumerator Reload()
    {
        Debug.Log("Reloading");
        txtAmmoCount.text = "Reloading...";
        yield return new WaitForSeconds(reloadTime);

        Debug.Log("Reload Complete");
        currentAmmo = maxAmmo;
    }
}
