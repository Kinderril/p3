using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;


public class WeaponChooser : MonoBehaviour, IPointerDownHandler,IPointerUpHandler
{
    public WeaponAnimation weaponAnimation;
    public bool isOpen = false;
    private Animator animator;
    private const string p_animator = "isOpen";
    public List<WeaponButton> weaponButons = new List<WeaponButton>(); 

    public void Init(List<Weapon> weapons)
    {
        animator = weaponAnimation.GetComponent<Animator>();
        weaponAnimation.Init(this);
        int i = 0;
        foreach (var weapon in weapons)
        {
            weaponButons[i].Init(weapon);
            i++;
        }

        foreach (var weaponButton in weaponButons)
        {
            if (weapons.Count < i)
            {
                weaponButton.Init(weapons[i]);
            }
            else
            {
                weaponButton.Lock();
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isOpen)
        {
        }
        animator.SetBool(p_animator, true);
        Debug.Log("OnPointerDown");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isOpen)
        {
            CheckPosition(eventData.hovered);
            animator.SetBool(p_animator, false);
        }
        Debug.Log(" UP up up " + eventData.hovered.Count);
    }

    private void CheckPosition(List<GameObject> hovered)
    {
        var weaponChooser = hovered.FirstOrDefault(x => x.GetComponent<WeaponButton>() != null);
        if (weaponChooser != null)
        {
            var weap = weaponChooser.GetComponent<WeaponButton>();
            weap.Select();
        }
    }

    public void AnimationOpenEnd()
    {
        isOpen = true;
    }

    public void AnimationCloseEnd()
    {
        isOpen = false;
    }
}

