using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

public static class SpellNameGenerator
{
    private static List<string> names;
    private static List<string> actions;
    static SpellNameGenerator()
    {
        var s1 =
            "Agate,Alyvia,Arabeth,Ardra,Brenna,Caryne,Dasi,Derris,Dynie,Eryke,Errine,Farale,Gavina,Glynna,Karran,Kierst,Kira,Kyale,Ladia,Mora,Moriana,Quiss,Sadi,Salina";
        var s2 ="Samia,Sephya,Shaundra,Siveth,Thana,Valiah,Zelda,Alaric,Alaron,Alynd,Asgoth,Berryn,Derrib,Eryk,Evo,Fausto,Gavin,Gorth,Jarak,Jasek,Kurn,Lan,Ledo,Lor";
        var s3 = "Mavel,Milandro,Sandar,Sharn,Tarran,Thane,Topaz,Tor,Torc,Travys,Trebor,Tylien,Vicart,Zircon";

        var namesTotal = s1 + "," + s2 + "," + s3;
        names = namesTotal.Split(',').ToList();

        var l1 =
            "Blaze,Fireball,Firebolt,Explodet,Blizzard,Alldain,CoralRain,Disperse,Sacrifice,Antidote,Majustis,MagicWall,Farewell,Radiant,Outside,Ray";
        var l2 = "Infermost,Blast,Bang,Zap,Beat,Strike,Fall,Shake,Push,Turn,Borrow";
        var secondNames = l1 + "," + l2;// + "," + s3;
        actions = secondNames.Split(',').ToList();
    }

    public static string GetName()
    {
        return names.RandomElement() + " " + actions.RandomElement();
    }
}

