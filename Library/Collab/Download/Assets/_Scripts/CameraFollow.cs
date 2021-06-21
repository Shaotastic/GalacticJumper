using UnityEngine;
using System.Collections;
using DG.Tweening;
using System;
using System.Runtime.CompilerServices;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float speed;
    public float playerFollowSpeed;
    //[SerializeField] private Vector3 m_Offset;
    public float yOffset;
    public bool m_CameraHasResetPosition;

    [SerializeField] Vector3 StartPosition;

    [SerializeField] float transitionDistance = 100;

    public static CameraFollow Instance;

    // Use this for initialization
    void Awake()
    {
        if (Instance == null)
            Instance = this;

        StartPosition = transform.position;
        //m_CameraHasResetPosition = true;
    }

    private void Start()
    {
        GameManager.Instance.OnDeath += CameraShake;
        //AdManager.Instance.AdBegin += () => { m_CameraHasResetPosition = false; };
        //AdManager.Instance.AdComplete += () => { m_CameraHasResetPosition = true; };
    }

    void LateUpdate()
    {
        switch (GameManager.Instance.m_Pause)
        {
            case false:
                if (GameManager.Instance.m_IsPlaying)
                {
                    if (!GameManager.Instance.GetPlayer().IsDead)
                    {
                        if (GameManager.Instance.GetPlayer().GetObject())
                        {
                            transform.position = Vector3.Lerp(transform.position,
                                new Vector3(transform.position.x,
                                GameManager.Instance.GetPlayer().GetObject().position.y + yOffset,
                                transform.position.z),
                                Time.deltaTime * speed);
                        }
                    }
                }
                break;
            case true:
                break;
        }
    }

    void CameraShake()
    {
        transform.DOShakePosition(0.6f, 1, 10, 90);
    }

    public void ResetCamera()
    {
        transform.position = StartPosition;
    }
}
