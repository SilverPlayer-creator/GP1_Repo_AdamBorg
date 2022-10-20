using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
namespace StarterAssets
{
    public class VacuumHeldTrash : MonoBehaviour
    {
        [Header("Trash Capacity")]
        List<Trash> _heldTrash = new List<Trash>();
        [SerializeField] int _maxCapacity;
        bool _capacityReached;
        [Header("Shoot Function")]
        [SerializeField] int _amountToShoot;
        [SerializeField] float _forwardOffset;
        //Events
        public delegate void SuckUp();
        public event SuckUp OnSuckUp;
        public delegate void Fire();
        public event Fire OnFire;
        public delegate void TrashAmountChange(float _currentTrashAmount, float _MaxCapacity);
        public event TrashAmountChange OnTrashAmountChanged;
        public void AddTrash(Trash newTrash)
        {
            _heldTrash.Add(newTrash);
            newTrash.Body.velocity = Vector3.zero;
            newTrash.gameObject.SetActive(false);
            if (_heldTrash.Count == _maxCapacity)
            {
                _capacityReached = true;
            }
            OnSuckUp?.Invoke();
            OnTrashAmountChanged?.Invoke(_heldTrash.Count, _maxCapacity);
        }

        public void ShootTrash(Vector3 pos, Quaternion rotation, Vector3 direction, float force)
        {
            if (_heldTrash.Count != 0)
            {
                //If more trash than amount to shoot, shoot out amount
                if (_heldTrash.Count > _amountToShoot)
                {
                    for (int i = 0; i < _amountToShoot; i++)
                    {
                        Trash firedTrash = _heldTrash[_heldTrash.Count - 1];
                        _heldTrash.Remove(firedTrash);
                        firedTrash.gameObject.SetActive(true);
                        firedTrash.transform.position = firedTrash.GetScore == 1 ? pos : pos + (transform.forward * _forwardOffset);
                        firedTrash.transform.rotation = rotation;
                        firedTrash.OnObjectedFired(direction, force);
                        OnTrashAmountChanged?.Invoke(_heldTrash.Count - i, _maxCapacity);
                        OnFire?.Invoke();
                    }
                }
                else
                {
                    for (int i = 0; i <= _heldTrash.Count; i++)
                    {
                        Trash firedTrash = _heldTrash[_heldTrash.Count - 1];
                        _heldTrash.Remove(firedTrash);
                        firedTrash.gameObject.SetActive(true);
                        firedTrash.transform.position = firedTrash.GetScore == 1 ? pos : pos + (transform.forward * _forwardOffset);
                        firedTrash.transform.rotation = rotation;
                        firedTrash.OnObjectedFired(direction, force);
                        OnTrashAmountChanged?.Invoke(_heldTrash.Count - i, _maxCapacity);
                        OnFire?.Invoke();
                    }
                }
                _capacityReached = false;
            }
        }
        public float GetTrashMass()
        {
            if (_heldTrash.Count != 0)
            {
                float _heldTrashMass = _heldTrash[_heldTrash.Count - 1].Body.mass;
                return _heldTrashMass;
            }
            return 0.1f;
        }
        public bool CapacityReached => _capacityReached;
        public float GetCapacity => (float)(_heldTrash.Count) / (float)_maxCapacity;
    }

}