using System.Collections.Generic;
using UnityEngine;

namespace ATools.Unity
{
    public class ObjectPoolManager : MonoBehaviour
    {
        [SerializeField] protected List<PoolObject> poolObjects;
        public int TotalPoolObjectCount { get; private set; } = 0;

        public PoolObject GetPoolObjectByIndex(int index)
        {
            return poolObjects[index];
        }

        protected virtual void Awake()
        {
            InitializeObjects();
        }

        public Planet GetPlanetFromPool(int poolIndex)
        {
            return poolObjects[poolIndex].GetPlanet();
        }

        public virtual void ResetPlanet()
        {
            foreach (PoolObject obj in poolObjects)
            {
                obj.ResetAllObjects();
            }

            Debug.Log("All objects in Pool have been reset");
        }

        public int GetPoolObjectCount
        {
            get { return poolObjects.Count; }
        }

        /// <summary>
        /// This initializes each PoolObject and generates a container within the ObjectPoolManager in the scene as child objects
        /// </summary>
        protected virtual void InitializeObjects()
        {
            for (int i = 0; i < poolObjects.Count; i++)
            {
                poolObjects[i].InitializeList(poolObjects[i].GetAmount);
                GameObject container = new GameObject();
                container.name = poolObjects[i].GetGameObject.name + " Container";
                container.transform.parent = this.transform;
                for (int j = 0; j < poolObjects[i].GetList().Capacity; j++)
                {
                    GameObject temp = Instantiate(poolObjects[i].GetGameObject);
                    temp.name = poolObjects[i].GetGameObject.name + " " + j;
                    temp.transform.parent = container.transform;
                    poolObjects[i].Add(temp.GetComponent<Planet>());
                    //temp.GetComponent<Planet>().in
                    //poolObjects[i].
                    TotalPoolObjectCount++;
                }
            }

            Debug.Log("Pool objects are initialized");
        }
    }

    [System.Serializable]
    public class PoolObject
    {
        [SerializeField] protected GameObject m_GameObject;
        [SerializeField] protected int m_Amount;
        [SerializeField] protected List<Planet> m_List;
        [SerializeField] private Planet m_Planet;
        [SerializeField] private int m_Index = -1;
        public virtual void InitializeList(int count)
        {
            m_List = new List<Planet>(count);

            if (m_GameObject)
                m_Planet = m_GameObject.GetComponent<Planet>();
            else
                Debug.LogError("Gameobject is null for this Pool Object.");
        }

        public void IntializeObjects()
        {
            for (int i = 0; i < m_List.Count; i++)
            {
                m_List[i].InitializeObject();
            }
        }

        public virtual List<Planet> GetList()
        {
            return m_List;
        }

        public void Add(Planet planet)
        {
            m_List.Add(planet);
        }

        public GameObject GetGameObject
        { get { return m_GameObject; } }

        public int GetAmount
        {
            get { return m_Amount; }
        }



        public Planet GetPlanet()
        {

            //Planet temp = m_List[index];
            //m_List.RemoveAt();            
            return m_List[IncrementIndex];
            //continue;

            //return m_List[]
            //return m_List[index].GetPlanet();
        }

        public void Reset(int index)
        {
            m_List[index].Reset();
        }

        public void ResetAllObjects()
        {
            for (int i = 0; i < m_List.Count; i++)
            {
                m_List[i].Reset();
            }
        }

        private int IncrementIndex
        {
            get {
                m_Index++;

                if (m_Index == m_List.Count)
                    m_Index = 0;

                return m_Index;
            }
        }
    }
}