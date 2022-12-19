using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;


[System.Serializable]
public class AmmoInventoryEntry
{
    [AmmoType]
    public int ammoType;
    public int amount = 0;
}


public class Controller : MonoBehaviour
{
    public static Controller Instance { get; protected set; }

    public Camera MainCamera;
    public Camera WeaponCamera;

    public Transform CameraPosition;
    public Transform WeaponPosition;

    public Weapon[] startingWeapons;

    //this is only use at start, allow to grant ammo in the inspector. ammoInventory is used during gameplay
    //esto solo se usa al inicio, permite otorgar munición en el inspector. ammoInventory se usa durante el juego
    public AmmoInventoryEntry[] startingAmmo;

    [Header("Control Settings")]
    public float MouseSensitivity = 100.0f;
    public float PlayerSpeed = 5.0f;
    public float RunningSpeed = 7.0f;
    public float JumpSpeed = 5.0f;

    // Cabecera para controlar el audio del jugador
    [Header("Audio")]
    public RandomPlayer FootstepPlayer;
    //public AudioClip JumpingAudioCLip;
    //public AudioClip LandingAudioClip;

    float verticalSpeed = 0.0f;
    bool isPaused = false;
    int currentWeapon;

    float verticalAngle, horizontalAngle;
    public float Speed { get; private set; } = 0.0f;

    public bool LockControl { get; set; }
    public bool CanPause { get; set; } = true;

    public bool briefcase { get; set; } = false;

    public bool Grounded => grounded;

    CharacterController characterController;

    bool grounded;
    float groundedTimer;
    float speedAtJump = 0.0f;


    List<Weapon> weapons = new List<Weapon>();
    Dictionary<int, int> ammoInventory = new Dictionary<int, int>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        isPaused = false;
        grounded = true;

        MainCamera.transform.SetParent(CameraPosition, false);
        MainCamera.transform.localPosition = Vector3.zero;
        MainCamera.transform.localRotation = Quaternion.identity;
        characterController = GetComponent<CharacterController>();

        for (int i = 0; i < startingWeapons.Length; ++i)
        {
            PickupWeapon(startingWeapons[i]);
        }

        for (int i = 0; i < startingAmmo.Length; ++i)
        {
            ChangeAmmo(startingAmmo[i].ammoType, startingAmmo[i].amount);
        }

        currentWeapon = -1;
        ChangeWeapon(0);

        for (int i = 0; i < startingAmmo.Length; ++i)
        {
            ammoInventory[startingAmmo[i].ammoType] = startingAmmo[i].amount;
        }

        verticalAngle = 0.0f;
        horizontalAngle = transform.localEulerAngles.y;
    }

    void Update()
    {

        bool wasGrounded = grounded;
        bool loosedGrounding = false;

        //definimos nuestra propia conexión a tierra y no usamos la del controlador de personaje ya que el controlador de personaje puede parpadear
        //entre aterrizado/no aterrizado en pequeños pasos y similares. Así que hacemos que el controlador "no esté conectado a tierra" solo si
        //si el controlador del jugador reporta no estar conectado a tierra por al menos .5 segundos;
        if (!characterController.isGrounded)
        {
            if (grounded)
            {
                groundedTimer += Time.deltaTime;
                if (groundedTimer >= 0.5f)
                {
                    loosedGrounding = true;
                    grounded = false;
                }
            }
        }
        else
        {
            groundedTimer = 0.0f;
            grounded = true;
        }

        Speed = 0;
        Vector3 move = Vector3.zero;
        if (!isPaused && !LockControl)
        {
            // Jump (si el jugador está en el suelo puede saltar 
            if (grounded && Input.GetButtonDown("Jump"))
            {
                verticalSpeed = JumpSpeed;
                grounded = false;
                loosedGrounding = true;
               // FootstepPlayer.PlayClip(JumpingAudioCLip, 0.8f, 1.1f);
            }

            // bool prunning = weapons[currentWeapon].CurrentState == Weapon.WeaponState.Idle && Input.GetButton("Run");

            // Debug.Log("Que hace esto:" + prunning);

            bool running = weapons[currentWeapon].CurrentState == Weapon.WeaponState.Idle && Input.GetButton("Run");
            float actualSpeed = running ? RunningSpeed : PlayerSpeed;
            //bool running = Input.GetButton("Run");
            //float actualSpeed = running ? RunningSpeed : PlayerSpeed;

            if (loosedGrounding)
            {
                speedAtJump = actualSpeed;
            }

            // Move around with WASD - Muévete con WASD
            move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            if (move.sqrMagnitude > 1.0f)
                move.Normalize();

            float usedSpeed = grounded ? actualSpeed : speedAtJump;

            move = move * usedSpeed * Time.deltaTime;

            move = transform.TransformDirection(move);
            characterController.Move(move);

            // Turn player -Girar jugador
            float turnPlayer = Input.GetAxis("Mouse X") * MouseSensitivity;
            horizontalAngle = horizontalAngle + turnPlayer;

            if (horizontalAngle > 360) horizontalAngle -= 360.0f;
            if (horizontalAngle < 0) horizontalAngle += 360.0f;

            Vector3 currentAngles = transform.localEulerAngles;
            currentAngles.y = horizontalAngle;
            transform.localEulerAngles = currentAngles;

            // Camera look up/down - mirar arriba y abajo con la cámara
            var turnCam = -Input.GetAxis("Mouse Y");
            turnCam = turnCam * MouseSensitivity;
            verticalAngle = Mathf.Clamp(turnCam + verticalAngle, -89.0f, 89.0f);
            currentAngles = CameraPosition.transform.localEulerAngles;
            currentAngles.x = verticalAngle;
            CameraPosition.transform.localEulerAngles = currentAngles;

           weapons[currentWeapon].triggerDown = Input.GetMouseButton(0);

            Speed = move.magnitude / (PlayerSpeed * Time.deltaTime);

            if (Input.GetButton("Reload"))
                weapons[currentWeapon].Reload();

            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                ChangeWeapon(currentWeapon - 1);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                ChangeWeapon(currentWeapon + 1);
            }

            //Key input to change weapon

                for (int i = 0; i < 10; ++i)
            {
                if (Input.GetKeyDown(KeyCode.Alpha0 + i))
                {
                    int num = 0;
                    if (i == 0)
                        num = 10;
                    else
                        num = i - 1;

                    if (num < weapons.Count)
                    {
                        ChangeWeapon(num);
                    }
                }
            }
        }

        // Fall down / gravity - caida gravedad
        verticalSpeed = verticalSpeed - 10.0f * Time.deltaTime;
        if (verticalSpeed < -10.0f)
            verticalSpeed = -10.0f; // max fall speed - velocidad maxima de caida
        var verticalMove = new Vector3(0, verticalSpeed * Time.deltaTime, 0);
        var flag = characterController.Move(verticalMove);
        if ((flag & CollisionFlags.Below) != 0)
            verticalSpeed = 0;

        if (!wasGrounded && grounded)
        {
           // FootstepPlayer.PlayClip(LandingAudioClip, 0.8f, 1.1f);
        }
    }

    public void DisplayCursor(bool display)
    {
        isPaused = display;
        Cursor.lockState = display ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = display;
    }

    void PickupWeapon(Weapon prefab)
    {
        //TODO : maybe find a better way than comparing name...
        if (weapons.Exists(weapon => weapon.name == prefab.name))
        {//if we already have that weapon, grant a clip size of the ammo type instead
            ChangeAmmo(prefab.ammoType, prefab.clipSize);
        }
        else
        {
            var w = Instantiate(prefab, WeaponPosition, false);
            w.name = prefab.name;
            w.transform.localPosition = Vector3.zero;
            w.transform.localRotation = Quaternion.identity;
            w.gameObject.SetActive(false);

            w.PickedUp(this);

            weapons.Add(w);
        }
    }

    void ChangeWeapon(int number)
    {
        if (currentWeapon != -1)
        {
            weapons[currentWeapon].PutAway();
            weapons[currentWeapon].gameObject.SetActive(false);
        }

        currentWeapon = number;

        if (currentWeapon < 0)
            currentWeapon = weapons.Count - 1;
        else if (currentWeapon >= weapons.Count)
            currentWeapon = 0;

        weapons[currentWeapon].gameObject.SetActive(true);
        weapons[currentWeapon].Selected();
    }

    public int GetAmmo(int ammoType)
    {
        int value = 0;
        ammoInventory.TryGetValue(ammoType, out value);

        return value;
    }

    public void ChangeAmmo(int ammoType, int amount)
    {
        if (!ammoInventory.ContainsKey(ammoType))
            ammoInventory[ammoType] = 0;

        var previous = ammoInventory[ammoType];
        ammoInventory[ammoType] = Mathf.Clamp(ammoInventory[ammoType] + amount, 0, 999);

        if (weapons[currentWeapon].ammoType == ammoType)
        {
            if (previous == 0 && amount > 0)
            {//we just grabbed ammo for a weapon that add non left, so it's disabled right now. Reselect it.
                weapons[currentWeapon].Selected();
            }

            WeaponInfoUI.Instance.UpdateAmmoAmount(GetAmmo(ammoType));
        }
    }

    public void PlayFootstep()
    {
        //FootstepPlayer.PlayRandom();
    }


}
