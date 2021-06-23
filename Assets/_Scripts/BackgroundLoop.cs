using UnityEngine;

public class BackgroundLoop : MonoBehaviour
{
    MaterialPropertyBlock m_Material;
    [SerializeField] MeshRenderer m_Renderer;
    float m_Threshold = 1000;

    // Start is called before the first frame update
    void Start()
    {
        m_Renderer.GetComponent<MeshRenderer>();
        m_Material = new MaterialPropertyBlock();
        m_Renderer.GetPropertyBlock(m_Material);
    }

    private void LateUpdate()
    {
        switch (GameManager.Instance.m_Pause)
        {
            case false:
                if (GameManager.Instance.m_GameStarted)
                {
                    float pos = (Camera.main.transform.position.y % m_Threshold) / m_Threshold;

                    m_Material.SetFloat("OffsetY", -pos);
                    m_Renderer.SetPropertyBlock(m_Material);
                }
                break;
            case true:
                break;
        }


    }
}
