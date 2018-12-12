using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class San11Date
{
    public int year;
    public int month; //必须大于1
    public int days;//必须大于1
    public San11Date(int y ,int m,int d) {
        year = y;
        month = m;
        days = d;
    }

    public void AddTurn() {
        days += 10;
        if (days>30) {
            month += 1;
            if (month>12) {
                year += 1;
                month = 1;
            }
            days = 1;
        }
    }

    public override string ToString()
    {
        string str = "公元" + year + "年 " + month + "月" + days + "日";
        return str;
    }

}

