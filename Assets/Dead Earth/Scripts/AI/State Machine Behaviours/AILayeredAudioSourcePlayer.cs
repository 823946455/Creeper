using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AILayeredAudioSourcePlayer : AIStateMachineLink
{
    [SerializeField] AudioCollection _collection = null;
    [SerializeField] int _bank = 0;
    [SerializeField] bool _looping = true;
    [SerializeField] bool _stopOnExit = false;

    float _prevLayerWeight = 0.0f;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animStateInfo, int layerIndex)
    {
        if (_stateMachine == null) return;
        float layerWeight = animator.GetLayerWeight(layerIndex);
        if (_collection != null)
        {
            if (layerIndex == 0 || layerWeight > 0.5f)
                _stateMachine.PlayAudio(_collection, _bank, layerIndex, _looping);
            else
                _stateMachine.StopAudio(layerIndex);
        }
        _prevLayerWeight = layerWeight;
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_stateMachine == null) return;
        float layerWeight = animator.GetLayerWeight(layerIndex);

        if (layerWeight != _prevLayerWeight && _collection != null)
        {
            if (layerWeight > 0.5) _stateMachine.PlayAudio(_collection, _bank, layerIndex, true);
            else _stateMachine.StopAudio(layerIndex);
        }
        _prevLayerWeight = layerWeight;
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_stateMachine && _stopOnExit)
            _stateMachine.StopAudio(layerIndex);


    }
}
