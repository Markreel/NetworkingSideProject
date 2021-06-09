using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;

public enum KeyType { Gold, Silver }

namespace MarkCode.Multiplayer
{
    public class KeyManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        public static GameObject LocalPlayerInstance;

        [SerializeField] private KeyType keyType;
        [SerializeField] private GameObject playerUiPrefab;

        [SerializeField] private float movementSpeed;

        private CharacterController characterController;

        private enum KeyState { Normal, Inserted }
        private KeyState keyState = KeyState.Normal;

        private Keyhole keyholeTarget;

        private void Awake()
        {
            if (photonView.IsMine)
            {
                PlayerManager.LocalPlayerInstance = gameObject;
                characterController = GetComponent<CharacterController>();
            }
            DontDestroyOnLoad(gameObject);

        }

        private void Start()
        {
            if (photonView.IsMine)
            {
                CameraFollow _camFollow = Camera.main.GetComponent<CameraFollow>();
                if (_camFollow != null) { _camFollow.Follow(transform); }
            }

            if (playerUiPrefab != null)
            {
                GameObject _uiGo = Instantiate(playerUiPrefab, transform);
                _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
            }
            else
            {
                Debug.LogWarning("<Color=Red><a>Missing</a></Color> PlayerUiPrefab reference on player Prefab.", this);
            }
        }

        private void Update()
        {
            if (photonView.IsMine)
            {
                if (keyholeTarget != null && Input.GetKeyDown(KeyCode.Space))
                {
                    switch (keyState)
                    {
                        default:
                        case KeyState.Normal:
                            keyholeTarget.Activate(this);
                            keyState = KeyState.Inserted;
                            break;
                        case KeyState.Inserted:
                            keyholeTarget.Deactivate(this);
                            keyState = KeyState.Normal;
                            break;
                    }
                }
            }
        }

        private void FixedUpdate()
        {
            if (photonView.IsMine)
            {
                if (keyState == KeyState.Normal)
                {
                    ProcessInputs();
                }
            }
        }

        private void OnTriggerEnter(Collider _other)
        {
            if (!photonView.IsMine) { return; }

            Keyhole _keyhole = _other.GetComponent<Keyhole>();
            if (_keyhole != null && _keyhole.Type == keyType) { keyholeTarget = _keyhole; }
        }

        private void OnTriggerExit(Collider _other)
        {
            if (!photonView.IsMine) { return; }

            Keyhole _keyhole = _other.GetComponent<Keyhole>();
            if (_keyhole == keyholeTarget) { keyholeTarget = null; }
        }

        private void ProcessInputs()
        {
            float _hor = Input.GetAxis("Horizontal");
            float _ver = Input.GetAxis("Vertical");

            Vector3 _move = new Vector3(_hor, 0, _ver) * movementSpeed / 100f;
            characterController.Move(_move);
        }

        public void SetSecondPlayerValues()
        {
            keyType = KeyType.Silver;
            transform.GetChild(0).localEulerAngles += Vector3.up * 90;
        }

        public void OnPhotonSerializeView(PhotonStream _stream, PhotonMessageInfo _info)
        {
            if (_stream.IsWriting)
            {

            }
            else
            {

            }
        }

    }
}