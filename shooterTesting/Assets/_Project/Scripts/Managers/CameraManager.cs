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
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            if (player != null) { return; }
            player = GameObject.FindWithTag("Player").transform;
        }
    }
}
