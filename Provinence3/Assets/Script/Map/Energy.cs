using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Energy
{

    private float powerLeft;
    private float maxpower = 60*3; // 3min;
    public Action<float, float> OnLeft;
    private Action<ItemId, int> activaAction;
//    private const float speedEnergyFall = 1.5f;
    public event Action OnRage;
    private bool isRageActivated = false;

    public Energy(Action<ItemId, int> activaAction,Action OnRage)
    {
        this.activaAction = activaAction;
        this.OnRage += OnRage;
    }

    public void Add(int value)
    {
        powerLeft = Mathf.Clamp(powerLeft + value, -1, maxpower);
        ActionPOwerLeft();
        activaAction(ItemId.energy, value);
    }
    private void ActionPOwerLeft()
    {
        if (!isRageActivated)
        {
            if (OnLeft != null)
            {
                OnLeft(powerLeft, maxpower);
            }

            if (powerLeft > maxpower)
            {
                if (OnRage != null)
                {
                    OnRage();
                }
                isRageActivated = true;
            }
        }
    }

    public void Dispose()
    {
        OnRage = null;
    }

    public void Update()
    {
        powerLeft += Time.deltaTime;
        ActionPOwerLeft();
    }

    public bool MorePowerLeft()
    {
        return powerLeft >= maxpower/2;
    }
}

