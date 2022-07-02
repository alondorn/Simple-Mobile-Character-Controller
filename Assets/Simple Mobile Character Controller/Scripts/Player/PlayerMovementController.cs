using System.Collections;
using System.Collections.Generic;
using AlonDorn.SimpleMobileCharacterController.Input;
using UnityEngine;
using UnityEngine.Events;


namespace AlonDorn.SimpleMobileCharacterController.Player
{
    public class PlayerMovementController : MonoBehaviour
    {

        [SerializeField] GameObject movementInputProvider;
        private Rigidbody _rigidbody;
        private float _horizontal;
        private float _vertical;
        private const float Speed = 350;


        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            Debug.Log("movement start");
            if (movementInputProvider != null)
            {
                //GameObject newJoystick = Instantiate(_inputProvider, GameObject.Find("Canvas").GetComponent<Transform>());
                //var uiJoystick = newJoystick.GetComponentInChildren<FixedJoystickWithEvent>();
                //uiJoystick.playerMovementEvent.AddListener(ProcessInputs);

                // var newInputProvider = movementInputProvider.GetComponentInChildren<IPointerInputProvider>();
                // newInputProvider.Instantiate(ProcessInputs);
                var uiCanvas = GameObject.Find("Canvas").GetComponent<Transform>();
                if (uiCanvas == null)
                {
                    Debug.LogWarning($"No UI Canvas found for instantiating the UI joystick. Skipping.");
                    return;
                }
                var newUIJoystick = Instantiate(movementInputProvider, uiCanvas);
                var newInputProvider = newUIJoystick.GetComponentInChildren<IPointerInputProvider>();
                newInputProvider.PointerInputEvent = new UnityEvent<Vector2>();
                newInputProvider.PointerInputEvent.AddListener(ProcessInputs);
            }
        }


        private void ProcessInputs(Vector2 axises)
        {
            _vertical = axises.y;
            _horizontal = axises.x;

            if (_vertical != 0 || _horizontal != 0)
            {
                _rigidbody.velocity = ((transform.forward * _vertical) + (transform.right * _horizontal)) * Speed * Time.fixedDeltaTime;
            }

        }
    }
}

