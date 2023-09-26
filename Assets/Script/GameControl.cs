using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    public List<GameObject> PlayerRiver;
    public List<GameObject> DollorRiver;
    public List<GameObject> LiDollorRiver;
    public List<List<GameObject>> CardRiver = new List<List<GameObject>>();
    public GameObject longpanel;
    public Text longtext;
    public UImanager uImanager;
    public Button checkButton;
    public Button CancelButton;
    public Button lichenButton;
    public Button eatButton;
    public Button ponButton;
    public Button gonButton;
    public Button lonButton;
    public Button leaveButton;
    public Button againButton;
    public PlayerController playerController;
    public Text playerPoints;
    public Text lastcard;
    int[] CardRiverCount = new int[4];
    int[] Points = new int[4];
    List<Fang_Cal> Player = new List<Fang_Cal>();
    int add;
    int[] card = new int[136];
    int[] random = new int[136];
    int card_pointer = 0;
    int stop;
    int round;
    int now_turn;
    public int now_player;
    public bool has_op = false;
    bool winer = false;
    int dolla;
    int dolla_li;
    int now_card_num;
    int can_long;
    int gon;
    int my_input;

    // Start is called before the first frame update
    void Start()
    {
        round = -1;
        init();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void init()
    {
        FindObjectOfType<EatCardController>().init();
        FindObjectOfType<SetEatCard>().init();
        longpanel.SetActive(false);
        LiDollorRiver[0].transform.parent.gameObject.SetActive(false);
        
        CardRiver.Clear();
        for (int i = 0; i < 4; i++)
        {
            DollorRiver[i].SetActive(false);
            LiDollorRiver[i].SetActive(false);
            CardRiver.Add(new List<GameObject>());
        }
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < PlayerRiver[i].transform.childCount; j++)
            {
                CardRiver[i].Add(PlayerRiver[i].transform.GetChild(j).gameObject);
            }
        }
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < CardRiver[i].Count; j++)
            {
                CardRiver[i][j].SetActive(false);
            }
        }
        // for (int i = 0; i < Player1.transform.childCount; i++)
        // {
        //     cardRiver1.Add(Player1.transform.GetChild(i).gameObject);
        //     cardRiver2.Add(Player2.transform.GetChild(i).gameObject);
        //     cardRiver3.Add(Player3.transform.GetChild(i).gameObject);
        //     cardRiver4.Add(Player4.transform.GetChild(i).gameObject);
        // }
        // for (int i = 0; i < cardRiver1.Count; i++)
        // {
        //     cardRiver1[i].SetActive(false);
        //     cardRiver2[i].SetActive(false);
        //     cardRiver3[i].SetActive(false);
        //     cardRiver4[i].SetActive(false);
        // }
        for (int i = 0; i < 4; i++) CardRiverCount[i] = 0;
        Player.Clear();
        for (int i = 0; i < 4; i++) Player.Add(new Fang_Cal());
        for (int i = 0; i < 4; i++) Points[i] = 0;
        // system("chcp 65001");
        add = 126;
        card = new int[136];
        random = new int[136];
        gencard(random, card);
        card_pointer = 0;
        stop = 122;
        for (int i = 0; i < 13; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                Player[j].Add(card[card_pointer++]);
            }
        }

        round = round + 1;
        if(round == 4)
        {
            longpanel.SetActive(true);
            longtext.text = "遊戲結束";
            leaveButton.gameObject.SetActive(true);
            againButton.gameObject.SetActive(true);
            return;
        }
        for (int i = 0; i < 4; i++) Player[i].Reset();
        gencard(random, card);
        card_pointer = 0;
        stop = 122;
        add = 126;
        for (int i = 0; i < 13; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                Player[j].Add(card[card_pointer++]);
            }
        }
        Player[0].bubble();
        Debug.Log("now card is " + Player[0].now_cards);
        uImanager.SetPlayerCards(Player[0].ReturnPlayerCard(), Player[0].now_cards);
        // uImanager.SetLastCards(true);

        now_turn = 0;
        now_player = round;
        has_op = false;
        winer = false;
        dolla = card[add];
        dolla_li = card[add + 1];
        set_dolla_ui();
        set_dolla();
        
        for (int i = 0; i < 4; i++) Player[i].Add_dolla(dolla);
        for (int i = 0; i < 4; i++) Player[i].Add_dolla_li(dolla_li);
        switch (round)
        {
            case 0:
                // cout << "東風局 座東風" << endl;
                uImanager.SetWind("東風局 座東風");
                Player[0].set_winds(48, 48);
                Player[1].set_winds(48, 56);
                Player[2].set_winds(48, 64);
                Player[3].set_winds(48, 72);
                break;
            case 1:
                // cout << "東風局 座北風" << endl;
                uImanager.SetWind("東風局 座北風");
                Player[1].set_winds(48, 48);
                Player[2].set_winds(48, 56);
                Player[3].set_winds(48, 64);
                Player[0].set_winds(48, 72);
                break;
            case 2:
                // cout << "東風局 座西風" << endl;
                uImanager.SetWind("東風局 座西風");
                Player[2].set_winds(48, 48);
                Player[3].set_winds(48, 56);
                Player[0].set_winds(48, 64);
                Player[1].set_winds(48, 72);
                break;
            case 3:
                // cout << "東風局 座南風" << endl;
                uImanager.SetWind("東風局 座南風");
                Player[3].set_winds(48, 48);
                Player[0].set_winds(48, 56);
                Player[1].set_winds(48, 64);
                Player[2].set_winds(48, 72);
                break;
            default:
                break;
        }
        StartRound();
    }
    public void StartRound()
    {
        checkButton.interactable = false;
        CancelButton.interactable = false;
        lichenButton.interactable = false;
        eatButton.interactable = false;
        ponButton.interactable = false;
        lonButton.interactable = false;
        gonButton.interactable = false;
        if (card_pointer >= stop)
        {
            Debug.Log("All over");
            Invoke("init", 3f);
            return;
        }
        bool getCard = false;
        if (has_op == false)
        {
            getCard = true;
            now_card_num = card[card_pointer++];
            Player[now_player].bubble();
            Player[now_player].Add(now_card_num);
            Debug.Log("拿到牌");
            // if (now_player == 0) cout << "拿到牌" << now_card_num << endl;
        }
        lastcard.text = "剩下" + (stop - card_pointer) + "張";
        has_op = false;
        if (now_player == 0)
        {
            uImanager.SetPlayerCards(Player[0].ReturnPlayerCard(), Player[0].now_cards);
            uImanager.SetLastCards(true,Player[0].now_cards);
            // Invoke(nameof(AIPlay), 3f);return;
            if (card_pointer >= stop) Player[0].last_card = 1;
            Debug.Log("now card in start start is " + Player[0].now_cards);
            int ans = 0;
            int tmp = Player[0].now_cards;
            if(getCard == true) 
            {
                ans = do_operation(now_card_num);
            }
            if (ans == 4)
            {
                checkButton.interactable = true;
                CancelButton.interactable = true;
            }
            if (ans == 0)
            {
                // for(int k=0;k<13;k++)
                // uImanager.SetLich();
            }
            if (ans == 1)
            {
                lichenButton.interactable = true;
                CancelButton.interactable = true;
            }
            if (ans == 3)
            {
                gonButton.interactable = true;
                CancelButton.interactable = true;
            }
            // Debug.Log("Here");
            playerController.SetSelect(true);
            now_turn++;
            // do_operation(now_card_num);
        }
        else
        {
            Invoke(nameof(AIPlay), 1f);
        }
        Debug.Log("now card in start end is " + Player[0].now_cards);
    }
    public void EndRound(GameObject Usecard)
    {
        CardRiver[0][CardRiverCount[0]].SetActive(true);
        CardRiver[0][CardRiverCount[0]].transform.GetChild(0).GetComponentInChildren<Image>().sprite = Usecard.transform.GetChild(1).GetComponent<Image>().sprite;

        int card_name = uImanager.getId[CardRiver[0][CardRiverCount[0]].transform.GetChild(0).GetComponentInChildren<Image>().sprite];
        CardRiverCount[0]++;
        Debug.Log(card_name);
        Debug.Log("now card is " + Player[0].now_cards);
        uImanager.SetLastCards(false,Player[0].now_cards);
        Player[0].Remove(card_name);
        Player[0].Add_river(card_name);
        Player[0].Reset_linshan();
        Player[0].Reset_chankan();
        Player[0].bubble();
        uImanager.SetPlayerCards(Player[0].ReturnPlayerCard(), Player[0].now_cards);
        now_player = (now_player + 1) % 4;
        StartRound();
    }
    
    int do_operation(int get_card)
    {
        can_long = Player[0].Howmany_fang(get_card);
        Player[0].one_fa = 0;
        if (can_long > 0)
        {
            // player[0].List_card();
            // cout << "請問要自摸嗎?(y/n)" << endl;

            // char ans = ' ';
            // // cin >> ans;
            // while (ans != 'y' && ans != 'n')
            // {
            //     // cout << "i don't know what is " << ans << endl;
            //     // cout << "input again,y or n?" << endl;
            //     // cin >> ans;
            // }
            // if (ans == 'y')
            // {
            // points[0] += can_long;
            // // cout << "You win" << endl;
            // // cout << "番數是" << can_long << endl;
            // player[0].List_card_long();
            // player[0].print_fang();
            return 4;
            // }
        }
        int reach = Player[0].check_reach();
        if (Player[0].Retrun_reach() == 1)
        {

            // player[0].List_card();
            // // cout << "按y繼續" << endl;
            // char y = ' ';
            // // cin >> y;
            // while (y != 'y')
            // {
            //     // cout << "按y拉幹" << endl;
            //     // cin >> y;
            // }
            // player[0].Remove(get_card);
            // player[0].Add_river(get_card);
            return 0;
        }
        if (reach == 1 && Player[0].Retrun_reach() == 0)
        {

            // player[0].List_card();
            // // cout << "請問要立直嗎?(y/n)" << endl;
            // char ans = ' ';
            // // cin >> ans;
            // while (ans != 'y' && ans != 'n')
            // {
            //     // cout << "i don't know what is " << ans << endl;
            //     // cout << "input again,y or n?" << endl;
            //     // cin >> ans;
            // }
            // if (ans == 'y')
            {
                // reach_player[0] = 1;
                // // cout << "請輸入要打的牌" << endl;
                // int card_name2 = 0;
                // // cin >> card_name;
                // while (player[0].Count(card_name2) == 0)
                // {
                //     // cout << "錯誤的牌" << endl;
                //     // cout << "請再輸入一次" << endl;
                //     // cin >> card_name;
                // }
                // player[0].Remove(card_name2);
                // player[0].Add_river(card_name2);
                // player[0].one_fa = 1;
                return 1;
            }
        }



        gon = Player[0].Check_gon_myself(get_card);
        if (gon >= 0)
        {

            // player[0].List_card();
            // // cout << "請問要槓牌嗎?(y/n)" << "牌：　" << gon << endl;
            // char ans = ' ';
            // // cin >> ans;
            // while (ans != 'y' && ans != 'n')
            // {
            //     // cout << "i don't know what is " << ans << endl;
            //     // cout << "input again,y or n?" << endl;
            //     // cin >> ans;
            // }
            // if (ans == 'y')
            // {
            //     // player[0].Gon(gon);
            //     // now_player = 3;
            //     // has_op = false;
            return 3;
            // }
        }


        // player[0].List_card();
        // // cout << "請輸入要打的牌" << endl;
        // int card_name = 0;
        // // cin >> card_name;
        // while (player[0].Count(card_name) == 0)
        // {
        //     // cout << "錯誤的牌" << endl;
        //     // cout << "請再輸入一次" << endl;
        //     // cin >> card_name;
        // }
        // player[0].Remove(card_name);
        // player[0].Add_river(card_name);
        return 0;
    }
    void AIPlay()
    {
        int throwcard = Player[now_player].selectCard();
        Player[now_player].Remove(throwcard);
        if (card_pointer >= stop) Player[0].last_card_other = 1;
        switch (now_player)
            {
                case 0:
                    CardRiver[0][CardRiverCount[now_player]].SetActive(true);
                    CardRiver[0][CardRiverCount[now_player]].transform.GetChild(0).GetComponentInChildren<Image>().sprite = uImanager.getIcon[throwcard];
                    CardRiverCount[now_player]++;
                    int card_name = uImanager.getId[CardRiver[0][CardRiverCount[0]].transform.GetChild(0).GetComponentInChildren<Image>().sprite];
                    uImanager.SetLastCards(false,Player[0].now_cards);
                    Player[0].Remove(card_name);
                    Player[0].Add_river(card_name);
                    Player[0].Reset_linshan();
                    Player[0].Reset_chankan();
                    Player[0].bubble();
                    uImanager.SetPlayerCards(Player[0].ReturnPlayerCard(), Player[0].now_cards);
                    break;
                case 1:
                    CardRiver[1][CardRiverCount[now_player]].SetActive(true);
                    CardRiver[1][CardRiverCount[now_player]].transform.GetChild(0).GetComponentInChildren<Image>().sprite = uImanager.getIcon[throwcard];
                    CardRiverCount[now_player]++;
                    break;
                case 2:
                    CardRiver[2][CardRiverCount[now_player]].SetActive(true);
                    CardRiver[2][CardRiverCount[now_player]].transform.GetChild(0).GetComponentInChildren<Image>().sprite = uImanager.getIcon[throwcard];
                    CardRiverCount[now_player]++;
                    break;
                case 3:
                    CardRiver[3][CardRiverCount[now_player]].SetActive(true);
                    CardRiver[3][CardRiverCount[now_player]].transform.GetChild(0).GetComponentInChildren<Image>().sprite = uImanager.getIcon[throwcard];
                    CardRiverCount[now_player]++;
                    break;
            }
        int reaction = check_op(throwcard, 0);
        // int reaction = -1;
        
        if (reaction == -1)
        {
            Player[now_player].Add_river(throwcard);
            
            now_player = (now_player + 1) % 4;
            StartRound();
        }
        if(reaction == 4)
        {
            // lonButton.interactable = true;
            // CancelButton.interactable = true;
        }
        if(reaction == 1)
        {
            // gonButton.interactable = true;
            // CancelButton.interactable = true;
        }
        if(reaction == 0)
        {
            // ponButton.interactable = true;
            // CancelButton.interactable = true;
        }
        if(reaction == 2)
        {
            // eatButton.interactable = true;
            // CancelButton.interactable = true;
        }

        
    }
    int check_op(int my_input, int now_player)//0 for eat and pon, 1 for gon, -1 for no op
    {
        int option = 0;
        Player[0].Add(my_input);
        Player[0].Set_long();
        can_long = Player[now_player].Howmany_fang(my_input);
        this.my_input = my_input;
        if (can_long > 0)
        {
            lonButton.interactable = true;
            CancelButton.interactable = true;
            option = 1;
            // cout << "請問要胡牌嗎?(y/n)" << endl;
            // char ans = ' ';
            // // cin >> ans;
            // while (ans != 'y' && ans != 'n')
            // {
            //     // cout << "i don't know what is " << ans << endl;
            //     // cout << "input again,y or n?" << endl;
            //     // cin >> ans;
            // }
            // if (ans == 'y')
            {
                // Points[0] += can_long;
                // cout << "You win" << endl;
                // cout << "番數是" << can_long << endl;
                // Player[0].List_card_long();
                // Player[0].print_fang();
                // return 4;
            }
        }
        Player[0].Reset_long();

        Player[0].Remove(my_input);
        int reach = Player[0].Retrun_reach();

        bool gon = Player[now_player].Check_gon(my_input, 1);
        if (gon == true && reach != 1)
        {
            option = 1;
            // Player[now_player].List_card();
            // cout << "請問要槓牌嗎? 牌:" << my_input;
            // cout << "yes or no?(y/n)";
            // char ans = ' ';
            // cin >> ans;
            // if (ans == 'y')
            // {
                this.gon = my_input;
                gonButton.interactable = true;
                CancelButton.interactable = true;
                // Player[now_player].Gon(my_input);
                // return 1;
            // }
        }
        bool pon = Player[now_player].Check_pon(my_input);
        if (pon == true&& reach != 1)
        {
            // Player[now_player].List_card();
            option = 1;
            ponButton.interactable = true;
            CancelButton.interactable = true;
            // // cout << "請問要碰牌嗎? 牌：" << my_input;
            // // cout << "\nyes or no?(y/n)";

            // char ans = ' ';
            // // cin >> ans;
            // if (ans == 'y')
            // {
            //     Player[now_player].Pon(my_input);
            //     Player[0].List_card();
            //     // cout << "請輸入要打的牌" << endl;
            //     int card_name = 0;
            //     // cin >> card_name;
            //     while (Player[0].Count(card_name) == 0)
            //     {
            //         // cout << "錯誤的牌" << endl;
            //         // cout << "請再輸入一次" << endl;
            //         // cin >> card_name;
            //     }
            //     Player[0].Remove(card_name);
            //     Player[0].Add_river(card_name);
                // return 0;
            // }
        }
        bool eat = Player[now_player].Check_eat(my_input);
        if (eat == true && this.now_player == 3&& reach != 1)
        {
            // Player[now_player].List_card();
            // // cout << "請問要吃嗎? 牌:" << my_input;
            // // cout << "\nyes or no?(y/n)";
            // char ans = ' ';
            // // cin >> ans;
            // if (ans == 'y')
            // {
            //     // cout << "請輸入要吃的牌" << endl;
            //     // cout << "input three number\n";
            //     int[] eat_who = new int[3];
            //     // cin >> eat_who[0] >> eat_who[1] >> eat_who[2];
            //     Player[now_player].Eat(eat_who[0], eat_who[1], eat_who[2], my_input);
            //     Player[0].List_card();
            //     // cout << "請輸入要打的牌" << endl;
            //     int card_name = 0;
            //     // cin >> card_name;
            //     while (Player[0].Count(card_name) == 0)
            //     {
            //         // cout << "錯誤的牌" << endl;
            //         // cout << "請再輸入一次" << endl;
            //         // cin >> card_name;
            //     }
            //     Player[0].Remove(card_name);
            //     Player[0].Add_river(card_name);
            option = 1;
            eatButton.interactable = true;
            CancelButton.interactable = true;
                // return 2;
            // }
        }
        if(option == 1)return 0;
        return -1;
    }

    void gencard(int[] random, int[] card)
    {
        int t = 0;
        // srand(time(NULL));
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                card[t++] = i;
            }
        }
        for (int i = 16; i < 25; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                card[t++] = i;
            }
        }
        for (int i = 32; i < 41; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                card[t++] = i;
            }
        }
        for (int j = 0; j < 4; j++)
        {
            card[t++] = 48;
            card[t++] = 56;
            card[t++] = 64;
            card[t++] = 72;
            card[t++] = 80;
            card[t++] = 88;
            card[t++] = 96;
        }
        for (int i = 0; i < 136; i++) random[i] = Random.Range(0, 1000);
        for (int i = 0; i < 136; i++)
        {
            for (int j = 0; j < 136 - i - 1; j++)
            {
                if (random[j] > random[j + 1])
                {
                    int tmp = random[j];
                    random[j] = random[j + 1];
                    random[j + 1] = tmp;
                    // swap(random[j], random[j + 1]);
                    // swap(card[j], card[j + 1]);
                    tmp = card[j];
                    card[j] = card[j + 1];
                    card[j + 1] = tmp;
                }
            }
        }
    }
    void set_dolla()
    {
        if (dolla < 8) dolla = dolla + 1;
        else if (16 <= dolla && dolla < 24) dolla = dolla + 1;
        else if (32 <= dolla && dolla < 40) dolla = dolla + 1;
        else if (dolla == 8) dolla = 0;
        else if (dolla == 24) dolla = 16;
        else if (dolla == 40) dolla = 32;
        else if (dolla >= 48 && dolla < 72) dolla = dolla + 8;
        else if (dolla == 72) dolla = 48;
        else if (dolla >= 80 && dolla < 96) dolla = dolla + 8;
        else if (dolla == 96) dolla = 80;

        if (dolla_li < 8) dolla_li = dolla_li + 1;
        else if (16 <= dolla_li && dolla_li < 24) dolla_li = dolla_li + 1;
        else if (32 <= dolla_li && dolla_li < 40) dolla_li = dolla_li + 1;
        else if (dolla_li == 8) dolla_li = 0;
        else if (dolla_li == 24) dolla_li = 16;
        else if (dolla_li == 40) dolla_li = 32;
        else if (dolla_li >= 48 && dolla_li < 72) dolla_li = dolla_li + 8;
        else if (dolla_li == 72) dolla_li = 48;
        else if (dolla_li >= 80 && dolla_li < 96) dolla_li = dolla_li + 8;
        else if (dolla_li == 96) dolla_li = 80;
    }
    void set_dolla_ui()
    {
        for (int i = 0; i < 4; i++)
        {
            if (DollorRiver[i].activeSelf == false)
            {
                DollorRiver[i].SetActive(true);
                LiDollorRiver[i].SetActive(true);
                DollorRiver[i].transform.GetChild(1).GetComponent<Image>().sprite = uImanager.getIcon[dolla];
                LiDollorRiver[i].transform.GetChild(1).GetComponent<Image>().sprite = uImanager.getIcon[dolla_li];
                return;
            }
        }
    }
    void Set_dolla_li_ui()
    {
        LiDollorRiver[0].transform.parent.gameObject.SetActive(true);
    }
    public void liche()
    {
        Player[0].one_fa = 1;
        if (now_turn == 0) Player[0].Set_double_reach();
        else Player[0].Set_reach();
    }
    public void selflon()
    {
        Points[0] += can_long;
        playerPoints.text = "player1:" + Points[0] + "player2:" + Points[1] + "\nplayer3:" + Points[2] +
        "player4:" + Points[3];
        winer = true;
        string ans = Player[0].print_fang();
        if(Player[0].Retrun_reach()==1)
        {
            Set_dolla_li_ui();
        }
        lonButton.interactable = false;
        CancelButton.interactable = false;
        longpanel.SetActive(true);
        longtext.text = ans;
        Invoke("init",10f);

        // Invoke("init", 3f);
        // cout << "You win" << endl;
        // cout << "番數是" << can_long << endl;
        // player[0].List_card_long();
        // player[0].print_fang();
    }
    public void Gon()
    {
        Player[0].Gon(gon);
        has_op = false;
        add += 2;
        dolla = card[add];
        dolla_li = card[add + 1];
        set_dolla_ui();
        set_dolla();
        for (int i = 0; i < 4; i++) Player[i].Add_dolla(dolla);
        for (int i = 0; i < 4; i++) Player[i].Add_dolla_li(dolla_li);
        Player[0].Set_linshan();
        
        FindObjectOfType<SetEatCard>().SetCard("Gon",gon);
        now_player = 0;
        StartRound();
    }
    public void Pon()
    {
        switch (now_player)
        {
            case 1:
                CardRiverCount[now_player]--;
                CardRiver[1][CardRiverCount[now_player]].SetActive(false);
                // CardRiver[1][CardRiverCount[now_player]].transform.GetChild(0).GetComponentInChildren<Image>().sprite = uImanager.getIcon[now_card_num];
                break;
            case 2:
                CardRiverCount[now_player]--;
                CardRiver[2][CardRiverCount[now_player]].SetActive(false);
                // CardRiver[2][CardRiverCount[now_player]].transform.GetChild(0).GetComponentInChildren<Image>().sprite = uImanager.getIcon[now_card_num];
                break;
            case 3:
                CardRiverCount[now_player]--;
                CardRiver[3][CardRiverCount[now_player]].SetActive(false);
                // CardRiver[3][CardRiverCount[now_player]].transform.GetChild(0).GetComponentInChildren<Image>().sprite = uImanager.getIcon[now_card_num];
                break;
        }
        Player[0].Pon(my_input);
        has_op = true;
        now_player = 0;
        FindObjectOfType<SetEatCard>().SetCard("Pon",my_input);
        StartRound();
    }
    public void Eat()
    {
        FindObjectOfType<EatCardController>().EnablePanel(Player[0],my_input);
    }
    public void Cancel()
    {
        checkButton.interactable = false;
        CancelButton.interactable = false;
        lichenButton.interactable = false;
        eatButton.interactable = false;
        ponButton.interactable = false;
        gonButton.interactable = false;
        lonButton.interactable = false;
        now_player = (now_player + 1)%4;
        StartRound();
    }
    public void Again()
    {
        SceneManager.LoadScene("SampleScene"); 
    }
    public void Leave()
    {
        Application.Quit();
    }
}

