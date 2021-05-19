using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace MarkCode.Multiplayer
{
    public class PlayerAnimatorManager : MonoBehaviourPun
    {
        [SerializeField]
        private float directionDampTime = 0.25f;

        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            if (!animator)
            {
                Debug.LogError("PlayerAnimatorManager is Missing Animator Component", this);
            }
        }

        private void Update()
        {
            if (!photonView.IsMine && PhotonNetwork.IsConnected) { return; }

            if (!animator) { return; }

            AnimatorStateInfo _stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            if (_stateInfo.IsName("Base Layer.Run"))
            {
                // When using trigger parameter
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    animator.SetTrigger("Jump");
                }
            }

            float _hor = Input.GetAxis("Horizontal");
            float _ver = Input.GetAxis("Vertical");

            if (_ver < 0) { _ver = 0; }

            animator.SetFloat("Speed", _hor * _hor + _ver * _ver);
            animator.SetFloat("Direction", _hor, directionDampTime, Time.deltaTime);
        }
    }
}
