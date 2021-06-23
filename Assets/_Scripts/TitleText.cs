using DG.Tweening;
using TMPro;
using UnityEngine;

public class TitleText : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI m_GalacticText, m_JumpText;

    [SerializeField] RectTransform m_PlayButton, m_OptionButton, m_LbButton;
    Sequence sequence;

    Vector2 jumpText;
    // Start is called before the first frame update
    void Start()
    {
        m_GalacticText.rectTransform.anchoredPosition = new Vector2(-1000, m_GalacticText.rectTransform.anchoredPosition.y);
        jumpText = m_JumpText.rectTransform.anchoredPosition;
        m_JumpText.DOFade(0, 0);
        m_JumpText.rectTransform.anchoredPosition = new Vector2(50, 200);

        m_PlayButton.DOScale(0, 0);
        m_OptionButton.DOScale(0, 0);
        m_LbButton.DOScale(0, 0);

        sequence = DOTween.Sequence();

        sequence.SetDelay(0.3f)
            .Append(m_GalacticText.rectTransform.DOAnchorPosX(-10, 0.3f))
            .Append(m_JumpText.rectTransform.DOAnchorPos(jumpText, 0.3f))
            .Join(m_JumpText.DOFade(1, 0.4f))
            .OnComplete(() =>
            {
                m_PlayButton.DOScale(2.1f, 0.3f);
                m_OptionButton.DOScale(1, 0.4f);
                m_LbButton.DOScale(1, 0.5f);
            });




    }
}
