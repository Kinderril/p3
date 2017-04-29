using UnityEngine;


public class BossSummoner : BossUnit
{
    private float summonTimeUpdate;
    public MonsterBornPosition BornPosition;

    public override void Init()
    {
        base.Init();
        BornPosition = Map.Instance.MonsterBornPositions.RandomElement();
        BornPosition.transform.position = transform.position;
        BornPosition.radius = 2f;
        BornPosition.unitsCout = 1;
        BornPosition.SetWork();
    }

    protected override void UpdateUnit()
    {
        base.UpdateUnit();
        if (summonTimeUpdate < Time.time)
        {
            summonTimeUpdate = Time.time + Random.Range(2f, 7f);
            BornPosition.BornMosters();
        }
    }


}

