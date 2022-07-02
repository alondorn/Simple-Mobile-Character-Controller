
using AlonDorn.SimpleMobileCharacterController.Input;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;


namespace AlonDorn.SimpleMobileCharacterController.Player
{
    public class PlayerRotationController : MonoBehaviour
    {
        [SerializeField] private GameObject rotationInputProvider;
        [SerializeField] private Transform headRoot;

        private Vector3 _eulerAngleVelocity;
        private const float HeadXRotationSpeed = 10;
        private Rigidbody _rigidbody;
       


        private void Awake()
        {
            _eulerAngleVelocity = new Vector3(0, 10, 0);
            _rigidbody = GetComponent<Rigidbody>();
            if(headRoot == null)
                headRoot = transform.Find("head root");
        }

        private void Start()
        {
            if (rotationInputProvider != null)
            {
                var newPointerInputManager = Instantiate(rotationInputProvider);
                var newInputProvider = newPointerInputManager.GetComponentInChildren<IPointerInputProvider>();
                newInputProvider.PointerInputEvent = new UnityEvent<Vector2>();
                newInputProvider.PointerInputEvent.AddListener(RotateByMouse);
                
                // newInputProvider.Instantiate(RotateByMouse);
            }
        }


        void RotateByMouse(Vector2 mousePositionDelta)
        {
            Quaternion deltaRotation = Quaternion.Euler(_eulerAngleVelocity * mousePositionDelta.x * Time.fixedDeltaTime);
            _rigidbody.MoveRotation(_rigidbody.rotation * deltaRotation);

            if (headRoot)
            {
                Vector3 currentRotation = headRoot.localEulerAngles;

                float inputInfluence = (-mousePositionDelta.y * HeadXRotationSpeed * Time.fixedDeltaTime);

                // Calculate for multiples
                //currentRotation.x = currentRotation.x % 360;

                // Calculate for wrapping
                if (currentRotation.x > 180)
                    currentRotation.x -= 360f;

                // Modify the rotation vector and reassign it
                currentRotation.x = Mathf.Clamp(currentRotation.x + inputInfluence, -30, 50);
                headRoot.localRotation = Quaternion.Euler(currentRotation);
            }

        }



    } 
}
