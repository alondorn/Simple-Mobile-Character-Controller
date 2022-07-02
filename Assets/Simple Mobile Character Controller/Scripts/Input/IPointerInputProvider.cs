using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AlonDorn.SimpleMobileCharacterController.Input
{
    public interface IPointerInputProvider
    {
        public Vector2 PointerInputValue { get; }

        public UnityEvent<Vector2> PointerInputEvent { get; set; }

        // public void Instantiate(UnityAction<Vector2> playerMovementAction);

    } 
}
