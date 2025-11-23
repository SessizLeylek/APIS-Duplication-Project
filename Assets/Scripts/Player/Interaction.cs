using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public LayerMask layerMask;
    public HoldingItem holdItem;
    public ShootingSystem shootSystem;
    [SerializeField] bool haveBatterySecondaryState = false;
    public bool havePistol = false;

    Transform camTransform;
    [HideInInspector] public Outline raycastedOutlineModel;  //Anahatı aktif edilmiş obje
    [HideInInspector] public int holdingItem = 1; //0 hiçbir şey, 1 pil, 2 silah
    [HideInInspector] public bool haveBattery = false;

    void Start()
    {
        shootSystem.particleCreator = FindObjectOfType<BurstParticleCreator>();
        camTransform = transform.GetChild(0);

        int lastSaveId = PlayerPrefs.GetInt("progress");
        if (haveBatterySecondaryState && (lastSaveId / 2 == UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex - 2 && lastSaveId % 2 == 1))
            haveBattery = true;

        if (haveBattery)
        {
            holdingItem = 1;
            holdItem.ChangeItem("battery", transform.position);
        }
        else if (havePistol)
        {
            holdingItem = 2;
            holdItem.ChangeItem("pistol", transform.position);
        }
    }

    void Update()
    {
        holdItem.ItemPositionUpdate();
        RaycastHit hit;
        bool raycastOutput = Physics.Raycast(camTransform.position, camTransform.forward, out hit, 2, layerMask);

        bool emptyOutlineModel = false;
        if (raycastOutput)
        {
            InteractableObject raycasted = hit.transform.GetComponent<InteractableObject>();
            if (!raycastedOutlineModel)
            {
                if (raycasted)
                {
                    bool enableOutline = false;
                    if (raycasted is Lever)
                        enableOutline = (raycasted as Lever).interactable;
                    else
                        enableOutline = true;
                    /*
                    if (raycasted.GetComponent<Lever>())
                        enableOutline = raycasted.GetComponent<Lever>().interactable;
                    else
                        enableOutline = true;*/

                    if (enableOutline)
                    {
                        raycastedOutlineModel = raycasted.outlineModel;
                        raycastedOutlineModel.enabled = true;
                        raycastedOutlineModel.OutlineWidth = 10;
                    }
                }
            }
            else if(raycasted)
            {
                if (raycastedOutlineModel != raycasted.outlineModel) emptyOutlineModel = true;
            }
            else
            {
                emptyOutlineModel = true;
            }
        }
        else if(raycastedOutlineModel)
        {
            emptyOutlineModel= true;
        }

        if (emptyOutlineModel && raycastedOutlineModel)
        {
            raycastedOutlineModel.enabled = false;
            raycastedOutlineModel = null;
        }

        //Tıklama
        if (Input.GetButtonDown("Interact") && Time.timeScale > 0)
        {
            //Nesnelerle etkileşim
            if(raycastOutput)
            {
                InteractableObject interactedWith = hit.transform.GetComponent<InteractableObject>();
                if (hit.transform.tag == "Interactable")
                {
                    interactedWith.OnInteract(this);

                    if (interactedWith.feedback)
                    {
                        SendMessage("InteractionMessage");
                    }
                    else {
                        shootSystem.Shoot(camTransform);
                        StartCoroutine(shootSystem.PistolRecoil());
                    }
                }
                else if (holdingItem == 2)
                {
                    shootSystem.Shoot(camTransform);
                    StartCoroutine(shootSystem.PistolRecoil());
                }
            }
            else
            {
                if(holdingItem == 2)
                {
                    shootSystem.Shoot(camTransform);
                    StartCoroutine(shootSystem.PistolRecoil());
                }
            }
        }

        //Envater SLotu değişme
        if (Input.GetButtonDown("ChangeItem"))
        {
            ChangeItem();
        }

        //Pistolun hareket ederken sağa sola savrulması
        shootSystem.PistolMovementUpdate(camTransform.forward);
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Vines")
        {
            shootSystem.SetChargeState(false);
        }
    }

    public void ChangeItem()
    {
        if (haveBattery && !(havePistol && holdingItem == 1))
        {
            holdingItem = 1;
            holdItem.ChangeItem("battery", transform.position);
        }
        else if (havePistol)
        {
            holdingItem = 2;
            holdItem.ChangeItem("pistol", transform.position);
        }
        else
        {
            holdingItem = 0;
            holdItem.ChangeItem("", Vector3.zero);
        }
    }

    public void GotPistol()
    {
        havePistol = true;
        holdingItem = 2;
        holdItem.ChangeItem("pistol", transform.position);
    }
}
