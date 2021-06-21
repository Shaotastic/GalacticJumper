using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    [SerializeField] private Image m_ProgressBar;
    [SerializeField] private TextMeshProUGUI m_Text;
    private Scene scnee;

    public static bool m_Loaded;
    // Start is called before the first frame update
    void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0)
            StartCoroutine(LoadASyncOperation());

       //if (SceneManager.GetActiveScene().buildIndex == 1)
       //    StartCoroutine(LoadLeaderBoard());
    }

    IEnumerator LoadASyncOperation()
    {
        //Scene gameLevel = SceneManager.LoadScene(1, new LoadSceneParameters(LoadSceneMode.Single));        
        yield return new WaitForSeconds(1);
        
        AsyncOperation gameLevel = SceneManager.LoadSceneAsync(1);
        while(gameLevel.progress < 1)
        {
            //Debug.Log(gameLevel.progress);
            m_Text.text = (gameLevel.progress * 100).ToString();
            m_ProgressBar.fillAmount = gameLevel.progress;
            yield return new WaitForEndOfFrame();
            m_Loaded = true;
        }


    }

    IEnumerator LoadLeaderBoard()
    {
        SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive);
        Debug.Log("Loading Leaderboard");
        yield return new WaitForEndOfFrame();
        Debug.Log("Loaded Leaderboard");
        m_Loaded = true;
    }
}
