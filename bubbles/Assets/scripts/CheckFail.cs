using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class CheckFail : MonoBehaviour
{
    public GameObject lose;
    private LayerMask checkLayer;

    void Start()
    {
        lose = GameObject.Find("lose");
        checkLayer = LayerMask.GetMask("deadline");
    }

    void Update(){
        Collider2D collider = this.GetComponent<Collider2D>();
        if(collider.IsTouchingLayers(checkLayer)){
            // Vector3 pos = lose.transform.position;
            // pos.z = -2;
            // lose.transform.position = pos;

            CountScore currentStatus = new CountScore();
            currentStatus.status = 2;
            //currentStatus.score = 0;
            string json = JsonUtility.ToJson(currentStatus, true);
            File.WriteAllText(Application.dataPath + "/StreamingAssets/CountScore.json", json);
        }
    }
}
