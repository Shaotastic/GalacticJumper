using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject m_ObjectToPool;
    public int m_AmountToPool;
    public List<GameObject> m_PooledObjects;

    public int NumberOfObjects { get => m_PooledObjects.Count; }

    private void Start()
    {
        m_PooledObjects = new List<GameObject>();

        GameObject temp;

        for (int i = 0; i < m_AmountToPool; i++)
        {
            temp = Instantiate(m_ObjectToPool, transform);
            temp.name = m_ObjectToPool.name + " " + i;
            temp.SetActive(false);
            m_PooledObjects.Add(temp);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < m_PooledObjects.Count; i++)
        {
            if (!m_PooledObjects[i].activeInHierarchy)
            {
                return m_PooledObjects[i];
            }
        }
        return null;
    }

}
