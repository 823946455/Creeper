using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveKeypad : InteractiveItem
{
    [SerializeField] protected Transform _elevator = null;
    [SerializeField] protected AudioCollection _collection = null;
    [SerializeField] protected int _bank = 0;
    [SerializeField] protected float _activationDelay = 0.0f;

    bool _isActivated = false;

    public override string GetText()
    {
        ApplicationManager appDatabase = ApplicationManager.instance;
        if (!appDatabase) return string.Empty;

        string powerState = appDatabase.GetGameState("POWER");
        string lockdownState = appDatabase.GetGameState("LOCKDOWN");
        string accessCodeState = appDatabase.GetGameState("ACCESSCODE");

        if (string.IsNullOrEmpty(powerState) || !powerState.Equals("TRUE"))
        {
            return "Keypad : No Power";
        }
        else 
        if(string.IsNullOrEmpty(lockdownState)|| !lockdownState.Equals("FALSE"))
        {
            return "Keypad : Under Lockdown";
        }
        else
        if(string.IsNullOrEmpty(accessCodeState) || !accessCodeState.Equals("TRUE"))
        {
            return "Keypad : Access Code Required";
        }
        return "Keypad";
    }
    public override void Activate(CharacterManager characterManager)
    {
        if (_isActivated) return;
        ApplicationManager appDatabase = ApplicationManager.instance;
        if (!appDatabase) return;

        string powerState = appDatabase.GetGameState("POWER");
        string lockdownState = appDatabase.GetGameState("LOCKDOWN");
        string accessCodeState = appDatabase.GetGameState("ACCESSCODE");

        if (string.IsNullOrEmpty(powerState) || !powerState.Equals("TRUE")) return;
        if (string.IsNullOrEmpty(lockdownState) || !lockdownState.Equals("FALSE")) return;
        if (string.IsNullOrEmpty(accessCodeState) || !accessCodeState.Equals("TRUE")) return;

        StartCoroutine(DoDelayedActivation(characterManager));
        _isActivated = true;
    }
    protected IEnumerator DoDelayedActivation(CharacterManager characterManager)
    {
        if (!_elevator) yield break;
        if (_collection != null)
        {
            AudioClip clip = _collection[_bank];
            if (clip)
            {
                if (AudioManager.instance)
                    AudioManager.instance.PlayOneShotSound(_collection.audioGroup,
                                                            clip,
                                                            _elevator.position,
                                                            _collection.volume,
                                                            _collection.spatialBlend,
                                                            _collection.priority);
            }
        }
        yield return new WaitForSeconds(_activationDelay);

        if (characterManager != null)
        {
            characterManager.transform.parent = _elevator;
            Animator animator = _elevator.GetComponent<Animator>();
            if (animator) animator.SetTrigger("Activate");
            if (characterManager.fpsController)
            {
                characterManager.fpsController.freezeMovement = true;
            }
        }
    }
}
