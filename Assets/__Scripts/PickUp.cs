using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public enum eType { none, key, health, grappler }
    [Header("Inscribed")]
    public eType itemType;
    private Collider2D colld;
    private const float colliderEnableDelay = 0.5f;

    void Awake()
    {
        colld = GetComponent<Collider2D>();
        colld.enabled = false;
        Invoke(nameof(EnableCollider), colliderEnableDelay
       );
    }

    void EnableCollider()
    {
        colld.enabled = true;
    }

    public GameObject guaranteedDrop { get; set; }
    public int tileNum { get; private set; }

    public virtual void Init(int fromTileNum, int tileX, int
   tileY)
    { 
        tileNum = fromTileNum;

        transform.position = new Vector3(tileX, tileY, 0) +
       MapInfo.OFFSET;
    }
}


