using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowRules : MonoBehaviour
{
    public GameObject ruleBtn;
    private Button rule;
    public GameObject ruleImage;
    
    void Start()
    {
        rule = ruleBtn.GetComponent<Button>();
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.V)){
            ruleImage.SetActive(true);
        }else{
            ruleImage.SetActive(false);
        }
    }
}
