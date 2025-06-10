using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootStrapper : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (ApplicationManager.instance) ApplicationManager.instance.LoadMainMenu();  
    }

    
}
