using UnityEngine;
using System.Collections;

public class TestScr : MonoBehaviour
{

    public FlashController FlashController;
	// Use this for initialization
	void Start () {
	    FlashController.Play();
	    StartCoroutine(Wa4fl());
	}

    private IEnumerator Wa4fl()
    {
        yield return new WaitForSeconds(5);
        FlashController.Play();
    }
}
