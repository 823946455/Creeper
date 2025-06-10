using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStickyDetector : MonoBehaviour
{
    FPSController _controller = null;
    //use this for initialization
    void Start()
    {
        _controller = GetComponentInParent<FPSController>();
    }
    void OnTriggerStay(Collider col)
    {
        AIStateMachine machine = GameSceneManager.instance.GetAIStateMachine(col.GetInstanceID());
        if(machine!=null && _controller != null)
        {
            _controller.DoStickyness();
            machine.VisualThreat.Set(AITargetType.Visual_Player, 
                                    _controller.characterController, 
                                    _controller.transform.position, 
                                    Vector3.Distance(machine.transform.position, _controller.transform.position));
            machine.SetStateOverride(AIStateType.Attack);
        }
    }

}
