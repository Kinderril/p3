using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class CraftResultPlace : MonoBehaviour
{

    public Image mainImage;
    public Text mainParameter;
    public Image catalysImage;

    public void Init(RecipeItem recipe, ExecCatalysItem catalys = null)
    {
        bool haveCatalys = catalys != null;
        catalysImage.gameObject.SetActive(haveCatalys);

    }
}

