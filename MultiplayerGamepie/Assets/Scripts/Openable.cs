using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace MarkCode.Multiplayer
{
    public class Openable : MonoBehaviourPun
    {
        [SerializeField] private Transform target;
        [SerializeField] private float openDuration;
        [SerializeField] private AnimationCurve openCurve;

        [SerializeField] private List<Keyhole> keyholesToBeActiveBeforeOpening = new List<Keyhole>();

        [SerializeField] private Vector3 closedPos;
        [SerializeField] private Vector3 openedPos;


        private float openValue;
        private bool isOpen;

        private IEnumerator IEOpen(bool _open)
        {
            photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
            float _timeKey = 0f;

            Vector3 _posA = target.localPosition;
            Vector3 _posB = _open ? openedPos : closedPos;
            isOpen = _open;

            while (_timeKey < 1f)
            {
                _timeKey += Time.deltaTime;
                float _curvedTimeKey = openCurve.Evaluate(_timeKey);

                target.localPosition = Vector3.Lerp(_posA, _posB, _curvedTimeKey);

                yield return new WaitForFixedUpdate();
            }
        }

        public void Open()
        {
            StartCoroutine(IEOpen(true));
            //ReceiveOpenEvent();
            //photonView.SendMessage("ReceiveOpenEvent");
        }

        public void Close()
        {
            StartCoroutine(IEOpen(false));
            //ReceiveOpenEvent();
            //photonView.SendMessage("ReceiveOpenEvent");
        }

        public void CheckKeyholesActiveness()
        {
            bool _allActive = true;

            foreach (var _keyhole in keyholesToBeActiveBeforeOpening)
            {
                if (!_keyhole.Active) { _allActive = false; break; }
            }

            if (_allActive && !isOpen) { Open(); }
            if (!_allActive && isOpen) { Close(); }
        }

    }
}
