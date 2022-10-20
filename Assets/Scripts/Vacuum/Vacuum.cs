using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace StarterAssets
{
    public class Vacuum : MonoBehaviour
    {
        [Header("Suck mode")]
        [SerializeField] _suckMode _currentMode;
        enum _suckMode { Mode1, Mode2 }

        [Header("Search")]
        [SerializeField] Transform _vacuumExit;
        [SerializeField] float _suckRadiusModeOne;
        [SerializeField] float _suckRadiusModeTwo;
        [SerializeField] int _maxSuckColliders;
        [SerializeField] [Range(1, 10)] int _suckSearchLength;
        [SerializeField] float _suckAngle;
        Collider[] _suckColliders;
        [Header("Suck strength")]
        [SerializeField] [Range(0.1f, 20)] float _defaultSuckStrength;
        [SerializeField] [Range(3.5f, 20f)] float _maxSuckStrength;
        [SerializeField] float _strengthIncreaseIncrement; //how much the suck strength should increase incrementally
        float _currentSuckStrength;
        float _currentMaxSuckStrength;
        [SerializeField] float _distanceToSuck;
        [SerializeField][Range(0.1f, 1f)] float _rumbleStrength;
        [SerializeField] LayerMask _trashLayer;
        [SerializeField] GameObject _suckVfx;
        bool _isSucking;
        float _windStrength;
        Renderer _suckRender;

        [Header("Collected Trash")]
        VacuumHeldTrash _trashHolder;

        [Header("Fire Function")]
        [SerializeField] float _shootStrength;
        [SerializeField] float _fireRate;
        [SerializeField] int _lineCount;
        LineRenderer _shootRenderer;
        float _nextFireTime;
        bool _canFire;
        public bool CanFire => _canFire;
        bool _canSuck;

        [Header("Mode Toggle")]
        [SerializeField] float _toggleDelay;
        [SerializeField] LayerMask _lineLayer;
        [SerializeField] AudioSource _source;
        [SerializeField] float _toggleInputDelay;
        [SerializeField] AudioClip _modeSound;
        float _toggleInputCountdown;
        public delegate void ModeChange(bool CanFire);
        public event ModeChange OnModeChange;
        float _nextToggleTime;
        bool _inAutoFireZone;
        bool _hasToggled;
        string _boolToTurnOff, _boolToTurnOn;
        

        List<Vector3> _searchSpots = new List<Vector3>();
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED

#endif
        private StarterAssetsInputs _input;

        private void Start()
        {
            _input = GetComponent<StarterAssetsInputs>();
            _currentSuckStrength = _defaultSuckStrength;
            _currentMaxSuckStrength = _maxSuckStrength;
            _trashHolder = GetComponent<VacuumHeldTrash>();
            _canSuck = true;
            _shootRenderer = GetComponent<LineRenderer>();
            _suckRender = _suckVfx.GetComponent<Renderer>();
            _suckColliders = new Collider[_maxSuckColliders];
            Debug.Log(_suckRender.material);
            _toggleInputCountdown = _toggleInputDelay;
        }
        private void Update()
        {
            Debug.Log("Sucking " + _input.MouseOneHeld);
            if (_input.MouseOneHeld && _canSuck)
            {
                Suck();
            }
            else if(!_input.MouseOneHeld && _canSuck || _input.MouseOneHeld && !_canSuck)
            {
                _searchSpots.Clear();
                _currentSuckStrength = _defaultSuckStrength;
                _isSucking = false;
            }
            if (_input.MouseOneHeld && _canFire && Time.time >= _nextFireTime)
            {
                FireTrash();
            }
            if(_input.MouseTwoReleased)
            {
                _input.RevertFireRelease();
                if (!_input.MouseOneHeld && !_inAutoFireZone)
                {
                    ToggleMode();
                }
            }
            DrawTrajectory(_vacuumExit.position);
            WindEffect();
            if (_hasToggled)
            {
                _toggleInputCountdown -= Time.deltaTime;
                switch (_boolToTurnOff)
                {
                    case "CanSuck":
                        _canSuck = false;
                        break;
                    case "CanFire":
                        _canFire = false;
                        InvokeEvent(false);
                        break;
                }
                if(_toggleInputCountdown <= 0)
                {
                    switch (_boolToTurnOn)
                    {
                        case "CanSuck":
                            _canSuck = true;
                            break;
                        case "CanFire":
                            _canFire = true;
                            InvokeEvent(true);
                            break;
                    }
                    _source.PlayOneShot(_modeSound);
                    _toggleInputCountdown = _toggleInputDelay;
                    _hasToggled = false;
                }
            }
            Rumble();
        }
        List<Vector3> SearchSpots()
        {
            List<Vector3> searchPos = new List<Vector3>();
            for (int i = 0; i < _suckSearchLength; i++)
            {
                Vector3 newSearch = _vacuumExit.forward * i;
                Vector3 exitPosition = _vacuumExit.position;
                searchPos.Add(newSearch + exitPosition);
            }
            return searchPos;
        }
        void IncreaseSuckStrength()
        {
            _maxSuckStrength = Mathf.Clamp(_maxSuckStrength, _defaultSuckStrength, _maxSuckStrength - _trashHolder.GetCapacity);
            if (_currentSuckStrength <= _currentMaxSuckStrength)
            {
                _currentSuckStrength += _strengthIncreaseIncrement * Time.deltaTime;
            }
                
            else
            {
                _currentSuckStrength = _currentMaxSuckStrength;
            }
            float capacity = _trashHolder.GetCapacity;
            if(capacity >= 0.25f &&  capacity <= 0.5f)
            {
                _currentMaxSuckStrength = _maxSuckStrength / 2;
            }
            if(capacity >= 0.5f && capacity <= 0.75f)
            {
                _currentMaxSuckStrength = _maxSuckStrength / 3;
            }
            if(capacity >= 0.75f && capacity <= 1)
            {
                _currentMaxSuckStrength = _maxSuckStrength / 4;
            }
            else
            {
                _currentMaxSuckStrength = _maxSuckStrength;
            }
        }
        void Suck()
        {
            _isSucking = true;
            if(_currentMode == _suckMode.Mode1)
            {
                _searchSpots = SearchSpots();
                foreach (Vector3 spot in _searchSpots)
                {
                    int searchPositionIndex = _searchSpots.IndexOf(spot) + 1; //+1 to make up for the first index, 0;
                    Collider[] foundObjects = Physics.OverlapSphere(spot, (_suckRadiusModeOne * searchPositionIndex), _trashLayer);
                    foreach (Collider item in foundObjects)
                    {
                        Trash trash = item.GetComponent<Trash>();
                        if (trash != null && !trash.Deposited)
                        {
                            trash.IsBeingSucked(true);
                            float distance = Vector3.Distance(trash.transform.position, _vacuumExit.position);
                            Vector3 direction = _vacuumExit.position - trash.transform.position;
                            trash.Body.velocity = direction * (_currentSuckStrength / distance);
                            if (distance <= _distanceToSuck && !_trashHolder.CapacityReached)
                            {
                                AddTrash(trash);
                            }
                        }
                    }
                }
            }
            if (_currentMode == _suckMode.Mode2)
            {
                int numColliders = Physics.OverlapSphereNonAlloc(_vacuumExit.position, _suckRadiusModeTwo, _suckColliders, _trashLayer);
                for (int i = 0; i < numColliders; ++i)
                {
                    Collider item = _suckColliders[i];
                    if (item.TryGetComponent(out Trash trash))
                    {
                        // Project them onto suck direction https://docs.unity3d.com/ScriptReference/Vector3.Project.html
                        Vector3 vecToTrash = item.transform.position - _vacuumExit.position;
                        Vector3 pointOnLine = _vacuumExit.position + Vector3.Project(vecToTrash, _vacuumExit.forward);

                        // Get closest point on collider for "good enough" overlap behavior
                        Vector3 closestPoint = item.ClosestPoint(pointOnLine);

                        float angle = Vector3.Angle(closestPoint - _vacuumExit.position, _vacuumExit.forward);

                        if (angle < _suckAngle && !trash.Deposited)
                        {
                            trash.IsBeingSucked(true);
                            float distance = Vector3.Distance(trash.transform.position, _vacuumExit.position);
                            //IncreaseSuckStrength();
                            Vector3 direction = _vacuumExit.position - trash.transform.position;
                            trash.Body.velocity = direction * (_currentSuckStrength / distance);
                            if (distance <= _distanceToSuck && !_trashHolder.CapacityReached)
                            {
                                AddTrash(trash);
                            }
                        }
                    }
                }
            }
            IncreaseSuckStrength();
            _windStrength = Mathf.Clamp(_windStrength += Time.deltaTime * 1.5f, 0, 1);
            _suckRender.material.SetFloat("_FillWind", _windStrength);
        }
        public void FireTrash()
        {
                Debug.Log("Fire");
                _trashHolder.ShootTrash(_vacuumExit.position, Quaternion.identity, _vacuumExit.forward, _shootStrength);
                _nextFireTime = Time.time + 1f / _fireRate;
        }
        void AddTrash(Trash trashtoAdd)
        {
            _trashHolder.AddTrash(trashtoAdd);
        }
        void ToggleMode()
        {
            if(Time.time >= _nextToggleTime && !_hasToggled)
            {
                _isSucking = false;
                //_canSuck = !_canSuck;
                //_canFire = !_canFire;
                if (_canSuck)
                {
                    StartToggle("CanSuck", "CanFire");
                }
                if(_canFire)
                {
                    StartToggle("CanFire", "CanSuck");
                }
                _nextToggleTime = Time.time + 1f / _toggleDelay;
                Debug.Log("Mode toggle");
            }
        }
        void StartToggle(string toTurnOff, string toTurnOn)
        {
            _hasToggled = true;
            _boolToTurnOff = toTurnOff;
            _boolToTurnOn = toTurnOn;
        }
        void InvokeEvent(bool status)
        {
            OnModeChange?.Invoke(status);
            Debug.Log("Event called as " + status);
        }
        public void DrawTrajectory(Vector3 startPos)
        {
            if (!_canFire)
            {
                _shootRenderer.positionCount = 0;
            }
            else
            {
                float mass = _trashHolder.GetTrashMass();
                Vector3 velocity = (_vacuumExit.forward * _shootStrength / 1) * Time.fixedDeltaTime;
                float flightDuration = (2 * velocity.y) + 10000;
                float stepTime = flightDuration / (float)_lineCount;
                _shootRenderer.positionCount = 2;
                for (int i = 0; i < _lineCount; i++)
                {
                    float timePassed = stepTime * i;
                    float height = velocity.y * timePassed - (0.5f * -Physics.gravity.y * timePassed * timePassed);
                    Vector3 curvePoint = startPos + new Vector3(velocity.x * timePassed, height, velocity.z * timePassed);
                    //_shootRenderer.SetPosition(i, curvePoint);
                }
                _shootRenderer.SetPosition(0, startPos);
                RaycastHit hitInfo;
                Ray ray = new Ray(startPos, startPos + transform.forward * 100);
                if (Physics.Raycast(ray, out hitInfo, _lineLayer))
                {
                    _shootRenderer.SetPosition(1, hitInfo.point);
                }
                else
                {
                    _shootRenderer.SetPosition(1, startPos + transform.forward * 100);
                }
            }
        }
        void AdvancedSuck()
        {
            //one overlapsphere that reaches the outer range of the search
            //check inside sphere, compare position to player normalized
            //do a dot product from the players
            //gets the angle between the object and player
            //use vector3 sqr instead of distance?          
        }
        void WindEffect()
        {
            if (_isSucking)
            {
                _windStrength = Mathf.Clamp(_windStrength += Time.deltaTime * 1.5f, 0, 1 - _trashHolder.GetCapacity);
            }
            else
            {
                _windStrength = Mathf.Clamp(_windStrength -= Time.deltaTime * 1.5f, 0, 1 - _trashHolder.GetCapacity);
            }
            _suckRender.material.SetFloat("_FillWind", _windStrength);
        }
        public bool IsSucking => _isSucking;
        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent(out Container container))
            {
                _inAutoFireZone = true;
                if (_canSuck)
                {
                    ToggleMode();
                }
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if(other.TryGetComponent(out Container container))
            {
                _inAutoFireZone = false;
                if (!_canSuck)
                {
                    ToggleMode();
                }
            }
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            switch (_currentMode)
            {
                case _suckMode.Mode1:
                    if (_suckSearchLength > 0)
                    {
                        List<Vector3> searchPos = new List<Vector3>();
                        for (int i = 0; i < _suckSearchLength; i++)
                        {
                            Vector3 newSearch = _vacuumExit.forward * i;
                            Vector3 exitPosition = _vacuumExit.position;
                            searchPos.Add(newSearch + exitPosition);
                        }
                        Gizmos.DrawWireSphere(_vacuumExit.position, _suckRadiusModeOne * 1);
                        foreach (Vector3 pos in searchPos)
                        {
                            Gizmos.DrawWireSphere(pos, _suckRadiusModeOne * (searchPos.IndexOf(pos) + 1));
                        }
                    }
                    break;
                case _suckMode.Mode2:
                    Vector3 _pos = _vacuumExit.position;
                    Gizmos.DrawWireSphere(_pos, _suckRadiusModeTwo);
                    break;
            }
        }
        void Rumble()
        {
            Debug.Log("Rumble");
            if (Gamepad.all.Count > 0)
            {
                if(_trashHolder.GetCapacity == 1)
                {
                    Gamepad.current.SetMotorSpeeds(_rumbleStrength, _rumbleStrength);
                }
                else
                {
                    Gamepad.current.SetMotorSpeeds(0, 0);
                }
            }
        }
        public void Restart()
        {
            Debug.Log("Load first level");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, 0);
        }
    }
}

