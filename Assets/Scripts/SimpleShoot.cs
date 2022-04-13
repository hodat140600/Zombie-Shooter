using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SimpleShoot : MonoBehaviour
{

    

    AudioSource gunAS;
    Animator anim;
    public AudioClip shootAC;
    public AudioClip dryFireAC;
    public AudioClip headShotAC;
    public AudioClip shootMetalAC;

    RaycastHit hit;

    public Text currentAmmoText;
    public Text carriedAmmoText;
        


    public ParticleSystem muzzleFlash;
    public ParticleSystem casingBullet;
    public GameObject bloodEffect;
    public GameObject metalBulletHole;

    [Header("Iron Sight")]
    public bool IronSightOn = false;

    public GameObject crosshair;
    Camera mainCam;
    int fovNormal = 60;
    int fovIronSight = 30;
    float smoothZoom = 3f;

   

    [SerializeField]
    float damageGun;
    [SerializeField]
    float headShotDamage = 100f;


    public int currentAmmo = 12;
    public int maxAmmo = 12;
    public int currentCarriedAmmo = 60;
    public int maxCarriedAmmo = 60;

    [SerializeField]
    float rateOfFire;
    float nextFire = 0;

    [SerializeField]
    int weaponRange;

    [SerializeField]
    Transform shootPoint;

    bool isReloading;

    public float shotPower = 100f;

    [Header("Layers Affected")]
    public LayerMask layer;

    void Start()
    {        
        muzzleFlash.Stop();
        casingBullet.Stop();
        gunAS = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        UpdateAmmoUI();
        mainCam = Camera.main;
        
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            IronSightOn = true;
            crosshair.SetActive(false);
            anim.SetBool("IronSightOn", true);
        }
        else if (Input.GetButtonUp("Fire2"))
        {
            IronSightOn = false;
            crosshair.SetActive(true);
            anim.SetBool("IronSightOn", false);
        }
        if (IronSightOn)
        {
            mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, fovIronSight, smoothZoom * Time.deltaTime);
        }
        else if (!IronSightOn)
        {
            mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, fovNormal, smoothZoom * Time.deltaTime);
        }

        if (Input.GetButtonDown("Fire1") && currentAmmo > 0 && !IronSightOn)
        {
            Shoot();
        }
        else if (Input.GetButtonDown("Fire1") && currentAmmo > 0 && IronSightOn)
        {
            ShootIronSight();
        }
        if (Input.GetButtonDown("Fire1") && currentAmmo <= 0)
        {
            DryFire();
        }
        else if (Input.GetKeyDown(KeyCode.R) && currentAmmo <= maxAmmo)
        {
            Reload();
        }
    }

    void Shoot()
    {
        if (!isReloading)
        {
            if (Time.time > nextFire)
            {
                nextFire = 0f;
                nextFire = Time.time + rateOfFire;


                anim.SetTrigger("Shoot");

                currentAmmo--;

                ShootRay();

                UpdateAmmoUI();
            }
        }
    }
    void ShootIronSight()
    {
        if (!isReloading)
        {
            if (Time.time > nextFire)
            {
                nextFire = 0f;
                nextFire = Time.time + rateOfFire;


                anim.SetTrigger("IronSightShoot");

                currentAmmo--;

                ShootRay();

                UpdateAmmoUI();
            }
        }
    }

    void ShootRay()
    {
        if (Physics.Raycast(shootPoint.position, shootPoint.forward, out hit, weaponRange))
        {
            if (hit.transform.tag == "Enemy")
            {
                EnemyHealth enemyHealthScript = hit.transform.GetComponent<EnemyHealth>();
                enemyHealthScript.DeductEnemyHealth(damageGun);
                Instantiate(bloodEffect, hit.point, transform.rotation);
            }
            else if (hit.transform.tag == "Head")
            {
                EnemyHealth enemyHealthScript = hit.transform.GetComponentInParent<EnemyHealth>();
                enemyHealthScript.DeductEnemyHealth(headShotDamage);
                Instantiate(bloodEffect, hit.point, transform.rotation);
                hit.transform.gameObject.SetActive(false);
                gunAS.PlayOneShot(headShotAC);
            }
            else if (hit.transform.tag == "Metal")
            {            
                Instantiate(metalBulletHole, hit.point, Quaternion.FromToRotation(Vector3.up,hit.normal));       
                gunAS.PlayOneShot(shootMetalAC);
            }
            else
            {
                Debug.Log(hit.transform.name);
            }
        }
    }

    void Reload()
    {
        if (!isReloading)
        {
            if (currentCarriedAmmo <= 0) { return; }
            anim.SetTrigger("Reload");
            StartCoroutine(ReloadCoutdown(2f));
        }
    }

    void DryFire()
    {
        if (!isReloading)
        {
            if (Time.time > nextFire)
            {
                nextFire = 0f;
                nextFire = Time.time + rateOfFire;
                anim.SetTrigger("DryFire");

                gunAS.PlayOneShot(dryFireAC);

            }
        }
    }

    public void UpdateAmmoUI()
    {
        currentAmmoText.text = currentAmmo.ToString();
        carriedAmmoText.text = currentCarriedAmmo.ToString();
    }

    IEnumerator ReloadCoutdown(float timer)
    {
        while(timer > 0f)
        {
            isReloading = true;
            timer -= Time.deltaTime;
            
            yield return null;
        }
        if (timer <= 0f)
        {
            int bulletsNeededToFillMag = maxAmmo - currentAmmo;
            int bulletsToDeduct = (currentCarriedAmmo >= bulletsNeededToFillMag) ? bulletsNeededToFillMag : currentCarriedAmmo;

            currentCarriedAmmo -= bulletsToDeduct;
            currentAmmo += bulletsToDeduct;
            isReloading = false;
            UpdateAmmoUI();
        }
    }
   
    IEnumerator PistolSoundAndMuzzleFlash()
    {
        muzzleFlash.Play();
        gunAS.PlayOneShot(shootAC);
        yield return new WaitForEndOfFrame();
        muzzleFlash.Stop();
        
    }

    IEnumerator EjectCasing()
    {
        casingBullet.Play();
        yield return new WaitForEndOfFrame();
        casingBullet.Stop();
    }
}
