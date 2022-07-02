using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace AlonDorn.SimpleMobileCharacterController.Input
{
    public class PointerPointerInputManager : MonoBehaviour, IPointerInputProvider
    {
        public UnityEvent<Vector2> PointerInputEvent { get; set; }
        public Vector2 PointerInputValue { get; private set; }


        [SerializeField] private InputActionReference pointerDownAction;
        [SerializeField] private InputActionReference pointerMoveAction;


        private bool _isPointerDown = false;
        private bool _isTouchScreen;
        private int _currentRotationTouchID;



        private void OnEnable()
        {
            pointerDownAction.action.Enable();
            pointerDownAction.action.performed += OnPointerDown;
            pointerDownAction.action.canceled += OnPointerUp;

            pointerMoveAction.action.Enable();
            pointerMoveAction.action.performed += OnPointerDrag;
        }


        private void Start()
        {
            if(SystemInfo.deviceType == DeviceType.Handheld)
            {
                _isTouchScreen = true;
            }
        }


        private void OnPointerDown(InputAction.CallbackContext obj)
        {
            if (_isTouchScreen)
            {
               for(int i = 0; i < Touchscreen.current.touches.Count; i++)
               {
                   if (IsPointerOverUIObject(Touchscreen.current.touches[i].position.ReadValue())) 
                       continue;
                   _currentRotationTouchID = i;
                   _isPointerDown = true;
                   return;
               }
               _isPointerDown = false;
            }
            else
            {
                _isPointerDown = !IsPointerOverUIObject(Pointer.current.position.ReadValue());
            }
        }

        private void OnPointerDrag(InputAction.CallbackContext obj)
        {
            if (!_isPointerDown) return;
            if (_isTouchScreen)
            {
                var touchDeltaPosition = Touchscreen.current.touches[_currentRotationTouchID].delta.ReadValue();
                PointerInputValue = touchDeltaPosition;
            }
            else
            {
                PointerInputValue = obj.ReadValue<Vector2>();
            }

            PointerInputEvent?.Invoke(PointerInputValue);
        }


        private void OnPointerUp(InputAction.CallbackContext obj)
        {
            Debug.Log("OnTouchUp");
            if (_isTouchScreen)
            {
                if (Touchscreen.current.touches.Count == 0)
                {
                    _isPointerDown = false;
                }
            }
            else
            {
                _isPointerDown = false;
            }
        }




        public void Instantiate(UnityAction<Vector2> playerMovementAction)
        {
            GameObject newPointerInputManager = Instantiate(this.gameObject);
            var pointerInputManager = newPointerInputManager.GetComponentInChildren<PointerPointerInputManager>();
            pointerInputManager.PointerInputEvent = new UnityEvent<Vector2>();
            pointerInputManager.PointerInputEvent.AddListener(playerMovementAction);
        }



        private bool IsPointerOverUIObject(Vector2 touchPosition)
        {
            var eventData = new PointerEventData(EventSystem.current) { position = touchPosition };
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            return results.Count > 0;
        }

    } 
}
