using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EatCardController : MonoBehaviour
{
    public GameObject panel;
    public GameObject confirmButton;
    public UImanager uImanager;
    public GameControl gameControl;
    GameObject now_select;
    int now_card;
    Fang_Cal now_player;
    // Start is called before the first frame update
    void Start()
    {
        now_select = null;
        panel.SetActive(false);
        for(int i=0;i<3;i++)
        {
            panel.transform.GetChild(i).GetComponent<Image>().enabled = false;
            panel.transform.GetChild(i).gameObject.SetActive(false);

        }
        confirmButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void EnablePanel(Fang_Cal player,int card_name)
    {
        panel.SetActive(true);
        confirmButton.SetActive(true);
        now_player = player;
        now_card = card_name;
        int[] count_array = new int[4];
        count_array[0] = player.Count(card_name - 2);
        count_array[1] = player.Count(card_name - 1);
        count_array[2] = player.Count(card_name + 1);
        count_array[3] = player.Count(card_name + 2);
        if (count_array[0] > 0 && count_array[1] > 0)
        {
            panel.transform.GetChild(0).gameObject.SetActive(true);
            panel.transform.GetChild(0).GetChild(0).GetChild(1).gameObject.GetComponent<Image>().sprite = 
                uImanager.getIcon[card_name - 2];
            panel.transform.GetChild(0).GetChild(1).GetChild(1).gameObject.GetComponent<Image>().sprite = 
                uImanager.getIcon[card_name - 1];
        }
        if (count_array[1] > 0 && count_array[2] > 0) 
        {
            panel.transform.GetChild(1).gameObject.SetActive(true);
            panel.transform.GetChild(1).GetChild(0).GetChild(1).gameObject.GetComponent<Image>().sprite = 
                uImanager.getIcon[card_name - 1];
            panel.transform.GetChild(1).GetChild(1).GetChild(1).gameObject.GetComponent<Image>().sprite = 
                uImanager.getIcon[card_name + 1];
        }
        if (count_array[2] > 0 && count_array[3] > 0)
        {
            panel.transform.GetChild(2).gameObject.SetActive(true);
            panel.transform.GetChild(2).GetChild(0).GetChild(1).gameObject.GetComponent<Image>().sprite = 
                uImanager.getIcon[card_name + 1];
            panel.transform.GetChild(2).GetChild(1).GetChild(1).gameObject.GetComponent<Image>().sprite = 
                uImanager.getIcon[card_name + 2];
        }

    }
    public void SelectCard(GameObject selectcard)
    {
        if(now_select != null)now_select.GetComponent<Image>().enabled = false;
        selectcard.GetComponent<Image>().enabled = true;
        now_select = selectcard;
    }
    public void ConfirmCard()
    {
        int one = uImanager.getId[now_select.transform.GetChild(0).GetChild(1).gameObject.GetComponent<Image>().sprite];
        int two = uImanager.getId[now_select.transform.GetChild(1).GetChild(1).gameObject.GetComponent<Image>().sprite];
        if(now_card<one && now_card < two)
        {
            now_player.Eat(now_card,one,two,now_card);
            FindObjectOfType<SetEatCard>().SetCard("Eat",now_card,one,two);
        }
        else if(now_card>one && now_card < two)
        {
            now_player.Eat(one,now_card,two,now_card);
            FindObjectOfType<SetEatCard>().SetCard("Eat",one,now_card,two);
        }
        if(now_card>one && now_card > two)
        {
            now_player.Eat(one,two,now_card,now_card);
            FindObjectOfType<SetEatCard>().SetCard("Eat",one,two,now_card);
        }
        panel.SetActive(false);
        confirmButton.SetActive(false);
        for(int i=0;i<3;i++)
        {
            panel.transform.GetChild(i).GetComponent<Image>().enabled = false;
            panel.transform.GetChild(i).gameObject.SetActive(false);

        }
        gameControl.has_op = true;
        gameControl.now_player = 0;
        gameControl.StartRound();
        
    }
    public void init()
    {
        now_select = null;
        panel.SetActive(false);
        for(int i=0;i<3;i++)
        {
            panel.transform.GetChild(i).GetComponent<Image>().enabled = false;
            panel.transform.GetChild(i).gameObject.SetActive(false);

        }
        confirmButton.SetActive(false);
    }
}
