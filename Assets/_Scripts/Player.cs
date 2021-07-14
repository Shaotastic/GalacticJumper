using System;
using DG.Tweening;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum Direction
    {
        Clockwise = 0,
        CounterClockwise
    }

    [SerializeField] private bool m_AutoMode;

    public Planet m_SelectedPlanet;
    public Transform m_PreviousPlanet;
    public float speed = 100;
    public float maxSpeed = 1;
    public float fireSpeed;
    public float radius;
    public float gravForce;
    public GameObject m_ExplosionParticle, m_SmokeParticle;
    [SerializeField] ObjectPool m_CircularSmoke;
    [SerializeField] private Transform m_CirculatSmokePosition;

    private Direction m_Direction;
    private static Direction m_TheDirection;
    Rigidbody rigid;
    [SerializeField] GameObject m_MeshRenderer;
    public Vector3 theDir;
    Vector2 prevVelocity;
    float m_TimeScore = 0;
    public float angle = 0;
    [SerializeField] bool playerStart;
    bool m_EnableTimeScore;

    PlayerSound m_Sound;
    ExplosionAnimation m_ExplosionAni;

    //[SerializeField] Planet m_CurrentPlanet;
    [SerializeField] bool canFire;

    // Use this for initialization
    void OnEnable()
    {
        Reset();
        float startSpeed = speed;
        speed *= 2;
        DOTween.To(() => speed, x => speed = x, startSpeed, 1f);
    }

    void Start()
    {
        prevVelocity = new Vector2();
        rigid = GetComponent<Rigidbody>();
        m_Sound = GetComponent<PlayerSound>();
        m_ExplosionAni = m_ExplosionParticle.GetComponent<ExplosionAnimation>();
        IsDead = false;
        playerStart = false;
        m_Direction = Direction.Clockwise;
        AdManager.Instance.AdFinished += AdReset;
        GameManager.Instance.OnReset += FullReset;
        m_CircularSmoke.transform.parent = null;
        Input.simulateMouseWithTouches = true;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        switch (GameManager.Instance.m_Pause)
        {
            case false:
                if (GameManager.Instance.m_IsPlaying)
                {
                    DeathChecks();

                    if (!IsDead)
                    {
                        if (m_EnableTimeScore)
                        {
                            if (m_TimeScore < 0)
                                m_TimeScore = 0;

                            m_TimeScore -= Time.fixedDeltaTime * 3;
                        }
                        switch (m_AutoMode)
                        {
                            case false:
                                if (!IsDead && canFire && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)))
                                    Fire();
                                break;

                            case true:
                                AutoMode();
                                break;
                        }
                    }
                }
                break;
            case true:
                break;
        }


    }

    void FixedUpdate()
    {
        switch (GameManager.Instance.m_Pause)
        {
            case false:
                if (m_SelectedPlanet != null && !IsDead)
                {
                    MovementCentripetal();
                }
                break;
            case true:
                break;
        }
    }

    void MovementCentripetal()
    {
        Vector3 diff = m_SelectedPlanet.transform.position - transform.position;

        Vector3 direction = new Vector3(-diff.y, diff.x) * radius / Mathf.Sqrt(Mathf.Pow(diff.x, 2) + Mathf.Pow(diff.y, 2));

        if (this.m_Direction == Direction.Clockwise)
            theDir = direction;
        else
            theDir = -direction;

        rigid.velocity = (new Vector2(theDir.x, theDir.y) + new Vector2(diff.x, diff.y)) * (speed * GameManager.Instance.GetSpeedMultiplier()) * Time.fixedDeltaTime;
        angle = Mathf.Atan2(theDir.y, theDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void Dead(bool enableExplode)
    {
        if (!IsDead && playerStart)
        {
            IsDead = true;
            GameManager.Instance.AddDeath();
            m_Sound.PlayDeathSound();
            rigid.velocity = Vector3.zero;
            if (enableExplode)
                m_ExplosionAni.StartSequence();
            m_MeshRenderer.SetActive(false);
            m_SmokeParticle.SetActive(false);
        }
    }

    void Fire()
    {
        if (!playerStart)
            playerStart = true;

        RemoveSelectedPlanet();

        rigid.velocity *= fireSpeed;

        if (canFire)
            m_Sound.PlayJumpSound();

        canFire = false;
        m_CircularSmoke.transform.position = m_CirculatSmokePosition.position;
        m_CircularSmoke.GetPooledObject().SetActive(true);
    }

    private void RemoveSelectedPlanet()
    {
        m_PreviousPlanet = m_SelectedPlanet.transform;
        m_SelectedPlanet.RemovePlayer();
        m_SelectedPlanet = null;
    }

    bool enablePlanetAngleGizmo;

    Vector3 huh, m_DebugPlanetVector, cross;

    void OnTriggerEnter(Collider col)
    {
        switch (col.tag)
        {
            case "Planet":

                prevVelocity = rigid.velocity;

                SetNewPlanet(col);

                CalculatePlanetAngles(col.transform);

                canFire = true;

                break;

            case "Dead":
                Dead(true);
                break;
        }
    }

    private void SetNewPlanet(Collider col)
    {
        m_SelectedPlanet = col.GetComponent<Planet>();

        if (!m_SelectedPlanet.WasVisited && playerStart)
        {
            m_SelectedPlanet.Visited(this);
            m_Sound.PlayLandSound();
            GameManager.Instance.AddScore();
        }
    }

    void OnTriggerExit(Collider col)
    {
        enablePlanetAngleGizmo = false;
        m_EnableTimeScore = false;
        canFire = false;
    }

    public Transform GetObject()
    {
        if (m_SelectedPlanet)
            return m_SelectedPlanet.transform;
        return null;
    }

    void OnDrawGizmos()
    {
        if (m_SelectedPlanet != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, m_SelectedPlanet.transform.position);
            Gizmos.color = Color.white;
            Gizmos.DrawLine(transform.position, transform.position + (new Vector3(theDir.x, theDir.y).normalized * 20));

            if (enablePlanetAngleGizmo)
            {
                //Gizmos.color = Color.white;
                Gizmos.DrawLine(transform.position, transform.position + (new Vector3(prevVelocity.x, prevVelocity.y).normalized * 2));

                if (m_SelectedPlanet)
                {
                    Gizmos.color = Color.green;
                    //Debug.Log(obj.position);
                    Gizmos.DrawLine(m_SelectedPlanet.transform.position, m_DebugPlanetVector);
                    Gizmos.color = Color.blue;
                    Gizmos.DrawLine(m_SelectedPlanet.transform.position, (m_SelectedPlanet.transform.position + huh));
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawLine(m_SelectedPlanet.transform.position, (m_SelectedPlanet.transform.position + cross));
                }
            }
        }
    }

    void AutoMode()
    {
        if (canFire && angle < 2 && angle > -2)
            Fire();
    }

    public bool IsDead { get; private set; }

    void DeathChecks()
    {
        if (playerStart && !IsDead)
        {
            Vector3 bound = Camera.main.WorldToScreenPoint(transform.position);

            if (m_SelectedPlanet == null && bound.y < 0 || bound.y > Camera.main.pixelRect.yMax + 10)
                Dead(true);

            if (m_SelectedPlanet == null && bound.x > Camera.main.pixelRect.xMax + 10)
            {
                Dead(true); //Bounce(bound);
            }
            else if (m_SelectedPlanet == null && bound.x < Camera.main.pixelRect.xMin - 10)
            {
                m_ExplosionParticle.transform.localRotation = Quaternion.Euler(0, 0, 90);
                Dead(true);
            }
        }
    }

    public void Reset()
    {
        IsDead = false;
        if (m_ExplosionAni)
            m_ExplosionAni.StopSequence();
        m_SmokeParticle.SetActive(true);
        m_MeshRenderer.SetActive(true);
        m_PreviousPlanet = null;
        playerStart = false;
        m_AutoMode = false;
        m_TimeScore = 0;
    }

    public void AdReset()
    {
        transform.position = m_PreviousPlanet.position + new Vector3(1, 1);
        Reset();
    }

    private void ResetPosition()
    {
        transform.position = new Vector3(0, 1);
    }

    public void FullReset()
    {
        ResetPosition();
        Reset();
    }

    private void CalculatePlanetAngles(Transform newPlanet)
    {
        Vector3 prevHalf = Vector3.zero, newHalf;

        if (m_PreviousPlanet)
        {
            prevHalf = m_PreviousPlanet.position;
        }

        newHalf = m_SelectedPlanet.transform.position;
        //Vector3 newVelocity 
        rigid.AddForce(prevVelocity * speed, ForceMode.Force);

        //Green line
        Vector3 planetVector = (prevHalf - newHalf).normalized;

        //Blue line
        Vector3 angleFromVector = transform.position - newHalf;

        huh = angleFromVector;
        m_DebugPlanetVector = planetVector;

        Vector3 crossProduct = new Vector3(planetVector.y, -planetVector.x);

        cross = crossProduct;
        float dot = Vector3.Dot(cross, angleFromVector.normalized);

        //Debug.Log(dot);

        if (dot > 0)
            this.m_Direction = Direction.Clockwise;
        else if (dot < 0)
            this.m_Direction = Direction.CounterClockwise;

        m_TheDirection = m_Direction;
    }

    public static Direction GetDirection()
    {
        return m_TheDirection;
    }
}