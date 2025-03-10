using UnityEngine;

public class gunRotation : MonoBehaviour
{
    [SerializeField] private Transform mainCam;

    void Awake()
    {
        mainCam = Camera.main.transform;
    }
    void Update()
    {
        float weaponRotation = mainCam.eulerAngles.x;
        rotateGun(weaponRotation);
    }
       private void rotateGun(float weaponRotationX)
    {
        Vector3 newRotation = new Vector3(weaponRotationX, 0f, 0f);
        transform.localRotation = Quaternion.Euler(newRotation);
    }
}
