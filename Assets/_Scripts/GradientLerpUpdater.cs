using UnityEngine;

public class GradientLerpUpdater : MonoBehaviour
{
    [SerializeField] Color m_BottomColor, m_TopColor;

    Renderer m_Renderer;
    MaterialPropertyBlock m_Material;

    [SerializeField] float m_LerpValue, m_ThresholdValue = 500;


    [SerializeField] bool trigger;

    private void Start()
    {
        m_Renderer = GetComponent<Renderer>();
        m_Material = new MaterialPropertyBlock();
        m_Renderer.GetPropertyBlock(m_Material);
        RandomColor();
        UpdateMaterial();
        GameManager.Instance.OnReset += RandomColor;
    }


    float lerp;

    private void Update()
    {
        switch (GameManager.Instance.m_Pause)
        {
            case false:
                if (GameManager.Instance.m_GameStarted)
                {
                    float positionValue = Camera.main.transform.position.y;

                    m_LerpValue = (positionValue % m_ThresholdValue) / m_ThresholdValue;

                    if (trigger && m_LerpValue < 0.2f)
                        trigger = false;

                    if (!trigger && m_LerpValue >= 0.99f)
                    {
                        trigger = true;
                        SetNewColor();
                        Debug.Log("GOTA TRIGGGAAA");
                        //m_LerpValue = 0;
                    }
                    m_LerpValue = Mathf.Clamp01(m_LerpValue);

                    if (!trigger)
                        UpdateMaterial();
                }
                break;
            case true:
                break;
        }
    }

    void SetNewColor()
    {
        m_BottomColor = m_TopColor;
        m_TopColor = ColorClamp(Random.ColorHSV(), 0, 0.7f);
    }

    void UpdateMaterial()
    {
        m_Material.SetColor("TopColor", m_TopColor);
        m_Material.SetColor("BottomColor", m_BottomColor);
        m_Material.SetFloat("LerpValue", m_LerpValue);
        m_Renderer.SetPropertyBlock(m_Material);
    }

    void RandomColor()
    {
        m_BottomColor = ColorClamp(Random.ColorHSV(), 0, 0.7f);
        m_TopColor = ColorClamp(Random.ColorHSV(), 0, 0.7f);
    }

    Color ColorClamp(Color color, float clampMin, float clampMax)
    {
        float r = Mathf.Clamp(color.r, clampMin, clampMax);
        float g = Mathf.Clamp(color.g, clampMin, clampMax);
        float b = Mathf.Clamp(color.b, clampMin, clampMax);

        return new Color(r, g, b);
    }
}

