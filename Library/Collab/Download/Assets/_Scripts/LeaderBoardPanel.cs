using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Scenes;
using UnityEngine;

public class LeaderBoardPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Rank;
    [SerializeField] TextMeshProUGUI PlayerName;
    [SerializeField] TextMeshProUGUI PlayerScore;
    [SerializeField] RectTransform m_RectTransform;

    public void SetPosition(Vector3 position)
    {
        transform.localPosition = position;
    }

    public void AssignPanel(string rank, string name, string score)
    {
        Rank.text = rank;
        PlayerName.text = name;
        PlayerScore.text = score;
    }

    public void AssignColor(Color color)
    {
        Rank.color = color;
        PlayerName.color = color;
        PlayerScore.color = color;
    }
}
