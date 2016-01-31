using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class AllParametersContainer : MonoBehaviour
{

    public GameParameterElement PrefabGameElement;
    private List<GameParameterElement> elementsParams = new List<GameParameterElement>();
    public Transform layoutGameElements;
    private bool isInit = false;
    public void Init()
    {
        if (!isInit)
        {
            LoadTotalParameters();
            isInit = true;
        }
        else
        {
            UpgradeValues();
        }
    }


    private void LoadTotalParameters()
    {
        foreach (ParamType v in Enum.GetValues(typeof(ParamType)))
        {
            var item = DataBaseController.GetItem(PrefabGameElement);
            item.Init(v);
            item.gameObject.transform.SetParent(layoutGameElements);
            elementsParams.Add(item);
        }
    }
    public void UpgradeValues()
    {
        foreach (var parameterUpgradeElement in elementsParams)
        {
            parameterUpgradeElement.UpgradeData();
        }
    }
}

