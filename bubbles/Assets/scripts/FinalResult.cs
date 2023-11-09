using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class FinalResult : MonoBehaviour
{
    public Text scoreText;
    public Text showText;
    public GameObject panel;
    private int total;
    public GameObject restartHolder;
    private int enable;

    public GameObject sound;
    private AudioSource soundOn;
    public AudioClip[] soundSource;

    void Start()
    {
        enable = 0;
        soundOn = sound.GetComponent<AudioSource>();
    }
    void Update()
    {
        string json = File.ReadAllText(Application.dataPath + "/StreamingAssets/CountScore.json");
        CountScore readScore = JsonUtility.FromJson<CountScore>(json);
        total = readScore.score;

        ShowScore();
        RenewStatus();
        Final();
    }

    void ShowScore(){
        scoreText.text = "Score:" + total.ToString();
    }

    void RenewStatus(){
        CountScore currentStatus = new CountScore();
        if (total>9999){
            currentStatus.status = 1;
            string json = JsonUtility.ToJson(currentStatus, true);
            File.WriteAllText(Application.dataPath + "/StreamingAssets/CountScore.json", json);
        }
    }

    void Final(){
        //To win the game, player should meet one of the requirements: score 10000 or have a biggest dog in the pool
        //Once player touches the line, lose game
        string json = File.ReadAllText(Application.dataPath + "/StreamingAssets/CountScore.json");
        CountScore final = JsonUtility.FromJson<CountScore>(json);
        int result = final.status;
        
        if(enable == 0){
            if(result != 0){
                soundOn.mute = true;
                restartHolder.SetActive(true);
                if(result == 1){              
                    showText.text = "赢了，不愧是博士!";
                    panel.SetActive(true);
                    soundOn.mute = false;
                    soundOn.PlayOneShot(soundSource[0]);
                    
                }else if(result == 2){
                    showText.text = "没关系！\n小僧觉得差不多就行了";
                    panel.SetActive(true);
                    soundOn.mute = false;
                    soundOn.PlayOneShot(soundSource[1]);
                }
                Restart();
            }
        }
    }

    void Restart(){
        Button restartBtn = restartHolder.GetComponentInChildren<Button>();
        restartBtn.onClick.AddListener(LoadStartScene);
        enable = 1;
    }

    void LoadStartScene()
    {
        //back to start page
        SceneManager.UnloadSceneAsync("GamePage");
        SceneManager.LoadScene("StartPage", LoadSceneMode.Additive);
    }
}

