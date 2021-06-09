using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MarkCode.Multiplayer
{
    public class PlayerUI : MonoBehaviour
    {
        [Tooltip("UI Text to display Player's Name")]
        [SerializeField]
        private TextMeshProUGUI playerNameText;

        private KeyManager target;


        private void Update()
        {
            if (target == null)
            {
                Destroy(gameObject);
                return;
            }
        }

        public void SetTarget(KeyManager _target)
        {
            if (_target == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> PlayMakerManager target for PlayerUI.SetTarget.", this);
                return;
            }

            target = _target;
            if (playerNameText != null)
            {
                playerNameText.text = target.photonView.Owner.NickName;
            }
        }
    }
}