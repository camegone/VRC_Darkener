
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace camegone.Darkener{

    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class DarkenerFollowPlayerPosition : UdonSharpBehaviour
    {
        [SerializeField] private GameObject _objectBeMoved = null;
        /*
        void Start()
        {
            
        }
        */

        void Update()
        {
            var lPlayer = Networking.LocalPlayer;
            if (lPlayer == null)
                return;
            
            _objectBeMoved.transform.position = lPlayer.GetPosition();
        }
    }
}