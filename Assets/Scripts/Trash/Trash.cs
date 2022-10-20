using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace StarterAssets
{
    public class Trash : MonoBehaviour
    {
        [Header("Container related values")]
        [SerializeField] float _containerSpeed;
        [SerializeField] float _distanceToScore; //the minimum distance to the container before being added to it
        [SerializeField] float _minRandomDistance, _maxRandomDistance;
        [Header("Value")]
        [SerializeField] int _scoreValue;
        Rigidbody _rb;
        bool _movingToContainer;
        bool _fired;
        bool _beingSucked;
        float _distanceToContainer;
        bool _deposited;
        Container _container;
        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }
        private void FixedUpdate()
        {
            if (_movingToContainer)
            {
                Vector3 direction = _container.transform.position - transform.position;
                _rb.velocity = direction * _containerSpeed;
                _distanceToContainer = Vector3.Distance(transform.position, _container.transform.position);
            }
            if (_movingToContainer && _distanceToContainer <= _distanceToScore)
            {
                _container.AddTrash(this, _scoreValue);
            }
        }
        public Rigidbody Body => _rb;
        public void OnObjectedFired(Vector3 direction, float force)
        {
            _rb.AddForce(direction * force);
            _fired = true;
            _beingSucked = false;
            Debug.Log("Add force");
        }
        public void DrawToContainer(Container container)
        {
            if (_fired)
            {
                _movingToContainer = true;
                _container = container;
                _rb.velocity = Vector3.zero;
            }
        }
        public void OnObjectSpilt(Vector3 pos)
        {
            float randomX = Random.Range(_minRandomDistance, _maxRandomDistance);
            float randomY = Random.Range(0, _maxRandomDistance);
            float randomZ = Random.Range(_minRandomDistance, _maxRandomDistance);
            Vector3 randomDirection = new Vector3(randomX, randomY, randomZ);
            transform.position = pos + randomDirection;
            _rb.velocity = randomDirection;
        }
        public void IsBeingSucked(bool beingSucked)
        {
            _beingSucked = beingSucked;
        }
        public void OnDeposited()
        {
            _rb.velocity = Vector3.zero;
            _fired = false;
            _movingToContainer = false;
            _deposited = true;
        }
        public bool IsFired => _fired;
        public float GetScore => _scoreValue;
        public bool Deposited => _deposited;
    }
}
