using System.Collections.Generic;
using UnityEngine;

public class ShatterShip : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] List<GameObject> m_ShipParts;
    [SerializeField] List<Vector3> m_PartPositions;

    void Start()
    {
        m_ShipParts = new List<GameObject>();
        m_PartPositions = new List<Vector3>();

        for (int i = 0; i < transform.childCount; i++)
        {
            m_ShipParts.Add(transform.GetChild(i).gameObject);
            m_ShipParts[i].SetActive(false);
            m_PartPositions.Add(transform.GetChild(i).localPosition);
        }

        GameManager.Instance.OnDeath += Instance_OnDeath;
        GameManager.Instance.OnReset += Instance_OnReset;
        AdManager.Instance.AdComplete += Instance_OnDeath;
        AdManager.Instance.AdFinished += Instance_OnReset;
    }

    private void Instance_OnReset()
    {
        for (int i = 0; i < m_ShipParts.Count; i++)
        {
            m_ShipParts[i].transform.localPosition = m_PartPositions[i];
            m_ShipParts[i].SetActive(false);
        }
    }

    private void Instance_OnDeath()
    {
        foreach (GameObject part in m_ShipParts)
        {
            part.SetActive(true);
        }
    }
}
