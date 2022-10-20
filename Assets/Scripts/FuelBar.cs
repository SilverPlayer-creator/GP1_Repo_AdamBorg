using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StarterAssets {
    public class FuelBar : MonoBehaviour
    {
        Slider fuelSlider;
        [SerializeField] JetPack jetPack;
   
        private void Start()
        {
            fuelSlider = GetComponent<Slider>();
            fuelSlider.maxValue = jetPack.GetMaxFuel();
            
        }

        private void Update()
        {
            fuelSlider.value = jetPack.GetCurrentFuel();
        }
    }
}

