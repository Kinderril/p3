using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;


public static class VisualEffectSetter
{
    public static Dictionary<int, Sprite> IconsDamage = new Dictionary<int, Sprite>();
    public static Dictionary<int, Sprite> IconsBuff = new Dictionary<int, Sprite>();
    public static Dictionary<int, AbsorberWithPosition> CastEffects = new Dictionary<int, AbsorberWithPosition>();
    public static Dictionary<int, BaseEffectAbsorber> BulletEffects = new Dictionary<int, BaseEffectAbsorber>();

    public static Dictionary<int, BaseEffectAbsorber> HitEffects = new Dictionary<int, BaseEffectAbsorber>();
    public static Dictionary<int, SummonnerBehaviour> TotemObjects = new Dictionary<int, SummonnerBehaviour>();
    public static Dictionary<int, BaseEffectAbsorber> LongEffects = new Dictionary<int, BaseEffectAbsorber>();

    private const string WAY_TO_ICON = "/Resources/sprites/Spell/";
    private const string WAY_TO_CAST_EFFECT = "/Resources/prefabs/visualEffects/Craft/Cast/";
    private const string WAY_TO_BULLET = "/Resources/prefabs/visualEffects/Craft/Bullet/";
    private const string WAY_TO_HIT = "";
    private const string WAY_TO_TOTEM = "/Resources/prefabs/visualEffects/Craft/Totem/";
    private const string WAY_TO_LONG_EFFECT = "";

    public static void LoadAll()
    {
        GetAllIcons();
        GetAll<BaseEffectAbsorber>(BulletEffects, WAY_TO_BULLET, "Bullet");
        GetAll<AbsorberWithPosition>(CastEffects, WAY_TO_CAST_EFFECT, "Cast");
        GetAll<SummonnerBehaviour>(TotemObjects, WAY_TO_TOTEM, "Totem");
    }

    private static void GetAll<T>(Dictionary<int, T> toDictionary, string way1, string wayLast)
    {
        var aa = "prefabs/visualEffects/Craft/" + wayLast + "/";
        var allObjects = UnityEngine.Resources.LoadAll(aa);
        for (int i = 0; i < allObjects.Length; i++)
        {
            var obj = allObjects[i] as GameObject;
            if (obj == null)
            {
                Debug.LogError("Wrong " + way1 + "   " + wayLast);
            }
            toDictionary.Add(i, obj.GetComponent<T>());
        }
    }

    private static void GetAllIcons()
    {
        SetIcons("Buff", IconsBuff);
        SetIcons("Damage", IconsDamage);
    }

    private static void SetIcons(string s, Dictionary<int, Sprite> list)
    {

        var aa = "sprites/Spell/" + s + "/";
        var allObjects = UnityEngine.Resources.LoadAll(aa);
        for (int i = 0; i < allObjects.Length; i++)
        {
            var b = allObjects[i];
            Sprite spr;
            var texture = b as Texture2D;
            if (texture != null)
            {
                spr = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            }
            else
            {
                spr = b as Sprite;
            }
            ;
            list.Add(i, spr);
        }
    }

    public static void Set(BaseSpell spell)
    {
//        return;
        var icons = spell.IsPositive() == EffectPositiveType.Positive ? IconsBuff : IconsDamage;

        var countIcon = icons.Count;

        spell.IdIcon = SMUtils.Range(0, countIcon);
        spell.IdCast = SMUtils.Range(0, CastEffects.Count);
        if (spell.TargetType != spell.StartType)
        {
            foreach (var baseEffect in spell.Bullet.Effect)
            {
                if (baseEffect.Duration > 0.2f)
                {
                    baseEffect.EffectVisualId = SMUtils.Range(0, LongEffects.Count);
                }
                else
                {
                    baseEffect.EffectVisualId = SMUtils.Range(0, HitEffects.Count);
                }
            }
        }
        else
        {
            foreach (var baseEffect in spell.Bullet.Effect)
            {
                if (baseEffect.Duration > 0.2f)
                {
                    baseEffect.EffectVisualId = SMUtils.Range(0, LongEffects.Count);
                }
            }
        }
        spell.Bullet.IdVisual = SMUtils.Range(0, BulletEffects.Count);
        if (spell.BaseSummon != null)
        {
            spell.BaseSummon.IdVisual = SMUtils.Range(0, TotemObjects.Count);
        }

    }
}

