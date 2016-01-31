using System;
using System.Collections.Generic;
using System.Linq;
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
    private readonly Dictionary<EffectType, VisualEffectBehaviour> visualEffectBehaviours = new Dictionary<EffectType, VisualEffectBehaviour>();

    public List<IShopExecute> allShopElements;
    public Chest chestPrefab;
    
    public DataStructs DataStructs;
    public GameObject debugCube;
    public FlyingNumbers FlyingNumber;
    public FlyingNumbers FlyingNumberWithPicture;
    public FlyNumberWIthDependence FlyNumberWIthDependence;
    public MapItem MapItemPrefab;
    public int maxLevel = 20;
    public List<BaseMonster> Monsters;
    public Dictionary<int, List<BaseMonster>> mosntersLevel = new Dictionary<int, List<BaseMonster>>();
    public List<BossUnit> BossUnits = new List<BossUnit>(); 
    public Hero prefabHero;
    public List<Weapon> Weapons;
    public Pool Pool;

    private void Awake()
    {
        for (var i = 0; i < maxLevel; i++)
        {
            mosntersLevel.Add(i, new List<BaseMonster>());
        }
        foreach (var baseMonster in Monsters)
        {
            mosntersLevel[baseMonster.Parameters.Level].Add(baseMonster);
        }
        LoadSprites();
        Pool = new Pool(this);
    }

    private void LoadSprites()
    {
        foreach (var mp in DataStructs.MainParametersImages)
        {
            MainParamSprites.Add(mp.type, Resources.Load<Sprite>("sprites/MainParameters/" + mp.path));
        }
        foreach (var mp in DataStructs.SlotImage)
        {
            SlotSprites.Add(mp.type, mp.path);
        }
        foreach (var mp in DataStructs.ItemImage)
        {
            ItemIdSprites.Add(mp.type, Resources.Load<Sprite>("sprites/Items/" + mp.path));
        }
        foreach (var mp in DataStructs.ParametersImages)
        {
            ParamTypeSprites.Add(mp.type, Resources.Load<Sprite>("sprites/Parameters/" + mp.path));
        }
        foreach (var mp in DataStructs.SpecialAbilityImage)
        {
            SpecialsSprites.Add(mp.type,  mp.path);
        }
        foreach (var mp in DataStructs.TalismanImage)
        {
            TalismansSprites.Add(mp.type,  mp.path);
        }
        foreach (var colorUi in DataStructs.ColorsOfUI)
        {
            itemsColors.Add(colorUi.type,colorUi.color);
        }
        foreach (var effectVisualsBehaviour in DataStructs.EffectVisualsBehaviours)
        {
            visualEffectBehaviours.Add(effectVisualsBehaviour.type, effectVisualsBehaviour.beh);
        }
    }
    
    public Sprite MainParameterIcon(MainParam mp)
    {
        return MainParamSprites[mp];
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


    public Color GetColor(ItemId f)
    {
        return itemsColors[f];
    }
}