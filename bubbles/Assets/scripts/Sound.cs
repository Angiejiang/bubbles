using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Sound : MonoBehaviour
{
    private Button soundBtn;
    public GameObject soundButton;
    private bool isSound = true;

    private AudioSource soundOn;
    public AudioClip[] soundSource;
    private int scoreMonitor;

    void Start()
    {
        soundBtn = soundButton.GetComponent<Button>();  
        soundOn = soundButton.GetComponent<AudioSource>();
        scoreMonitor = 0;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S)){
            SoundControl();
        }
        CheckMerge();
    }

    void SoundControl(){
        switch(isSound){
            case true:
            isSound = false;
            soundBtn.image.sprite = Resources.Load<Sprite>("soundOff");
            soundOn.mute = true;
            break;

            case false:
            isSound = true;
            soundBtn.image.sprite = Resources.Load<Sprite>("soundOn");
            soundOn.mute = false;
            break;
        }
    }

    void RandomSound(){
        int rand = Random.Range(0,5);
        soundOn.PlayOneShot(soundSource[rand]);
    }

    //once the score changes, it means there's a merge, so play random sound
    void CheckMerge(){
        string read = File.ReadAllText(Application.dataPath + "/StreamingAssets/CountScore.json");
        CountScore readScore = JsonUtility.FromJson<CountScore>(read);
        int currentScore = readScore.score;
        int currentStatus = readScore.status;
        if(currentScore == 0){
            scoreMonitor = 0;
        } 
        if(currentStatus == 0){
            if(scoreMonitor != currentScore){
                RandomSound();
                scoreMonitor = currentScore;
            }
        }
    }
}
