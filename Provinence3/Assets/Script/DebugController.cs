using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;


public class DebugController : Singleton<DebugController>
{
    public bool ALL_TIME_DROP = false;
    public bool ALL_TALISMAN_CHARGED = false;
    public bool MAIN_HERO_MEGAHP = false;
    public bool LESS_COUNT_BOSS_COME = false;
    public bool GET_START_BOOST = false;
    public bool GET_ALL_TYPE_WEAPONS_BOOST = false;
    public bool RESPAWN_TIME_CREEPS_FAST = false;
    public bool ALWAYS_GOOD_END = false;
    public bool CHANCE_STUN_100 = false;
    public bool QUEST_REWARD_ITEM = false;
    public bool QUEST_COMPLETE = false;

    public Text InfoField1;
    public Text InfoField2;
}

