using UnityEngine;


public class BossShield : BossUnit
{
    private float shieldTimeUpdate;
    private int lvl;
    public override void Init()
    {
        base.Init();
        lvl = Parameters.Level;
    }

    protected override void UpdateUnit()
    {
        base.UpdateUnit();
        if (shieldTimeUpdate < Time.time)
        {
            shieldTimeUpdate = Time.time + Random.Range(1f, 2f);
            if (Shield < lvl*50)
            {
                Shield += 3*lvl;
            }
        }
    }
}

