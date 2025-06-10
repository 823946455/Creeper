using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LockdownTrigger : MonoBehaviour
{
    [SerializeField] protected float _downloadTime = 10.0f;
    [SerializeField] protected Slider _downloadBar = null;
    [SerializeField] protected Text _hintText = null;
    [SerializeField] protected MaterialController _materialController = null;
    [SerializeField] protected GameObject _lockedLight = null;
    [SerializeField] protected GameObject _unlockedLight = null;

    private ApplicationManager _applicationManager = null;
    private GameSceneManager _gameSceneManager = null;
    private bool _inTrigger = false;
    private float _downloadProgress = 0.0f;
    private AudioSource _audioSource = null;
    private bool _downloadComplete = false;

    void OnEnable()
    {
        _applicationManager = ApplicationManager.instance;
        _audioSource = GetComponent<AudioSource>();

        _downloadProgress = 0.0f;

        if (_materialController != null)
            _materialController.OnStart();

        if (_applicationManager != null)
        {
            string lockedDown = _applicationManager.GetGameState("LOCKDOWN");
            if(string.IsNullOrEmpty(lockedDown) || lockedDown.Equals("TRUE"))
            {
                if (_materialController != null) _materialController.Activate(false);
                if (_unlockedLight) _unlockedLight.SetActive(false);
                if (_lockedLight) _lockedLight.SetActive(true);
                _downloadComplete = false;
            }
            else
            if (lockedDown.Equals(false))
            {
                if (_materialController != null) _materialController.Activate(true);
                if (_unlockedLight) _unlockedLight.SetActive(true);
                if (_lockedLight) _lockedLight.SetActive(false);
                _downloadComplete = true;
            }
        }
        ResetSoundandUI();
    }
    void Update()
    {
        if (_downloadComplete) return;
        if (_inTrigger)
        {
            if (Input.GetButton("Use"))
            {
                if (_audioSource && !_audioSource.isPlaying)
                    _audioSource.Play();

                _downloadProgress = Mathf.Clamp(_downloadProgress + Time.deltaTime, 0.0f, _downloadTime);
                if (_downloadProgress != _downloadTime)
                {
                    if (_downloadBar)
                    {
                        _downloadBar.gameObject.SetActive(true);
                        _downloadBar.value = _downloadProgress / _downloadTime;
                    }
                    return;
                }
                else
                {
                    _downloadComplete = true;

                    ResetSoundandUI();

                    if (_hintText) _hintText.text = "Successful Deactivation";

                    _applicationManager.SetGameState("LOCKDOWN", "FALSE");

                    if (_materialController != null) _materialController.Activate(true);
                    if (_unlockedLight) _unlockedLight.SetActive(true);
                    if (_lockedLight) _lockedLight.SetActive(false);
                    return;
                }
            }
        }
        _downloadProgress = 0.0f;
        ResetSoundandUI();
    }
    void ResetSoundandUI()
    {
        if (_audioSource && _audioSource.isPlaying) _audioSource.Stop();
        if (_downloadBar)
        {
            _downloadBar.value = _downloadProgress;
            _downloadBar.gameObject.SetActive(false);
        }
        if (_hintText) _hintText.text = "Hold 'Use' Button To Deactivate";
    }
    void OnTriggerEnter(Collider other)
    {
        if (_inTrigger || _downloadComplete) return;
        if (other.CompareTag("Player")) _inTrigger = true;

    }
    void OnTriggerExit(Collider other)
    {
        if (_downloadComplete) return;
        if (other.CompareTag("Player")) _inTrigger = false;
    }
}
