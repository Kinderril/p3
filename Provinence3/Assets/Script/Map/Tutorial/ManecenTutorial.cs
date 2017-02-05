using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class ManecenTutorial : MonoBehaviour
{
    public TutorialLevel TutorialLevel;
    private bool isHitted = false;
    public bool OnlyWeapon;
    public BaseEffectAbsorber HitEffect;


    void OnTriggerEnter(Collider collider)
    {
        if (!isHitted)
        {
            var bullet = collider.GetComponent<Bullet>();
            if (bullet != null)
            {
                if (OnlyWeapon)
                {
                    if (bullet.bulletHolder is Weapon)
                    {
                        TutorialLevel.ManecenKilledWeapon();
                        isHitted = true;
                        Dispose();
                    }
                }
                else
                {
                    if (!(bullet.bulletHolder is Weapon))
                    {
                        TutorialLevel.ManecenKilledSpell();
                        isHitted = true;
                        Dispose();
                    }
                }
            }
        }
    }

    private void Dispose()
    {
        var par = transform.parent;
        HitEffect.transform.SetParent(par,true);
        HitEffect.Play();
        var g = GetComponent<BaseMonster>();
        TutorialLevel.Monsters.Remove(g);
        var p = transform.position;
        transform.position = new Vector3(p.x,p.y-100,p.z);
    }
}

