using System.Collections.Generic;
using UnityEngine;

public class GeneratePlanet : MonoBehaviour
{
    [SerializeField] private PlanetObjectPoolManager m_PoolManager;
    [SerializeField] private List<Planet> m_PlanetPool;
    [SerializeField] private const int m_PoolSize = 3;
    [SerializeField] private float m_Distance;

    public float m_Left, m_Right;
    public float m_Min, m_Max;
    public float m_HighestPosition = 0;
    public float percentageChance = 0;

    public bool m_ActivateWindow;

    Vector3 playerVector;

    int previousIndex = -1;
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        GameManager.Instance.OnReset += Initialize;
        AdManager.Instance.AdFinished += Reset;
    }

    private void InitializeObjects()
    {
        m_PlanetPool[0].transform.position = transform.position;

        for (int i = 1; i < m_PoolSize; i++)
        {
            m_PlanetPool[i].transform.position = new Vector3(Random.Range(m_Left, m_Right), m_PlanetPool[i - 1].transform.position.y + Random.Range(m_Min, m_Max), 0);
        }

        m_ActivateWindow = false;

        Debug.Log("Planet Generator Initialized");
    }

    // Request from Manager A planet x3
    // check when a planet is below the screenspace
    // Reset that planet
    // Request another one 

    void Update()
    {
        switch (GameManager.Instance.m_Pause)
        {
            case false:
                if (GameManager.Instance.m_IsPlaying)
                {
                    UpdateMap();
                }
                break;
            case true:
                break;
        }
    }

    void RequestPlanet()
    {
        m_PlanetPool.Add(RequestPlanetFromPool());
    }

    Planet RequestPlanetFromPool()
    {
        int randomIndex = Random.Range(0, m_PoolManager.GetPoolObjectCount);

        while (randomIndex == previousIndex)
        {
            randomIndex = Random.Range(0, m_PoolManager.GetPoolObjectCount);
        }

        previousIndex = randomIndex;

        Planet temp = m_PoolManager.GetPlanetFromPool(randomIndex);

        return m_PoolManager.GetPlanetFromPool(randomIndex);
    }

    void UpdateMap()
    {
        foreach (Planet planet in m_PlanetPool)
        {
            if (Camera.main.WorldToViewportPoint(planet.transform.position).y > 0.75)
                m_HighestPosition = planet.transform.position.y;
            else if (Camera.main.WorldToViewportPoint(planet.transform.position).y <= -0.6f)
            {
                planet.Reset();

                m_PlanetPool.Remove(planet);
                Planet temp = RequestPlanetFromPool();

                temp.transform.position = new Vector3(Random.Range(m_Left, m_Right), Random.Range(m_Min, m_Max) + m_HighestPosition, 0);
                m_PlanetPool.Add(temp);

                break;
            }

            if (Camera.main.WorldToViewportPoint(planet.transform.position).y < 0)
                planet.SetOffScreen();
        }
    }

    public void Initialize()
    {
        m_PlanetPool = new List<Planet>(m_PoolSize);

        for (int i = 0; i < m_PoolSize; i++)
        {
            RequestPlanet();
        }

        InitializeObjects();
    }

    void UpdateWindow(GameObject planet, Vector3 playerPosition)
    {
        playerVector = playerPosition - planet.transform.position;

        Vector3 dir = Vector3.Normalize(m_PlanetPool[1].transform.position - m_PlanetPool[0].transform.position);
        Vector3 cross = Vector3.Normalize(new Vector3(dir.y, -dir.x)) * m_PlanetPool[0].GetComponent<SphereCollider>().radius;
        if (Player.GetDirection() == Player.Direction.Clockwise)
        {
            cross = cross * -1;
        }
        float angle = Vector3.Angle(cross, playerVector);
    }

    private void OnDrawGizmos()
    {
        if (m_PlanetPool == null || m_PlanetPool.Count < 2)
            return;

        Vector3 dir = m_PlanetPool[1].transform.position - m_PlanetPool[0].transform.position;

        Gizmos.DrawLine(m_PlanetPool[0].transform.position, m_PlanetPool[0].transform.position + dir);

        Vector3 cross = Vector3.Normalize(new Vector3(dir.y, -dir.x)) * m_PlanetPool[0].GetComponent<SphereCollider>().radius;
        if (Player.GetDirection() == Player.Direction.CounterClockwise)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(m_PlanetPool[0].transform.position, m_PlanetPool[0].transform.position + cross);
        }
        else
        {
            cross = cross * -1;

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(m_PlanetPool[0].transform.position, m_PlanetPool[0].transform.position + cross);
        }
    }

    private void Reset()
    {
        foreach (Planet planet in m_PlanetPool)
        {
            planet.ReEnablePlanet();
        }
    }
}
