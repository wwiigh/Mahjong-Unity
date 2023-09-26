using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    bool Canselect = false;
    GameObject select_card;
    GameObject now_select_card;
    // Start is called before the first frame update
    void Start()
    {
        // Canselect = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetSelect(bool value)
    {
        Canselect = value;        
        Debug.Log("In Enable " + value);
    }
    public void Select(GameObject gameObject)
    {
        select_card = gameObject;
    }
    public void UseCard()
    {
        if(Canselect==false||now_select_card==null)return;
        Canselect = false;
        select_card = now_select_card;
        now_select_card.transform.GetChild(0).gameObject.SetActive(false);    
        FindObjectOfType<GameControl>().EndRound(select_card);
        now_select_card = null;
        // return select_card;
    }
    public void EnableCard(GameObject card)
    {
        Debug.Log("In Enable");
        if(Canselect==false)return;
        if(now_select_card!=null)
        {
            now_select_card.transform.GetChild(0).gameObject.SetActive(false);
        }
        now_select_card = card;
        now_select_card.transform.GetChild(0).gameObject.SetActive(true);
    }
}
