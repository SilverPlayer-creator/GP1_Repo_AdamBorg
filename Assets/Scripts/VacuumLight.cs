using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarterAssets
{
    public class VacuumLight : MonoBehaviour
    {
        [SerializeField] Material _red, _green;
        [SerializeField] VacuumHeldTrash _holder;
        MeshRenderer _rend;
        Material _setMaterial;
        private void Awake()
        {
            _holder.OnTrashAmountChanged += ChangeLight;
            _rend = GetComponent<MeshRenderer>();
        }
        void ChangeLight(float current, float max)
        {
            float capacity = current / max;
            if (capacity < 1) _setMaterial = _green;
            else _setMaterial = _red;
            _rend.material = _setMaterial;
        }
        private void OnDisable()
        {
            _holder.OnTrashAmountChanged -= ChangeLight;
        }
    }
}

