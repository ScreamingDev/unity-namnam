using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

namespace Game.Character
{
    public class MoveToClickPoint : MonoBehaviour
    {

        // Usually the ThirdPersonController group 
        public GameObject target;
       
        // The game char that is controlled
        private ThirdPersonCharacter _character;
        
        // Body of the players character
        private Rigidbody _body;
        
        // Player view / camera
        private Camera _mainCam;
        
        // Mouse-Distance-Scaling
        public float pointerSneakRange = 100.0f;

        // Reserve memory for often used information ...
        private RaycastHit _hit; 
        private Vector3 _movement;
        private Ray _ray;
        private readonly Vector3 _one = new Vector3(1, 0, 1);
        // ... instead of generating such things on-the-fly 60 times per second 

        private void Start()
        {
            _body = target.GetComponent<Rigidbody>();
            _character = target.GetComponent<ThirdPersonCharacter>();
            _mainCam = Camera.main;

            if (!_character)
            {
                throw new Exception("The assigned game object has no ThirdPersonCharacter");
            }
        }

        private void FixedUpdate()
        {
            if (false == Input.GetMouseButton(0))
            {
                return;
            }

            CalculateDistance();
            ApplySpeed();

            _character.Move(_movement);
        }

        /// <summary>
        /// Determine speed by mouse-to-char distance
        /// 
        /// The mouse will allow the character to sneak
        /// and run depending on the pointer-to-character distance on the screen.
        /// 
        /// <see cref="CalculateDistance"/>
        /// </summary>
        private void ApplySpeed()
        {
            _movement /= pointerSneakRange;
            _movement = Vector3.Min(_one, _movement);
        }

        private void CalculateDistance()
        {
            _ray = _mainCam.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(_ray, out _hit))
            {
                // Click is outside of the map
                return;
                
                // @todo B UX user can get stuck when no GameObject is on the ground
                // Example: Level design is a skybox/nothing on the ground and just a small path 
            }
            
            // @todo C UX char changes speed unintended
            // Example: An object on Z=10 has a different distance from the player body (in XY)
            // ... than the very same object on Z=0 due to the perspective,
            // ... so without moving the mouse the distance suddenly changes.
            _movement = (_hit.point - _body.position);
            _movement.y = 0; // we don't care about height differences for movement
        }
    }
}