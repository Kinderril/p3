using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading;
using UnityEngine;

public enum ItemId
{
    money,
    crystal,
    energy,
    health
}

public enum PoolType
{
    flyNumberInGame,
    flyNumberInUI,
    flyNumberWithPicture,
    effectVisual,
    bullet,
}

public class DataBaseController : Singleton<DataBaseController>
{
    private readonly Dictionary<ItemId, Sprite> ItemIdSprites = new Dictionary<ItemId, Sprite>();
    private readonly Dictionary<TalismanType, Sprite> TalismansSprites = new Dictionary<TalismanType, Sprite>();
    private readonly Dictionary<SpecialAbility, Sprite> SpecialsSprites = new Dictionary<SpecialAbility, Sprite>();
    private readonly Dictionary<MainParam, Sprite> MainParamSprites = new Dictionary<MainParam, Sprite>();
    private readonly Dictionary<ParamType, Sprite> ParamTypeSprites = new Dictionary<ParamType, Sprite>();
    private readonly Dictionary<Slot, Sprite> SlotSprites = new Dictionary<Slot, Sprite>();
    private readonly Dictionary<ItemId, Color> itemsColors = new Dictionary<ItemId, Color>();
    private readonly Dictionary<Rarity, Color> rarityColors = new Dictionary<Rarity, Color>();
    private readonly Dictionary<TalismanType, AbsorberWithPosition> TalismanEffects = new Dictionary<TalismanType, AbsorberWithPosition>();
    private readonly Dictionary<ParamType, Color> parameterColors = new Dictionary<ParamType, Color>();
    private readonly Dictionary<CraftItemType,Sprite> CraftItemsSprites = new Dictionary<CraftItemType, Sprite>(); 
    private readonly Dictionary<EffectType, VisualEffectBehaviour> visualEffectBehaviours = new Dictionary<EffectType, VisualEffectBehaviour>();
    private readonly Dictionary<int,Taple<int,int>> costByLevelItems = new Dictionary<int, Taple<int, int>>(); 
    
    public Chest chestPrefab;
    public QuestGiver QuestGiverPrefab;

    public DataStructs DataStructs;
    public CraftDB CraftDB;
    public FlyingNumbers FlyingNumber;
    public FlyingNumbers FlyingNumberWithPicture;
    public FlyNumberWIthDependence FlyNumberWIthDependence;
    public GoldMapItem GoldMapItemPrefab;
    public ItemMapItem ItemMapItemPrefab;
    public HealMapItem HealMapItemPrefab;
    public int maxLevel = 20;
    public List<BaseMonster> Monsters;
    public Dictionary<int, List<BaseMonster>> mosntersLevel = new Dictionary<int, List<BaseMonster>>();
    public Dictionary<int, Dictionary<int, string>> RespawnPositionsNames;

    public List<BossUnit> BossUnits = new List<BossUnit>(); 
    public Hero prefabHero;
    public List<Weapon> Weapons;
    public Pool Pool;

    public Shader simpleShader;
    public Shader flashShader;

    public void Init()
    {
        try
        {
            if (simpleShader == null)
                simpleShader = Shader.Find("Custom/FogInside");
            CraftDB = new CraftDB();
            for (var i = 0; i < maxLevel; i++)
            {
                mosntersLevel.Add(i, new List<BaseMonster>());
            }
            foreach (var baseMonster in Monsters)
            {
                mosntersLevel[baseMonster.ParametersScriptable.Level].Add(baseMonster);
            }
            LoadSprites();
            Pool = new Pool(this);
            LoadRespawnPointsNames();
            if (flashShader == null)
                flashShader = Shader.Find("Custom/FogInsideFlash");
            DebugController.Instance.InfoField2.text = flashShader.ToString();
        }
        catch (Exception ex)
        {
            DebugController.Instance.InfoField1.text = ex.ToString();
        }
    }
    

    private void LoadRespawnPointsNames()
    {
        RespawnPositionsNames = new Dictionary<int, Dictionary<int, string>>();
        RespawnPositionsNames.Add(1,new Dictionary<int, string>() { {1,"Trainings"} });
        RespawnPositionsNames.Add(2, new Dictionary<int, string>() { { 1, "Forest" } , { 2, "Lake" } , { 3, "Fields" } , { 4, "Town" } });

        for (int i = 1; i < DataStructs.MISSION_LAST_INDEX+1; i++)
        {
            var pos = DataStructs.GetRespawnPointsCountByMission(i);
            var namesCount = RespawnPositionsNames[i].Count;
            if (pos != namesCount)
            {
                Debug.LogError("WRONG DATA!!!! IN RESPAWN POINTS");
            }
        }
    }


    private void LoadSprites()
    {
        foreach (CraftItemType v in Enum.GetValues(typeof(CraftItemType)))
        {
            CraftItemsSprites.Add(v,UnityEngine.Resources.Load<Sprite>("sprites/CraftItems/" + v.ToString()));
        }

        foreach (MainParam v in Enum.GetValues(typeof(MainParam)))
        {
            MainParamSprites.Add(v, UnityEngine.Resources.Load<Sprite>("sprites/Parameters/" + v.ToString()));
        }

//        foreach (var mp in DataStructs.MainParametersImages)
//        {
//            MainParamSprites.Add(mp.type, Resources.Load<Sprite>("sprites/MainParameters/" + mp.path));
//        }
        foreach (Slot v in Enum.GetValues(typeof(Slot)))
        {
            SlotSprites.Add(v, UnityEngine.Resources.Load<Sprite>("sprites/Slot/" + v.ToString()));
        }

        foreach (ItemId v in Enum.GetValues(typeof(ItemId)))
        {
            ItemIdSprites.Add(v, UnityEngine.Resources.Load<Sprite>("sprites/Items/" + v.ToString()));
        }
//        foreach (var mp in DataStructs.ItemImage)
//        {
//            ItemIdSprites.Add(mp.type, Resources.Load<Sprite>("sprites/Items/" + mp.path));
//        }
//        foreach (var mp in DataStructs.ParametersImages)
//        {
//            ParamTypeSprites.Add(mp.type, Resources.Load<Sprite>("sprites/Parameters/" + mp.path));
//        }
        foreach (ParamType v in Enum.GetValues(typeof(ParamType)))
        {
            ParamTypeSprites.Add(v, UnityEngine.Resources.Load<Sprite>("sprites/Parameters/" + v.ToString()));
        }

        foreach (SpecialAbility v in Enum.GetValues(typeof(SpecialAbility)))
        {
            SpecialsSprites.Add(v, UnityEngine.Resources.Load<Sprite>("sprites/SpecialAbility/" + v.ToString()));
        }
        foreach (TalismanType v in Enum.GetValues(typeof(TalismanType)))
        {
            TalismansSprites.Add(v, UnityEngine.Resources.Load<Sprite>("sprites/Talisman/" + v.ToString()));
        }
        foreach (var colorUi in DataStructs.ColorsOfUI)
        {
            itemsColors.Add(colorUi.type,colorUi.color);
        }
        foreach (var colorUi in DataStructs.ColorRarity)
        {
            rarityColors.Add(colorUi.Rarity, colorUi.Color);
        }
        foreach (var effects in DataStructs.EffectVisualsTalisman)
        {
            if (effects.EffectAbsorber != null)
                TalismanEffects.Add(effects.type, effects.EffectAbsorber);
        }
        foreach (var colorUi in DataStructs.ColorParameter)
        {
            parameterColors.Add(colorUi.ParamType, colorUi.Color);
        }
        foreach (var effectVisualsBehaviour in DataStructs.EffectVisualsBehaviours)
        {
            visualEffectBehaviours.Add(effectVisualsBehaviour.type, effectVisualsBehaviour.beh);
        }
        foreach (var itemsByLevel in DataStructs.CostItemsByLevel)
        {
            costByLevelItems.Add(itemsByLevel.level,new Taple<int, int>(itemsByLevel.money, itemsByLevel.crystal));
        }
    }
    
    public Sprite MainParameterIcon(MainParam mp)
    {
        return MainParamSprites[mp];
    }


    public Taple<int, int> GetCostItemsByLevel(int level)
    {
        if (costByLevelItems.ContainsKey(level))
            return costByLevelItems[level];
        return new Taple<int, int>(0,0);
    }

    public Sprite CraftItemSprite(CraftItemType c)
    {
        return CraftItemsSprites[c];
    }

    public Sprite SlotIcon(Slot mp)
    {
        return SlotSprites[mp];
    }
    public VisualEffectBehaviour VisualEffectBehaviour(EffectType mp)
    {
        return visualEffectBehaviours[mp];
    }

    public Sprite ItemIcon(ItemId itemId)
    {
        return ItemIdSprites[itemId];
    }

    public Sprite ParameterIcon(ParamType mp)
    {
        return ParamTypeSprites[mp];
    }
    public Sprite SpecialAbilityIcon(SpecialAbility itemId)
    {
        return SpecialsSprites[itemId];
    }

    public Sprite TalismanIcon(TalismanType mp)
    {
        return TalismansSprites[mp];
    }

    public static T GetItem<T>(T item, Vector3 pos) where T : MonoBehaviour
    {
        return (Instantiate(item.gameObject, pos, Quaternion.identity) as GameObject).GetComponent<T>();
    }

    public static T GetItem<T>(T item) where T : MonoBehaviour
    {
        return (Instantiate(item.gameObject, Vector3.zero, Quaternion.identity) as GameObject).GetComponent<T>();
    }
    public static T GetItem<T>(string item) where T : MonoBehaviour
    {
        return (GameObject.Instantiate(Resources.Load(item)) as GameObject).GetComponent<T>();
    }


    public Color GetColor(ItemId f)
    {
        return itemsColors[f];
    }
    public Color GetColor(Rarity f)
    {
        return rarityColors[f];
    }
    public AbsorberWithPosition GetEffect(TalismanType f)
    {
        if (TalismanEffects.ContainsKey(f))
        {
            return TalismanEffects[f];
        }
        return null;
    }
    public Color GetColor(ParamType f)
    {
        return parameterColors[f];
    }
    public Color GetColor(CraftItemType arg1)
    {
        return DataStructs.CraftItemColor;
    }

    public CraftItemType GetRandomCraftItemType()
    {
        WDictionary<CraftItemType> d = new WDictionary<CraftItemType>(new Dictionary<CraftItemType, float>(
        ){
            {CraftItemType.Iron, 2f },
            {CraftItemType.Wood, 2f },
            {CraftItemType.Leather, 2f },
            {CraftItemType.Thread, 2f },

            {CraftItemType.Bone, 1f },
            {CraftItemType.Mercury, 1f },
            {CraftItemType.Gems, 1f },
            {CraftItemType.Silver, 1f },

            {CraftItemType.Splinter, 0.6f },
        })
        ;
        
        return d.Random();
    }
}