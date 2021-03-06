using DG.Tweening;
using UnityEngine;

public class Planet : MonoBehaviour
{
    private Vector3 m_RotationDirection;
    [SerializeField] private float m_RotationSpeed = 10f;
    [SerializeField] private bool m_Visited;
    [SerializeField] private bool m_GasPlanet;
    [SerializeField] private Renderer m_Renderer;
    [SerializeField] private GameObject m_Atmosphere;
    [SerializeField] private ExplosionAnimation m_Explosion;
    [SerializeField] private Player m_Player;
    [SerializeField] ShatterPlanet m_ShatterPlanet;

    [SerializeField] private bool m_HasExploded;

    [SerializeField] float m_LerpTimer = 0;
    [SerializeField] float m_Crack = 1;
    //MaterialPropertyBlock m_Material;
    [SerializeField] Material m_Material;
    private Vector3 m_ResetPosition = new Vector3(-500, -500);
    private bool m_OffScreen;

    private Sequence m_Sequence;

    void Start()
    {
        m_Renderer = GetComponent<Renderer>();
        m_Material = m_Renderer.material;
        //m_Material = new MaterialPropertyBlock();
        //m_Renderer.GetPropertyBlock(m_Material);

        if (!m_Atmosphere)
            m_Atmosphere = transform.GetChild(1).gameObject;

        if (!m_Explosion)
            m_Explosion = transform.GetChild(0).GetComponent<ExplosionAnimation>();

        m_Explosion.gameObject.SetActive(false);
        transform.position = m_ResetPosition;
        m_ShatterPlanet.SpawnPlanet(false);
        InitializeObject();
        m_OffScreen = false;
        m_Visited = false;
    }

    public void InitializeObject()
    {
        m_RotationDirection = new Vector3(Random.Range(-4, 4), Random.Range(-4, 4), Random.Range(-4, 4));
        m_Visited = false;
    }

    private void Update()
    {
        switch (GameManager.Instance.m_Pause)
        {
            case false:
                if (m_Visited && !m_HasExploded && !m_OffScreen)
                {
                    UpdateMaterial();
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
                if (m_Visited && !m_HasExploded && !m_OffScreen)
                {
                    m_LerpTimer += (Time.fixedDeltaTime / GameManager.Instance.GetPlanetDecaySpeed());

                    if (m_LerpTimer >= 0.5f)
                    {
                        m_Sequence = DOTween.Sequence();
                        m_Sequence.Append(DOTween.To(() => m_Crack, x => m_Crack = x, 0, (GameManager.Instance.GetPlanetDecaySpeed() / 1.8f)));
                    }

                    if (m_LerpTimer >= 1)
                    {
                        Explode();
                    }
                    if (m_LerpTimer >= 0.97f)
                    {
                        SoundManager.Instance.PlayPlanetExplosion();
                    }

                }
                transform.RotateAround(transform.position, m_RotationDirection, Time.fixedDeltaTime * m_RotationSpeed);
                break;
            case true:
                break;
        }
    }

    void UpdateMaterial()
    {
        m_Material.SetColor("MainColor", Color.Lerp(Color.white, Color.black, m_LerpTimer));
        m_Material.SetFloat("_CrackAlpha", m_Crack);
        //m_Renderer.SetPropertyBlock(m_Material);
    }

    void ResetMaterial()
    {
        m_Crack = 1;
        m_Material.SetColor("MainColor", Color.white);
        m_Material.SetFloat("_CrackAlpha", m_Crack);
        //m_Renderer.SetPropertyBlock(m_Material);
    }

    public void Reset()
    {
        transform.position = m_ResetPosition;
        ReEnablePlanet();
        ResetMaterial();
    }

    public void ReEnablePlanet()
    {
        m_Explosion.StopSequence();
        m_Visited = false;
        m_Renderer.enabled = true;
        m_Atmosphere.SetActive(true);
        m_Player = null;
        m_LerpTimer = 0;
        m_Explosion.transform.localScale = Vector3.one;
        m_Explosion.gameObject.SetActive(false);
        m_OffScreen = false;
        m_HasExploded = false;
        m_ShatterPlanet.Instance_OnReset();
        m_Sequence.Kill();
        ResetMaterial();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            m_Player = null;
            //m_Visited = false;
        }
    }

    private void Explode()
    {
        m_Atmosphere.SetActive(false);
        m_Renderer.enabled = false;
        m_ShatterPlanet.SpawnPlanet(true);
        m_Explosion.StartSequence();
        m_HasExploded = true;
        if (m_Player)
            m_Player.Dead(false);
    }

    public void Visited(Player player)
    {
        if (!m_Visited)
        {
            m_Visited = true;
            m_Player = player;
            ResetMaterial();
        }
    }

    public bool WasVisited
    {
        get => m_Visited;
    }


    public void SetOffScreen()
    {
        m_OffScreen = true;
    }
}

