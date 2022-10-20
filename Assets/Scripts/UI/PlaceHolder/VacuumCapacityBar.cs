using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace StarterAssets
{
    public class VacuumCapacityBar : MonoBehaviour
    {
        [SerializeField] VacuumHeldTrash _trashHolder;
        [SerializeField] float _smoothTime;
        float _capacity;
        Image _image;
        private void Awake()
        {
            _image = GetComponent<Image>();
            _trashHolder.OnTrashAmountChanged += ChangeUI;
        }

        // Update is called once per frame
        void Update()
        {
            if (_image.fillAmount != _capacity)
            {
                _image.fillAmount = Mathf.Lerp(_image.fillAmount, _capacity, _smoothTime * Time.deltaTime);
            }
        }
        void ChangeUI(float currentAmount, float maxCapacity)
        {
            float percent = currentAmount / maxCapacity;
            _capacity = percent;
        }
        private void OnDisable()
        {
            _trashHolder.OnTrashAmountChanged -= ChangeUI;
        }
    }
}
