using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StarterAssets
{
    [RequireComponent(typeof(PlayerInput))]
    public class JetPack : MonoBehaviour
    {
        [Header("Jetpack")]
        [Tooltip("Jetpack fly speed of the character in m/s")]
        [SerializeField] [Range(0, 10)] private float _flySpeed;

        [Tooltip("Maximum flytime in seconds")]
        [SerializeField] [Range(0, 10)] private float _maxFuel;

        [Tooltip("Fuel recharge rate while grounded")]
        [SerializeField] [Range(0, 10)] private float _groundRefillRate;

        [Tooltip("Fuel recharge rate while airborne")]
        [SerializeField] [Range(0, 10)] private float _airRefillRate;

        [Tooltip("Delay before fuel recharge starts while falling")]
        [SerializeField] [Range(0, 5)] private float _airRefillDelay;

        [Tooltip("Vertical speed of the jetpack")]
        [SerializeField] [Range(0, 10)] private float _jetpackForce;

        [Tooltip("How much of your fuel bar it costs to start flying")]
        [SerializeField] [Range(0, 1)] private float _startCost;

        [Tooltip("How quickly your fuel bar depletes while flying")]
        [SerializeField] [Range(0, 5)] private float _flyCost;

        [Tooltip("Changes quickly the jetpack flame effect grows/shrinks")]
        [SerializeField] [Range(0, 10)] private float _windSpeed;

        private float _currentFuel;
        private float _fuelPercentage;
        private bool _started;
        private float _delayTimer = 0;
        private float _windSize;


        [Header("Components")]
        [SerializeField] private StarterAssetsInputs _input;
        [SerializeField] private AudioSource _jetpackSound;
        [SerializeField] private GameObject _jetWind;
        private Renderer __renderJetWind;

        private void Awake()
        {
            _input = GetComponent<StarterAssetsInputs>();
            __renderJetWind = _jetWind.GetComponent<Renderer>();
        }
        void Start()
        {
            _currentFuel = _maxFuel;

        }
        private void Update()
        {
            SoundToggle();

            __renderJetWind.material.SetFloat("_Alpha", _windSize);

            _fuelPercentage = _currentFuel / _maxFuel;

            if (!_input.jump)
            {
                _started = false;
            }
        }
        public float GetSpeed()
        {
            return _flySpeed;
        }
        public float GetPercentage()
        {
            return _fuelPercentage;
        }
        public float GetCurrentFuel()
        {
            return _currentFuel;
        }
        public float GetMaxFuel()
        {
            return _maxFuel;
        }
        public float GetForce()
        {
            return _jetpackForce;
        }


        public void Flying()
        {
            WindStart();
            _currentFuel -= Time.deltaTime * _flyCost;
            _delayTimer = 0;
        }
        public bool FullTank()
        {
            if (_currentFuel < _maxFuel)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public void GroundRefill()
        {
            WindStop();
            if (_currentFuel < _maxFuel)
            {
                _currentFuel += Time.deltaTime * _groundRefillRate;
            }

        }
        public void AirRefill()
        {
            WindStop();
            if (_currentFuel < _maxFuel)
            {
                _delayTimer += Time.deltaTime;
                if (_delayTimer > _airRefillDelay)
                {
                    _currentFuel += Time.deltaTime * _airRefillRate;
                }
            }

        }
        public void StartCost()
        {
            float startCost = _maxFuel * _startCost;
            if (!_started)
            {
                _started = true;
                WindStart();
                if (_currentFuel > startCost)
                {
                    _currentFuel -= startCost;
                }
                else
                {
                    _currentFuel = 0;
                }
            }
        }
        public bool StartCheck()
        {
            if (_fuelPercentage > _startCost)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void WindStart()
        {
            if(_windSize < 1.25)
            {
                _windSize += Time.deltaTime * _windSpeed;
            }

        }
        private void WindStop()
        {
            if (_windSize > 0)
            {
                _windSize -= Time.deltaTime * _windSpeed;
            }
        }
        private void SoundToggle()
        {
            if (_started && !_jetpackSound.isPlaying)
            {
                _jetpackSound.Play();
            }
            else if(!_started)
            {
                _jetpackSound.Stop();
            }
        }
    }
}