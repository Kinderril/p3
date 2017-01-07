
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RespawnPointToggle : MonoBehaviour
{
    public Toggle Toggle;
    public int ID;
    public Text text;


    void OnEnable()
    {
        StartCoroutine(WaitRnd());
    }

    private IEnumerator WaitRnd()
    {
        var anim = GetComponentInChildren<Animator>();
        if (anim != null)
            anim.enabled = false;
        yield return new WaitForSeconds(Random.value/2f);
        if (anim != null)
            anim.enabled = true;
    }
}
