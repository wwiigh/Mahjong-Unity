using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Player
{

    /* data */
    protected int[] river_array = new int[30];
    protected int river_array_index;
    protected int[] card = new int[20];
    public int now_cards;
    protected int[] pon = new int[4];
    protected int[] gon = new int[4];
    protected int[,] eat = new int[4, 3];
    protected int pon_index;
    protected int gon_index;
    protected int eat_index;
    protected int reach;
    protected int double_reach;
    protected int do_op;

    protected int check_linshan;
    protected int check_chankan;
    //東南西北
    protected int guest_winds;
    protected int field_winds;

    //和他人
    protected int long_other;
    protected List<int> card_river = new List<int>();
    protected List<int> use_card = new List<int>();
    protected List<int> yee = new List<int>();//疫牌
    protected List<int> dolla_arr = new List<int>();//寶牌
    protected List<int> dolla_arr_li = new List<int>();//寶牌
    protected List<int> cannot_long = new List<int>();//振聽

    public int one_fa;
    public int last_card;
    public int last_card_other;
    public int first_card_start;//天和
    public int first_card_after;//地和
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public Player()
    {
        for (int i = 0; i < 20; i++) card[i] = 1000;
        long_other = 0;
        now_cards = 0;
        pon_index = 0;
        gon_index = 0;
        eat_index = 0;
        reach = 0;
        double_reach = 0;
        do_op = 0;
        one_fa = 0;
        last_card = 0;
        last_card_other = 0;
        check_chankan = 0;
        check_linshan = 0;
        first_card_after = 0;
        first_card_start = 0;
        yee.Add(80);
        yee.Add(88);
        yee.Add(96);
        river_array_index = 0;
    }
    public void Add_dolla(int dora)
    {
        if (dolla_arr.Contains(dora) == false)
            dolla_arr.Add(dora);
        return;
    }
    public void Add_dolla_li(int dora)
    {
        if (dolla_arr_li.Contains(dora) == false)
            dolla_arr_li.Add(dora);
        return;
    }
    public void Add(int card_name)
    {
        card[now_cards] = card_name;
        now_cards++;
    }
    public bool Remove(int card_name)
    {
        int now = -1;
        for (int i = 0; i < now_cards; i++)
        {
            if (card[i] == card_name)
            {
                now = i;
                card[i] = 1000;
                break;
            }
        }
        if (now == -1)
        {
            //cout<<"error happen in remove "<<card_name<<endl;
            return false;
        }
        now_cards--;
        bubble(card, 20);
        return true;
    }
    public int Count(int card_name)
    {
        int ans = 0;
        for (int i = 0; i < now_cards; i++)
        {
            if (card[i] == card_name) ans++;
        }
        return ans;
    }
    public bool Check_long()
    {
        List<int> more_than_two = new List<int>();
        int[] tmp = new int[20];
        int tmp_cards = now_cards;
        for (int i = 0; i < now_cards; i++)
        {
            if (Count(card[i]) >= 2)
            {
                if (more_than_two.Contains(card[i]) == false)
                    more_than_two.Add(card[i]);
            }
            tmp[i] = card[i];
        }
        while (more_than_two.Count > 0)
        {
            int top = more_than_two[0];
            more_than_two.Remove(top);
            //cout<<"now check "<<top<<endl;
            Remove(top);
            Remove(top);

            for (int i = 0; i < now_cards; i++)
            {
                if (Count(card[i]) >= 3)
                {
                    //cout<<"i find three "<<card[i]<<endl;
                    Remove(card[i]);
                    Remove(card[i]);
                    Remove(card[i]);
                    i = -1;
                }
            }
            bool ok = true;
            while (now_cards > 0)
            {
                //cout<<"now cards "<<now_cards<<endl;
                int now_num = card[0];
                //cout<<"now num "<<now_num<<endl;
                if (Count(now_num + 1) == 0 || Count(now_num + 2) == 0)
                {
                    //cout<<"no long"<<endl;
                    ok = false;
                    break;
                }
                else
                {
                    Remove(now_num);
                    Remove(now_num + 1);
                    Remove(now_num + 2);
                }
                //List_card();
            }
            for (int i = 0; i < tmp_cards; i++) card[i] = tmp[i];
            now_cards = tmp_cards;
            if (ok == true) return true;
        }
        return false;
    }
    public void List_card()
    {
        Debug.Log("list\n");
        Debug.Log("now you have " + now_cards + " cards");
        for (int i = 0; i < now_cards; i++)
        {
            if (card[i] > 45)
            {
                switch (card[i])
                {
                    case 48:
                        Debug.Log("東 ");
                        break;
                    case 56:
                        Debug.Log("南 ");
                        break;
                    case 64:
                        Debug.Log("西 ");
                        break;
                    case 72:
                        Debug.Log("北 ");
                        break;
                    case 80:
                        Debug.Log("白 ");
                        break;
                    case 88:
                        Debug.Log("發 ");
                        break;
                    case 96:
                        Debug.Log("中 ");
                        break;
                    default:
                        break;
                }
            }
            else
            {
                if (card[i] == 0 || card[i] == 16 || card[i] == 32) Debug.Log("一 ");
                if (card[i] == 1 || card[i] == 17 || card[i] == 33) Debug.Log("二 ");
                if (card[i] == 2 || card[i] == 18 || card[i] == 34) Debug.Log("三 ");
                if (card[i] == 3 || card[i] == 19 || card[i] == 35) Debug.Log("四 ");
                if (card[i] == 4 || card[i] == 20 || card[i] == 36) Debug.Log("五 ");
                if (card[i] == 5 || card[i] == 21 || card[i] == 37) Debug.Log("六 ");
                if (card[i] == 6 || card[i] == 22 || card[i] == 38) Debug.Log("七 ");
                if (card[i] == 7 || card[i] == 23 || card[i] == 39) Debug.Log("八 ");
                if (card[i] == 8 || card[i] == 24 || card[i] == 40) Debug.Log("九 ");
            }
        }
        Debug.Log("\n");
        for (int i = 0; i < now_cards; i++)
        {
            if (card[i] / 9 == 0) Debug.Log("萬 ");
            else if (card[i] / 9 <= 2) Debug.Log("筒 ");
            else if (card[i] / 9 <= 4) Debug.Log("條 ");
            else if (card[i] <= 72) Debug.Log("風 ");
            else Debug.Log("  ");
        }
        Debug.Log("\n");
        for (int i = 0; i < now_cards; i++)
        {
            //  Debug.Log(setw)(2) << card[i] << " ";
        }
        //  Debug.Log("\n");
        //  Debug.Log("鳴牌") << endl;
        int[] tmp = new int[20];
        int index = 0;
        for (int i = 0; i < eat_index; i++)
        {
            for (int j = 0; j < 3; j++) tmp[index++] = eat[i, j];
        }
        for (int i = 0; i < pon_index; i++)
        {
            for (int j = 0; j < 3; j++) tmp[index++] = pon[i];
        }
        for (int i = 0; i < gon_index; i++)
        {
            for (int j = 0; j < 4; j++) tmp[index++] = gon[i];
        }
        for (int i = 0; i < index; i++)
        {
            if (tmp[i] > 45)
            {
                switch (tmp[i])
                {
                    case 48:
                        Debug.Log("東 ");
                        break;
                    case 56:
                        Debug.Log("南 ");
                        break;
                    case 64:
                        Debug.Log("西 ");
                        break;
                    case 72:
                        Debug.Log("北 ");
                        break;
                    case 80:
                        Debug.Log("白 ");
                        break;
                    case 88:
                        Debug.Log("發 ");
                        break;
                    case 96:
                        Debug.Log("中 ");
                        break;
                    default:
                        break;
                }
            }
            else
            {
                if (tmp[i] == 0 || tmp[i] == 16 || tmp[i] == 32) Debug.Log("一 ");
                if (tmp[i] == 1 || tmp[i] == 17 || tmp[i] == 33) Debug.Log("二 ");
                if (tmp[i] == 2 || tmp[i] == 18 || tmp[i] == 34) Debug.Log("三 ");
                if (tmp[i] == 3 || tmp[i] == 19 || tmp[i] == 35) Debug.Log("四 ");
                if (tmp[i] == 4 || tmp[i] == 20 || tmp[i] == 36) Debug.Log("五 ");
                if (tmp[i] == 5 || tmp[i] == 21 || tmp[i] == 37) Debug.Log("六 ");
                if (tmp[i] == 6 || tmp[i] == 22 || tmp[i] == 38) Debug.Log("七 ");
                if (tmp[i] == 7 || tmp[i] == 23 || tmp[i] == 39) Debug.Log("八 ");
                if (tmp[i] == 8 || tmp[i] == 24 || tmp[i] == 40) Debug.Log("九 ");
            }
        }
        Debug.Log("\n");
        for (int i = 0; i < index; i++)
        {
            if (tmp[i] / 9 == 0) Debug.Log("萬 ");
            else if (tmp[i] / 9 <= 2) Debug.Log("筒 ");
            else if (tmp[i] / 9 <= 4) Debug.Log("條 ");
            else if (tmp[i] <= 72) Debug.Log("風 ");
            else Debug.Log("   ");
        }
        Debug.Log("\n");
        for (int i = 0; i < index; i++)
        {
            //  Debug.Log(setw)(2) << tmp[i] << " ";
        }
        Debug.Log("\n");
    }
    public void List_card_long()
    {
        //  Debug.Log("you)'re cards\n" << endl;
        for (int i = 0; i < eat_index; i++)
        {
            card[now_cards++] = eat[i, 0];
            card[now_cards++] = eat[i, 1];
            card[now_cards++] = eat[i, 2];
        }
        for (int i = 0; i < pon_index; i++)
        {
            card[now_cards++] = pon[i];
            card[now_cards++] = pon[i];
            card[now_cards++] = pon[i];
        }
        for (int i = 0; i < gon_index; i++)
        {
            card[now_cards++] = gon[i];
            card[now_cards++] = gon[i];
            card[now_cards++] = gon[i];
            card[now_cards++] = gon[i];
        }
        for (int i = 0; i < now_cards; i++)
        {
            if (card[i] > 45)
            {
                switch (card[i])
                {
                    case 48:
                        Debug.Log("東 ");
                        break;
                    case 56:
                        Debug.Log("南 ");
                        break;
                    case 64:
                        Debug.Log("西 ");
                        break;
                    case 72:
                        Debug.Log("北 ");
                        break;
                    case 80:
                        Debug.Log("白 ");
                        break;
                    case 88:
                        Debug.Log("發 ");
                        break;
                    case 96:
                        Debug.Log("中 ");
                        break;
                    default:
                        break;
                }
            }
            else
            {
                if (card[i] == 0 || card[i] == 16 || card[i] == 32) Debug.Log("一 ");
                if (card[i] == 1 || card[i] == 17 || card[i] == 33) Debug.Log("二 ");
                if (card[i] == 2 || card[i] == 18 || card[i] == 34) Debug.Log("三 ");
                if (card[i] == 3 || card[i] == 19 || card[i] == 35) Debug.Log("四 ");
                if (card[i] == 4 || card[i] == 20 || card[i] == 36) Debug.Log("五 ");
                if (card[i] == 5 || card[i] == 21 || card[i] == 37) Debug.Log("六 ");
                if (card[i] == 6 || card[i] == 22 || card[i] == 38) Debug.Log("七 ");
                if (card[i] == 7 || card[i] == 23 || card[i] == 39) Debug.Log("八 ");
                if (card[i] == 8 || card[i] == 24 || card[i] == 40) Debug.Log("九 ");
            }
        }
        Debug.Log("\n");
        for (int i = 0; i < now_cards; i++)
        {
            if (card[i] / 9 == 0) Debug.Log("萬 ");
            else if (card[i] / 9 <= 2) Debug.Log("筒 ");
            else if (card[i] / 9 <= 4) Debug.Log("條 ");
            else if (card[i] <= 72) Debug.Log("風 ");
            // else Debug.Log("   )";
        }
        Debug.Log("\n");
        for (int i = 0; i < now_cards; i++)
        {
            //  Debug.Log(setw)(2) << card[i] << " ";
        }
        Debug.Log("\n");

        //  Debug.Log("寶牌") << endl;
        for (int j = 0; j < dolla_arr.Count; j++)
        {
            var i = dolla_arr[j];
            int x = i;
            if (x > 45)
            {
                switch (x)
                {
                    case 48:
                        Debug.Log("東 ");
                        break;
                    case 56:
                        Debug.Log("南 ");
                        break;
                    case 64:
                        Debug.Log("西 ");
                        break;
                    case 72:
                        Debug.Log("北 ");
                        break;
                    case 80:
                        Debug.Log("白 ");
                        break;
                    case 88:
                        Debug.Log("發 ");
                        break;
                    case 96:
                        Debug.Log("中 ");
                        break;
                    default:
                        break;
                }
            }
            else
            {
                if (x == 0 || x == 16 || x == 32) Debug.Log("一 ");
                if (x == 1 || x == 17 || x == 33) Debug.Log("二 ");
                if (x == 2 || x == 18 || x == 34) Debug.Log("三 ");
                if (x == 3 || x == 19 || x == 35) Debug.Log("四 ");
                if (x == 4 || x == 20 || x == 36) Debug.Log("五 ");
                if (x == 5 || x == 21 || x == 37) Debug.Log("六 ");
                if (x == 6 || x == 22 || x == 38) Debug.Log("七 ");
                if (x == 7 || x == 23 || x == 39) Debug.Log("八 ");
                if (x == 8 || x == 24 || x == 40) Debug.Log("九 ");
            }
        }
        Debug.Log("\n");
        for (int j = 0; j < dolla_arr.Count; j++)
        {
            int i = dolla_arr[j];
            int x = i;
            if (x / 9 == 0) Debug.Log("萬 ");
            else if (x / 9 <= 2) Debug.Log("筒 ");
            else if (x / 9 <= 4) Debug.Log("條 ");
            else if (x <= 72) Debug.Log("風 ");
            else Debug.Log("  ");
        }
        Debug.Log("\n");
        for (var j = 0; j < dolla_arr.Count; j++)
        {
            int i = dolla_arr[j];
            int x = i;
            //  Debug.Log(setw)(2) << x << " ";
        }
        Debug.Log("\n");
        if (Retrun_reach() == 0) return;
        //  Debug.Log("裏寶牌)" << endl;
        for (var j = 0; j < dolla_arr_li.Count; j++)
        {
            int i = dolla_arr_li[j];
            int x = i;
            if (x > 45)
            {
                switch (x)
                {
                    case 48:
                        Debug.Log("東 ");
                        break;
                    case 56:
                        Debug.Log("南 ");
                        break;
                    case 64:
                        Debug.Log("西 ");
                        break;
                    case 72:
                        Debug.Log("北 ");
                        break;
                    case 80:
                        Debug.Log("白 ");
                        break;
                    case 88:
                        Debug.Log("發 ");
                        break;
                    case 96:
                        Debug.Log("中 ");
                        break;
                    default:
                        break;
                }
            }
            else
            {
                if (x == 0 || x == 16 || x == 32) Debug.Log("一 ");
                if (x == 1 || x == 17 || x == 33) Debug.Log("二 ");
                if (x == 2 || x == 18 || x == 34) Debug.Log("三 ");
                if (x == 3 || x == 19 || x == 35) Debug.Log("四 ");
                if (x == 4 || x == 20 || x == 36) Debug.Log("五 ");
                if (x == 5 || x == 21 || x == 37) Debug.Log("六 ");
                if (x == 6 || x == 22 || x == 38) Debug.Log("七 ");
                if (x == 7 || x == 23 || x == 39) Debug.Log("八 ");
                if (x == 8 || x == 24 || x == 40) Debug.Log("九 ");
            }
        }
        Debug.Log("\n");
        for (var j = 0; j < dolla_arr_li.Count; j++)
        {
            int i = dolla_arr_li[j];
            int x = i;
            if (x / 9 == 0) Debug.Log("萬 ");
            else if (x / 9 <= 2) Debug.Log("筒 ");
            else if (x / 9 <= 4) Debug.Log("條 ");
            else if (x <= 72) Debug.Log("風 ");
            else Debug.Log("  ");
        }
        Debug.Log("\n");
        // for (auto i = dolla_arr_li.begin(); i != dolla_arr_li.end(); i++)
        // {
        //     int x = (*i);
        // //  Debug.Log(setw)(2) << x << " ";
        // }
        Debug.Log("\n");
    }
    public void List_card_river()
    {
        //  Debug.Log("riv)er" << "\n";
        for (int i = 0; i < river_array_index; i++)
        {
            if (river_array[i] > 45)
            {
                switch (river_array[i])
                {
                    case 48:
                        Debug.Log("東 ");
                        break;
                    case 56:
                        Debug.Log("南 ");
                        break;
                    case 64:
                        Debug.Log("西 ");
                        break;
                    case 72:
                        Debug.Log("北 ");
                        break;
                    case 80:
                        Debug.Log("白 ");
                        break;
                    case 88:
                        Debug.Log("發 ");
                        break;
                    case 96:
                        Debug.Log("中 ");
                        break;
                    default:
                        break;
                }
            }
            else
            {
                if (river_array[i] == 0 || river_array[i] == 16 || river_array[i] == 32) Debug.Log("一 ");
                if (river_array[i] == 1 || river_array[i] == 17 || river_array[i] == 33) Debug.Log("二 ");
                if (river_array[i] == 2 || river_array[i] == 18 || river_array[i] == 34) Debug.Log("三 ");
                if (river_array[i] == 3 || river_array[i] == 19 || river_array[i] == 35) Debug.Log("四 ");
                if (river_array[i] == 4 || river_array[i] == 20 || river_array[i] == 36) Debug.Log("五 ");
                if (river_array[i] == 5 || river_array[i] == 21 || river_array[i] == 37) Debug.Log("六 ");
                if (river_array[i] == 6 || river_array[i] == 22 || river_array[i] == 38) Debug.Log("七 ");
                if (river_array[i] == 7 || river_array[i] == 23 || river_array[i] == 39) Debug.Log("八 ");
                if (river_array[i] == 8 || river_array[i] == 24 || river_array[i] == 40) Debug.Log("九 ");
            }
        }
        Debug.Log("\n");
        for (int i = 0; i < river_array_index; i++)
        {
            if (river_array[i] / 9 == 0) Debug.Log("萬 ");
            else if (river_array[i] / 9 <= 2) Debug.Log("筒 ");
            else if (river_array[i] / 9 <= 4) Debug.Log("條 ");
            else if (river_array[i] <= 72) Debug.Log("風 ");
            // else Debug.Log("   )";
        }
        Debug.Log("\n");
        for (int i = 0; i < river_array_index; i++)
        {
            //  Debug.Log(setw)(2) << river_array[i] << " ";
        }
        Debug.Log("\n");
    }
    //state 0 for myself 1 for other
    public bool Check_gon(int card_name, int state)
    {
        int can_gon = Count(card_name);
        if (can_gon == 3) return true;

        for (int i = 0; i < gon_index; i++)
        {
            if (gon[i] == card_name && state == 0) return true;
        }

        return false;
    }
    public int Check_gon_myself(int get_card)
    {
        for (int i = 0; i < now_cards; i++)
        {
            if (Count(card[i]) == 4) return card[i];
        }
        for (int i = 0; i < pon_index; i++)
        {
            if (pon[i] == get_card) return get_card;
        }
        return -1;
    }
    public bool Check_pon(int card_name)
    {
        int can_pon = Count(card_name);
        if (can_pon >= 2) return true;
        else return false;
    }
    public bool Check_eat(int card_name)
    {
        int[] count_array = new int[4];
        count_array[0] = Count(card_name - 2);
        count_array[1] = Count(card_name - 1);
        count_array[2] = Count(card_name + 1);
        count_array[3] = Count(card_name + 2);
        if (count_array[0] > 0 && count_array[1] > 0) return true;
        if (count_array[1] > 0 && count_array[2] > 0) return true;
        if (count_array[2] > 0 && count_array[3] > 0) return true;
        else return false;
    }
    public void Set_linshan()
    {
        check_linshan = 1;
    }
    public void Reset_linshan()
    {
        check_linshan = 0;

    }
    public void Set_chankan()
    {
        check_chankan = 1;
    }
    public void Reset_chankan()
    {
        check_chankan = 0;
    }
    public void Eat(int card_name_1, int card_name_2, int card_name_3, int now_card)
    {
        if (card_name_1 != now_card)
            Remove(card_name_1);
        if (card_name_2 != now_card)
            Remove(card_name_2);
        if (card_name_3 != now_card)
            Remove(card_name_3);
        eat[eat_index, 0] = card_name_1;
        eat[eat_index, 1] = card_name_2;
        eat[eat_index++, 2] = card_name_3;
        if (use_card.Contains(card_name_1) == false)
            use_card.Add(card_name_1);
        if (use_card.Contains(card_name_2) == false)
            use_card.Add(card_name_2);
        if (use_card.Contains(card_name_3) == false)
            use_card.Add(card_name_3);
        do_op = 1;
    }
    public void Pon(int card_name)
    {
        Remove(card_name);
        Remove(card_name);
        if (use_card.Contains(card_name) == false)
            use_card.Add(card_name);
        do_op = 1;
        pon[pon_index++] = card_name;
    }
    public void Gon(int card_name)
    {
        if (Count(card_name) == 3)
        {
            Remove(card_name);
            Remove(card_name);
            Remove(card_name);
            do_op = 1;
            gon[gon_index++] = card_name;
        }
        else if (Count(card_name) == 4)
        {
            for (int i = 0; i < 4; i++) Remove(card_name);
            gon[gon_index++] = card_name;
        }
        else if (Count(card_name) == 1)
        {
            for (int i = 0; i < pon_index; i++)
            {
                if (pon[i] == card_name)
                {
                    Remove(card_name);
                    return;
                }
            }
            //  Debug.Log("err)or happen " << endl;
        }
        return;
    }
    public void Set_reach()
    {
        reach = 1;
    }
    public void Set_last_card()
    {
        last_card = 1;
    }
    public void Set_last_card_other()
    {
        last_card_other = 1;
    }
    public void Set_double_reach()
    {
        double_reach = 1;
    }
    public void Set_op()
    {
        do_op = 1;
    }
    public void Reset_op()
    {
        do_op = 0;
    }
    public void Set_long()
    {
        long_other = 1;
        return;
    }
    public void Reset_long()
    {
        long_other = 0;
        return;
    }
    public int Retrun_reach()
    {
        return reach;
    }
    public int Retrun_no_op()
    {
        return do_op;
    }
    public void set_winds(int field, int guest)
    {
        field_winds = field;
        guest_winds = guest;
    }
    public void Add_river(int get_card)
    {
        if (card_river.Contains(get_card) == false)
            card_river.Add(get_card);
        river_array[river_array_index++] = get_card;
        return;
    }
    public void Reset()
    {
        for (int i = 0; i < 20; i++) card[i] = 1000;
        long_other = 0;
        now_cards = 0;
        pon_index = 0;
        gon_index = 0;
        eat_index = 0;
        reach = 0;
        double_reach = 0;
        do_op = 0;
        one_fa = 0;
        last_card = 0;
        last_card_other = 0;
        check_chankan = 0;
        check_linshan = 0;
        first_card_after = 0;
        first_card_start = 0;
        yee.Clear();
        use_card.Clear();
        yee.Add(80);
        yee.Add(88);
        yee.Add(96);
        river_array_index = 0;
        dolla_arr.Clear();
        dolla_arr_li.Clear();
        cannot_long.Clear();
        card_river.Clear();
    }
    void bubble(int[] arr, int len)
    {
        for (int i = 0; i < len; i++)
        {
            for (int j = 0; j < len - i - 1; j++)
            {
                if (arr[j] > arr[j + 1])
                {
                    int tmp = arr[j];
                    arr[j] = arr[j + 1];
                    arr[j + 1] = tmp;
                }
            }
        }
    }
    public int[] ReturnPlayerCard()
    {
        return card;
    }
    public void bubble()
    {
        bubble(card, 20);
    }
    public int selectCard()
    {
        for(int i=0;i<now_cards;i++)
        {
            int num = card[i];
            if(num == 0 || num == 16 || num == 32)
            {
                if(Count(num+1)==0 && Count(num+2)==0 && Count(num) == 1 )
                {
                    return num;
                }
            }
            else if(num == 8 || num == 24 || num == 40)
            {
                if(Count(num-1)==0 && Count(num-2)==0&& Count(num) == 1)
                {
                    return num;
                }
            }
            else if(num >= 48)
            {
                if(Count(num)==1)
                {
                    return num;
                }
            }
            else if(num == 1 || num == 17 || num == 33 || num == 7 || num == 23 || num == 39 )
            {
                if(Count(num-1)==0 && Count(num+1)==0 && Count(num)==1)
                {
                    return num;
                }
            }
            else
            {
                if(Count(num-1)==0 && Count(num+1)==0 && Count(num-2)==0 && Count(num+2)==0  && Count(num)==1)
                {
                    return num;
                }
            }
        }
        return card[now_cards-1];
    }
}








