using System.Collections;
using AlonDorn.SimpleMobileCharacterController.Input;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


    public class FixedJoystickWithEvent : Joystick, IPointerInputProvider
    {
        
        public Vector2 PointerInputValue => _input;
        public UnityEvent<Vector2> PointerInputEvent { get; set; }

        private Vector2 _input;

        

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            CollectInputsAndPublish();
        }


        public override void OnDrag(PointerEventData eventData)
        {
            base.OnDrag(eventData);
            CollectInputsAndPublish();
        }


        void CollectInputsAndPublish()
        {
            if(PointerInputEvent != null)
            {
                _input.x = Horizontal;
                _input.y = Vertical;
                PointerInputEvent.Invoke(_input);
            }
            
        }

        public void Instantiate(UnityAction<Vector2> playerMovementAction)
        {
            GameObject newJoystick = Instantiate(this.gameObject, GameObject.Find("Canvas").GetComponent<Transform>());
            var uiJoystick = newJoystick.GetComponentInChildren<FixedJoystickWithEvent>();
            uiJoystick.PointerInputEvent = new UnityEvent<Vector2>();
            uiJoystick.PointerInputEvent.AddListener(playerMovementAction);
        }


       
    }

