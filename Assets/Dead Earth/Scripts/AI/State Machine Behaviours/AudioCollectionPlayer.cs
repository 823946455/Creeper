using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCollectionPlayer : AIStateMachineLink
{
    [SerializeField] ComChannelName _commandChannel = ComChannelName.ComChannel1;
    [SerializeField] AudioCollection _collection = null;
    [SerializeField] CustomCurve _customCurve = null;
    [SerializeField] StringList _layerExclusions = null; 

    int _previousCommand = 0;
    AudioManager _audioManager = null;
    int _commandChannelHash = 0;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animStateInfo, int layerIndex)
    {
        _audioManager = AudioManager.instance;
        _previousCommand = 0;
        if (_commandChannelHash == 0)
            _commandChannelHash = Animator.StringToHash(_commandChannel.ToString()); 
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animStateInfo, int layerIndex)
    {
        if (layerIndex != 0 && animator.GetLayerWeight(layerIndex).Equals(0.0f)) return;
        if (_stateMachine == null) return;

        if (_layerExclusions != null)
        {
            for(int i=0; i<_layerExclusions.count; i++)
            {
                if (_stateMachine.IsLayerActive(_layerExclusions[i]))
                {
                    return;
                }
            }
        }

        int customCommand = (_customCurve == null) ? 0 : Mathf.FloorToInt(_customCurve.Evaluate(animStateInfo.normalizedTime-(long)animStateInfo.normalizedTime));

        int command;
        if (customCommand != 0) command = customCommand;
        else command = Mathf.FloorToInt(animator.GetFloat(_commandChannelHash));
        if(_previousCommand!=command && command>0 && _audioManager!=null && _collection!=null)
        {
            int bank = Mathf.Max(0, Mathf.Min(command - 1, _collection.bankCount - 1));
            _audioManager.PlayOneShotSound(_collection.audioGroup,
                                            _collection[bank],
                                            _stateMachine.transform.position,
                                            _collection.volume,
                                            _collection.spatialBlend,
                                            _collection.priority);
        }
        _previousCommand = command;
    }
}
