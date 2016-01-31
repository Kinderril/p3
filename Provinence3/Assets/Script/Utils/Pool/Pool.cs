using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class Pool
{
//    private List<PoolElement> FlyNumberInUIpool = new List<PoolElement>();
//    private  FlyNumberInGamepool = new List<PoolElement>();
    private Dictionary<PoolType,List<PoolElement>> poolOfElements = new Dictionary<PoolType, List<PoolElement>>();
    private DataBaseController dataBaseController;

    public Pool(DataBaseController dataBaseController)
    {
        foreach (PoolType pType in Enum.GetValues(typeof(PoolType)))
        {
            poolOfElements.Add(pType,new List<PoolElement>());
        }
        this.dataBaseController = dataBaseController;
        Prewarm();
    }

    private void Prewarm()
    {
        for (int i = 0; i < 10; i++)
        {
            var element = DataBaseController.GetItem(dataBaseController.FlyNumberWIthDependence);
            element.gameObject.SetActive(false);
            poolOfElements[PoolType.flyNumberInGame].Add(element);
            element.transform.SetParent(dataBaseController.transform);
        }
        for (int i = 0; i < 10; i++)
        {

            var element = DataBaseController.GetItem(dataBaseController.FlyingNumber);
            element.gameObject.SetActive(false);
            poolOfElements[PoolType.flyNumberInUI].Add(element);
            element.transform.SetParent(dataBaseController.transform);
        }
    }

    public T GetItemFromPool<T>(PoolType poolType, Vector3 pos = default(Vector3)) where T : PoolElement
    {
        PoolElement element = null;
        var dic = poolOfElements[poolType];
        element = GetNoUsed(dic);
        if (element == null)
        {
            switch (poolType)
            {
                case PoolType.flyNumberInGame:
                    element = DataBaseController.GetItem(dataBaseController.FlyNumberWIthDependence);
                  break;
                case PoolType.flyNumberInUI:
                     element = DataBaseController.GetItem(dataBaseController.FlyingNumber);
                    break;
                case PoolType.flyNumberWithPicture:
                    element = DataBaseController.GetItem(dataBaseController.FlyingNumberWithPicture);
                break;
            }

            dic.Add(element);
        }

        element.transform.localPosition = pos;
        element.Init();
        return element as T;
    }

    public VisualEffectBehaviour GetItemFromPool(EffectType effectType)
    {
        VisualEffectBehaviour element = null;
        var dic = poolOfElements[PoolType.effectVisual];
        for (int i = 0; i < dic.Count; i++)
        {
            var e = dic[i] as VisualEffectBehaviour;
            if (!e.IsUsing && e.EffectType == effectType)
                return e ;
        }
        element = DataBaseController.GetItem(dataBaseController.VisualEffectBehaviour(effectType));
        dic.Add(element);
        return element; 
    }

    private PoolElement GetNoUsed(List<PoolElement> lis)
    {
        for (int i = 0; i < lis.Count; i++)
        {
            var e = lis[i];
            if (!e.IsUsing)
                return e;
        }
        return null;
    }
}

