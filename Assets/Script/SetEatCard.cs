using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetEatCard : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> EatCard;
    public UImanager uImanager;
    int index;
    List<int> gonHistory = new List<int>();
    void Start()
    {
        index = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetCard(string type, int card1,int card2 = 1000,int card3 = 1000)
    {
        switch(type)
        {
            case "Eat":
                EatCard[index].SetActive(true);
                EatCard[index].transform.GetChild(0).gameObject.SetActive(false);
                EatCard[index].transform.GetChild(1).GetComponentInChildren<Image>().sprite = uImanager.getIcon[card1];
                EatCard[index].transform.GetChild(2).GetComponentInChildren<Image>().sprite = uImanager.getIcon[card2];
                EatCard[index].transform.GetChild(3).GetComponentInChildren<Image>().sprite = uImanager.getIcon[card3];
                break;
            case "Pon":
                EatCard[index].SetActive(true);
                gonHistory.Add(card1);
                EatCard[index].transform.GetChild(0).gameObject.SetActive(false);
                EatCard[index].transform.GetChild(1).GetComponentInChildren<Image>().sprite = uImanager.getIcon[card1];
                EatCard[index].transform.GetChild(2).GetComponentInChildren<Image>().sprite = uImanager.getIcon[card1];
                EatCard[index].transform.GetChild(3).GetComponentInChildren<Image>().sprite = uImanager.getIcon[card1];
                break;
            case "Gon":
                if(gonHistory.Contains(card1) == false)
                {
                    EatCard[index].SetActive(true);
                    EatCard[index].transform.GetChild(0).gameObject.SetActive(true);
                    EatCard[index].transform.GetChild(0).GetComponentInChildren<Image>().sprite = uImanager.getIcon[card1];
                    EatCard[index].transform.GetChild(1).GetComponentInChildren<Image>().sprite = uImanager.getIcon[card1];
                    EatCard[index].transform.GetChild(2).GetComponentInChildren<Image>().sprite = uImanager.getIcon[card1];
                    EatCard[index].transform.GetChild(3).GetComponentInChildren<Image>().sprite = uImanager.getIcon[card1];
                }
                else
                {
                    for(int i=0;i<index;i++)
                    {
                        Sprite sprite1 = EatCard[i].transform.GetChild(1).GetComponentInChildren<Image>().sprite;
                        Sprite sprite2 = EatCard[i].transform.GetChild(2).GetComponentInChildren<Image>().sprite;
                        int id1 = uImanager.getId[sprite1];
                        int id2 = uImanager.getId[sprite2];
                        if(id1 == id2 && id1 == card1)
                        {
                            EatCard[i].transform.GetChild(0).gameObject.SetActive(true);
                            EatCard[i].transform.GetChild(0).GetComponentInChildren<Image>().sprite = uImanager.getIcon[card1];
                            return;
                        }

                    }
                }
                
                break;
            default:
                break;
        }
        index ++;
    }
    public void init()
    {
        index = 0;
        foreach (var item in EatCard)
        {
            item.SetActive(false);
        }
    }
}
