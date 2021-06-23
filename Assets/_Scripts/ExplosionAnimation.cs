using DG.Tweening;
using UnityEngine;

public class ExplosionAnimation : MonoBehaviour
{
    MaterialPropertyBlock m_Material;
    Renderer m_Renderer;
    Sequence sequence;
    float m_Alpha;
    [SerializeField] float m_FirstSize = 20;
    [SerializeField] float m_FirstTime = 0.3f;

    [SerializeField] float m_SecondSize = 40;
    [SerializeField] float m_SecondTime = 1f;

    // Start is called before the first frame update
    void Awake()
    {
        sequence = DOTween.Sequence();
        m_Renderer = GetComponent<Renderer>();
        m_Material = new MaterialPropertyBlock();
        m_Renderer.GetPropertyBlock(m_Material);
        transform.DOScale(0, 0);
        m_Alpha = 1;
    }

    // Update is called once per frame
    void Update()
    {
        switch (GameManager.Instance.m_Pause)
        {
            case false:
                if (sequence.IsPlaying())
                {
                    UpdateMaterials();
                }
                break;
            case true:
                break;
        }
    }

    public void StartSequence()
    {
        gameObject.SetActive(true);
        Vector3 target = Camera.main.transform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(target, Vector3.up);
        sequence = DOTween.Sequence();
        m_Alpha = 1;
        sequence.Append(transform.DOScale(m_FirstSize, m_FirstTime)).
            SetEase(Ease.OutSine).
            Append(transform.DOScale(m_SecondSize, m_SecondTime)).
            Join(DOTween.To(() => m_Alpha, x => m_Alpha = x, 0, m_SecondTime));
    }

    public bool IsSequencePlaying()
    {
        return sequence.IsPlaying();
    }

    private void UpdateMaterials()
    {
        m_Material.SetFloat("ExplosionAlpha", m_Alpha);
        m_Renderer.SetPropertyBlock(m_Material);
    }

    private void OnDisable()
    {
        sequence.Complete(true);
        sequence.Pause();
        transform.DOScale(0, 0);
    }

    public void StopSequence()
    {
        DOTween.Kill(sequence);
        transform.DOScale(0, 0);
        gameObject.SetActive(false);
    }

}
