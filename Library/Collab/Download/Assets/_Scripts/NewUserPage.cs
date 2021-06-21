using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Text.RegularExpressions;
using System.Linq;
using com.shephertz.app42.paas.sdk.csharp.game;

public class NewUserPage : MonoBehaviour
{
    [SerializeField] TMP_InputField m_InputField;
    [SerializeField] Button m_ChangeName;
    [SerializeField] TextMeshProUGUI m_ValidText;
    [SerializeField] GameObject m_ConfirmName;
    [SerializeField] Button m_AcceptName, m_Cancel;

    string[] badWords = {
        "anal",
"anus",
"arse",
"asshole",
"asswipe",
"ballsack",
"balls",
"bastard",
"bitch",
"biatch",
"bloody",
"blowjob",
"blow job",
"bollock",
"bollok",
"boner",
"boob",
"bugger",
"bum",
"butt",
"buttplug",
"clit",
"clitoris",
"cock",
"coon",
"crap",
"cunt",
"cum",
"damn",
"dick",
"dildo",
"dyke",
"fag",
"feck",
"fellate",
"fellatio",
"felching",
"fuck",
"f u c k",
"fudgepacker",
"fudge packer",
"flange",
"Goddamn",
"God damn",
"homo",
"jerk",
"jizz",
"knobend",
"knob end",
"labia",
"muff",
"nigger",
"nigga",
"omg",
"penis",
"piss",
"poop",
"prick",
"pube",
"pussy",
"queer",
"scrotum",
"sex",
"shit",
"s hit",
"sh1t",
"slut",
"smegma",
"spunk",
"tit",
"titties",
"tosser",
"turd",
"twat",
"vagina",
"wank",
"whore",
"wtf" };

    private string newName;

    private int m_NameType = 0;

    // Start is called before the first frame update
    void Start()
    {
        m_ConfirmName.SetActive(false);
        m_ChangeName.onClick.AddListener(CheckNameValidity);
        m_AcceptName.onClick.AddListener(AssignNewUser);
        m_Cancel.onClick.AddListener(CloseWindow);
    }


    public void CheckNameValidity()
    {
        char[] charRemove = (@" ~!@#$%^&*()_+{}|:<>?`-=[]\;',./".ToCharArray());
        newName = SpecialCharacterFilter(m_InputField.text, charRemove);
        if (ToFamilyFriendlyString(newName))//, badWords);
        {
            m_ValidText.color = Color.grey;
            m_ValidText.text = "Checking...";

            //App24Leaderboard.SetUserName(newName);
            if (GameManager.Instance.HasInternet())
                StartCoroutine(DoesUserExist(newName));
            else
            {
                //Temporary store the user name, Check validity when they come back on
                PlayerPrefs.SetInt("UsernameTemp", 1);
                m_NameType = 2;
                AssignNewUser();
            }
        }
        else
        {
            m_ValidText.color = Color.red;
            m_ValidText.text = "Username is invalid";
            //m_InputField.text = String.Empty;
        }
    }

    IEnumerator DoesUserExist(string theName)
    {
        App24Leaderboard.m_UserService.GetUser(theName, new CheckUserName());
        yield return new WaitForSeconds(0.5f);
        if (CheckUserName.check)
        {
            m_ValidText.color = Color.red;
            m_ValidText.text = "Username already exists.";
        }
        else
        {
            m_ValidText.color = Color.green;
            m_ValidText.text = "Username is available";
            m_ConfirmName.SetActive(true);
            m_NameType = 1;
            AssignNewUser();
        }
    }

    void AssignNewUser()
    {     
        App24Leaderboard.SetUserName(newName, m_NameType);
        App24Leaderboard.CreateNewUser(newName);
        this.gameObject.SetActive(false);
    }

    void CloseWindow()
    {
        m_ConfirmName.SetActive(false);
    }
    

    public string SpecialCharacterFilter(string str, char[] charsToRemove)
    {
        foreach (char c in charsToRemove)
        {
            str = str.Replace(c.ToString(), String.Empty);
        }

        return str;
    }

    public string Filter(string input, string[] badWords)
    {
        string eh = badWords.ToString();

        input = input.ToLower();

        var re = new Regex(
            @"\b("
            + string.Join("|", badWords.Select(word =>
                string.Join(@"\s*", word.ToCharArray())))
            + @")\b", RegexOptions.IgnoreCase);
        return re.Replace(input, match =>
        {
            return new string('*', match.Length);
        });
    }

    public bool ToFamilyFriendlyString(string input)
    {
        foreach (string fWord in badWords)
        {
            //  Replace the word with *'s (but keep it the same length)
            string strReplace = "";
            for (int i = 0; i <= fWord.Length; i++)
            {
                strReplace += "*";
            }
            input = Regex.Replace(input.ToString(), fWord, strReplace, RegexOptions.IgnoreCase);

            if (input.Contains("*"))
            {
                return false;
            }
        }
        return true;
    }
}
