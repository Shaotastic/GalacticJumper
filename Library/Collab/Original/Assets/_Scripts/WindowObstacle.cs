using UnityEngine;

[ExecuteInEditMode]
public class WindowObstacle : MonoBehaviour
{
    [Range(0, 7)]
    public float m_WindowSize = 0;

    [SerializeField] Collider LeftWall, RightWall;
    // Start is called before the first frame update
    void Start()
    {
        CalculateWindowSize();
    }

    // Update is called once per frame
    void Update()
    {
        CalculateWindowSize();
    }

    void CalculateWindowSize()
    {
        LeftWall.gameObject.transform.position = new Vector3(transform.position.x - LeftWall.bounds.extents.x - m_WindowSize, LeftWall.transform.position.y, LeftWall.transform.position.z);
        RightWall.gameObject.transform.position = new Vector3(transform.position.x + RightWall.bounds.extents.x + m_WindowSize, RightWall.transform.position.y, RightWall.transform.position.z);
    }

    public void SetWindowSize(float value)
    {
        m_WindowSize = value;
    }
}
