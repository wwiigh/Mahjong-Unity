using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UImanager : MonoBehaviour
{
    public GameObject playercards;
    public List<Sprite> cardIcon = new List<Sprite>();
    public Text nowWind;
    public Dictionary<int,Sprite> getIcon = new Dictionary<int, Sprite>();
    public Dictionary<Sprite,int> getId = new Dictionary<Sprite,int>();
    // Start is called before the first frame update
    void Awake()
    {
        int nowSpriteIndex = 0;
        for(int i=0;i<9;i++)
        getIcon.Add(i, cardIcon[nowSpriteIndex++]);
        for(int i=16;i<25;i++)
        getIcon.Add(i, cardIcon[nowSpriteIndex++]);
        for(int i=32;i<41;i++)
        getIcon.Add(i, cardIcon[nowSpriteIndex++]);
        for(int i=48;i<=96;i+=8)
        getIcon.Add(i, cardIcon[nowSpriteIndex++]);
        nowSpriteIndex = 0;
        for(int i=0;i<9;i++)
        getId.Add(cardIcon[nowSpriteIndex++],i);
        for(int i=16;i<25;i++)
        getId.Add(cardIcon[nowSpriteIndex++],i);
        for(int i=32;i<41;i++)
        getId.Add(cardIcon[nowSpriteIndex++],i);
        for(int i=48;i<=96;i+=8)
        getId.Add(cardIcon[nowSpriteIndex++],i);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetPlayerCards(int[] card,int length)
    {
        
        for(int i=0;i<length;i++)
        {
            playercards.transform.GetChild(i).gameObject.SetActive(true);
            playercards.transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite = getIcon[card[i]];
        }
        for(int i=length;i<14;i++)
        {
            // if(card[i]==1000)continue;
            // playercards.transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite = getIcon[card[i]];
            playercards.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    public void SetLastCards(bool value,int index)
    {
        if(index == 14 || index == 11 || index == 8 ||index == 5 || index == 2)
        playercards.transform.GetChild(index-1).gameObject.SetActive(value);
    }
    public void SetLich()
    {
        for(int i=0;i<13;i++)
        playercards.transform.GetChild(i).GetComponentInChildren<Button>().interactable = false;;
    }
    public void SetWind(string text)
    {
        nowWind.text = text;
    }
}
