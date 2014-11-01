using UnityEngine;
using System.Collections;

public class MoneyControl : MonoBehaviour
{

    public int money { get; set; }

    void Start()
    {
        //初始金钱
        money = 1000;
    }

    public void AddMoney(int i)
    {
        money += i;
        MainUI.playerui.ResetMoney();
    }

    public void Losemoney(int i)
    {
        money -= i;
        MainUI.playerui.ResetMoney();
    }
}
