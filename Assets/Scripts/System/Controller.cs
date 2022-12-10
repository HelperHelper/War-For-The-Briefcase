using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Controller : MonoBehaviour
{
    public static Controller Instance { get; protected set; }

    public Camera MainCamera;
    //public Camera WeaponCamera;

    public Transform CameraPosition;
    //public Transform WeaponPosition;

    [Header("Control Settings")]
    public float MouseSensitivity = 100.0f;
    public float PlayerSpeed = 5.0f;
    public float RunningSpeed = 7.0f;
    public float JumpSpeed = 5.0f;

    // Cabecera para controlar el audio del jugador
    [Header("Audio")]
    //public RandomPlayer FootstepPlayer;
    //public AudioClip JumpingAudioCLip;
    //public AudioClip LandingAudioClip;

    float m_VerticalSpeed = 0.0f;
    bool m_IsPaused = false;
    int m_CurrentWeapon;

    float m_VerticalAngle, m_HorizontalAngle;
    public float Speed { get; private set; } = 0.0f;

    public bool LockControl { get; set; }
    public bool CanPause { get; set; } = true;

    public bool Grounded => m_Grounded;

    CharacterController m_CharacterController;

    bool m_Grounded;
    float m_GroundedTimer;
    float m_SpeedAtJump = 0.0f;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        m_IsPaused = false;
        m_Grounded = true;

        MainCamera.transform.SetParent(CameraPosition, false);
        MainCamera.transform.localPosition = Vector3.zero;
        MainCamera.transform.localRotation = Quaternion.identity;
        m_CharacterController = GetComponent<CharacterController>();

        m_VerticalAngle = 0.0f;
        m_HorizontalAngle = transform.localEulerAngles.y;
    }

    void Update()
    {

        bool wasGrounded = m_Grounded;
        bool loosedGrounding = false;

        //definimos nuestra propia conexión a tierra y no usamos la del controlador de personaje ya que el controlador de personaje puede parpadear
        //entre aterrizado/no aterrizado en pequeños pasos y similares. Así que hacemos que el controlador "no esté conectado a tierra" solo si
        //si el controlador de caracteres reporta no estar conectado a tierra por al menos .5 segundos;
        if (!m_CharacterController.isGrounded)
        {
            if (m_Grounded)
            {
                m_GroundedTimer += Time.deltaTime;
                if (m_GroundedTimer >= 0.5f)
                {
                    loosedGrounding = true;
                    m_Grounded = false;
                }
            }
        }
        else
        {
            m_GroundedTimer = 0.0f;
            m_Grounded = true;
        }

        Speed = 0;
        Vector3 move = Vector3.zero;
        if (!m_IsPaused && !LockControl)
        {
            // Jump (we do it first as 
            if (m_Grounded && Input.GetButtonDown("Jump"))
            {
                m_VerticalSpeed = JumpSpeed;
                m_Grounded = false;
                loosedGrounding = true;
               // FootstepPlayer.PlayClip(JumpingAudioCLip, 0.8f, 1.1f);
            }

            //  bool running = m_Weapons[m_CurrentWeapon].CurrentState == Weapon.WeaponState.Idle && Input.GetButton("Run");
            // float actualSpeed = running ? RunningSpeed : PlayerSpeed;
            bool running = Input.GetButton("Run");
            float actualSpeed = running ? RunningSpeed : PlayerSpeed;

            if (loosedGrounding)
            {
                m_SpeedAtJump = actualSpeed;
            }

            // Move around with WASD - Muévete con WASD
            move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            if (move.sqrMagnitude > 1.0f)
                move.Normalize();

            float usedSpeed = m_Grounded ? actualSpeed : m_SpeedAtJump;

            move = move * usedSpeed * Time.deltaTime;

            move = transform.TransformDirection(move);
            m_CharacterController.Move(move);

            // Turn player -Girar jugador
            float turnPlayer = Input.GetAxis("Mouse X") * MouseSensitivity;
            m_HorizontalAngle = m_HorizontalAngle + turnPlayer;

            if (m_HorizontalAngle > 360) m_HorizontalAngle -= 360.0f;
            if (m_HorizontalAngle < 0) m_HorizontalAngle += 360.0f;

            Vector3 currentAngles = transform.localEulerAngles;
            currentAngles.y = m_HorizontalAngle;
            transform.localEulerAngles = currentAngles;

            // Camera look up/down - mirar arriba y abajo con la cámara
            var turnCam = -Input.GetAxis("Mouse Y");
            turnCam = turnCam * MouseSensitivity;
            m_VerticalAngle = Mathf.Clamp(turnCam + m_VerticalAngle, -89.0f, 89.0f);
            currentAngles = CameraPosition.transform.localEulerAngles;
            currentAngles.x = m_VerticalAngle;
            CameraPosition.transform.localEulerAngles = currentAngles;

           // m_Weapons[m_CurrentWeapon].triggerDown = Input.GetMouseButton(0);

            Speed = move.magnitude / (PlayerSpeed * Time.deltaTime);

            //if (Input.GetButton("Reload"))
            //    m_Weapons[m_CurrentWeapon].Reload();

            //if (Input.GetAxis("Mouse ScrollWheel") < 0)
            //{
            //    ChangeWeapon(m_CurrentWeapon - 1);
            //}
            //else if (Input.GetAxis("Mouse ScrollWheel") > 0)
            //{
            //    ChangeWeapon(m_CurrentWeapon + 1);
            //}

            //Key input to change weapon

            //    for (int i = 0; i < 10; ++i)
            //    {
            //        if (Input.GetKeyDown(KeyCode.Alpha0 + i))
            //        {
            //            int num = 0;
            //            if (i == 0)
            //                num = 10;
            //            else
            //                num = i - 1;

            //            if (num < m_Weapons.Count)
            //            {
            //                ChangeWeapon(num);
            //            }
            //        }
            //    }
        }

        // Fall down / gravity - caida gravedad
        m_VerticalSpeed = m_VerticalSpeed - 10.0f * Time.deltaTime;
        if (m_VerticalSpeed < -10.0f)
            m_VerticalSpeed = -10.0f; // max fall speed
        var verticalMove = new Vector3(0, m_VerticalSpeed * Time.deltaTime, 0);
        var flag = m_CharacterController.Move(verticalMove);
        if ((flag & CollisionFlags.Below) != 0)
            m_VerticalSpeed = 0;

        if (!wasGrounded && m_Grounded)
        {
           // FootstepPlayer.PlayClip(LandingAudioClip, 0.8f, 1.1f);
        }
    }

    public void DisplayCursor(bool display)
    {
        m_IsPaused = display;
        Cursor.lockState = display ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = display;
    }

    //void PickupWeapon(Weapon prefab)
    //{
    //    //TODO : maybe find a better way than comparing name...
    //    if (m_Weapons.Exists(weapon => weapon.name == prefab.name))
    //    {//if we already have that weapon, grant a clip size of the ammo type instead
    //        ChangeAmmo(prefab.ammoType, prefab.clipSize);
    //    }
    //    else
    //    {
    //        var w = Instantiate(prefab, WeaponPosition, false);
    //        w.name = prefab.name;
    //        w.transform.localPosition = Vector3.zero;
    //        w.transform.localRotation = Quaternion.identity;
    //        w.gameObject.SetActive(false);

    //        w.PickedUp(this);

    //        m_Weapons.Add(w);
    //    }
    //}

    //void ChangeWeapon(int number)
    //{
    //    if (m_CurrentWeapon != -1)
    //    {
    //        m_Weapons[m_CurrentWeapon].PutAway();
    //        m_Weapons[m_CurrentWeapon].gameObject.SetActive(false);
    //    }

    //    m_CurrentWeapon = number;

    //    if (m_CurrentWeapon < 0)
    //        m_CurrentWeapon = m_Weapons.Count - 1;
    //    else if (m_CurrentWeapon >= m_Weapons.Count)
    //        m_CurrentWeapon = 0;

    //    m_Weapons[m_CurrentWeapon].gameObject.SetActive(true);
    //    m_Weapons[m_CurrentWeapon].Selected();
    //}

    //public int GetAmmo(int ammoType)
    //{
    //    int value = 0;
    //    m_AmmoInventory.TryGetValue(ammoType, out value);

    //    return value;
    //}

    //public void ChangeAmmo(int ammoType, int amount)
    //{
    //    if (!m_AmmoInventory.ContainsKey(ammoType))
    //        m_AmmoInventory[ammoType] = 0;

    //    var previous = m_AmmoInventory[ammoType];
    //    m_AmmoInventory[ammoType] = Mathf.Clamp(m_AmmoInventory[ammoType] + amount, 0, 999);

    //    if (m_Weapons[m_CurrentWeapon].ammoType == ammoType)
    //    {
    //        if (previous == 0 && amount > 0)
    //        {//we just grabbed ammo for a weapon that add non left, so it's disabled right now. Reselect it.
    //            m_Weapons[m_CurrentWeapon].Selected();
    //        }

    //        WeaponInfoUI.Instance.UpdateAmmoAmount(GetAmmo(ammoType));
    //    }
    //}

    //public void PlayFootstep()
    //{
    //    FootstepPlayer.PlayRandom();
    //}


}
