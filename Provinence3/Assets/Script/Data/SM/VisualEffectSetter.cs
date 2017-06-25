using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;


public static class VisualEffectSetter
{
    public static Dictionary<int,Sprite> Icons = new Dictionary<int, Sprite>(); 
    public static Dictionary<int,AbsorberWithPosition> CastEffects = new Dictionary<int, AbsorberWithPosition>(); 
    public static Dictionary<int,BaseEffectAbsorber> BulletEffects = new Dictionary<int, BaseEffectAbsorber>();

    public static Dictionary<int,BaseEffectAbsorber> HitEffects = new Dictionary<int, BaseEffectAbsorber>(); 
    public static Dictionary<int,BaseEffectAbsorber> TotemEffects = new Dictionary<int, BaseEffectAbsorber>(); 
    public static Dictionary<int,BaseEffectAbsorber> LongEffects = new Dictionary<int, BaseEffectAbsorber>(); 

    private const string WAY_TO_ICON = "/Resources/sprites/Spell/";
    private const string WAY_TO_CAST_EFFECT = "/Resources/prefabs/visualEffects/Craft/Cast/";
    private const string WAY_TO_BULLET = "/Resources/prefabs/visualEffects/Craft/Bullet/";
    private const string WAY_TO_HIT = "";
    private const string WAY_TO_TOTEM = "";
    private const string WAY_TO_LONG_EFFECT = "";

    public static void LoadAll()
    {
        GetAllIcons();
        //        GetAllCasts();
        GetAll<BaseEffectAbsorber>(BulletEffects, WAY_TO_BULLET, "Bullet");
        GetAll<AbsorberWithPosition>(CastEffects, WAY_TO_CAST_EFFECT, "Cast");
    }

//    private static void GetAllCasts()
//    {
//        string way = Application.dataPath + WAY_TO_CAST_EFFECT;
//        var files = Directory.GetFiles(way, "*.prefab", SearchOption.AllDirectories);
//        var c = files.Length;
//        for (int i = 0; i < c; i++)
//        {
//            var way2 = "prefabs/visualEffects/Craft/Cast/" + i.ToString();
//            var b = UnityEngine.Resources.Load(way2) as GameObject;
//            CastEffects.Add(i,b.GetComponent<AbsorberWithPosition>());
//        }
//    }
    private static void GetAll<T>(Dictionary<int,T> toDictionary,string way1,string wayLast )
    {
//        var subType = ".prefab";
//        string way = Application.dataPath + way1;
//        string[] filesNames;
        //#if UNITY_ANDROID
        //#else
        //        filesNames = Directory.GetFiles(way, "*"+ subType, SearchOption.AllDirectories);

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


//        DirectoryInfo dataDir = new DirectoryInfo(way);
//        FileInfo[] fileinfo = dataDir.GetFiles("*" + subType);
//        filesNames =  fileinfo.Select(x => x.Name).ToArray();
//
//        var c = filesNames.Length;
//        for (int i = 0; i < c; i++)
//        {
//            var fileName = filesNames[i];
//
//            var lastnameArray = fileName.Split('/');
//            var lastname = lastnameArray[lastnameArray.Length - 1].Replace(subType,"");
//            var way2 = "prefabs/visualEffects/Craft/"+ wayLast + "/" + lastname.ToString();
//            var b = UnityEngine.Resources.Load(way2) as GameObject;
//            if (b == null)
//            {
//                Debug.LogError("Wrong "  + way1 +  "   " + wayLast);
//            }
//            toDictionary.Add(i,b.GetComponent<T>());
//        }
    }

    private static void GetAllIcons()
    {


        var aa = "sprites/Spell/";
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
            Icons.Add(i, spr);
        }


//
//        string way = Application.dataPath + WAY_TO_ICON;
//        FileInfo[] fileinfo;
//        int c = 0;
////#if UNITY_ANDROID
//        DirectoryInfo dataDir = new DirectoryInfo(way);
//        fileinfo = dataDir.GetFiles("*.png");
//        c = fileinfo.Length;
////#else
////        var files = Directory.GetFiles(way,"*.png", SearchOption.AllDirectories);
////        c = files.Length;
////#endif
//        for (int i = 0; i < c; i++)
//        {
//            var way2 = "sprites/Spell/" + i.ToString();
//            var b = UnityEngine.Resources.Load(way2);
//            Sprite spr;
//            var texture = b as Texture2D;
//            if (texture != null)
//            {
//                spr = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),new Vector2(0.5f,0.5f));
//            }
//            else
//            {
//                spr = b as Sprite;
//            }
//;
//            Icons.Add(i,spr);
//        }
    } 

    public static void Set(BaseSpell spell)
    {
//        return;

        var countIcon = Icons.Count;

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
            spell.BaseSummon.IdVisual = SMUtils.Range(0, TotemEffects.Count);
        }

    }

//    public static AbsorberWithPosition GetCaseById(int idCast)
//    {
//        return DataBaseController.GetItem<AbsorberWithPosition>(CastEffects[idCast]);
//    }
}

