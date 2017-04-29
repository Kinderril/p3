using UnityEngine;


public class BossHealing : BossUnit
{
    private float HealingTimeUpdate;

    protected override void UpdateUnit()
    {
        base.UpdateUnit();
        if (HealingTimeUpdate < Time.time)
        {
            HealingTimeUpdate = Time.time + Random.Range(3f, 5f);
            var hp2heal = Parameters.MaxHp / 4f;
            GetHeal(hp2heal);
        }
    }
}

