using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

namespace MarkCode.Multiplayer
{
    [RequireComponent(typeof(TMP_InputField))]
    public class PlayerNameInputField : MonoBehaviour
    {
        private const string playerNamePrefKey = "PlayerName";
        private void Start()
        {
            string _defaultName = string.Empty;
            TMP_InputField _inputField = GetComponent<TMP_InputField>();
            if (_inputField != null)
            {
                if (PlayerPrefs.HasKey(playerNamePrefKey))
                {
                    _defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                    _inputField.text = _defaultName;
                }
            }

            PhotonNetwork.NickName = _defaultName;
        }

        public void SetPlayerName(string _value)
        {
            if (string.IsNullOrEmpty(_value))
            {
                Debug.LogError("Player Name is null or empty");
                return;
            }

            PhotonNetwork.NickName = _value;
            PlayerPrefs.SetString(playerNamePrefKey, _value);
        }

    }
}
