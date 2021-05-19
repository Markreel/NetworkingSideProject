using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

public class EventManager : MonoBehaviour
{
    public const byte OpenOpenable = 1;

    public void SendEvent(PhotonView _photonView, string _method, object _value)
    {
        _photonView.SendMessage(_method, _value);


        //object[] content = new object[] { new Vector3(10.0f, 2.0f, 5.0f), 1, 2, 5, 10 }; // Array contains the target position and the IDs of the selected units
        //RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // You would have to set the Receivers to All in order to receive this event on the local client as well
        //PhotonNetwork.RaiseEvent(OpenOpenable, content, raiseEventOptions, SendOptions.SendReliable);
    }
}
