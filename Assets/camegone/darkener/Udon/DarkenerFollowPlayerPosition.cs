using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
// v to use VRCCameraSettings v
using VRC.SDK3.Rendering;

namespace camegone.Darkener{

    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class DarkenerFollowPlayerPosition : UdonSharpBehaviour
    {
        [SerializeField] private GameObject _objectBeMoved = null;
        [SerializeField] private bool _isFollowRotation = true;
        public bool IsFollowRotation
        {
            get => _isFollowRotation;
            set => _isFollowRotation = value;
        }
        /*
        void Start()
        {
            
        }
        */

        void Update()
        {
            var cam = VRCCameraSettings.ScreenCamera;
            if (cam == null)
                return;
            // move object
            _objectBeMoved.transform.position = cam.Position;
            if (IsFollowRotation)
            {
                // rotate object
                _objectBeMoved.transform.rotation = cam.Rotation;
            }
        }
    }
}