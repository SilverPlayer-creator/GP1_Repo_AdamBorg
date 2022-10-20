using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace StarterAssets
{
    public class LevelLights : MonoBehaviour
    {
        [SerializeField] Container _container;
        [SerializeField] float _intensityIncrease;
        Light _light;
        private void Awake()
        {
            _light = GetComponent<Light>();
            _container.OnTrashpickUp += IncreaseLight;
        }
        void IncreaseLight(float pickedUp, float max)
        {
            _light.intensity += (pickedUp / max) * _intensityIncrease;
        }
        private void OnDisable()
        {
            _container.OnTrashpickUp -= IncreaseLight;
        }
    }
}
