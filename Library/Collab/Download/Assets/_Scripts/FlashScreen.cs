using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashScreen : MonoBehaviour
{

    RawImage image;

    public float fadeIn, fadeOut;

    Sequence sequence;

    public delegate void FlashEvent();

    public static event TweenCallback AtFlash, AfterFlash; 

    public void Reset()
    {
        sequence.Kill(true);
        image.color = new Color(1, 1, 1, 0);
    }

    // Use this for initialization
    void Start()
    {
        image = GetComponent<RawImage>();
        image.color = new Color(1, 1, 1, 0);
        image.enabled = true;
        GameManager.Instance.OnDeath += FlashsScreen;
        GameManager.Instance.OnReset += Reset;
        AdManager.Instance.AdFinished += Reset;
        //sequence.onComplete += AfterFlash;
    }

    void FlashsScreen()
    {
        sequence = DOTween.Sequence();

        sequence.Append(image.DOFade(1, fadeIn)).AppendCallback(AtFlash).Append(image.DOColor(new Color(0, 0, 0, 0.7f), fadeOut)).AppendCallback(AfterFlash) ;
    }
}
