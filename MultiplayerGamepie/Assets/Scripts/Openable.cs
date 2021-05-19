using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using Photon.Pun;

public class Openable : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform target;
    [SerializeField] private float openDuration;
    [SerializeField] private AnimationCurve openCurve;

    [SerializeField] private Vector3 closedPos;
    [SerializeField] private Vector3 openedPos;

    private float openValue;
    private bool isOpen;

    private void ReceiveOpenEvent()
    {
        StartCoroutine(IEOpen());
    }

    private IEnumerator IEOpen()
    {
        float _timeKey = 0f;

        Vector3 _posA = target.localPosition;
        Vector3 _posB = isOpen ? closedPos : openedPos;
        isOpen = isOpen ? false : true;

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
        ReceiveOpenEvent();
        photonView.SendMessage("ReceiveOpenEvent");
    }

}
