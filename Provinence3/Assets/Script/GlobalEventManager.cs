using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class GlobalEventManager : Singleton<GlobalEventManager>
{
    public event Action<BaseMonster> OnMonsterGetHit;

    public void MonsterGetHit(BaseMonster m)
    {
        if (OnMonsterGetHit != null)
        {
            OnMonsterGetHit(m);
        }
    }
}

