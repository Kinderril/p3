using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class Localizer
{

    public static string MainParameterName(MainParam mp)
    {
        string ss = "";
        switch (mp)
        {
            case MainParam.HP:
                ss = "Health points";
                break;
            case MainParam.DEF:
                ss = "Defence";
                break;
            case MainParam.ATTACK:
                ss = "Attack";
                break;
        }
        return ss;
    }
    public static string MainParameterInfo(MainParam mp)
    {
        string ss = "";
        switch (mp)
        {
            case MainParam.HP:
                ss = "+" + Formuls.HP_COEF + " Heaths";
                break;
            case MainParam.DEF:
                ss = "+" + Formuls.PDEF_COEF + " Physic defence \n" +
                     "+" + Formuls.MDEF_COEF + " Magic defence";
                break;
            case MainParam.ATTACK:
                ss = "+" + Formuls.PATTACK_COEF + " Physic Attack \n" +
                     "+" + Formuls.MATTACK_COEF + " Magic Attack";
                break;
        }
        return ss;
    }
}
