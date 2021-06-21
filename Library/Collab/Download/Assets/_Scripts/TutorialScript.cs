using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour
{
    [SerializeField] Sprite Hand1, Hand2; 
    [SerializeField] Image reference;


    public void UnTap()
    {
        reference.sprite = Hand1;
    }

    public void Tap()
    {
        reference.sprite = Hand2;
    }
}
