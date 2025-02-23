using UnityEngine;

namespace Platformer397
{

    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {

        [SerializeField] private InputReader input;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private Vector3 movement;

        [SerializeField] private float moveSpeed = 200f;
        [SerializeField] private float rotationSpeed = 200f;

        [SerializeField] private Transform mainCam;

        public Weapon hoveredWeapon = null;
        public AmmoBox hoveredAmmoBox = null;
        // Start is called once before the first execution of Update after the MonoBehaviour is created

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;
            mainCam = Camera.main.transform;
        }
        void Start()
        {
            input.EnablePlayerActions();
        }

        private void OnEnable()
        {
            input.Move += GetMovement;
            input.Interact += HandleInteraction;
        }

        private void OnDisable()
        {
            input.Move -= GetMovement;
            input.Interact -= HandleInteraction;
        }

        private void FixedUpdate()
        {
            UpdateMovement();
            UpdateRotation();
        }

        private void UpdateMovement()
        {
            var adjustedDirection = mainCam.forward * movement.z + mainCam.right * movement.x;
            adjustedDirection.y = 0f; // Prevents unwanted vertical movement

            if (adjustedDirection.magnitude > 0f)
            {
                //handle movement
                HandleMovement(adjustedDirection);
            }
            else
            {
                //not change, but need to apply rigidbody y movement for gravity
                rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
            }
        }

        private void HandleMovement(Vector3 adjustedMovement)
        {
            var velocity = adjustedMovement * moveSpeed * Time.fixedDeltaTime;
            rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z);
        }

        private void UpdateRotation()
        {
            Vector3 newRotation = new Vector3(0, mainCam.eulerAngles.y, 0);
            transform.rotation = Quaternion.Euler(newRotation);
        }

        private void GetMovement(Vector2 move)
        {
            movement.x = move.x;
            movement.z = move.y;
        }

        private void HandleInteraction(bool interact)
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject objectHitByRaycast = hit.transform.gameObject;

                if (objectHitByRaycast.GetComponent<Weapon>() && objectHitByRaycast.GetComponent<Weapon>().isActiveWeapon == false)
                {
                    hoveredWeapon = objectHitByRaycast.gameObject.GetComponent<Weapon>();
                    hoveredWeapon.GetComponent<Outline>().enabled = true;

                    if (interact)
                    {
                        WeaponManager.Instance.PickUpWeapon(objectHitByRaycast.gameObject);
                    }
                }
                else
                {
                    if (hoveredWeapon)
                    {
                        hoveredWeapon.GetComponent<Outline>().enabled = false;
                    }
                }

                //Ammo Box
                if (objectHitByRaycast.GetComponent<AmmoBox>())
                {
                    hoveredAmmoBox = objectHitByRaycast.gameObject.GetComponent<AmmoBox>();
                    hoveredAmmoBox.GetComponent<Outline>().enabled = true;

                    if (interact)
                    {
                        WeaponManager.Instance.PickupAmmo(hoveredAmmoBox);
                        Destroy(objectHitByRaycast.gameObject);
                    }
                }
                else
                {
                    if (hoveredAmmoBox)
                    {
                        hoveredAmmoBox.GetComponent<Outline>().enabled = false;
                    }
                }
            }
        }

    }
}
