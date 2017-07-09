using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Schema;
using UnityEngine;


public class SummonInGame
{
    private Unit owner;
    private SpellInGame spellInGame;
    private SummonnerBehaviour cacheGameObject;

    public SummonInGame(SpellInGame spellInGame, Unit owner)
    {
        this.spellInGame = spellInGame;
        this.owner = owner;
        cacheGameObject = VisualEffectSetter.TotemObjects[spellInGame.sourseItem.SpellData.BaseSummon.IdVisual];
    }

    public void Use()
    {
        var p = owner.transform.position;
        p.x = p.x + SMUtils.Range(-1f, 1f);
        p.z = p.z + SMUtils.Range(-1f, 1f);
        var item = DataBaseController.GetItem<SummonnerBehaviour>(cacheGameObject, p);
//        var p = owner.transform.position;
//        item.transform.position = new Vector3();
        Utils.GroundTransform(item.transform);
        item.Init(owner,spellInGame);
    }
}

