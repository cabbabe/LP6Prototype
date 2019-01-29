using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class M4Script : MonoBehaviour
{
    [Header("Properties")]
    public float damage = 21f;
    public float fireRate = 0.3f;
    public float range = 100f;
    public float impactForce = 30f;
    public float ejectForce = 200f;
    public float spreadFactorNoAim = 0.09f;
    public float spreadFactor = 0.08f;

    public int maxClips = 10;
    public int bulletsPerClip = 25;
    public int bulletsLeft;
    public int currentBullets;
    public int[] clips;
    public int currentClipIndex = 0;

    [SerializeField] private float recoil = 0.0f;
    [SerializeField] private float recoilAiming = -1.0f;
    [SerializeField] private float maxRecoil_x = -20f;
    [SerializeField] private float maxRecoil_y = 20f;
    [SerializeField] private float recoilSpeed = 2f;

    public GameObject ammoText;
    public GameObject magText;

    private Vector3 originalPosition;
    public Vector3 aimPosition;
    public float adsSpeed = 8;
    public enum ShootMode { Auto, Semi }
    public ShootMode shootingMode;
    public GameObject playerCam;

    [Header("Setup")]
    float fireTimer;
    public Transform shootPoint;
    public GameObject hitParticles;
    public GameObject ejectPort;
    public GameObject bulletShell;
    public GameObject bulletImpact;
    public ParticleSystem muzzleFlash;
    public ParticleSystem tracerParticle;
    private bool shootInput;
    private Animator anim;

    [Header("Sound Effects")]
    public AudioClip shootSound;
    public AudioClip NoAmmo;
    private AudioSource _AudioSource;

    [Header("Debug")]
    public bool isReloading;
    public bool isReloadingEmpty;
    public bool isInspecting;
    public bool isAiming = false;
    public bool isFiring;
    public bool isDrawing;
    public bool isMagChecking;

    void Start()
    {
        anim = GetComponent<Animator>();
        _AudioSource = GetComponent<AudioSource>();

        currentBullets = bulletsPerClip;
        bulletsLeft = currentBullets;
        originalPosition = transform.localPosition;

        clips = new int[maxClips];

        for (int i = 0; i < clips.Length; i++)
        {
            clips[i] = bulletsPerClip;
        }
    }

    public void StartRecoil(float recoilParam, float recoilAimingParam, float maxRecoil_xParam, float recoilSpeedParam)
    {
        recoil = recoilParam;
        recoilAiming = recoilAimingParam;
        maxRecoil_x = maxRecoil_xParam;
        recoilSpeed = recoilSpeedParam;
        maxRecoil_y = Random.Range(-maxRecoil_xParam, maxRecoil_xParam);
    }

    void recoiling()
    {
        if (bulletsLeft <= 0)
            return;

        if (isDrawing)
            return;

        if (currentBullets <= 0)
            return;

        if (isReloading)
            return;

        if (recoil > 0f)
        {

            Quaternion maxRecoil = Quaternion.Euler(-maxRecoil_x, maxRecoil_y, 0f);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, maxRecoil, Time.deltaTime * recoilSpeed);
            recoil -= Time.deltaTime;
        }
        else
        {
            recoil = 0f;
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.identity, Time.deltaTime * recoilSpeed / 2);
        }

        if (recoilAiming > -1f)
        {

            Quaternion maxRecoil = Quaternion.Euler(-maxRecoil_x, maxRecoil_y, -1f);
            // Dampen towards the target rotation
            transform.localRotation = Quaternion.Slerp(transform.localRotation, maxRecoil, Time.deltaTime * recoilSpeed);
            recoilAiming -= Time.deltaTime;
        }
        else
        {
            recoilAiming = -1f;
            // Dampen towards the target rotation
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.identity, Time.deltaTime * recoilSpeed / 2);
        }

        if (Input.GetButton("Fire1") && isAiming)
        {
            recoilAiming++;
        }
        else
        {
            recoilAiming = -1;
        }

        if (Input.GetButton("Fire1") && !isAiming)
        {
            recoil++;
        }
        else
        {
            recoil = 0;
        }
    }

    void Update()
    {
        switch (shootingMode)
        {
            case ShootMode.Auto:
                shootInput = Input.GetButton("Fire1");
                break;

            case ShootMode.Semi:
                shootInput = Input.GetButtonDown("Fire1");
                break;
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
                isReloading = false;
        }
        if (Input.GetKeyDown(KeyCode.V) && !isReloading && !isAiming && !isDrawing && !isFiring && !isReloadingEmpty)
        {
            isMagChecking = true;
            anim.CrossFadeInFixedTime("AmmoCheck", 0.0f);
           
        }

        ammoText.SetActive(anim.GetCurrentAnimatorStateInfo(0).IsName("AmmoCheck"));
        ammoText.GetComponent<Text>().text = currentBullets + " / " + bulletsPerClip;

        magText.GetComponent<Text>().text = "Mags left "+ clips.Length;

        if (shootInput)
        {
            if (currentBullets > 0 && isAiming == true)
                FireScoped();
            else if (currentBullets > 0 && isAiming == false)
                FireNoScope();
            else if (clips[currentClipIndex] <= 0)
                CheckReload();

        }

        if (Input.GetMouseButtonDown(1) && !isReloading && !isDrawing && !isReloadingEmpty)
        {
            isAiming = true;
            playerCam.GetComponent<Camera>().fieldOfView = 55;
        }
        else if (Input.GetMouseButtonUp(1) && !isDrawing)
        {
            isAiming = false;
            playerCam.GetComponent<Camera>().fieldOfView = 60;
        }

        if (Input.GetKeyDown(KeyCode.R) && !isAiming && !isDrawing)
        {
            if (!isAiming && bulletsLeft > 0)
            {
                CheckReload();
            }
        }
        else
        {
            if (currentBullets <= 0 && !isReloading)
            {
                if (Input.GetButtonDown("Fire1"))
                    PlayNoAmmoSound();
            }
        }

        if (currentClipIndex >= clips.Length)
        {
            currentClipIndex = 0;
        }

        currentBullets = clips[currentClipIndex];

        if (isDrawing)
        {
            playerCam.GetComponent<Camera>().fieldOfView = 60;
        }

        if (fireTimer < fireRate)
            fireTimer += Time.deltaTime;

        ScopeInOut();
        recoiling();
        Inspect();
    }

    void FixedUpdate()
    {
        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);

        isReloading = info.IsName("Reload");
        isReloadingEmpty = info.IsName("ReloadEmpty");
        isInspecting = info.IsName("Inspect");
        isFiring = info.IsName("Fire");
        isDrawing = info.IsName("Draw");
        anim.SetBool("Aim", isAiming);
    }

    private void ScopeInOut()
    {
        if (isDrawing)
            return;

        if (isInspecting)
            return;

        if (Input.GetButton("Fire2") && !isReloading && !isReloadingEmpty && !isInspecting)
        {
            isAiming = true;
            if (isInspecting) return;
            playerCam.GetComponent<Camera>().fieldOfView = 50;

            transform.localPosition = Vector3.Lerp(transform.localPosition, aimPosition, Time.deltaTime * adsSpeed);

        }
        else
        {
            isAiming = false;
            playerCam.GetComponent<Camera>().fieldOfView = 60;
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, Time.deltaTime * adsSpeed);
        }
    }

    private void FireNoScope()
    {
        if (isReloading)
            return;

        if (isReloadingEmpty)
            return;

        if (isDrawing)
            return;

        if (isAiming == false)
        {
            if (fireTimer < fireRate || currentBullets <= 0 || isReloading)
                return;

            RaycastHit hit;

            Vector3 shootDirection = shootPoint.transform.forward;
            shootDirection.x += Random.Range(-spreadFactorNoAim, +spreadFactorNoAim);
            shootDirection.y += Random.Range(-spreadFactorNoAim, +spreadFactorNoAim);

            if (Physics.Raycast(shootPoint.position, shootDirection, out hit, range))
            {
                Debug.Log(hit.transform.name + " found.");

                GameObject hitParticleEffect = Instantiate(hitParticles, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
                hitParticleEffect.transform.SetParent(hit.transform);
                GameObject bulletHole = Instantiate(bulletImpact, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                bulletHole.transform.SetParent(hit.transform);

                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * impactForce);
                }

                Destroy(hitParticleEffect, 1f);
                Destroy(bulletHole, 2f);

                if (hit.transform.GetComponent<HealthController>())
                {
                    hit.transform.GetComponent<HealthController>().ApplyDamage(damage);
                }
            }

            if (currentBullets >= 1)
            {
                anim.CrossFadeInFixedTime("Fire", 0.08f);
                PlayShootSound();
                isReloading = false;
            }

            GameObject shell = Instantiate(bulletShell, ejectPort.transform.position, ejectPort.transform.rotation);
            shell.GetComponent<Rigidbody>().AddForce(shell.transform.forward * 200f);
            shell.GetComponent<Rigidbody>().AddTorque(transform.right * 500f);

            Destroy(shell, 3f);

            muzzleFlash.Play();
            tracerParticle.Play();

            clips[currentClipIndex]--;

            currentBullets--;

            fireTimer = 0.0f;
        }
    }

    private void FireScoped()
    {
        if (isReloading)
            return;

        if (isDrawing)
            return;

        if (isAiming == true)
        {
            if (fireTimer < fireRate || currentBullets <= 0 || isReloading)
                return;

            RaycastHit hit;

            Vector3 shootDirection = shootPoint.transform.forward;
            shootDirection.x += Random.Range(-spreadFactor, +spreadFactor);
            shootDirection.y += Random.Range(-spreadFactor, +spreadFactor);

            if (Physics.Raycast(shootPoint.position, shootDirection, out hit, range))
            {
                Debug.Log(hit.transform.name + " found.");

                GameObject hitParticleEffect = Instantiate(hitParticles, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
                hitParticleEffect.transform.SetParent(hit.transform);
                GameObject bulletHole = Instantiate(bulletImpact, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));

                bulletHole.transform.SetParent(hit.transform);

                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * impactForce);
                }

                Destroy(hitParticleEffect, 1f);
                Destroy(bulletHole, 2f);

                if (hit.transform.GetComponent<HealthController>())
                {
                    hit.transform.GetComponent<HealthController>().ApplyDamage(damage);
                }
            }

            if (currentBullets >= 1)
            {
                anim.CrossFadeInFixedTime("Fire", 0.08f);
                PlayShootSound();
                isReloading = false;
            }

            GameObject shell = Instantiate(bulletShell, ejectPort.transform.position, ejectPort.transform.rotation);
            shell.GetComponent<Rigidbody>().AddForce(shell.transform.forward * 200f);
            shell.GetComponent<Rigidbody>().AddTorque(transform.right * 500f);

            Destroy(shell, 3f);

            muzzleFlash.Play();
            tracerParticle.Play();

            clips[currentClipIndex]--;

            currentBullets--;

            fireTimer = 0.0f;
        }

    }

    void CheckReload()
    {
        if (clips[currentClipIndex] >= bulletsPerClip) return;
        if (clips.Length <= 1) return;
        else if (isReloading) return;

        DoReload();
    }

    public void Reload()
    {
        if (isDrawing)
            return;

        if (bulletsLeft <= 0) return;

        isReloading = true;
        currentBullets = clips[currentClipIndex];
        RefillAmmo();

    }

    public void ReloadEmpty()
    {
        if (isDrawing)
            return;

        if (isReloading)
            return;

        //if (bulletsLeft <= 0) return;

        RefillAmmo();
    }

    private void DoReload()
    {
        if (isDrawing)
            return;

        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);

        if (isReloading) return;
        if (isReloadingEmpty) return;

        if (currentBullets > 0)
        {
            anim.CrossFadeInFixedTime("Reload", 0.0f);
        }

        if (currentBullets <= 0)
        {
            anim.CrossFadeInFixedTime("ReloadEmpty", 0.0f);
        }


    }

    void RefillAmmo()
    {
        Debug.Log("RefillAmmo");
        if (clips[currentClipIndex] == 0)
        {
            int[] newClips = new int[clips.Length - 1];
            int oldClipIndex = 0;
            int newClipIndex = 0;

            while(newClipIndex < newClips.Length)
            {
                if (clips[oldClipIndex] == 0)
                {
                    Debug.Log(clips[currentClipIndex]);
                    oldClipIndex++;
                    continue;
                }

                newClips[newClipIndex++] = clips[oldClipIndex++];
            }

            if (currentClipIndex > clips.Length)
            {
                currentClipIndex = 0;
            }

            clips = newClips;

            return;
        }

        currentClipIndex++;


        if (currentClipIndex > clips.Length)
        {
            currentClipIndex = 0;
        }

    }

    private void Inspect()
    {
        if (isReloading) return;

        if (isReloadingEmpty) return;

        if (isDrawing) return;

        if (isAiming) return;

        if (isFiring) return;

        if (isInspecting) return;

        if (Input.GetKeyDown(KeyCode.F) && !isAiming)
        {
            isAiming = false;
            isInspecting = true;
            anim.CrossFadeInFixedTime("Inspect", 0.0f);
        }
        else if (Input.GetKeyUp(KeyCode.F))
        {
            isInspecting = false;
        }
    }

    private void PlayShootSound()
    {
        _AudioSource.PlayOneShot(shootSound);
    }

    private void PlayNoAmmoSound()
    {
        _AudioSource.PlayOneShot(NoAmmo);
    }
}
