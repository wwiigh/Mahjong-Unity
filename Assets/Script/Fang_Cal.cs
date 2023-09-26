using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fang_Cal : Player
{
    struct pair
    {
        public int a,b,c;
        public pair(int _a,int _b, int _c)
        {
            a = _a;
            b = _b;
            c = _c;
        }
    }
    /* data */
    int anko;
    int eye;
    int is_seven;
    int[] final_fang = new int[42];
    public Fang_Cal() : base()
    {
        anko = 0;
        eye = 0;
        is_seven = 0;
        for (int i = 0; i < 42; i++) final_fang[i] = 0;
    }

    public int Howmany_fang(int get_card)
    {
        for (int i = 0; i < 42; i++) final_fang[i] = 0;
        use_card.Clear();
        for (int i = 0; i < now_cards; i++)
        {
            if (use_card.Contains(card[i]) == false)
                use_card.Add(card[i]);
        }
        for (int i = 0; i < eat_index; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (use_card.Contains(eat[i, j]) == false)
                    use_card.Add(eat[i, j]);
            }
        }
        for (int i = 0; i < pon_index; i++)
        {
            if (use_card.Contains(pon[i]) == false)
                use_card.Add(pon[i]);
        }
        for (int i = 0; i < gon_index; i++)
        {
            if (use_card.Contains(gon[i]) == false)
                use_card.Add(gon[i]);
        }
        int ans = 0;
        //判國士 or 13聽
        ans = kokushi_13(get_card);
        if (ans > 0)
        {
            if (ans == 13) final_fang[27] = 13;
            if (ans == 26) final_fang[28] = 26;
            return ans;
        }
        //判純九連
        ans = real_tureen(get_card);
        if (ans == 26)
        {
            final_fang[37] = 26;
            return ans;
        }
        //判九連

        ans = tureen();
        if (ans == 13)
        {
            final_fang[36] = 13;
            return ans;
        }
        ans = tenhou();
        //cout<<"天和 "<<ans<<endl;
        if (ans > 0)
        {
            final_fang[39] = 13;
            return ans;
        }
        ans = qihou();
        //cout<<"地和 "<<ans<<endl;
        if (ans > 0)
        {
            final_fang[40] = 13;
            return ans;
        }
        ans = 0;

        ans += daisangen();

        final_fang[29] = ans;
        //cout<<"大三 "<<ans<<endl;

        ans += sue_ann(get_card);
        //cout<<"四暗 "<<sue_ann(get_card)<<endl;
        final_fang[30] = sue_ann(get_card);


        ans += two_e_saw();
        final_fang[31] = two_e_saw();
        //cout<<"字一色 "<<two_e_saw()<<endl;

        ans += ryu_eso();
        final_fang[32] = ryu_eso();
        //cout<<"綠一色 "<<ryu_eso()<<endl;

        ans += show_sucy();
        final_fang[33] = show_sucy();
        //cout<<"小四 "<<show_sucy()<<endl;

        ans += daisy();
        final_fang[34] = daisy();
        //cout<<"大四 "<<daisy()<<endl;

        ans += chin_lo();
        final_fang[35] = chin_lo();
        //cout<<"清老頭 "<<chin_lo()<<endl;

        ans += sukants();
        final_fang[38] = sukants();
        //cout<<"四槓 "<<sukants()<<endl;
        if (ans > 0) return ans;

        ans = 0;
        //判七對子
        ans += cheat_toys();
        final_fang[18] = cheat_toys();
        //cout<<"七對 "<<ans<<endl;
        if (ans > 0) is_seven = 1;
        //判斷么
        //cout<<"size"<<use_card.size()<<endl;
        ans += Thanyao(get_card);
        final_fang[3] = Thanyao(get_card);


        //判立直，門清自摸，一發
        ans += reach(get_card);
        final_fang[0] = reach(get_card);
        //cout<<"立直 "<<reach(get_card);




        //判三槓子

        //cout<<"三槓子 "<<sankanz();
        //cout<<ans<<endl;
        List<int> two = new List<int>();
        int[] tmp = new int[20];
        int tmp_cards = now_cards;

        for (int i = 0; i < now_cards; i++)
        {
            if (Count(card[i]) >= 2)
            {
                if (two.Contains(card[i]) == false)
                    two.Add(card[i]);
            }
            tmp[i] = card[i];
        }
        int big_ans = 0;
        int[] tmpp = new int[42];
        for (int j = 0; j < 42; j++) tmpp[j] = final_fang[j];
        bool is_biger = false;
        for (int k = 0; k != two.Count; k++)
        {
            int i = two[k];
            //cout<<now_cards<<endl;
            int tmp_ans = 0;
            int e = i;
            Remove(e);
            Remove(e);
            eye = e;
            int tmp_eat_index = eat_index;
            int tmp_pon_index = pon_index;
            bool is_ok = true;


            while (now_cards > 0)
            {

                int x = card[0];
                if (Count(x) > 2)
                {
                    Remove(x);
                    Remove(x);
                    Remove(x);
                    pon[pon_index++] = x;
                }
                else
                {
                    bool[] ok = new bool[3];
                    for (int j = 0; j < 3; j++) ok[j] = Remove(x + j);
                    if (ok[0] == false || ok[1] == false || ok[2] == false)
                    {
                        is_ok = false;
                        break;
                    }
                    for (int j = 0; j < 3; j++) eat[eat_index, j] = x + j;
                    eat_index++;
                }
            }
            anko = pon_index - tmp_pon_index;
            if (is_ok == true)
                tmp_ans = tmpcal(get_card);
            if (big_ans < tmp_ans)
            {
                is_biger = true;
                for (int j = 0; j < 42; j++) tmpp[j] = final_fang[j];
                big_ans = tmp_ans;
            }
            Add(i);
            Add(i);
            for (int j = 0; j < tmp_cards; j++)
            {
                card[j] = tmp[j];
            }
            now_cards = tmp_cards;
            pon_index = tmp_pon_index;
            eat_index = tmp_eat_index;
        }
        ans += big_ans;
        if (is_biger == true)
            for (int i = 0; i < 42; i++) final_fang[i] = tmpp[i];
        if (ans > 0 || Check_long() == true)//判門清 一發
        {
            //cout<<"門清 "<<tsumo()<<endl;
            ans += ippatsu();
            final_fang[2] = ippatsu();

            final_fang[1] = tsumo();
            ans += tsumo();
        }
        return ans;
    }
    public int reach(int get_card)//立直 0
    {
        int rea = Retrun_reach();
        if (rea == 0) return 0;
        int ans = check_special(get_card);
        if (ans > 0) return 1;
        bool anss = Check_long();
        if (anss == true) return 1;
        return 0;
    }
    public int tsumo()//門前清自摸 1
    {
        if (do_op == 1) return 0;
        else if (long_other == 1) return 0;
        else return 1;
    }
    public int ippatsu()//一發 2
    {
        return one_fa;
    }
    public int Thanyao(int get_card)//段么 3
    {
        // cout<<use_card.size()<<endl;
        if (Check_long() == false && check_special(get_card) == 0) return 0;
        // cout<<use_card.size()<<endl;
        // set<int>::iterator iter;
        for (int i = 0; i < use_card.Count; i++)
        {
            int iter = use_card[i];
            int num = iter;
            //cout<<"num"<<num<<endl;
            if (num % 8 == 0) return 0;
        }
        return 1;
    }
    public int pinf(int get_card)//平和 4
    {
        if (do_op == 1)
            return 0;
        foreach (var i in yee)
        {
            int x = i;
            if (x == eye) return 0;
        }
        for (int i = 10; i <= 12; i++)
        {
            if (eye == i * 8) return 0;
        }
        if (pon_index != 0 || gon_index != 0 || eat_index != 4) return 0;
        for (int i = 0; i < 4; i++)
        {
            if (eat[i, 0] == get_card || eat[i, 2] == get_card) return 1;
        }
        return 0;
    }
    
    public int epako()//一盃口 5
    {
        if (liang_peko() > 0) return 0;
        if (do_op == 1)
            return 0;
        int check_cheat_toys = cheat_toys();
        if (check_cheat_toys > 0) return 0;
        List<pair> eat_pair = new List<pair>();
        // set<pair<int, pair<int, int>>> eat_pair;
        for (int i = 0; i < eat_index; i++)
        {
            pair tmp_pair = new pair(eat[i, 0], eat[i, 1], eat[i, 2]);
            var find_pair = CompareList(eat_pair,tmp_pair);;
            if (find_pair == true) return 1;
            eat_pair.Add(tmp_pair);
        }
        return 0;
    }
    public int yakuhai()//役牌 6
    {
        int ans = 0;
        for (int k = 0; k < use_card.Count; k++)
        {
            int i = use_card[k];
            int x = i;
            bool find = false;
            for (int j = 0; j < pon_index; j++)
            {
                if (pon[j] == x)
                {
                    find = true;
                    break;
                }
            }
            for (int j = 0; j < gon_index; j++)
            {
                if (gon[j] == x)
                {
                    find = true;
                    break;
                }
            }
            if (find == false) continue;
            if (yee.Contains(x) == true) ans += 1;
            if (x == guest_winds) ans += 1;
            if (x == field_winds) ans += 1;
        }
        return ans;
    }
    public int linshan()//領上 7 
    {
        return check_linshan;
    }
    public int chankan()//搶槓 8
    {
        return check_chankan;
    }
    public int high_tay()//海底 9
    {
        if (last_card == 1) return 1;
        else
            return 0;
    }
    public int houtei()//河底 10
    {
        if (last_card_other == 1) return 1;
        else
            return 0;
    }
    public int dora(int get_card)//寶牌 11
    {
        int ans = 0;
        for (int i = 0; i < now_cards; i++)
        {
            int x = card[i];
            if (dolla_arr.Contains(x) == true) ans += 1;
            if (Retrun_reach() != 1) continue;
            if (dolla_arr_li.Contains(x) == true) ans += 1;
        }
        for (int i = 0; i < eat_index; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                int x = eat[i, j];
                if (dolla_arr.Contains(x) == true) ans += 1;
                if (Retrun_reach() != 1) continue;

                if (dolla_arr_li.Contains(x) == true) ans += 1;
            }
        }
        for (int i = 0; i < pon_index; i++)
        {
            int x = pon[i];
            if (dolla_arr.Contains(x) == true) ans += 3;
            if (Retrun_reach() != 1) continue;
            if (dolla_arr_li.Contains(x) == true) ans += 3;
        }
        for (int i = 0; i < gon_index; i++)
        {
            int x = gon[i];
            if (dolla_arr.Contains(x) == true) ans += 4;
            if (Retrun_reach() != 1) continue;
            if (dolla_arr_li.Contains(x) == true) ans += 4;
        }
        if (dolla_arr.Contains(eye) == true) ans += 2;
        if (Retrun_reach() != 1) return ans;
        if (dolla_arr_li.Contains(eye) == true) ans += 2;
        return ans;
    }
 
    public int sanshoku()//三色同順 12
    {
        if (eat_index < 3) return 0;
        List<pair> tmp_eat = new List<pair>();
        // set<pair<int, pair<int, int>>> tmp_eat;
        for (int i = 0; i < eat_index; i++)
        {
            if(CompareList(tmp_eat,new pair(eat[i, 0], eat[i, 1], eat[i, 2]))==false)
            tmp_eat.Add(new pair(eat[i, 0], eat[i, 1], eat[i, 2]));
        }
        var first = tmp_eat[0];
        var first_find = new pair(first.a + 16, first.b + 16, first.c + 16);
        var second_find = new pair(first.a + 32, first.b + 32, first.c + 32);
        var check_1 = CompareList(tmp_eat,first_find);
        var check_2 = CompareList(tmp_eat,second_find);
        if (check_1 == true && check_2 == true)
        {
            if (do_op == 0) return 2;
            else return 1;
        }
        // var iter = tmp_eat.b;
        first = tmp_eat[1];
        first_find = new pair(first.a + 16, first.b + 16, first.c + 16);
        second_find = new pair(first.a + 32, first.b + 32, first.c + 32);
        check_1 = CompareList(tmp_eat,first_find); 
        check_2 = CompareList(tmp_eat,second_find); 
        if (check_1 == true && check_2 == true)
        {
            if (do_op == 0) return 2;
            else return 1;
        }
        return 0;
    }
    public int sandoko()//三色同刻 13
    {
        List<int> tmp = new List<int>();
        if (pon_index + gon_index < 3) return 0;

        for (int i = 0; i < pon_index; i++)
        {
            if (tmp.Contains(pon[i]) == false)
                tmp.Add(pon[i]);
        }
        for (int i = 0; i < gon_index; i++)
        {
            if (tmp.Contains(gon[i]) == false)
            tmp.Add(gon[i]);
        }
        if (tmp.Count == 0) return 0;
        var iter = tmp[0];

        int x = iter;
        var check_1 = tmp.Contains(x + 16);
        var check_2 = tmp.Contains(x + 32);
        if (check_1 == true && check_2 == true) return 2;
        if (tmp.Count <= 1) return 0;
        x = tmp[1];
        check_1 = tmp.Contains(x + 16);
        check_2 = tmp.Contains(x + 32);
        if (check_1 == true && check_2 == true) return 2;
        return 0;
    }
    
    public int ikkitsukan()//一氣通貫 14
    {
        if (eat_index < 3) return 0;
        List<pair> tmp_eat = new List<pair>();
        // set<pair<int, pair<int, int>>> tmp_eat;
        for (int i = 0; i < eat_index; i++)
        {
            if(CompareList(tmp_eat,new pair(eat[i,0],eat[i,1],eat[i,2]))==false)
            tmp_eat.Add(new pair(eat[i,0],eat[i,1],eat[i,2]));
        }
        var first = tmp_eat[0];
        var first_find = new pair(first.a + 3, first.b + 3, first.c + 3);
        var second_find = new pair(first.a + 6, first.b + 6, first.c + 6);
        var check_1 = CompareList(tmp_eat,first_find);
        var check_2 = CompareList(tmp_eat,second_find);
        if (check_1  && check_2)
        {
            if (do_op == 0) return 2;
            else return 1;
        }
        first = tmp_eat[1];
        first_find = new pair(first.a + 3, first.b + 3, first.c + 3);
        second_find = new pair(first.a + 6, first.b + 6, first.c + 6);
        check_1 = CompareList(tmp_eat,first_find);
        check_2 = CompareList(tmp_eat,second_find);
        if (check_1 && check_2 )
        {
            if (do_op == 0) return 2;
            else return 1;
        }
        return 0;
    }
    public int toy_toy()//對對 15
    {
        if (eat_index == 0) return 2;
        return 0;
    }
    public int san_anko()//三暗刻 16
    {
        if (anko == 3) return 2;
        return 0;
    }
    public int sankanz()//三槓子 17 
    {
        if (gon_index == 3) return 2;
        else
            return 0;
    }
    public int cheat_toys()//七對子 18
    {
        if (do_op == 1) return 0;
        for (var i = 0; i < use_card.Count; i++)
        {
            int x = use_card[i];
            if (Count(x) != 2) return 0;
        }
        return 2;
    }
    public int chanta()//混全帶么 19
    {
        int check = cheat_toys();
        bool word = false;
        if (check == 2) return 0;
        if (eat_index == 0) return 0;
        if (eye % 8 != 0) return 0;

        if (eye > 40) word = true;
        for (int i = 0; i < eat_index; i++)
        {
            if (eat[i,0] % 8 == 0) continue;
            else if (eat[i,2] % 8 == 0) continue;
            else return 0;
        }
        for (int i = 0; i < pon_index; i++)
        {
            if (pon[i] > 40) word = true;
            if (pon[i] % 8 == 0) continue;
            else return 0;
        }
        for (int i = 0; i < gon_index; i++)
        {
            if (gon[i] > 40) word = true;
            if (gon[i] % 8 == 0) continue;
            else return 0;
        }
        if (word == false) return 0;
        if (do_op == 1) return 1;
        else return 2;
    }
    public int honlouto()//混老頭 20
    {
        if (eye % 8 != 0) return 0;
        if (eat_index > 0) return 0;
        for (int i = 0; i < pon_index; i++)
        {
            if (pon[i] % 8 == 0) continue;
            else return 0;
        }
        for (int i = 0; i < gon_index; i++)
        {
            if (gon[i] % 8 == 0) continue;
            else return 0;
        }
        return 2;
    }
    public int shosangen()//小三元 21
    {
        if (eye != 80 && eye != 88 && eye != 96) return 0;
        bool[] ok = new bool[3];
        ok[0] = false;
        ok[1] = false;
        ok[2] = false;
        ok[(eye - 80) / 8] = true;
        for (int i = 0; i < pon_index; i++)
        {
            if (pon[i] < 80) continue;
            ok[(pon[i] - 80) / 8] = true;
        }
        for (int i = 0; i < gon_index; i++)
        {
            if (gon[i] < 80) continue;
            ok[(gon[i] - 80) / 8] = true;
        }
        if (ok[0] == true && ok[1] == true && ok[2] == true) return 2;
        else return 0;
    }
    public int dubry()//雙立直 22
    {
        return double_reach;
    }
    public int honitsu()//混一色 23
    {
        var iter = use_card[0];
        int number = iter;
        if (number > 40) return 0;
        int which = 0;
        if (number < 9) which = 0;
        else if (number < 25) which = 1;
        else if (number < 41) which = 2;
        bool word = false;
        for (int i = 0; i < use_card.Count; i++)
        {
            int x = use_card[i];
            if (x > 40)
            {
                word = true;
                break;
            }
            if (which == 0 && x > 8) return 0;
            else if (which == 1 && (x < 16 || x > 24)) return 0;
            else if (which == 2 && x < 32) return 0;
        }
        if (word == false) return 0;
        if (do_op == 1) return 2;
        else return 3;
    }
    public int jun_chan()//純全帶么 24
    {
        int check = cheat_toys();
        if (check == 2) return 0;
        if (eat_index == 0) return 0;
        if (eye % 8 != 0) return 0;

        if (eye > 40) return 0;
        for (int i = 0; i < eat_index; i++)
        {
            if (eat[i,0] % 8 == 0) continue;
            else if (eat[i,2] % 8 == 0) continue;
            else return 0;
        }
        for (int i = 0; i < pon_index; i++)
        {
            if (pon[i] > 40) return 0;
            if (pon[i] % 8 == 0) continue;
            else return 0;
        }
        for (int i = 0; i < gon_index; i++)
        {
            if (gon[i] > 40) return 0;
            if (gon[i] % 8 == 0) continue;
            else return 0;
        }

        if (do_op == 1) return 2;
        else return 3;
    }
    public int liang_peko()//二盃口 25
    {
        if (do_op == 1) return 0;
        if (eat_index != 4) return 0;
        List<pair> tmp_eat = new List<pair>();
        for (int i = 0; i < eat_index; i++)
        {
            if(CompareList(tmp_eat,new pair(eat[i,0], eat[i,1], eat[i,2]))==false)
            tmp_eat.Add(new pair(eat[i,0], eat[i,1], eat[i,2]));
        }

        if (tmp_eat.Count != 2) return 0;
        if (is_seven == 1) return 1;
        else return 3;
    }
    public int chinitsu()//清一色 26
    {
        int iter = 0;
        int number = use_card[iter];
        if (number > 40) return 0;
        int which=0;
        if (number < 9) which = 0;
        else if (number < 25) which = 1;
        else if (number < 41) which = 2;
        bool word = false;
        for (iter = 0; iter < use_card.Count; iter++)
        {
            int x = use_card[iter];
            if (x > 40)
            {
                return 0;
            }
            if (which == 0 && x > 8) return 0;
            else if (which == 1 && (x < 16 || x > 24)) return 0;
            else if (which == 2 && x < 32) return 0;
        }
        if (do_op == 1) return 5;
        else return 6;
    }
    public int kokushi()//國士 27
    {
        for (int i = 0; i < 13; i++)
        {
            bool check = true;
            if (Count(i * 8) == 2)
            {
                for (int j = 0; j < 13; j++)
                {
                    if (j != i && Count(j * 8) != 1)
                    {
                        check = false;
                        break;
                    }
                }
            }
            else continue;
            if (check == true)
            {
                return 13;
            }
        }
        return 0;
    }
    public int kokushi_13(int get_card)//國士13面 28
    {
        int ans = kokushi();
        if (ans == 13)
        {
            if (Count(get_card) == 2) return 26;
            else return 13;
        }
        return 0;
    }
    public int daisangen()//大三元 29
    {
        bool[] ok = new bool[3];
        ok[0] = false;
        ok[1] = false;
        ok[2] = false;
        for (int i = 0; i < pon_index; i++)
        {

            if (pon[i] < 80) continue;
            ok[(pon[i] - 80) / 8] = true;
        }
        for (int i = 0; i < gon_index; i++)
        {
            if (gon[i] < 80) continue;
            ok[(gon[i] - 80) / 8] = true;
        }
        //cout<<"here"<<endl;
        if (ok[0] == true && ok[1] == true && ok[2] == true) return 13;
        else return 0;
    }
    public int sue_ann(int get_card)//四暗 30
    {
        if (get_card == eye) return 0;
        if (anko == 4) return 13;
        else return 0;
    }
    public int two_e_saw()//字一色 31
    {
        for (int i = 0; i < use_card.Count; i++)
        {
            int x = use_card[i];
            if (x <= 40) return 0;
        }
        return 13;
    }
    public int ryu_eso()//綠一色 32
    {
        for (int i = 0; i < use_card.Count; i++)
        {
            int x = use_card[i];
            if (x != 33 && x != 34 && x != 35 && x != 37 && x != 39 && x != 88) return 0;
        }
        return 13;
    }
    public int show_sucy()//小四 33
    {
        if (eye != 48 && eye != 56 && eye != 64 && eye != 72) return 0;
        bool[] ok = new bool[4];
        for (int i = 0; i < 4; i++) ok[i] = false;
        ok[(eye - 48) / 8] = true;
        for (int i = 0; i < pon_index; i++)
        {
            int x = pon[i];
            if (x == 48 || x == 56 || x == 64 || x == 72)
            {
                ok[(x - 48) / 8] = true;
            }
        }
        for (int i = 0; i < gon_index; i++)
        {
            int x = gon[i];
            if (x == 48 || x == 56 || x == 64 || x == 72)
            {
                ok[(x - 48) / 8] = true;
            }
        }
        if (ok[0] == false || ok[1] == false || ok[2] == false || ok[3] == false) return 0;
        else return 13;
    }
    public int daisy()//大四 34
    {
        bool[] ok = new bool[4];
        for (int i = 0; i < 4; i++) ok[i] = false;
        for (int i = 0; i < pon_index; i++)
        {
            int x = pon[i];
            if (x == 48 || x == 56 || x == 64 || x == 72)
            {
                ok[(x - 48) / 8] = true;
            }
        }
        for (int i = 0; i < gon_index; i++)
        {
            int x = gon[i];
            if (x == 48 || x == 56 || x == 64 || x == 72)
            {
                ok[(x - 48) / 8] = true;
            }
        }
        if (ok[0] == false || ok[1] == false || ok[2] == false || ok[3] == false) return 0;
        else return 26;
    }
    public int chin_lo()//清老頭 35
    {
        for (int i = 0; i < use_card.Count; i++)
        {
            int x = use_card[i];
            if (x != 0 && x != 8 && x != 16 && x != 24 && x != 32 && x != 40) return 0;
        }
        return 13;
    }
    public int tureen()//九連 36
    {
        bool find = false;
        int tmp_num = 0;
        for (int i = 0; i < 9; i++)
        {
            if (Count(i) == 2)
            {
                find = true;
                Remove(i);
                tmp_num = i;
                break;
            }
        }
        if (find == false)
        {
            return 0;
        }

        if (Count(0) == 3 && Count(1) == 1 && Count(2) == 1 &&
            Count(3) == 1 && Count(4) == 1 && Count(5) == 1 &&
            Count(6) == 1 && Count(7) == 1 && Count(8) == 3)
        {
            Add(tmp_num);
            return 13;
        }

        Add(tmp_num);
        return 0;
    }
    public int real_tureen(int get_card)//純正九連 37
    {
        Remove(get_card);
        if (Count(0) == 3 && Count(1) == 1 && Count(2) == 1 &&
            Count(3) == 1 && Count(4) == 1 && Count(5) == 1 &&
            Count(6) == 1 && Count(7) == 1 && Count(8) == 3)
        {
            Add(get_card);
            return 26;
        }
        Add(get_card);
        return 0;
    }
    public int sukants()//四槓 38
    {
        if (gon_index == 4) return 13;
        else
            return 0;
    }
    public int tenhou()//天和 39
    {
        bool is_long = Check_long();
        if (first_card_start == 1 && is_long == true) return 13;
        else return 0;
    }
    public int qihou()//地和 40
    {
        bool is_long = Check_long();
        if (first_card_after == 1 && is_long == true) return 13;
        else return 0;
    }
    public int nagashi()//流局 41
    {
        // set<int>::iterator iter;
        for (int iter = 0; iter < card_river.Count; iter++)
        {
            int num = card_river[iter];
            if (num % 8 != 0) return 0;
        }
        return 4;
    }
    public int tmpcal(int get_card)
    {
        int ans = 0;

        //盼海底，河底
        ans += high_tay();
        final_fang[9] = high_tay();
        //cout<<"海底 "<<high_tay();

        ans += houtei();
        final_fang[10] = houtei();
        //cout<<"河底 "<<houtei();

        //三槓
        ans += sankanz();
        final_fang[17] = sankanz();

        //判平和
        ans += pinf(get_card);
        final_fang[4] = pinf(get_card);
        //cout<<"平和 "<<pinf(get_card)<<endl;
        //判一盃口
        ans += epako();
        final_fang[5] = epako();
        //cout<<"一盃口 "<<epako()<<endl;

        ans += yakuhai();
        final_fang[6] = yakuhai();
        // cout<<"役牌 "<<yakuhai()<<endl;

        ans += linshan();
        final_fang[7] = linshan();
        //cout<<"領上 "<<linshan()<<endl;

        ans += chankan();
        final_fang[8] = chankan();
        //cout<<"搶槓 "<<chankan()<<endl;

        ans += dora(get_card);
        final_fang[11] = dora(get_card);
        //cout<<"寶牌 "<<dora()<<endl;

        ans += sanshoku();
        final_fang[12] = sanshoku();
        //cout<<"三色同順 "<<sanshoku()<<endl;

        ans += sandoko();
        final_fang[13] = sandoko();
        //cout<<"三色同刻 "<<sandoko()<<endl;

        ans += ikkitsukan();
        final_fang[14] = ikkitsukan();
        //cout<<"一氣 "<<ikkitsukan()<<endl;

        ans += toy_toy();
        final_fang[15] = toy_toy();
        //cout<<"對對 "<<toy_toy()<<endl;

        ans += san_anko();
        final_fang[16] = san_anko();
        //cout<<"三暗刻 "<<san_anko()<<endl;

        ans += chanta();
        final_fang[19] = chanta();
        //cout<<"混全帶么 "<<chanta()<<endl;

        ans += honlouto();
        final_fang[20] = honlouto();
        //cout<<"混老頭 "<<honlouto()<<endl;

        ans += shosangen();
        final_fang[21] = shosangen();
        //cout<<"小三元 "<<shosangen()<<endl;

        ans += dubry();
        final_fang[22] = dubry();
        //cout<<"雙立直 "<<dubry()<<endl;

        ans += honitsu();
        final_fang[23] = honitsu();
        //cout<<"混一色 "<<honitsu()<<endl;

        ans += jun_chan();
        final_fang[24] = jun_chan();
        //cout<<"純全帶么 "<<jun_chan()<<endl;

        ans += liang_peko();
        final_fang[25] = liang_peko();
        //cout<<"二盃口 "<<liang_peko()<<endl;

        ans += chinitsu();
        final_fang[26] = chinitsu();
        //cout<<"清一色 "<<chinitsu()<<endl;

        //cout<<"total ans "<<ans<<endl<<endl;
        return ans;
    }
    //數支役滿自行計算

    public int check_reach()
    {
        if (do_op == 1) return 0;
        for (int j = 0; j < now_cards; j++)
        {
            //cout<<use_card.size()<<endl;
            int y = card[j];
            Remove(y);
            bool find = false;
            for (int i = 0; i <= 8; i++)
            {
                Add(i);
                find = Check_long();
                int ans = check_special(i);
                if (find == true || ans > 0)
                {
                    //cout<<ans<<endl;
                    Remove(i);
                    Add(y);
                    //cout<<"card is "<<i<<endl;
                    return 1;
                }
                Remove(i);
            }
            for (int i = 16; i <= 24; i++)
            {
                Add(i);
                int ans = check_special(i);
                find = Check_long();
                if (find == true || ans > 0)
                {
                    //cout<<"card is "<<i<<endl;
                    Add(y);
                    Remove(i);
                    return 1;
                }
                Remove(i);
            }

            for (int i = 32; i <= 40; i++)
            {
                Add(i);
                int ans = check_special(i);
                find = Check_long();
                if (find == true || ans > 0)
                {
                    //cout<<"card is "<<i<<endl;
                    Add(y);
                    Remove(i);
                    return 1;
                }
                Remove(i);
            }
            for (int i = 6; i <= 12; i++)
            {
                int x = i * 8;
                Add(x);
                int ans = check_special(x);

                find = Check_long();
                if (find == true || ans > 0)
                {
                    //cout<<"card is "<<x<<endl;
                    Add(y);
                    Remove(x);
                    return 1;
                }
                Remove(x);
            }
            Add(y);
        }
        return 0;
    }
    public int check_special(int get_card)
    {
        if (do_op == 1) return 0;
        List<int> tmp = new List<int>();
        // set<int> tmp;
        foreach(var t in use_card)
        {
            if(tmp.Contains(t)==false)
            tmp.Add(t);
        } 
        use_card.Clear();
        for (int i = 0; i < now_cards; i++) 
        {
            if(use_card.Contains(card[i])==false)
            use_card.Add(card[i]);
        }int ans = 0;
        ans = cheat_toys();
        if (ans > 0)
        {
            use_card.Clear();
            foreach (var t in use_card)
            {
                if(use_card.Contains(t)==false)
                use_card.Add(t);
            } 
            return ans;
        }
        ans = kokushi();
        if (ans > 0)
        {
            use_card.Clear();
            foreach(var t in tmp)
            {
                if(use_card.Contains(t)==false)
                use_card.Add(t);
            }
            return ans;
        }
        ans = kokushi_13(get_card);
        if (ans > 0)
        {
            use_card.Clear();
            foreach(var t in tmp)
            {
                if(use_card.Contains(t)==false)
                use_card.Add(t);
            }
            return ans;
        }
        ans = tureen();
        if (ans > 0)
        {
            use_card.Clear();
            foreach(var t in tmp)
            {
                if(use_card.Contains(t)==false)
                use_card.Add(t);
            }
            // for (auto t = tmp.begin(); t != tmp.end(); t++) use_card.insert((*t));
            return ans;
        }
        ans = real_tureen(get_card);
        if (ans > 0)
        {
            use_card.Clear();
            foreach(var t in tmp)
            {
                if(use_card.Contains(t)==false)
                use_card.Add(t);
            }
            // for (auto t = tmp.begin(); t != tmp.end(); t++) use_card.insert((*t));
            return ans;
        }
        return 0;
    }
    public string print_fang()
    {
        string ans = "";
        if (final_fang[18] == 2 && final_fang[25] == 1)
        {
            final_fang[18] = 0;
            final_fang[25] = 3;
        }
        if (final_fang[0] > 0) ans+=" 立直 "+ final_fang[0];
        if (final_fang[1] > 0) ans+=" 門前清自摸 "+ final_fang[1];
        if (final_fang[2] > 0) ans+=" 一發 "+ final_fang[2];
        if (final_fang[3] > 0) ans+=" 斷么九 "+ final_fang[3];
        if (final_fang[4] > 0) ans+=" 平和 "+ final_fang[4];
        if (final_fang[5] > 0) ans+=" 一盃口 "+ final_fang[5];
        if (final_fang[6] > 0) ans+=" 役牌 "+ final_fang[6];
        if (final_fang[7] > 0) ans+=" 領上開花 "+ final_fang[7];
        if (final_fang[8] > 0) ans+=" 搶槓 "+ final_fang[8];
        if (final_fang[9] > 0) ans+=" 海底撈月 "+ final_fang[9];
        if (final_fang[10] > 0) ans+=" 河底撈魚 " +final_fang[10];
        if (final_fang[11] > 0) ans+=" 寶牌 " +final_fang[11];
        if (final_fang[12] > 0) ans+=" 三色同順 " +final_fang[12];
        if (final_fang[13] > 0) ans+=" 三色同刻 " +final_fang[13];
        if (final_fang[14] > 0) ans+=" 一氣通貫 " +final_fang[14];
        if (final_fang[15] > 0) ans+=" 對對和 " +final_fang[15];
        if (final_fang[16] > 0) ans+=" 三暗刻 " +final_fang[16];
        if (final_fang[17] > 0) ans+=" 三槓子 " +final_fang[17];
        if (final_fang[18] > 0) ans+=" 七對子" +final_fang[18];
        if (final_fang[19] > 0) ans+=" 混全帶么 " +final_fang[19];
        if (final_fang[20] > 0) ans+=" 混老頭 " +final_fang[20];
        if (final_fang[21] > 0) ans+=" 小三元 " +final_fang[21];
        if (final_fang[22] > 0) ans+=" 雙立直 " +final_fang[22];
        if (final_fang[23] > 0) ans+=" 混一色 " +final_fang[23];
        if (final_fang[24] > 0) ans+=" 純全帶么 " +final_fang[24];
        if (final_fang[25] > 0) ans+=" 二盃口 " +final_fang[25];
        if (final_fang[26] > 0) ans+=" 清一色 " +final_fang[26];
        if (final_fang[27] > 0) ans+=" 國士無雙 " +final_fang[27];
        if (final_fang[28] > 0) ans+=" 國士無雙13面待聽 " +final_fang[28];
        if (final_fang[29] > 0) ans+=" 大三元 " +final_fang[29];
        if (final_fang[30] > 0) ans+=" 四暗刻 " +final_fang[30];
        if (final_fang[31] > 0) ans+=" 字一色 " +final_fang[31];
        if (final_fang[32] > 0) ans+=" 綠一色 " +final_fang[32];
        if (final_fang[33] > 0) ans+=" 小四喜 " +final_fang[33];
        if (final_fang[34] > 0) ans+=" 大四喜 " +final_fang[34];
        if (final_fang[35] > 0) ans+=" 清老頭 " +final_fang[35];
        if (final_fang[36] > 0) ans+=" 九連寶燈 " +final_fang[36];
        if (final_fang[37] > 0) ans+=" 純正九連寶燈 " +final_fang[37];
        if (final_fang[38] > 0) ans+=" 四槓子 " +final_fang[38];
        if (final_fang[39] > 0) ans+=" 天和 " +final_fang[39];
        if (final_fang[40] > 0) ans+=" 地和 " +final_fang[40];
        if (final_fang[41] > 0) ans+=" 流局滿貫 " +final_fang[41];
        return ans;
    }
    public bool check_cannot_long(int get_card)
    {
        for (int i = 0; i < river_array_index; i++)
        {
            if (get_card == river_array[i]) return false;
            Add(river_array[i]);
            if (Howmany_fang(river_array[i]) > 0)
            {
                Remove(river_array[i]);
                return false;
            }
            Remove(river_array[i]);

        }


        return true;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    bool CompareList(List<pair> pair,pair value)
    {
        foreach(var item in pair)
        {
            if(item.a == value.a && item.b == value.b && item.c == value.c)return true;
        }
        return false;
    }
}






















