using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlassBreakController : MonoBehaviour
{
    public Slider energySlider;
    public Button triggerButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startBreaking() {
        energySlider.enabled = false;
        triggerButton.enabled = false;
    }
}
