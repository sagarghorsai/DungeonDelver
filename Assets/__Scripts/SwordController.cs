using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    private GameObject sword;
    private GameObject magicSwordPrefab;
    private Dray dray;
    private bool isFullHealth;
    private bool magicSwordSpawned;

    public Transform magicSwordSpawnPoint;
    public float magicSwordSpeed = 5f;

    void Start()
    {
        Transform swordT = transform.Find("Sword");
        Transform swordS = transform.Find("magicSword");
        if (swordT == null)
        {
            Debug.LogError("Could not find Sword child of SwordController.");
            return;
        }
        sword = swordT.gameObject;
        magicSwordPrefab = swordS.gameObject;

        dray = GetComponentInParent<Dray>();
        if (dray == null)
        {
            Debug.LogError("Could not find parent component Dray.");
            return;
        }

        sword.SetActive(false);
        magicSwordPrefab.SetActive(false);
        magicSwordSpawned = false; 
    }

    void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, 90 * dray.facing);

        isFullHealth = dray.health == dray.maxHealth;

        sword.SetActive(dray.mode == Dray.eMode.attack);

        if (!magicSwordSpawned && isFullHealth && dray.mode == Dray.eMode.attack)
        {
            magicSwordPrefab.SetActive(true);
            ShootMagicSword();
            magicSwordSpawned = true;
            magicSwordPrefab.SetActive(false);
        }
        else if (!isFullHealth || dray.mode != Dray.eMode.attack)
        {
            magicSwordSpawned = false;
        }
    }

    void ShootMagicSword()
    {
        if (magicSwordPrefab != null && magicSwordSpawnPoint != null)
        {
            // Instantiate magic sword
            GameObject magicSword = Instantiate(magicSwordPrefab, magicSwordSpawnPoint.position, magicSwordSpawnPoint.rotation);

            // Determine shoot direction
            Vector3 shootDirection = magicSwordSpawnPoint.right;

            // Apply velocity to magic sword's Rigidbody
            Rigidbody2D magicSwordRigidbody = magicSword.GetComponent<Rigidbody2D>();
            if (magicSwordRigidbody != null)
            {
                magicSwordRigidbody.velocity = shootDirection * magicSwordSpeed;

                // Destroy magic sword after a certain duration
                StartCoroutine(DestroyMagicSwordDelayed(magicSword));
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
