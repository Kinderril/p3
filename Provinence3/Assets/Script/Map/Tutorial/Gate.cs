using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class Gate : MonoBehaviour
{
    public void Open()
    {
        gameObject.SetActive(false);        
    }

    public void Close()
    {
        gameObject.SetActive(true);
    }
}

