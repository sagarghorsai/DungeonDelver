using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Grappler : MonoBehaviour, IGadget
{
    public enum eMode { gIdle, gOut, gRetract, gPull }
    [Header("Inscribed")]
    [Tooltip("Speed at which Grappler extends (doubleding Retract mode)")]
    public float grappleSpd = 10;
    [Tooltip("Maximum length that Grappler will reach")]
    public float maxLength = 7.25f;
    [Tooltip("Minimum distance of Grappler from Dray")]
    public float minLength = 0.5f;
    [Tooltip("Health deducted when Dray ends a grapple on an unsafe tile")]
    public int unsafeTileHealthPenalty = 2;

    [Header("Dynamic")]
    [SerializeField]
    private eMode _mode = eMode.gIdle;
    public eMode mode
    {
        get { return _mode; }
        private set { _mode = value; }
    }

    private LineRenderer line;
    private Rigidbody2D rigid;
    private Collider2D colld;

    private Vector3 p0, p1;
    private int facing;
    private Dray dray;
    private System.Func<IGadget, bool> gadgetDoneCallback;

    private Vector2[] directions = new Vector2[] {
 Vector2.right, Vector2.up, Vector2.left, Vector2.down
};
    private Vector3[] dirV3s = new Vector3[] {
 Vector3.right, Vector3.up, Vector3.left, Vector3.down
};

    void Awake()
    {
        line = GetComponent<LineRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        colld = GetComponent<Collider2D>();
    }

    void Start()
    {
        gameObject.SetActive(false);
    }

    void SetGrappleMode(eMode newMode)
    {
        switch (newMode)
        {
            case eMode.gIdle:
                transform.DetachChildren();
                gameObject.SetActive(false);

                if (dray != null && dray.controlledBy == this as IGadget)
                {
                    dray.controlledBy = null;
                    dray.physicsEnabled = true;
                }

                break;

            case eMode.gOut:
                gameObject.SetActive(true);
                rigid.velocity = directions[facing] * grappleSpd;
                break;

            case eMode.gRetract:
                rigid.velocity = -directions[facing] *
               (grappleSpd * 2);
                break;

            case eMode.gPull:
                p1 = transform.position;
                rigid.velocity = Vector2.zero;
                dray.controlledBy = this;
                dray.physicsEnabled = false;
                break;
        }

        mode = newMode;
    }

    void FixedUpdate()
    {
        p1 = transform.position;
        line.SetPosition(1, p1);

        switch (mode)
        {
            case eMode.gOut:
                if ((p1 - p0).magnitude >= maxLength)
                {
                    SetGrappleMode(eMode.gRetract);
                }
                break;

            case eMode.gRetract:

                if (Vector3.Dot((p1 - p0), dirV3s[facing]) < 0)
                    GrappleDone();
                break;

            case eMode.gPull:
                if ((p1 - p0).magnitude > minLength)
                {
                    p0 += dirV3s[facing] * grappleSpd *
                   Time.fixedDeltaTime;
                    dray.transform.position = p0;
                    line.SetPosition(0, p0);
                    transform.position = p1;
                }
                else
                {
                    p0 = p1 - (dirV3s[facing] * minLength);
                    dray.transform.position = p0;
                     Vector2 checkPos = (Vector2)p0 + new
                    Vector2(0, -0.25f); // g
                    if (MapInfo.UNSAFE_TILE_AT_VECTOR2(checkPos))
                    {
                        dray.ResetInRoom(unsafeTileHealthPenalty
                       );
                    }
                    GrappleDone();
                }
                break;
        }
    }

    void LateUpdate()
    {
        p1 = transform.position;
        line.SetPosition(1, p1);
    }
    void OnTriggerEnter2D(Collider2D colld)
    {

        string otherLayer =
       LayerMask.LayerToName(colld.gameObject.layer);

        switch (otherLayer)
        {
            case "Items":
                PickUp pup = colld.GetComponent<PickUp>();
                if (pup == null) return;

                pup.transform.SetParent(transform);
                pup.transform.localPosition = Vector3.zero;
                SetGrappleMode(eMode.gRetract);
                break;

            case "Enemies":
                Enemy e = colld.GetComponent<Enemy>();
                if (e != null) SetGrappleMode(eMode.gRetract
               );
                break;

            case "GrapTiles":
                SetGrappleMode(eMode.gPull);
                break;

            default:
                SetGrappleMode(eMode.gRetract);
                break;
        }
    }
    void GrappleDone()
    {
        SetGrappleMode(eMode.gIdle);
        gadgetDoneCallback(this);
    }




    #region IGadget_Implementation 
    public bool GadgetUse(Dray tDray,
    System.Func<IGadget, bool> tCallback)
    {
        if (mode != eMode.gIdle) return false;

        dray = tDray;
        gadgetDoneCallback = tCallback;
        transform.localPosition = Vector3.zero;

        facing = dray.GetFacing();
        p0 = dray.transform.position;
        p1 = p0 + (dirV3s[facing] * minLength);
        gameObject.transform.position = p1;
        gameObject.transform.rotation =
        Quaternion.Euler(0, 0, 90 * facing);

        line.positionCount = 2;
        line.SetPosition(0, p0);
        line.SetPosition(1, p1);

        SetGrappleMode(eMode.gOut);

        return true;
    }

    public bool GadgetCancel()
    {
        if (mode == eMode.gPull) return false;
        SetGrappleMode(eMode.gIdle);
        gameObject.SetActive(false);
        return true;
    }


    #endregion
}
