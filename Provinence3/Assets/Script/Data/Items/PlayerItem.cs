using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public enum Rarity
{
    Normal,
    Magic,
    Rare,
    Uniq,
}
public enum SpecialAbility
{
    //W - need to work
    //2-work
    //1-work - need test
    //0-Cancel
    none = 0,
    penetrating,//2
    AOE,//0
    critical,//2
    homing,//0
    push,//0
    slow,//2
    removeDefence,//0
    vampire,//2
    chain,//0
    clear,//2
    dot,//W
    stun,//1
    distance,//1
    hp,//2
    shield,//2
}

public enum Slot
{
    physical_weapon,
    magic_weapon,
    body,
    helm,
    bonus,
    Talisman,
    executable,
    recipe,
}

public class PlayerItem : BaseItem ,IEnhcant
{
    public const int ENCHANT_PLAYER_COEF = 5;
    public Dictionary<ParamType, float> parameters;
    public SpecialAbility specialAbilities = SpecialAbility.none; 
    public Rarity Rare;
    public int enchant = 0;
    public const char FIRSTCHAR = '%';
    
    public PlayerItem(Dictionary<ParamType, float> pparams, Slot slot, Rarity rare, float totalPoints)
    {
        this.cost = Formuls.PlayerItemCost(totalPoints, rare);
        this.parameters = pparams;
        this.Slot = slot;
        this.Rare = rare;
        RenderCam.Instance.DoRender(slot,out icon);
        LoadTexture();
        Id = Utils.GetId();
        isEquped = false;
    }

//    public 

    public PlayerItem(Dictionary<ParamType, float> pparams, Slot slot, Rarity isRare, int cost,bool isEquiped,string name,string icon,int enchant,int id)
    {
        this.Id = id;
        Utils.SetId(Id);
        this.enchant = enchant;
        this.cost = cost;
        this.parameters = pparams;
        this.Slot = slot;
        this.name = name;
        this.Rare = isRare;
        this.isEquped = isEquiped;
        this.icon = icon;
        string p = "";
        p += "Slot:" + Slot;
        foreach (var pparam in pparams)
        {
            p += pparam.Key + "{" + pparam.Value + "}";
        }
        Debug.Log("Weapon loaded :  " + p);
    }

    public override void LoadTexture()
    {
        Utils.LoadTexture(icon,out IconSprite);
    }

    
    public override string Save()
    {
        StringBuilder par = new StringBuilder();
        foreach (var parameter in parameters)
        {
            par.Append((int)parameter.Key);
            par.Append(DPAR);
            par.Append(parameter.Value);
            par.Append(DELEM);
        }
        StringBuilder ss = new StringBuilder();
        ss.Append((int)Slot);
        ss.Append(DELEM);
        ss.Append((int)Rare);
        ss.Append(DELEM);
        ss.Append(icon.ToString());
        ss.Append(DELEM);
        ss.Append(name.ToString());
        ss.Append(DELEM);
        ss.Append(cost.ToString());
        ss.Append(DELEM);
        ss.Append(isEquped.ToString());
        ss.Append(DELEM);
        ss.Append(enchant.ToString());
        ss.Append(DELEM);
        ss.Append(Id.ToString());
        ss.Append(DELEM);
        StringBuilder specials = new StringBuilder();
        specials.Append((int)specialAbilities);
        var result = par.ToString() + MDEL + ss.ToString() + MDEL + specials.ToString();
//        Debug.Log("ITEM SAVE STRING :" + result);
        return result;
    }

    public void Enchant(int sum)
    {
        enchant = Mathf.Clamp(enchant + sum,0,30);
    }
    public BaseItem BaseItem
    {
        get { return this; }
    }

    public override char FirstChar()
    {
        return FIRSTCHAR;
    }

    public override void Activate(Hero hero,Level lvl)
    {
        foreach (var parameter in parameters)
        {
            hero.Parameters.SetAbsolute(parameter.Key, hero.Parameters[parameter.Key] + (1 + enchant / ENCHANT_PLAYER_COEF) * parameter.Value);
        }
    }

    public static PlayerItem Create(string item)
    {
        Debug.Log("Execute from:   " + item);
        var Part1 = item.Split(MDEL);
        var Part2 = Part1[1].Split(DELEM);
        //PART2
        Slot slot = (Slot) Convert.ToInt32(Part2[0]);
        Rarity isRare = (Rarity)Convert.ToInt32(Part2[1]);
        string icon = Part2[2];
        string name = Part2[3];
        int cost = Convert.ToInt32(Part2[4]);
        bool isEquped = Convert.ToBoolean(Part2[5]);
        int enchant = Mathf.Abs(Convert.ToInt32(Part2[6]));
        int id = Mathf.Abs(Convert.ToInt32(Part2[7]));
        //PART1
        var firstPart = Part1[0].Split(DELEM);
        Dictionary<ParamType, float> itemParameters = new Dictionary<ParamType, float>();
        //Debug.Log(">>>Part1[0]   " + Part1[0]);
        foreach (var s in firstPart)
        {
            if (s.Length < 3)
                break;
            var pp = s.Split(DPAR);
            ParamType type = (ParamType)Convert.ToInt32(pp[0]);
            float value = Convert.ToSingle(pp[1]);
            itemParameters.Add(type,value);
        }
        PlayerItem playerItem = new PlayerItem(itemParameters, slot, isRare, cost, isEquped, name, icon, enchant, id);
        //Debug.Log(">>>Part3[0]   :" + Part3.ToString());
        var Part3 = Part1[2];
        var spec = (SpecialAbility) Convert.ToInt32(Part3.ToString());
        playerItem.specialAbilities = spec;
        playerItem.LoadTexture();
        return playerItem;
    }
}

