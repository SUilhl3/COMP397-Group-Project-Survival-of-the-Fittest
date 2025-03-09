using UnityEngine;

namespace Platformer397
{

    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {

        [Header("Player Movement")]
        [SerializeField] private InputReader input;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private Vector3 movement;

        [SerializeField] private float moveSpeed = 200f;
        [Header("Camera")]
        [SerializeField] private Transform mainCam;

        [Header("Misc")]
        public string currentRoom = "Room1";

        [SerializeField] private int money = 500;

        public Weapon hoveredWeapon = null;
        public AmmoBox hoveredAmmoBox = null;

        [Header("Player Health")]
        [SerializeField] private float playerHealth = 100f;
        [SerializeField] private int playerMaxHealth = 100;
        [SerializeField] private float lastTimeHit = 0f;
        [SerializeField] private float timeToHeal = 3f;
        private float regenTimer = 0f;
        private float regenDuration = 3f; //time it takes to regen to full hp

        [Header("Perks: ")]
        private bool hasJug = false;
        private bool hasSpeedCola = false;
        private bool hasDoubleTap = false;
        private bool hasQuickRevive = false;
        private bool hasDeadshot = false;

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

        void Update()
        {
            lastTimeHit += Time.deltaTime;
            if(lastTimeHit >= timeToHeal && playerHealth < playerMaxHealth)
            {
                regenTimer += Time.deltaTime;
                playerHealth += (playerMaxHealth / regenDuration) * Time.deltaTime;
                playerHealth = Mathf.Min(playerHealth, playerMaxHealth);
            }
            else{regenTimer = 0f;}
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

        public int getMoney(){return money;}
        public void addMoney(int amount){money += amount;}

        public void decreaseMoney(int amount)
        {
            if (money - amount < 0){}
            else{money -= amount;}
        }
        //I dont think this does anything
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

                // //Ammo Box
                // if (objectHitByRaycast.GetComponent<AmmoBox>())
                // {
                //     hoveredAmmoBox = objectHitByRaycast.gameObject.GetComponent<AmmoBox>();
                //     hoveredAmmoBox.GetComponent<Outline>().enabled = true;

                //     if (interact)
                //     {
                //         WeaponManager.Instance.PickupAmmo(hoveredAmmoBox);
                //         Destroy(objectHitByRaycast.gameObject);
                //     }
                // }
                // else
                // {
                //     if (hoveredAmmoBox)
                //     {
                //         hoveredAmmoBox.GetComponent<Outline>().enabled = false;
                //     }
                // }
            }
        } //end of HandleInteraction

        public void takeDamage(int amount)
        {
            Debug.Log("Player took damage");
            playerHealth -= amount;
            lastTimeHit = 0;
            if (playerHealth <= 0)
            {
                //do game over stuff 
                //transistion to game over screen
                Debug.Log("Game Over");
            }
        }

        public void maxHPChange(int amount)
        {
            playerMaxHealth += amount;
        }

        public bool checkPerk(string perk)
        {
            switch(perk)
            {
                case "jug":
                    return hasJug;
                case "speed":
                    return hasSpeedCola;
                case "double-tap":
                    return hasDoubleTap;
                case "quick-revive":
                    return hasQuickRevive;
                case "deadshot":
                    return hasDeadshot;
                default:
                    return false;
            }
        }
        public void losePerk(string perk)
        {
            switch(perk)
            {
                case "jug":
                    hasJug = false;
                    break;
                case "speed":
                    hasSpeedCola = false;
                    break;
                case "double-tap":
                    hasDoubleTap = false;
                    break;
                case "quick-revive":
                    hasQuickRevive = false;
                    break;
                case "deadshot":
                    hasDeadshot = false;
                    break;
            }
        }

        public void addPerk(string perk)
        {
            switch(perk)
            {
                case "jug":
                    hasJug = true;
                    break;
                case "speed":
                    hasSpeedCola = true;
                    break;
                case "double-tap":
                    hasDoubleTap = true;
                    break;
                case "quick-revive":
                    hasQuickRevive = true;
                    break;
                case "deadshot":
                    hasDeadshot = true;
                    break;
            }
        }

    }
}
