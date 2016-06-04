using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class ExecutableItemInfo : BaseItemInfo
{
    public Text descField;
    public void Init(ExecutableItem executableItem)
    {
        base.Init(executableItem);
        NameLabel.text = executableItem.ExecutableType.ToString();
    }
}
