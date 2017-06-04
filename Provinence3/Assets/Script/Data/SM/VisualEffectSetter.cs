using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;


public static class VisualEffectSetter
{
    public static Dictionary<int,Sprite> Icons = new Dictionary<int, Sprite>(); 

    private const string WAY_TO_ICON = "/Resources/sprites/Spell";
    private const string WAY_TO_CAST_EFFECT = "";
    private const string WAY_TO_BULLET = "";
    private const string WAY_TO_HIT = "";
    private const string WAY_TO_TOTEM = "";
    private const string WAY_TO_LONG_EFFECT = "";

    public static void LoadAll()
    {
        GetAllIcons();
    }

    private static void GetAllIcons()
    {
        string way = Application.dataPath + WAY_TO_ICON;
        var files = Directory.GetFiles(way,"*.png", SearchOption.AllDirectories);
        var c = files.Length;
        for (int i = 0; i < c; i++)
        {
            var way2 = "sprites/Spell/" + i.ToString();
            var b = UnityEngine.Resources.Load(way2);
            Sprite spr;
            var texture = b as Texture2D;
            if (texture != null)
            {
                spr = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),new Vector2(0.5f,0.5f));
            }
            else
            {
                spr = b as Sprite;
            }
;
            Icons.Add(i,spr);
        }
    } 

    public static void Set(BaseSpell spell)
    {
        var countIcon = Directory.GetFiles(
             Application.dataPath + WAY_TO_ICON,
             "*.png", SearchOption.AllDirectories).Length;
        var countCast = Directory.GetFiles(
             Application.dataPath + WAY_TO_CAST_EFFECT,
             "*.prefab", SearchOption.AllDirectories).Length;
        var countBullets = Directory.GetFiles(
             Application.dataPath + WAY_TO_BULLET,
             "*.prefab", SearchOption.AllDirectories).Length;
        var countHits = Directory.GetFiles(
             Application.dataPath + WAY_TO_HIT,
             "*.prefab", SearchOption.AllDirectories).Length;
        var countLongs = Directory.GetFiles(
             Application.dataPath + WAY_TO_LONG_EFFECT,
             "*.prefab", SearchOption.AllDirectories).Length;
        var totemsCnt = Directory.GetFiles(
             Application.dataPath + WAY_TO_TOTEM,
             "*.prefab", SearchOption.AllDirectories).Length;

        spell.IdIcon = SMUtils.Range(0, countIcon);
        spell.IdCast = SMUtils.Range(0, countCast);
        if (spell.TargetType != spell.StartType)
        {
            foreach (var baseEffect in spell.Bullet.Effect)
            {
                if (baseEffect.Duration > 0.2f)
                {
                    baseEffect.EffectVisualId = SMUtils.Range(0, countLongs);
                }
                else
                {
                    baseEffect.EffectVisualId = SMUtils.Range(0, countHits);
                }
            }
        }
        else
        {
            foreach (var baseEffect in spell.Bullet.Effect)
            {
                if (baseEffect.Duration > 0.2f)
                {
                    baseEffect.EffectVisualId = SMUtils.Range(0, countLongs);
                }
            }
        }
        spell.Bullet.IdVisual = SMUtils.Range(0, countBullets);
        if (spell.BaseSummon != null)
        {
            spell.BaseSummon.IdVisual = SMUtils.Range(0, totemsCnt);
        }

    }
}

