using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public static Weapon Instance;

    public float fireRate = 0;
    public int Damage = 10;
    public LayerMask WhatToHit;

    public Transform BulletTrailPrefab;
    public Transform MuzzleFlashPrefab;
    public Transform hitPrefab;
    float timeToSpawnEffect = 0;
    public float effectSpawnRate = 10;

    //handle camera shaking
    public float camShakeAmt = 0.1f;
    public float camShakeLength = 0.05f;
    CameraShake camShake;

    public string weaponShootSound = "DefaultShot";

    float timeToFire = 0;
    Transform firePoint;

    //caching
    AudioManager audioManager;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        firePoint = FindObjectOfType<Weapon>().transform.Find("FirePoint"); //transform.Find("FirePoint");
        if(firePoint == null)
        {
            Debug.LogError("No Firepoint");
        }
    }

    void Start()
    {
        camShake = GameMaster.gm.GetComponent<CameraShake>();
        if (camShake == null)
            Debug.LogError("no cameraShake script found on gm object");

        audioManager = AudioManager.instance;
        if(audioManager == null)
        {
            Debug.LogError("no audio manager found in scene");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(fireRate == 0)
        {
            if(Input.GetButtonDown("Fire1"))//cia gali buti sudas, pakeisti kaip video parode
            {
                Shoot();
            }
        }
        else
        {
            if(Input.GetButton("Fire1") && Time.time > timeToFire)
            {
                timeToFire = Time.time + 1 / fireRate;
                Shoot();
            }
        }
    }

    void Shoot()
    {
        Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);
        RaycastHit2D hit = Physics2D.Raycast(firePointPosition, mousePosition-firePointPosition, 100, WhatToHit);

        if(hit.collider != null)
        {
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if(enemy != null)
            {
                enemy.DamageEnemy(Damage);
            }
        }

        if (Time.time >= timeToSpawnEffect)
        {
            Vector3 hitPos;
            Vector3 hitNormal;

            if (hit.collider == null)
            {
                hitPos = (mousePosition - firePointPosition) * 9999;
                hitNormal = new Vector3(9999, 9999, 9999);
            }
            else
            {
                hitPos = hit.point;
                hitNormal = hit.normal;
            }


            Effect(hitPos, hitNormal);
            timeToSpawnEffect = Time.time + 1 / effectSpawnRate;
        }
    }

    void Effect(Vector3 hitPos, Vector3 hitNormal)
    {
        Transform trail = Instantiate(BulletTrailPrefab, firePoint.position, firePoint.rotation) as Transform;
        LineRenderer lr = trail.GetComponent<LineRenderer>();

        if(lr != null)
        {
            lr.SetPosition(0, firePoint.position);
            lr.SetPosition(1, hitPos);
        }

        Destroy(trail.gameObject, 0.04f);

        if (hitNormal != new Vector3(9999, 9999, 9999)) 
        {
            Transform hitParticle = (Transform)Instantiate(hitPrefab, hitPos, Quaternion.FromToRotation(Vector3.right, hitNormal));
            Destroy(hitParticle.gameObject, 1f);
        }

        Transform clone = (Transform)Instantiate(MuzzleFlashPrefab, firePoint.position, firePoint.rotation);
        clone.parent = firePoint;
        float size = Random.Range(0.3f, 0.5f);
        clone.localScale = new Vector3(size, size, size);
        Destroy(clone.gameObject, 0.02f);


        //shake the camera
        camShake.Shake(camShakeAmt, camShakeLength);

        //Play shoot sound
        audioManager.PlaySound(weaponShootSound);
    }
}
