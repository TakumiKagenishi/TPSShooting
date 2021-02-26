using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shot : MonoBehaviour
{
    public enum ShootMode
    {
        AUTO, 
        SEMIAUTO,
    }

    public bool shootEnabled = true;

    [SerializeField]
    ShootMode shootMode = ShootMode.AUTO;

    [SerializeField]
    int maxAmmo = 100;

    [SerializeField]
    int maxSupplyValue = 50;

    [SerializeField]
    int damage = 10;

    [SerializeField]
    float shootInterval = 0.1f;

    [SerializeField]
    float shootRange = 50;

    [SerializeField]
    float supplyInterval = 0.1f;

    [SerializeField]
    Vector3 muzzleFlashScale;

    [SerializeField]
    GameObject muzzleFlashPrefab;

    [SerializeField]
    GameObject hitEffectPrefab;

    [SerializeField]
    Image ammoGauge;

    [SerializeField]
    Text ammoText;

    [SerializeField]
    Image supplyGauge;

    bool shooting = false;
    bool supplying = false;

    int ammo = 0;
    int supplyValue = 0;

    GameObject muzzleFlash;

    GameObject hitEffect;

    public int Ammo
    {
        set
        {
            ammo = Mathf.Clamp(value, 0, maxAmmo);
            ammoText.text = ammo.ToString("D3");
            float scaleX = (float)ammo / maxAmmo;
            ammoGauge.rectTransform.localScale = new Vector3(scaleX, 1, 1);
        }

        get
        {
            return ammo;
        }
    }

    public int SupplyValue
    {
        set
        {
            supplyValue = Mathf.Clamp(value, 0, maxSupplyValue);
            if(supplyValue >= maxSupplyValue)
            {
                ammo = maxAmmo;
                supplyValue = 0;
            }

            float scaleX = (float)supplyValue / maxSupplyValue;
            supplyGauge.rectTransform.localScale = new Vector3(scaleX, 1, 1);
        }

        get
        {
            return supplyValue;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        InitGun();
    }

    // Update is called once per frame
    void Update()
    {
        if(shootEnabled & ammo > 0 & GetInput())
        {
            StartCoroutine(ShootTimer());
        }

        if(shootEnabled)
        {
            StartCoroutine(SupplyTimer());
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void InitGun()
    {
        Ammo = maxAmmo;
        SupplyValue = 0;
    }

    bool GetInput()
    {
        switch(shootMode)
        {
            case ShootMode.AUTO:
                return
            Input.GetMouseButton(0);

            case ShootMode.SEMIAUTO:
                return
            Input.GetMouseButtonDown(0);
        }

        return false;
    }

    IEnumerator ShootTimer()
    {
        if(! shooting)
        {
            shooting = true;
            if(muzzleFlashPrefab != null)
            {
                if(muzzleFlash != null)
                {
                    muzzleFlash.SetActive(true);
                }

                else
                {
                    muzzleFlash = Instantiate(muzzleFlashPrefab, transform.position, transform.rotation);
                    muzzleFlash.transform.SetParent(gameObject.transform);
                    muzzleFlash.transform.localScale = muzzleFlashScale;
                }
            }

            Shoot();

            yield return new WaitForSeconds(shootInterval);

            if(muzzleFlash != null)
            {
                muzzleFlash.SetActive(false);
            }

            if(hitEffect != null)
            {
                if(hitEffect.activeSelf)
                {
                    hitEffect.SetActive(false);
                }
            }

            shooting = false;
        }

        else
        {
            yield return null;
        }
    }

    IEnumerator SupplyTimer()
    {
        if(!supplying)
        {
            supplying = true;
            SupplyValue++;
            yield return new WaitForSeconds(supplyInterval);
            supplying = false;
        }
    }

    void Shoot()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, shootRange))
        {
            if(hitEffectPrefab != null)
            {
                if(hitEffect != null)
                {
                    hitEffect.transform.position = hit.point;
                    hitEffect.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
                    hitEffect.SetActive(true);
                }

                else
                {
                    hitEffect = Instantiate(hitEffectPrefab, hit.point, Quaternion.identity);
                }
            }

            string tagName = hit.collider.gameObject.tag;
            if(tagName == "Enemy")
            {
                EnemyController enemy = hit.collider.gameObject.GetComponent<EnemyController>();
                enemy.Hp -= damage;
            }
        }

        Ammo--;
    }
}
