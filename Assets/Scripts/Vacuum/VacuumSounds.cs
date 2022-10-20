using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarterAssets
{
    public class VacuumSounds : MonoBehaviour
    {
        private enum _state { inactive, started, going, stopping}
        private _state _currentState;
        [SerializeField] AudioClip _suckSoundStart, _suckSoundMid, _suckSoundEnd, _fireSound, _suckUpSound;
        float _timeToMidSound;
        float _midSoundLength, _stopSoundLength;
        bool _started;
        bool _stopped;
        bool _suckSoundPlayed;
        float _suckPitch = 1;
        VacuumHeldTrash _holder;
        Vacuum _vacuum;
        AudioSource _source;
        private void Awake()
        {
            _vacuum = GetComponent<Vacuum>();
            _holder = GetComponent<VacuumHeldTrash>();
            _source = GetComponent<AudioSource>();
            _holder.OnSuckUp += PlaySuckUpSound;
            _holder.OnFire += PlayFireSound;
            _suckPitch = 1;
        }
        private void Update()
        {
            PlaySuckSound();
            if (_holder.GetCapacity >= 0.5f)
            {
                _suckPitch = 0.5f;
            }
            else
            {
                _suckPitch = 1 - _holder.GetCapacity;
            }
            _source.pitch = _suckPitch;
        }
        void PlaySuckSound()
        {
            //if no sound has been played, play the start sound
            //if the start sound has played for a set time, go to the main sound
            //if the main sound has played for a set time, play it again
            //if the vacuum stops, play the stop sounds
            switch (_currentState)
            {
                case _state.inactive:
                    _started = false;
                    _stopped = false;
                    _midSoundLength = _suckSoundMid.length;
                    _timeToMidSound = _suckSoundStart.length;
                    _stopSoundLength = _suckSoundEnd.length;
                    if (_vacuum.IsSucking)
                    {
                        _currentState = _state.started;
                    }
                    break;
                case _state.started:
                    if (!_started)
                    {
                        _source.PlayOneShot(_suckSoundStart);
                        _started = true;
                    }
                    else
                    {
                        _timeToMidSound -= Time.deltaTime;
                        if(_timeToMidSound <= 0)
                        {
                            _source.Stop();
                            _started = false;
                            _currentState = _state.going;
                        }
                    }
                    if (!_vacuum.IsSucking)
                    {
                        _source.Stop();
                        _currentState = _state.stopping;
                    }
                    break;
                case _state.going:
                    //float _capacity = _holder.GetCapacity;
                    //float _pitch = Mathf.Clamp(_capacity, 0.5f, 1);
                    //Debug.Log("Pitch: " + _pitch);
                    if (!_suckSoundPlayed)
                    {
                        _midSoundLength = _suckSoundMid.length;
                        _source.PlayOneShot(_suckSoundMid);
                        _suckSoundPlayed = true;
                    }
                    else
                    {
                        _midSoundLength -= Time.deltaTime;
                        if(_midSoundLength <= 0)
                        {
                            _suckSoundPlayed = false;
                        }
                    }
                    if (!_vacuum.IsSucking)
                    {
                        _suckSoundPlayed = false;
                        _source.Stop();
                        _currentState = _state.stopping;
                    }
                    break;
                case _state.stopping:
                    if (!_stopped)
                    {
                        _source.PlayOneShot(_suckSoundEnd);
                        _stopped = true;
                    }
                    else
                    {
                        _stopSoundLength -= Time.deltaTime;
                        if(_stopSoundLength <= 0)
                        {
                            _currentState = _state.inactive;
                        }
                    }
                    break;
            }
        }
        void PlayFireSound()
        {
            _source.pitch = Random.Range(0.75f, 1);
            _source.PlayOneShot(_fireSound);
        }
        void PlaySuckUpSound()
        {
            _source.pitch = Random.Range(0.8f, 1);
            _source.PlayOneShot(_suckUpSound);
        }
        private void OnDisable()
        {
            _holder.OnSuckUp -= PlaySuckUpSound;
            _holder.OnFire -= PlayFireSound;
        }
    }
}

