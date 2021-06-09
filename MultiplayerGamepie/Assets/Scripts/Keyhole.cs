using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MarkCode.Multiplayer
{
    public class Keyhole : MonoBehaviour
    {
        [SerializeField] public KeyType Type;
        [SerializeField] private Transform attachPoint;
        [SerializeField] private Transform exitPoint;
        [SerializeField] private List<Openable> OpenablesToUpdate = new List<Openable>();

        public bool Active { get; private set; }

        private void UpdateOpenables()
        {
            foreach (var _openable in OpenablesToUpdate)
            {
                _openable.CheckKeyholesActiveness();
            }
        }

        public void Activate(KeyManager _key)
        {
            _key.transform.parent = attachPoint;
            _key.transform.localPosition = new Vector3(0, -0.5f, 1.25f); 
            _key.transform.localEulerAngles = Type == KeyType.Gold ? new Vector3(0, -90, 45) : new Vector3(45, -180, 0);

            Active = true;
            UpdateOpenables();
        }

        public void Deactivate(KeyManager _key)
        {
            _key.transform.parent = null;
            _key.transform.position = exitPoint.position;
            _key.transform.forward = Vector3.forward;

            Active = false;
            UpdateOpenables();
        }
    }
}