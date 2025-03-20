using UnityEngine;
using Unity.Cinemachine;

namespace Platformer397
{

    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {

        [Header("Player Movement")]
        [SerializeField] private InputReader input;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private Vector3 movement;

        public float moveSpeed = 200f;
        public float originalSpeed;
        [Header("Camera")]
        [SerializeField] private Transform mainCam;
        private float previousYRotation;
        private float rotationSpeed;
        [SerializeField] private float rotationAmount = 100f;
        [SerializeField] private CinemachinePanTilt panTilt;
        private float panAxisValue;
        private float camSpeed;

        [Header("Misc")]
        public string currentRoom = "Room1";

        [SerializeField] private int money = 500;

        public Weapon hoveredWeapon = null;
        public AmmoBox hoveredAmmoBox = null;

        [Header("Player Health")]
        public float playerHealth = 100f;
        public int playerMaxHealth = 100;
        [SerializeField] private float lastTimeHit = 0f;
        [SerializeField] private float timeToHeal = 3f;
        private float regenTimer = 0f;
        private float regenDuration = 3f; //time it takes to regen to full hp

        [Header("Perks: ")]
        public bool hasJug = false;
        public bool hasSpeedCola = false;
        public bool hasDoubleTap = false;
        public bool hasQuickRevive = false;
        public bool hasDeadshot = false;
        public jug jugger = null;
        public doubleTap dbTap = null;
        public speed speedCola = null;
        public quickRevive quick = null;
        public deadshot dShot = null;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;
            mainCam = Camera.main.transform;
            originalSpeed = moveSpeed;
        }
        void Start()
        {
            input.EnablePlayerActions();
            previousYRotation = panTilt.PanAxis.Value;
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
            // HUDManager.Instance.HealthShower(playerHealth, playerMaxHealth);
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
            if(panTilt != null) {panAxisValue = panTilt.PanAxis.Value;}
            camSpeed = CalculateCameraPanSpeed(panAxisValue);
            UpdateRotation(camSpeed);
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
                rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
            }
        }

        private void UpdateRotation(float speed)
        {   
            // transform.Rotate(0f, 500f * Time.deltaTime, 0f);
            transform.Rotate(0f,speed,0f);
        }

        private float CalculateCameraPanSpeed(float panValue)
        {
            //get the rotation of the camera
            float currentYRotation = panValue;
            float rotationDelta = Mathf.DeltaAngle(previousYRotation, currentYRotation);
            previousYRotation = currentYRotation; //update the previous rotation so that is accurate next time function is called
            return rotationDelta;
        }

        private void HandleMovement(Vector3 adjustedMovement)
        {
            Vector3 velocity = rb.position + adjustedMovement.normalized * moveSpeed / 1000;
            rb.MovePosition(velocity);
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

                // Debug.Log("Game Over");

                //should add all the perks to game manager so we can reference the game manager instance rather than having multiple game objects on the things that need them
                if(checkPerk("quick-revive")){quick.downed();}
                else{playerHealth = 0;} //reset health for now so we can test things but later we need to change scenes or have something happen to end game
                loseAllPerks();
            }
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
                    jugger.resetHealth();
                    break;
                case "speed":
                    hasSpeedCola = false;
                    speedCola.resetEverything();
                    break;
                case "double-tap":
                    hasDoubleTap = false;
                    dbTap.resetEverything();
                    break;
                case "quick-revive":
                    hasQuickRevive = false;
                    break;
                case "deadshot":
                    hasDeadshot = false;
                    dShot.resetEverything();
                    break;
            }
        }

        public void loseAllPerks()
        {
            if(hasJug){losePerk("jug");}
            if(hasSpeedCola){losePerk("speed");}
            if(hasDoubleTap){losePerk("double-tap");}
            if(hasQuickRevive){losePerk("quick-revive");}
            if(hasDeadshot){losePerk("deadshot");}
        }

        public void addPerk(string perk)
        {
            switch(perk)
            {
                case "jug":
                    hasJug = true;
                    jugger = InteractionManager.Instance.jugger;
                    break;
                case "speed":
                    hasSpeedCola = true;
                    speedCola = InteractionManager.Instance.speedCola;
                    break;
                case "double-tap":
                    hasDoubleTap = true;
                    dbTap = InteractionManager.Instance.dbTap;
                    break;
                case "quick-revive":
                    hasQuickRevive = true;
                    quick = InteractionManager.Instance.quick;
                    break;
                case "deadshot":
                    hasDeadshot = true;
                    dShot = InteractionManager.Instance.dShot;
                    break;
            }
        }

    }
}
