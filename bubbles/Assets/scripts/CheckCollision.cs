using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CheckCollision : MonoBehaviour
{
    private int tagID;
    private string tagName;
    private GameObject newObj;
    private int newName;

    public NameHandler nameHandler;
    public GameObject soundTrack;
    private List<string> Checklist = new List<string>();
    
    void Start()
    {
        tagName = this.tag;
        tagID = int.Parse(tagName);
    }

    void OnCollisionEnter2D (Collision2D col)
    {
        if(tagID < 8){   //if the biggest item exists, do not check collision for it
            GameObject collidedObj = col.gameObject;
            if (collidedObj.tag == tagName)
            {
                Checklist.Add(collidedObj.name);
                CheckFirstCol(collidedObj,tagID);
            }
        }

        if(tagID == 8){    //if got the biggest dog, win
            CountScore currentStatus = new CountScore();
            currentStatus.status = 1;
            string json = JsonUtility.ToJson(currentStatus, true);
            File.WriteAllText(Application.dataPath + "/StreamingAssets/CountScore.json", json);
        }
    }

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
 
    private void AddScore(int tagid){
        string read = File.ReadAllText(Application.dataPath + "/StreamingAssets/CountScore.json");
        CountScore readScore = JsonUtility.FromJson<CountScore>(read);
        int currentScore = readScore.score;

        currentScore += tagid * 50;
        CountScore newScore = new CountScore();
        newScore.score = currentScore;
        string write = JsonUtility.ToJson(newScore, true);
        File.WriteAllText(Application.dataPath + "/StreamingAssets/CountScore.json", write);
    }

    //in case the current obj hit at least two same objects at the same time, check which one is the first hit
    void CheckFirstCol(GameObject obj, int tagid){
        string firstCollision = Checklist[0];
        if(obj.name == firstCollision){
            Vector3 newPos = obj.transform.position; //get current pos for merging items
            Destroy(this);
            Destroy(obj);
            string read = File.ReadAllText(Application.dataPath + "/StreamingAssets/CountScore.json");
            CountScore readScore = JsonUtility.FromJson<CountScore>(read);
            int statusQuo = readScore.status;
            if(int.Parse(this.name)> int.Parse(firstCollision) && statusQuo == 0){
                tagid+=1;
                tagName = tagid.ToString();
                AddScore(tagid);
                newObj = Instantiate(obj, newPos, Quaternion.identity);
                newObj.tag = tagName;
                GetNewName(newObj);
                newObj.AddComponent<PolygonCollider2D>();
                Rigidbody2D rb = newObj.GetComponent<Rigidbody2D>();
                rb.mass = 0.9f;
                rb.gravityScale = 0.5f;
                SpriteRenderer newSpriteRenderer = newObj.GetComponent<SpriteRenderer>();
                Sprite newSprite = Resources.Load<Sprite>(tagName);
                newSpriteRenderer.sprite = newSprite;
                newObj.AddComponent<CheckCollision>();
                StartCoroutine(CheckLose(newObj));
            }
            
        }
    }

    IEnumerator CheckLose(GameObject obj){
        yield return new WaitForSeconds(1.8f);
        obj.AddComponent<CheckFail>();
    }

}