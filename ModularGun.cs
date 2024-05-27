using UnityEngine;

public class ModularGun : MonoBehaviour
{
    public enum GunMode
    {
        Auto,
        Shotgun,
        SemiAuto
    }
    public GunMode gunMode = GunMode.Auto;
    public KeyCode reloadKey = KeyCode.R;
    public KeyCode switchModeKey = KeyCode.M;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float bulletForce = 20f;
    public int magazineSize = 10;
    public int bulletsInMagazine;
    public float shootCooldown = 0.1f;
    public float dissolveTime = 2f;
    private bool canShoot = true;
    private bool isReloading = false;
    private int shotgunSize = 3;
    private GunMode[] firingModes = { GunMode.Auto, GunMode.Shotgun, GunMode.SemiAuto };
    public bool enableModeSwitcher = true;
    private int currentModeIndex = 0;

    private void Start()
    {
        bulletsInMagazine = magazineSize;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && canShoot && bulletsInMagazine > 0 && !isReloading)
        {
            Shoot();
        }

        if (Input.GetKeyDown(reloadKey) && bulletsInMagazine < magazineSize && !isReloading)
        {
            Reload();
        }

        if (enableModeSwitcher && Input.GetKeyDown(switchModeKey))
        {
            SwitchFiringMode();
        }
    }

    private void Shoot()
    {
        switch (gunMode)
        {
            case GunMode.Auto:
                FireAuto();
                break;
            case GunMode.Shotgun:
                FireShotgun();
                break;
            case GunMode.SemiAuto:
                FireSemiAuto();
                break;
        }
    }

    private void FireAuto()
    {
        if (canShoot)
        {
            if (bulletsInMagazine > 0)
            {
                GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
                Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
                bulletRb.AddForce(bulletSpawnPoint.forward * bulletForce, ForceMode.Impulse);
                Destroy(bullet, dissolveTime);
                bulletsInMagazine--;
            }

            if (bulletsInMagazine <= 0)
            {
                Reload();
            }

            canShoot = false;
            Invoke("ResetCanShoot", shootCooldown);
        }
    }

    private void FireShotgun()
    {
        for (int i = 0; i < shotgunSize; i++)
        {
            if (bulletsInMagazine > 0)
            {
                GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
                Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
                bulletRb.AddForce(bulletSpawnPoint.forward * bulletForce, ForceMode.Impulse);
                Destroy(bullet, dissolveTime);
                bulletsInMagazine--;
            }
            else
            {
                break;
            }
        }

        if (bulletsInMagazine <= 0)
        {
            Reload();
        }
    }

    private void FireSemiAuto()
    {
        if (Input.GetMouseButtonDown(0) && canShoot && bulletsInMagazine > 0)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            bulletRb.AddForce(bulletSpawnPoint.forward * bulletForce, ForceMode.Impulse);
            Destroy(bullet, dissolveTime);
            bulletsInMagazine--;

            if (bulletsInMagazine <= 0)
            {
                Reload();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            canShoot = true;
        }
    }

    private void Reload()
    {
        if (!isReloading)
        {
            isReloading = true;

            // Simulate reloading time
            Invoke("FinishReload", 2f);
        }
    }

    private void FinishReload()
    {
        bulletsInMagazine = magazineSize;
        isReloading = false;
    }

    private void ResetCanShoot()
    {
        canShoot = true;
    }

    private void SwitchFiringMode()
    {
        currentModeIndex = (currentModeIndex + 1) % firingModes.Length;
        gunMode = firingModes[currentModeIndex];
    }
}
