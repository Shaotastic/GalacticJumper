using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class ShatterPlanet : MonoBehaviour
{
    //[SerializeField] List<GameObject> m_PlanetPieces;
    [SerializeField] List<Vector3> m_PartPositions;
    [SerializeField] List<Rigidbody> m_Rigidbodies;
    [SerializeField] float m_ExplosionForce = 50;

    void Start()
    {
        //m_PlanetPieces = new List<GameObject>();
        m_PartPositions = new List<Vector3>();
        m_Rigidbodies = new List<Rigidbody>();

        for (int i = 0; i < transform.childCount; i++)
        {
            //m_PlanetPieces.Add(transform.GetChild(i).gameObject);
            //m_PlanetPieces[i].SetActive(false);
            m_Rigidbodies.Add(transform.GetChild(i).gameObject.GetComponent<Rigidbody>());
            m_Rigidbodies[i].gameObject.SetActive(false);
            //m_Rigidbodies.Add(m_PlanetPieces[i].GetComponent<Rigidbody>());
            m_PartPositions.Add(transform.GetChild(i).localPosition);
        }

        GameManager.Instance.OnReset += Instance_OnReset;
    }

    public void Instance_OnReset()
    {
        //for (int i = 0; i < m_PlanetPieces.Count; i++)
        //{
        //    m_PlanetPieces[i].transform.localPosition = m_PartPositions[i];
        //    m_PlanetPieces[i].SetActive(false);
        //}

        for(int i = 0; i < m_Rigidbodies.Count; i++)
        {
            m_Rigidbodies[i].transform.localPosition = m_PartPositions[i];
            m_Rigidbodies[i].gameObject.SetActive(false);
        }
    }

    public void SpawnPlanet(bool toggle)
    {
        for (int i = 0; i < m_Rigidbodies.Count; i++)
        {
            m_Rigidbodies[i].gameObject.SetActive(toggle);

            if (toggle)
            {
                m_Rigidbodies[i].AddExplosionForce(m_ExplosionForce, transform.position, 4, 0, ForceMode.Impulse);
            }

        }

    }
}