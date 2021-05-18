using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;

namespace MarkCode.Multiplayer
{
    public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        [Tooltip("The current Health of our player")]
        public float Health = 1f;

        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        public static GameObject LocalPlayerInstance;

        [Tooltip("The Player's UI GameObject Prefab")]
        public GameObject PlayerUiPrefab;

        [Tooltip("The Beams GameObject to control")]
        [SerializeField] private GameObject beams;

        private bool IsFiring;

        private void Awake()
        {
            if (beams == null) { Debug.LogError("<Color=Red><a>Missing</a></Color> Beams Reference.", this); }
            else { beams.SetActive(false); }


            if (photonView.IsMine) { PlayerManager.LocalPlayerInstance = gameObject; }
            DontDestroyOnLoad(gameObject);

        }

        private void Start()
        {
            CameraWork _cameraWork = gameObject.GetComponent<CameraWork>();

            if (_cameraWork != null)
            {
                if (photonView.IsMine)
                {
                    _cameraWork.OnStartFollowing();
                }
            }
            else
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> CameraWork Component on playerPrefab.", this);
            }

            if (PlayerUiPrefab != null)
            {
                GameObject _uiGo = Instantiate(PlayerUiPrefab, transform);
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
                ProcessInputs();
                if (Health <= 0f)
                {
                    GameManager.Instance.LeaveRoom();
                }
            }

            // trigger Beams active state
            if (beams != null && IsFiring != beams.activeInHierarchy)
            {
                beams.SetActive(IsFiring);
            }
        }

        private void OnTriggerEnter(Collider _other)
        {
            if (!photonView.IsMine) { return; }

            // We are only interested in Beamers
            // we should be using tags but for the sake of distribution, let's simply check by name.
            if (!_other.name.Contains("Beam")) { return; }

            Health -= 0.1f;
        }

        private void OnTriggerStay(Collider _other)
        {
            // we dont' do anything if we are not the local player.
            if (!photonView.IsMine) { return; }

            // We are only interested in Beamers
            // we should be using tags but for the sake of distribution, let's simply check by name.
            if (!_other.name.Contains("Beam")) { return; }

            // we slowly affect health when beam is constantly hitting us, so player has to move to prevent death.
            Health -= 0.1f * Time.deltaTime;
        }

        private void ProcessInputs()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (!IsFiring)
                {
                    IsFiring = true;
                }
            }

            if (Input.GetButtonUp("Fire1"))
            {
                if (IsFiring)
                {
                    IsFiring = false;
                }
            }
        }

        public void OnPhotonSerializeView(PhotonStream _stream, PhotonMessageInfo _info)
        {
            if (_stream.IsWriting)
            {
                // We own this player: send the others our data
                _stream.SendNext(IsFiring);
                _stream.SendNext(Health);
            }
            else
            {
                // Network player, receive data
                IsFiring = (bool)_stream.ReceiveNext();
                Health = (float)_stream.ReceiveNext();
            }
        }

    }
}