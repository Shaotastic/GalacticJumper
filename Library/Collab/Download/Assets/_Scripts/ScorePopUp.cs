using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class ScorePopUp : MonoBehaviour
{
    TextMeshProUGUI m_TextBox;
    float topY = -220;
    [SerializeField] float botY = -275;
    // Start is called before the first frame update
    void Start()
    {
        m_TextBox = GetComponent<TextMeshProUGUI>();
        //GameManager.Instance.OnBonusScore += StartPopUp;
        //topY = m_TextBox.rectTransform.anchoredPosition.y;
        //StartPopUp();
    }

    void StartPopUp(double score)
    {
        switch (score)
        {
            case 0:
                m_TextBox.text = "";
                break;
            default:
                m_TextBox.text = "+" + score;
                break;
        }

        Sequence seq = DOTween.Sequence();
        seq.Append(m_TextBox.rectTransform.DOAnchorPosY(botY, 0, true)).Append(m_TextBox.DOFade(1, 0.2f)).Join(m_TextBox.rectTransform.DOAnchorPosY(topY, 0.4f, true)).AppendInterval(0.5f).Join(m_TextBox.DOFade(0, 0.2f));

    }
}
