using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct SubtitleText
{
    public float time;
    public string text;
}
public class cutsceneText : MonoBehaviour
{
    public TextMeshProUGUI Subtitles;
    public SubtitleText[] subtitleText;
    public GameMenager game;
    GameObject subtitleGO;
    
    // Start is called before the first frame update
    void Start()
    {
        
        subtitleGO = game.Subtitles;
        
        StartSubtitles();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void StartSubtitles()
    {
        StartCoroutine(SubtitleCoroutine());
    }
    IEnumerator SubtitleCoroutine()
    {
        subtitleGO.SetActive(true);
        foreach(var subtitle in subtitleText)
        {
            Subtitles.text = subtitle.text;

            yield return new WaitForSecondsRealtime(subtitle.time);
       }
        subtitleGO.SetActive(false);
        powrot();
   }
    void powrot()
    {
        SceneManager.LoadScene("BossBattle");
           
    }

}
