using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public UImanager uImanager;
    public GameObject Player1;
    public GameObject Player2;
    public GameObject Player3;
    public GameObject Player4;
    public List<GameObject> cardRiver1;
    public List<GameObject> cardRiver2;
    public List<GameObject> cardRiver3;
    public List<GameObject> cardRiver4;
    public Button checkButton;
    public Button CancelButton;
    public Button lichenButton;
    public Button eatButton;
    public Button ponButton;
    public Button gonButton;
    public Text playerPoints;
    int[] cardRiverCount = new int[4];
    List<Fang_Cal> player = new List<Fang_Cal>();
    int[] reach_player = new int[4];
    int[] points = new int[4];
    int now_turn = 0;
    int now_player = 0;
    bool has_op = false;
    bool winer = false;
    int dolla;
    int dolla_li;
    int round = 0;
    int add = 126;
    int[] card = new int[136];
    int[] random = new int[136];
    int card_pointer = 0;
    int stop = 122;
    int now_card_num = 0;
    int can_long = 0;
    int gon;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Player1.transform.childCount; i++)
        {
            cardRiver1.Add(Player1.transform.GetChild(i).gameObject);
            cardRiver2.Add(Player2.transform.GetChild(i).gameObject);
            cardRiver3.Add(Player3.transform.GetChild(i).gameObject);
            cardRiver4.Add(Player4.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < cardRiver1.Count; i++)
        {
            cardRiver1[i].SetActive(false);
            cardRiver2[i].SetActive(false);
            cardRiver3[i].SetActive(false);
            cardRiver4[i].SetActive(false);
        }
        for (int i = 0; i < 4; i++) cardRiverCount[i] = 0;
        for (int i = 0; i < 4; i++) player.Add(new Fang_Cal());
        reach_player = new int[4];
        points = new int[4];
        for (int i = 0; i < 4; i++) points[i] = 0;
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
                player[j].Add(card[card_pointer++]);
            }
        }

        round = 0;
        for (int i = 0; i < 4; i++) player[i].Reset();
        gencard(random, card);
        card_pointer = 0;
        stop = 122;
        add = 126;
        for (int i = 0; i < 13; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                player[j].Add(card[card_pointer++]);
            }
        }
        uImanager.SetPlayerCards(player[0].ReturnPlayerCard(), 13);
        // uImanager.SetLastCards(true);

        now_turn = 0;
        now_player = round;
        has_op = false;
        winer = false;
        dolla = card[add];
        dolla_li = card[add + 1];
        set_dolla();
        for (int i = 0; i < 4; i++) player[i].Add_dolla(dolla);
        for (int i = 0; i < 4; i++) player[i].Add_dolla_li(dolla_li);
        switch (round)
        {
            case 0:
                // cout << "東風局 座東風" << endl;
                uImanager.SetWind("東風局 座東風");
                player[0].set_winds(48, 48);
                player[1].set_winds(48, 56);
                player[2].set_winds(48, 64);
                player[3].set_winds(48, 72);
                break;
            case 1:
                // cout << "東風局 座北風" << endl;
                uImanager.SetWind("東風局 座北風");
                player[1].set_winds(48, 48);
                player[2].set_winds(48, 56);
                player[3].set_winds(48, 64);
                player[0].set_winds(48, 72);
                break;
            case 2:
                // cout << "東風局 座西風" << endl;
                uImanager.SetWind("東風局 座西風");
                player[2].set_winds(48, 48);
                player[3].set_winds(48, 56);
                player[0].set_winds(48, 64);
                player[1].set_winds(48, 72);
                break;
            case 3:
                // cout << "東風局 座南風" << endl;
                uImanager.SetWind("東風局 座南風");
                player[3].set_winds(48, 48);
                player[0].set_winds(48, 56);
                player[1].set_winds(48, 64);
                player[2].set_winds(48, 72);
                break;
            default:
                break;
        }
        StartRound();
    }
    void StartRound()
    {
        checkButton.interactable = false;
        CancelButton.interactable = false;
        lichenButton.interactable = false;
        eatButton.interactable = false;
        ponButton.interactable = false;
        gonButton.interactable = false;
        if (card_pointer >= stop)
        {
            Debug.Log("All over");
            Invoke("Restart", 3f);
            return;
        }
        if (has_op == false)
        {
            now_card_num = card[card_pointer++];
            player[now_player].bubble();
            player[now_player].Add(now_card_num);
            Debug.Log("拿到牌");
            // if (now_player == 0) cout << "拿到牌" << now_card_num << endl;
        }
        uImanager.SetPlayerCards(player[0].ReturnPlayerCard(), 13);
        // uImanager.SetLastCards(true);
        has_op = false;
        if (now_player == 0)
        {
            if (card_pointer >= stop) player[0].last_card = 1;
            now_turn++;
            FindObjectOfType<PlayerController>().SetSelect(true);
            do_operation(now_card_num);
        }
        else
        {
            AIPlay();
        }
    }
    public void EndRound(GameObject Usecard)
    {
        // uImanager.SetLastCards(false);
        cardRiver1[cardRiverCount[0]].SetActive(true);
        cardRiver1[cardRiverCount[0]].transform.GetChild(0).GetComponentInChildren<Image>().sprite = Usecard.transform.GetChild(1).GetComponent<Image>().sprite;

        int card_name = uImanager.getId[cardRiver1[cardRiverCount[0]].transform.GetChild(0).GetComponentInChildren<Image>().sprite];
        cardRiverCount[0]++;
        Debug.Log(card_name);
        player[0].Remove(card_name);
        player[0].Add_river(card_name);
        player[0].Reset_linshan();
        player[0].Reset_chankan();
        player[0].bubble();
        uImanager.SetPlayerCards(player[0].ReturnPlayerCard(), 13);
        now_player = (now_player + 1) % 4;
        StartRound();
    }
    void AIPlay()
    {
        player[now_player].Remove(now_card_num);
        if (card_pointer >= stop) player[0].last_card_other = 1;
        player[now_player].Add_river(now_card_num);
        switch (now_player)
        {
            case 1:
                cardRiver2[cardRiverCount[now_player]].SetActive(true);
                cardRiver2[cardRiverCount[now_player]].transform.GetChild(0).GetComponentInChildren<Image>().sprite = uImanager.getIcon[now_card_num];
                cardRiverCount[now_player]++;
                break;
            case 2:
                cardRiver3[cardRiverCount[now_player]].SetActive(true);
                cardRiver3[cardRiverCount[now_player]].transform.GetChild(0).GetComponentInChildren<Image>().sprite = uImanager.getIcon[now_card_num];
                cardRiverCount[now_player]++;
                break;
            case 3:
                cardRiver4[cardRiverCount[now_player]].SetActive(true);
                cardRiver4[cardRiverCount[now_player]].transform.GetChild(0).GetComponentInChildren<Image>().sprite = uImanager.getIcon[now_card_num];
                cardRiverCount[now_player]++;
                break;
        }
        now_player = (now_player + 1) % 4;
        StartRound();
    }
    // Update is called once per frame
    void Update()
    {

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
    void bubble(int[] arr, int len)
    {
        for (int i = 0; i < len; i++)
        {
            for (int j = 0; j < len - i - 1; j++)
            {
                if (arr[j] > arr[j + 1])
                {
                    // swap(arr[j], arr[j + 1]);
                    int tmp = arr[j];
                    arr[j] = arr[j + 1];
                    arr[j + 1] = tmp;
                }
            }
        }
    }
    int check_op(int my_input, int now_player)//0 for eat and pon, 1 for gon, -1 for no op
    {
        player[0].Add(my_input);
        player[0].Set_long();
        int can_long = player[now_player].Howmany_fang(my_input);


        if (can_long > 0)
        {
            // cout << "請問要胡牌嗎?(y/n)" << endl;
            char ans = ' ';
            // cin >> ans;
            while (ans != 'y' && ans != 'n')
            {
                // cout << "i don't know what is " << ans << endl;
                // cout << "input again,y or n?" << endl;
                // cin >> ans;
            }
            if (ans == 'y')
            {
                points[0] += can_long;
                // cout << "You win" << endl;
                // cout << "番數是" << can_long << endl;
                player[0].List_card_long();
                player[0].print_fang();
                return 4;
            }
        }
        player[0].Reset_long();

        player[0].Remove(my_input);
        int reach = player[0].Retrun_reach();
        if (reach == 1) return -1;
        bool gon = player[now_player].Check_gon(my_input, 1);
        if (gon == true)
        {
            player[now_player].List_card();
            // cout << "請問要槓牌嗎? 牌:" << my_input;
            // cout << "yes or no?(y/n)";
            char ans = ' ';
            // cin >> ans;
            if (ans == 'y')
            {
                player[now_player].Gon(my_input);
                return 1;
            }
        }
        bool pon = player[now_player].Check_pon(my_input);
        if (pon == true)
        {
            player[now_player].List_card();
            // cout << "請問要碰牌嗎? 牌：" << my_input;
            // cout << "\nyes or no?(y/n)";

            char ans = ' ';
            // cin >> ans;
            if (ans == 'y')
            {
                player[now_player].Pon(my_input);
                player[0].List_card();
                // cout << "請輸入要打的牌" << endl;
                int card_name = 0;
                // cin >> card_name;
                while (player[0].Count(card_name) == 0)
                {
                    // cout << "錯誤的牌" << endl;
                    // cout << "請再輸入一次" << endl;
                    // cin >> card_name;
                }
                player[0].Remove(card_name);
                player[0].Add_river(card_name);
                return 0;
            }
        }
        bool eat = player[now_player].Check_eat(my_input);
        if (eat == true && now_player == 3)
        {
            player[now_player].List_card();
            // cout << "請問要吃嗎? 牌:" << my_input;
            // cout << "\nyes or no?(y/n)";
            char ans = ' ';
            // cin >> ans;
            if (ans == 'y')
            {
                // cout << "請輸入要吃的牌" << endl;
                // cout << "input three number\n";
                int[] eat_who = new int[3];
                // cin >> eat_who[0] >> eat_who[1] >> eat_who[2];
                player[now_player].Eat(eat_who[0], eat_who[1], eat_who[2], my_input);
                player[0].List_card();
                // cout << "請輸入要打的牌" << endl;
                int card_name = 0;
                // cin >> card_name;
                while (player[0].Count(card_name) == 0)
                {
                    // cout << "錯誤的牌" << endl;
                    // cout << "請再輸入一次" << endl;
                    // cin >> card_name;
                }
                player[0].Remove(card_name);
                player[0].Add_river(card_name);
                return 0;
            }
        }
        return -1;
    }
    // void set_addition(int additional_card, int &additional)
    // {
    //     if (additional_card == 8) additional = 0;
    //     else if (additional_card == 24) additional = 16;
    //     else if (additional_card == 40) additional = 32;
    //     else if (additional_card == 48) additional = 56;
    //     else if (additional_card == 56) additional = 64;
    //     else if (additional_card == 64) additional = 72;
    //     else if (additional_card == 72) additional = 40;
    //     else if (additional_card == 80) additional = 88;
    //     else if (additional_card == 88) additional = 96;
    //     else if (additional_card == 96) additional = 80;
    //     else additional = additional_card + 1;
    //     return;
    // }
    int go_gon(int my_input)
    {
        player[0].List_card();
        // cout << "請問要槓牌嗎? 牌:" << my_input;
        // cout << "yes or no?(y/n)";
        char ans = ' ';
        // cin >> ans;
        if (ans == 'y')
        {
            player[0].Gon(my_input);
            return 1;
        }
        return 0;
    }
    void test_use(int[] card)
    {
        for (int i = 53; i < 136; i++) card[i] = 0;
    }
    //0 for 一般，1 for 立直, 2 for 吃碰 3 for 槓 4 for 胡牌
    int do_operation(int get_card)
    {
        can_long = player[0].Howmany_fang(get_card);
        player[0].one_fa = 0;
        if (can_long > 0)
        {
            // player[0].List_card();
            // cout << "請問要自摸嗎?(y/n)" << endl;
            checkButton.interactable = true;
            CancelButton.interactable = true;
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
            // return 4;
            // }
        }
        int reach = player[0].check_reach();
        if (player[0].Retrun_reach() == 1)
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
        if (reach == 1 && player[0].Retrun_reach() == 0)
        {
            lichenButton.interactable = true;
            CancelButton.interactable = true;
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



        gon = player[0].Check_gon_myself(get_card);
        if (gon >= 0)
        {
            gonButton.interactable = true;
            CancelButton.interactable = true;
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
            //     return 3;
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

    int main()
    {
        for (int i = 0; i < 4; i++) points[i] = 0;
        // system("chcp 65001");
        int add = 126;
        int[] card = new int[136];
        int[] random = new int[136];
        gencard(random, card);
        int card_pointer = 0;
        int stop = 122;
        for (int i = 0; i < 13; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                player[j].Add(card[card_pointer++]);
            }
        }
        //here
        for (int round = 0; round < 4; round++)
        {
            for (int i = 0; i < 4; i++) player[i].Reset();
            gencard(random, card);
            card_pointer = 0;
            stop = 122;
            add = 126;
            for (int i = 0; i < 13; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    player[j].Add(card[card_pointer++]);
                }
            }
            int now_turn = 0;
            int now_player = round;
            bool has_op = false;
            bool winer = false;
            dolla = card[add];
            dolla_li = card[add + 1];
            set_dolla();
            for (int i = 0; i < 4; i++) player[i].Add_dolla(dolla);
            for (int i = 0; i < 4; i++) player[i].Add_dolla_li(dolla_li);
            //here
            switch (round)
            {
                case 0:
                    // cout << "東風局 座東風" << endl;
                    player[0].set_winds(48, 48);
                    player[1].set_winds(48, 56);
                    player[2].set_winds(48, 64);
                    player[3].set_winds(48, 72);
                    break;
                case 1:
                    // cout << "東風局 座北風" << endl;
                    player[1].set_winds(48, 48);
                    player[2].set_winds(48, 56);
                    player[3].set_winds(48, 64);
                    player[0].set_winds(48, 72);
                    break;
                case 2:
                    // cout << "東風局 座西風" << endl;
                    player[2].set_winds(48, 48);
                    player[3].set_winds(48, 56);
                    player[0].set_winds(48, 64);
                    player[1].set_winds(48, 72);
                    break;
                case 3:
                    // cout << "東風局 座南風" << endl;
                    player[3].set_winds(48, 48);
                    player[0].set_winds(48, 56);
                    player[1].set_winds(48, 64);
                    player[2].set_winds(48, 72);
                    break;
                default:
                    break;
            }
            //Here
            while (card_pointer < stop)
            {
                // cout << "剩下 " << stop - card_pointer << "張" << endl;
                int now_card_num = 0;
                if (has_op == false)
                {

                    now_card_num = card[card_pointer++];
                    player[now_player].Add(now_card_num);
                    // if (now_player == 0) cout << "拿到牌" << now_card_num << endl;
                }
                has_op = false;
                if (now_player == 0)
                {
                    for (int i = 0; i < 4; i++) player[i].List_card_river();
                    if (card_pointer >= stop) player[0].last_card = 1;
                    //0 for 一般，1 for 立直, 2 for 吃碰 3 for 槓 4 for 胡牌
                    int ans = do_operation(now_card_num);
                    player[0].List_card();
                    if (ans == 1)
                    {
                        if (now_turn == 0) player[0].Set_double_reach();
                        else player[0].Set_reach();
                    }
                    if (ans == 3)
                    {
                        add += 2;
                        dolla = card[add];
                        dolla_li = card[add + 1];
                        set_dolla();
                        for (int i = 0; i < 4; i++) player[i].Add_dolla(dolla);
                        for (int i = 0; i < 4; i++) player[i].Add_dolla_li(dolla_li);
                        player[now_player].Set_linshan();
                        continue;
                    }
                    if (ans == 4)
                    {
                        winer = true;
                        break;
                    }
                    player[now_player].Reset_linshan();
                    player[now_player].Reset_chankan();
                    now_turn++;
                }
                else
                {
                    player[now_player].Remove(now_card_num);
                    // cout << "player" << now_player << "丟出" << now_card_num << endl;
                    //player[0].List_card();
                    if (card_pointer >= stop) player[0].last_card_other = 1;
                    int reaction = check_op(now_card_num, 0);
                    if (reaction == -1)
                    {
                        player[now_player].Add_river(now_card_num);
                    }
                    else if (reaction == 1)
                    {
                        dolla = card[add];
                        dolla_li = card[add + 1];
                        set_dolla();
                        for (int i = 0; i < 4; i++) player[i].Add_dolla(dolla);
                        for (int i = 0; i < 4; i++) player[i].Add_dolla_li(dolla_li);
                        has_op = false;
                        now_player = 3;
                    }
                    else if (reaction == 4)
                    {
                        winer = true;
                        break;
                    }
                    else if (reaction == 0) now_player = 0;
                }
                now_player++;
                now_player = now_player % 4;
            }
            if (winer == false)
            {
                // cout << "no winer" << endl;
                // cout << "your card" << endl;
                player[0].List_card();
            }
            char tmp;
            // cout << "按y繼續" << endl;
            // cin >> tmp;
        }
        // cout << "遊戲結束" << endl;
        // cout << "你的分數： " << points[0] << endl;
        // system("pause");
        return 0;
    }
    void Restart()
    {
        Debug.Log("Restart");
        cardRiver1.Clear();
        cardRiver2.Clear();
        cardRiver3.Clear();
        cardRiver4.Clear();
        for (int i = 0; i < Player1.transform.childCount; i++)
        {
            cardRiver1.Add(Player1.transform.GetChild(i).gameObject);
            cardRiver2.Add(Player2.transform.GetChild(i).gameObject);
            cardRiver3.Add(Player3.transform.GetChild(i).gameObject);
            cardRiver4.Add(Player4.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < cardRiver1.Count; i++)
        {
            cardRiver1[i].SetActive(false);
            cardRiver2[i].SetActive(false);
            cardRiver3[i].SetActive(false);
            cardRiver4[i].SetActive(false);
        }
        for (int i = 0; i < 4; i++) cardRiverCount[i] = 0;
        player.Clear();
        for (int i = 0; i < 4; i++) player.Add(new Fang_Cal());
        reach_player = new int[4];
        points = new int[4];
        for (int i = 0; i < 4; i++) points[i] = 0;
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
                player[j].Add(card[card_pointer++]);
            }
        }

        round = round + 1;
        for (int i = 0; i < 4; i++) player[i].Reset();
        gencard(random, card);
        card_pointer = 0;
        stop = 122;
        add = 126;
        for (int i = 0; i < 13; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                player[j].Add(card[card_pointer++]);
            }
        }
        uImanager.SetPlayerCards(player[0].ReturnPlayerCard(), 13);
        // uImanager.SetLastCards(true);

        now_turn = 0;
        now_player = round;
        has_op = false;
        winer = false;
        dolla = card[add];
        dolla_li = card[add + 1];
        set_dolla();
        for (int i = 0; i < 4; i++) player[i].Add_dolla(dolla);
        for (int i = 0; i < 4; i++) player[i].Add_dolla_li(dolla_li);
        switch (round)
        {
            case 0:
                // cout << "東風局 座東風" << endl;
                uImanager.SetWind("東風局 座東風");
                player[0].set_winds(48, 48);
                player[1].set_winds(48, 56);
                player[2].set_winds(48, 64);
                player[3].set_winds(48, 72);
                break;
            case 1:
                // cout << "東風局 座北風" << endl;
                uImanager.SetWind("東風局 座北風");
                player[1].set_winds(48, 48);
                player[2].set_winds(48, 56);
                player[3].set_winds(48, 64);
                player[0].set_winds(48, 72);
                break;
            case 2:
                // cout << "東風局 座西風" << endl;
                uImanager.SetWind("東風局 座西風");
                player[2].set_winds(48, 48);
                player[3].set_winds(48, 56);
                player[0].set_winds(48, 64);
                player[1].set_winds(48, 72);
                break;
            case 3:
                // cout << "東風局 座南風" << endl;
                uImanager.SetWind("東風局 座南風");
                player[3].set_winds(48, 48);
                player[0].set_winds(48, 56);
                player[1].set_winds(48, 64);
                player[2].set_winds(48, 72);
                break;
            default:
                break;
        }
        StartRound();
    }
    public void selflon()
    {
        points[0] += can_long;
        playerPoints.text = "player 1" + points[0] + "player 2" + points[1] + "player 3" + points[2] +
        "player 4" + points[3];
        winer = true;
        Invoke("Restart", 3f);
        // cout << "You win" << endl;
        // cout << "番數是" << can_long << endl;
        // player[0].List_card_long();
        // player[0].print_fang();
    }
    public void lichen()
    {
        reach_player[0] = 1;
        // cout << "請輸入要打的牌" << endl;
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
        player[0].one_fa = 1;
        if(now_turn==0)player[0].Set_double_reach();
        else player[0].Set_reach();
    }
    public void Gon()
    {
        
        player[0].Gon(gon);
        now_player = 0;
        has_op = false;
        add += 2;
        dolla = card[add];
        dolla_li = card[add+1];
        set_dolla();
        for(int i=0;i<4;i++)player[i].Add_dolla(dolla);
        for(int i=0;i<4;i++)player[i].Add_dolla_li(dolla_li);
        player[now_player].Set_linshan();
        StartRound();
    }
}
