using UnityEngine;

public class PoolElement : MonoBehaviour
{
    private Transform baseParent;
    protected bool isUsing;
    public int ID_Pool = 0;
    public int timesUse = 0;

    public bool IsUsing
    {
        get { return isUsing; }
    }

    public void SetBaseParent(Transform baseParent)
    {
        this.baseParent = baseParent;
        transform.SetParent(baseParent, false);
    }

    public virtual void Init()
    {
        ID_Pool = Utils.GetId();
        timesUse++;
//        Debug.Log("Start Use " + isUsing + "  " + ID + "   " + name);
        isUsing = true;
        if (baseParent == null)
        {
            baseParent = DataBaseController.Instance.transform;
        }
        gameObject.SetActive(IsUsing);
    }

    public virtual void EndUse()
    {
        
//        Debug.Log("END Use " + isUsing + "  " + ID);
        if (gameObject != null)
        {
            isUsing = false;
            SetToBaseParent();
            gameObject.SetActive(IsUsing);
        }
    }

    public void SetToBaseParent()
    {
        if (name.Contains("Visu"))
            Debug.Log("SetToBaseParent " + name);
        transform.SetParent(baseParent, false);
    }
}