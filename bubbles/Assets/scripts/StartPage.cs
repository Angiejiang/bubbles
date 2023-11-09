using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using UnityEngine.Experimental.Rendering;

public class StartPage : MonoBehaviour
{
    public GameObject startButton;
    public GameObject ruleButton;
    public GameObject cover;
    Button startBtn;
    public Sprite newSprite;
    public GameObject background;
    private AudioSource bgMusic;
    public AudioClip knock;
    private AudioSource knockSound;

    void Start()
    {
        startBtn = startButton.GetComponent<Button>();
        knockSound = startButton.GetComponent<AudioSource>();
        bgMusic = background.GetComponent<AudioSource>();
        bgMusic.Stop();
        startBtn.onClick.AddListener(StartGame);
    }

    void StartGame()
    {
        bgMusic.Play();
        knockSound.PlayOneShot(knock);
        startBtn.image.sprite = Resources.Load<Sprite>("knockOn");
        StartCoroutine(loadScene());
    }

    IEnumerator loadScene(){
        yield return new WaitForSeconds(0.6f);
        startButton.SetActive(false);
        ruleButton.SetActive(false);
        cover.SetActive(false);
        SceneManager.UnloadSceneAsync("StartPage");
        SceneManager.LoadScene("GamePage", LoadSceneMode.Additive);
    }
}
