using Unity.Cinemachine;
using UnityEngine;

namespace Platformer397
{
    public class CameraManager : MonoBehaviour
    {
        // References to the cinemachineVirtualCamera 
        [SerializeField] private Transform player;

        private void Awake()
        {
#if (!UNITY_ANDROID)
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
#endif
            if (player != null) { return; }
            player = GameObject.FindWithTag("Player").transform;

        }
    }
}
