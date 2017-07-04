using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


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
        var item = DataBaseController.GetItem<SummonnerBehaviour>(cacheGameObject, p);
        item.Init(owner,spellInGame);
    }
}

