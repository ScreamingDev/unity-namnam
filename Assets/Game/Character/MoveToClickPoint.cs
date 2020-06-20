using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

namespace Game.Character
{
    public class MoveToClickPoint : MonoBehaviour
    {
        public GameObject character;

        private Rigidbody body;
        private ThirdPersonUserControl ctrl;

        private Vector3 movement;

        private float update = 0.0f;

        private Vector3 xzOnly;
        private Vector3 xzPlus = new Vector3(1,0,1);
        private Vector3 xzMinus = new Vector3(1,0,1);

        private void Start()
        {
            body = character.GetComponent<Rigidbody>();
            ctrl = character.GetComponent<ThirdPersonUserControl>();
            xzOnly = new Vector3(1.0f,0,1.0f);
        }

        private void FixedUpdate()
        {
            return;
            // read inputs
            float h = 1.0f;
            float v = 1.0f;
            
            Transform m_Cam = Camera.main.transform;

            // calculate camera relative direction to move:
            Vector3 m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 m_Move = v*m_CamForward + h*m_Cam.right;
            
            Debug.Log(String.Format("{0} {1} {2}", m_Move.x, m_Move.y, m_Move.z));

#if !MOBILE_INPUT
            // walk speed multiplier
            if (Input.GetKey(KeyCode.LeftShift)) m_Move *= 0.5f;
#endif

            // pass all parameters to the character control script
            ctrl.Character().Move(m_Move, false, false);

            return;
            if (!Input.GetMouseButton(0)) return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (!Physics.Raycast(ray, out hit))
            {
                return;
            }

            Vector3 direction = (hit.point - body.position);

            // ctrl.Character().Move(1.0f * Vector3.forward +  * Vector3.right);
            
            // The further the mouse away the faster
            Vector3 move = Vector3.Scale(direction, xzOnly);
            move = Vector3.Min(new Vector3(1,0,1), move);
            move = Vector3.Max(new Vector3(-1,0,-1), move);

            // Vector3 move = Vector3.Scale(direction, xzOnly).normalized.Scale(xzOnly);
            
            Debug.Log(String.Format("{0} {1} {2}", move.x, move.y, move.z));
            
            ctrl.Character().Move(move, false, false);
            //ctrl.Character().Move(new Vector3(2.0f, 0, 0), false, false);
        }
    }
}