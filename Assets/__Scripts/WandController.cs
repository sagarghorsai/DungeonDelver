using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandController : MonoBehaviour
{
    private GameObject Wand;
    private GameObject MagicProjectile;
    private Dray dray;
    private bool isFullHealth;
    private bool projectileSpawned;

    public Transform magicSwordSpawnPoint;
    public float magicSwordSpeed = 5f;

    void Start()
    {
        Transform WandT = transform.Find("Wand");
        Transform MagicProjectileS = transform.Find("MagicProjectile");
        if (WandT == null)
        {
            Debug.LogError("Could not find Wand child of WandController.");
            return;
        }
        Wand = WandT.gameObject;
        MagicProjectile = MagicProjectileS.gameObject;

        dray = GetComponentInParent<Dray>();
        if (dray == null)
        {
            Debug.LogError("Could not find parent component Dray.");
            return;
        }

        Wand.SetActive(false);
        MagicProjectile.SetActive(false);
        projectileSpawned = false;
    }

    void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, 90 * dray.facing);

        isFullHealth = dray.health == dray.maxHealth;

        Wand.SetActive(dray.mode == Dray.eMode.attack);

        if (!projectileSpawned &&  dray.mode == Dray.eMode.attack)
        {
            MagicProjectile.SetActive(true);
            ShootMagicWand();
            projectileSpawned = true;
            MagicProjectile.SetActive(false);
        }
        else if (dray.mode != Dray.eMode.attack)
        {
            projectileSpawned = false;
        }
    }

    void ShootMagicWand()
    {
        if (MagicProjectile != null && magicSwordSpawnPoint != null)
        {
            GameObject wand = Instantiate(MagicProjectile, magicSwordSpawnPoint.position, magicSwordSpawnPoint.rotation);
            Vector2 shootDirection = magicSwordSpawnPoint.right;

            Rigidbody2D magicSwordRigidbody = wand.GetComponent<Rigidbody2D>();
            if (magicSwordRigidbody != null)
            {
                magicSwordRigidbody.velocity = shootDirection * magicSwordSpeed;
                StartCoroutine(DestroyMagicSwordDelayed(wand));

            }

        }
    }

    IEnumerator DestroyMagicSwordDelayed(GameObject magicSword)
    {
        yield return new WaitForSeconds(3f);

        if (magicSword != null)
        {
            Destroy(magicSword);
        }
    }
}
