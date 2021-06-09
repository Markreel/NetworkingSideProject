using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

namespace MarkCode.Multiplayer
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        public static GameManager Instance;

        [Tooltip("The prefab to use for representing the player")]
        public GameObject PlayerPrefab;

        private void Start()
        {
            Instance = this;

            if (PlayerPrefab == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
            }
            else
            {
                if (PlayerManager.LocalPlayerInstance == null)
                {
                    Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);

                    GameObject _obj = PhotonNetwork.Instantiate(PlayerPrefab.name, new Vector3(0f, 0f, 0f), PlayerPrefab.transform.rotation, 0);

                    KeyManager _keyMan = _obj.GetComponent<KeyManager>();
                    if(_keyMan != null && !PhotonNetwork.IsMasterClient)
                    {
                        _keyMan.SetSecondPlayerValues();
                    }
                }
                else
                {
                    Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
                }
            }
        }

        private void LoadArena()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("Trying to Load a level but we are not the master Client");
            }
            Debug.LogFormat("Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
            PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.CurrentRoom.PlayerCount);
        }

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        public override void OnPlayerEnteredRoom(Player _other)
        {
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", _other.NickName); // not seen if you're the player connecting

            if (PhotonNetwork.IsMasterClient)
            {
                //Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
                //LoadArena();
            }
        }

        public override void OnPlayerLeftRoom(Player _other)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", _other.NickName); // seen when other disconnects

            if (PhotonNetwork.IsMasterClient)
            {
                //Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
                //LoadArena();
            }
        }
    }
}