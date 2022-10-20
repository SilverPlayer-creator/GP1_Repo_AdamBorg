using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace StarterAssets
{
    public class Container : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] Transform _player;
        [SerializeField] float _speed;
        [SerializeField] float _minHeight, _maxHeight;
        [Header("Deposit Location")]
        [SerializeField] Transform _depositLocation;

        public delegate void TrashPickedUp(float _trashPickedup, float _maxTrash);
        public event TrashPickedUp OnTrashpickUp;
        public delegate void WinCondition();
        public event WinCondition OnWinCondition;

        [Header("Trash/Score Amount")]
        [SerializeField] int _maxScore;
        int _currentScore;

        [Header("Doors")]
        [SerializeField] Transform _door1, _door2;
        [SerializeField] float _doorSpeed;
        float _door1OpenRotation = 0, _door1CurrentRotation;
        float _door2OpenRotation = 0, _door2CurrentRotation;
        float _door1ClosedRotation = 60;
        float _door2ClosedRotation = 50;
        bool _doorsOpen;
        private Vacuum _vacuum;
        private void Awake()
        {
            float tries = 0;
            while (_vacuum == null && tries < 50)
            {
                _vacuum = _player.GetComponent<Vacuum>();
                tries++;
            }
            SubscribeToEvent();
        }
        private void Start()
        {
            OnTrashpickUp?.Invoke(0, _maxScore);
        }
        private void Update()
        {
            
            RotateDoors();
        }
        private void FixedUpdate()
        {
            float yPos = Mathf.Clamp(_player.position.y, _minHeight, _maxHeight);
            Vector3 desiredPos = new Vector3(transform.position.x, yPos, transform.position.z);
            Vector3 follow = Vector3.Lerp(transform.position, desiredPos, _speed * Time.fixedDeltaTime);
            transform.position = follow;
            
        }
        public void AddTrash(Trash trashToAdd, int scoreAdded)
        {
            //trashToAdd.gameObject.SetActive(false);
            trashToAdd.transform.position = _depositLocation.position;
            trashToAdd.OnDeposited();
            _currentScore += scoreAdded;
            OnTrashpickUp?.Invoke(_currentScore, _maxScore);
            //Debug.Log("Amount of trash in container: " + _trashAdded);
            if (_currentScore >= _maxScore)
            {
                OnWinCondition?.Invoke();
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            Trash trash = other.GetComponent<Trash>();
            if (trash != null && trash.IsFired)
            {
                trash.DrawToContainer(this);
            }
        }
        private void DoorStatus(bool status)
        {
            Debug.Log("Event listened to");
            _doorsOpen = status;
        }
        void SubscribeToEvent()
        {
            _vacuum.OnModeChange += DoorStatus;
            Debug.Log("Subscribed to event");
        }
        void RotateDoors()
        {
            Debug.Log("Doors should be open: " + _vacuum.CanFire);
            if (!_doorsOpen)
            {
                if (_door1CurrentRotation <= 50)
                {
                    _door1CurrentRotation += _doorSpeed * Time.deltaTime;
                }
                if (_door2CurrentRotation <= 60)
                {
                    _door2CurrentRotation += _doorSpeed * Time.deltaTime;
                }
            }
            else
            {
                if (_door1CurrentRotation >= 0)
                {
                    _door1CurrentRotation -= _doorSpeed * Time.deltaTime;
                }

                if (_door2CurrentRotation >= 0)
                {
                    _door2CurrentRotation -= _doorSpeed * Time.deltaTime;
                }
            }
            _door1.transform.localRotation = Quaternion.Euler(_door1CurrentRotation, _door1.localRotation.y, _door1.localRotation.z);
            _door2.transform.localRotation = Quaternion.Euler(_door2CurrentRotation, _door2.localRotation.y, _door2.localRotation.z);
        }
        private void OnDisable()
        {
            _vacuum.OnModeChange -= DoorStatus;
        }
    }    
}
