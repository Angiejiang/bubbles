using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Controller: MonoBehaviour
{
    public GameObject controller;
    public GameObject prefab;
    public GameObject restartBtn;
    public GameObject panel;
    private int newName;
    public NameHandler nameHandler;
    private GameObject newObj;
    private float speed;
    private int publicPicName;
    private string publicPicNameString;
    private SpriteRenderer spriteRenderer;
    private Sprite newSprite;
    private Vector3 spawnPoint;

    void Start()
    {
        restartBtn.SetActive(false);
        panel.SetActive(false);
        speed = 10.0f * Time.deltaTime;
        publicPicName= GenerateRandom(); //assign the first item
        publicPicNameString = publicPicName.ToString();

        CountScore newScore = new CountScore();
        newScore.score = 0;
        newScore.status = 0;
        string write = JsonUtility.ToJson(newScore, true);
        File.WriteAllText(Application.dataPath + "/StreamingAssets/CountScore.json", write);
    }

    void Update()
    {
        //controller movement
        float xPos = Input.GetAxis("Mouse X");
        float move = xPos * speed;
        float presentX = controller.transform.position.x;

        if (presentX <= 2.5f && presentX >= -2.5f)
        {
            controller.transform.Translate(move, 0, 0);
        }
        if (presentX > 2.5f)
        {
            controller.transform.Translate(move - 0.2f, 0, 0);
        }
        if (presentX < -2.5f)
        {
            controller.transform.Translate(move + 0.2f, 0, 0);
        }
        
        //let prefab move with the controller
        prefab.transform.position = controller.transform.position;
        spawnPoint = controller.transform.position;

        //assign prefab with current item pic
        GenerateOriginal();

        //if click, drop down the new generated item
        if (Input.GetMouseButtonDown(0))
        {   
            SwapSprite();
        }
    }

    void GenerateOriginal(){
        newSprite = Resources.Load<Sprite>(publicPicNameString);
        spriteRenderer = prefab.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = newSprite;
    }

    //generate random item per click
    int GenerateRandom(){
        return Random.Range(0,5);
    }

    //share new name with checkCollision.cs by reading the same name value from class NameHandler
    //this is for add correct new name for the combined items
    void GetNewName(GameObject obj){
        string json = File.ReadAllText(Application.dataPath + "/StreamingAssets/NameHandler.json");
        NameHandler currentName = JsonUtility.FromJson<NameHandler>(json);
        newName = currentName.nameValue;
        obj.name = newName.ToString();
        newName += 1;
        SaveCurrentName(newName);
    }

    private void SaveCurrentName(int name)
    {
        // save current life data to json
        NameHandler currentName = new NameHandler();
        currentName.nameValue = name;
        string json = JsonUtility.ToJson(currentName, true);
        File.WriteAllText(Application.dataPath + "/StreamingAssets/NameHandler.json", json);
    }

    //store current item number in publicPicName, assign new generated item tag, rigidbody and collider, 
    //then generate a new item num for the next iteration
    void SwapSprite(){
        //instantiate and set attribs
        newObj = Instantiate(prefab, spawnPoint, Quaternion.identity);
        newObj.tag = publicPicNameString;
        GetNewName(newObj);
        
        //add rigidbody and collider
        newObj.AddComponent<PolygonCollider2D>();
        SpriteRenderer newSpriteRenderer = newObj.GetComponent<SpriteRenderer>();
        Rigidbody2D newRb = newObj.AddComponent<Rigidbody2D>();
        newSpriteRenderer.sprite = newSprite;
        newRb.mass = 0.9f;
        newRb.gravityScale = 0.5f;
        newRb.isKinematic = false;

        //if generated a new item, attach it with the collision check script
        newObj.AddComponent<CheckCollision>();
        StartCoroutine(CheckLose(newObj)); //add a new attribute to check if the item is dropped into the pool

        //renew publicPicName
        publicPicName = GenerateRandom();
        publicPicNameString = publicPicName.ToString();
    }

    //only if the item is dropped into the pool will it be checked collision with deadline
    IEnumerator CheckLose(GameObject obj){
        yield return new WaitForSeconds(1.1f);
        obj.AddComponent<CheckFail>();
    }
}
