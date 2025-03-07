using Spine.Unity.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerButton : MonoBehaviour
{
    public SpineboyBeginnerModel model;
    public void OnPowerButtonClicked()
    {
        if (model != null)
            model.TryPower();
    }
    
    
}

