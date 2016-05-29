
 using UnityEngine;
 using UnityEngine.UI;

public class RarityElement : MonoBehaviour
{
    public Image img;
    public void Set(Rarity rarity)
    {
        img.color = DataBaseController.Instance.GetColor(rarity);
    }
}

