using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Duplication : MonoBehaviour
{
    //Bu kod, ana karakterin kopyalama kapısından geçmesiyle kopyalanıp silinmesini sağlar

    public RenderTexture duplicatedViewTexture;
    public RenderTexture duplicatedForegroundTexture;
    public RenderTexture mainForegroundTexture;
    public Transform fpsController;

    [HideInInspector] public Camera mainCamera;
    [HideInInspector] public Camera duplicatedCamera;
    [HideInInspector] public Camera mainCameraForeground;
    [HideInInspector] public Camera duplicatedCameraForeground;
    AudioListener listenerMain;
    AudioListener listenerDuplicated;
    Image flashEffectImage; //kamera parlaması objesi
    RawImage duplicatedView;    //kopyalanmış karakterin görüntüsü
    RawImage duplicatedForeground;    //kopyalanmış karakterin önplan görüntüsü
    [HideInInspector] public bool isDuplicated = false;
    [HideInInspector] public int cameraMode = 0; //Kameraların ekranda nasıl gözükeceği
    bool twinCamera;

    void Start()
    {
        //RenderTexture boyutu ayarlama
        int _h = Screen.height;
        int _w = Screen.width;
        if (mainForegroundTexture.height != _h || mainForegroundTexture.width != _w)
        {
            mainForegroundTexture.height = _h;
            mainForegroundTexture.width = _w;
            duplicatedViewTexture.height = Mathf.RoundToInt(_h * 0.36f);
            duplicatedForegroundTexture.height = Mathf.RoundToInt(_h * 0.36f);
            duplicatedViewTexture.width = Mathf.RoundToInt(_w * 0.36f);
            duplicatedForegroundTexture.width = Mathf.RoundToInt(_w * 0.36f);
        }

        //Bileşen atama
        duplicatedCamera = fpsController.GetComponentInChildren<Camera>();
        duplicatedCameraForeground = duplicatedCamera.transform.GetChild(0).GetComponent<Camera>();
        mainCamera = GetComponentInChildren<Camera>();
        mainCameraForeground = mainCamera.transform.GetChild(0).GetComponent<Camera>();
        listenerMain = mainCamera.GetComponent<AudioListener>();
        listenerDuplicated = duplicatedCamera.GetComponent<AudioListener>();
        flashEffectImage = GameObject.Find("Canvas/FlashEffect").GetComponent<Image>();
        duplicatedView = GameObject.Find("Canvas/DuplicatedView").GetComponent<RawImage>();
        duplicatedForeground = GameObject.Find("Canvas/DuplicatedViewForeground").GetComponent<RawImage>();

        SetCameras(2);

        SetViewRanges(1);

        twinCamera = PlayerPrefs.GetInt("setting_twincam") == 1;
    }

    void Update()
    {
        //Kamera modunu değiştir
        if (Input.GetButtonDown("SwapPlayer") && isDuplicated)
        {
            cameraMode = 1 - cameraMode;
            SetCameras(cameraMode);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        //Kopyalan
        if(col.tag == "DuplicationGate")
        {
            isDuplicated = !isDuplicated;

            Vector3 gateDirection;
            #region Kopyalayıcı yönü hesaplama
            if (Mathf.Round(col.transform.forward.x) == 0)
            {
                //Kapı -/+Z doğrultusunda
                if (col.transform.position.z > transform.position.z)
                    gateDirection = new Vector3(0, 0, 1);
                else
                    gateDirection = new Vector3(0, 0, -1);

            }
            else
            {
                //Kapı -/+X doğrultusunda
                if (col.transform.position.x > transform.position.x)
                    gateDirection = new Vector3(1, 0, 0);
                else
                    gateDirection = new Vector3(-1, 0, 0);
            }
            #endregion

            StartCoroutine(PassThroughGate(gateDirection, col.transform.GetComponent<DuplicationGate>().duplicationOffset));

            if (isDuplicated)
            {
                StartCoroutine(FlashEffect());
                GameObject.FindObjectOfType<SfxPlayer>().PlaySFX(0, transform.position);
                GameObject.FindObjectOfType<BurstParticleCreator>().CreateEffect(0, transform.position, Vector3.zero);

                fpsController.GetComponent<Interaction>().haveBattery = col.transform.GetComponent<DuplicationGate>().duplicatedHaveBattery;
                fpsController.GetComponent<Interaction>().shootSystem.isCharged = col.transform.GetComponent<DuplicationGate>().duplicatedChargedPistol;
            }
            else
            {
                StartCoroutine(FlashEffect());
                GameObject.FindObjectOfType<SfxPlayer>().PlaySFX(1, transform.position);
                GameObject.FindObjectOfType<BurstParticleCreator>().CreateEffect(0, transform.position, Vector3.zero);

                col.transform.GetComponent<DuplicationGate>().duplicatedHaveBattery = fpsController.GetComponent<Interaction>().haveBattery;
                col.transform.GetComponent<DuplicationGate>().duplicatedChargedPistol = fpsController.GetComponent<Interaction>().shootSystem.isCharged;
                if (fpsController.GetComponent<Interaction>().raycastedOutlineModel)
                {
                    fpsController.GetComponent<Interaction>().raycastedOutlineModel.enabled = false;
                    fpsController.GetComponent<Interaction>().raycastedOutlineModel = null;
                }
            }
        }
    }

    void SetViewRanges(float rangeMultiplier)
    {
        //Görüş mesafesi ayarlama
        float viewrange = PlayerPrefs.GetInt("setting_viewrange");
        mainCamera.farClipPlane = viewrange * rangeMultiplier;
        duplicatedCamera.farClipPlane = viewrange;
        RenderSettings.fogStartDistance = (viewrange - 40) * rangeMultiplier;
        RenderSettings.fogEndDistance = (viewrange - 10) * rangeMultiplier;
    }

    void SetCameras(int mode)
    {
        //kameraları ayarlama
        switch (mode)
        {
            case 0: //ana kamera büyük, ikiz küçük
                if (twinCamera)
                {
                    mainCamera.enabled = true;
                    duplicatedCamera.enabled = true;

                    duplicatedView.enabled = true;
                    duplicatedForeground.enabled = true;
                }
                else
                {
                    mainCamera.enabled = true;
                    duplicatedCamera.enabled = false;
                }
                mainCamera.targetTexture = null;
                duplicatedCamera.targetTexture = duplicatedViewTexture;
                mainCameraForeground.targetTexture = mainForegroundTexture;
                duplicatedCameraForeground.targetTexture = duplicatedForegroundTexture;

                //AudioListener
                listenerMain.enabled = true;
                listenerDuplicated.enabled = false;
                break;
            case 1: //ana kamera küçük, ikiz büyük
                if (twinCamera)
                {
                    duplicatedCamera.enabled = true;
                    mainCamera.enabled = true;

                    duplicatedView.enabled = true;
                    duplicatedForeground.enabled = true;
                }
                else
                {
                    duplicatedCamera.enabled = true;
                    mainCamera.enabled = false;
                }
                mainCamera.targetTexture = duplicatedViewTexture;
                duplicatedCamera.targetTexture = null;
                mainCameraForeground.targetTexture = duplicatedForegroundTexture;
                duplicatedCameraForeground.targetTexture = mainForegroundTexture;

                //AudioListener
                listenerMain.enabled = false;
                listenerDuplicated.enabled = true;
                break;
            case 2: //sadece ana kamera
                mainCamera.enabled = true;
                duplicatedCamera.enabled = true;

                duplicatedView.enabled = false;
                duplicatedForeground.enabled = false;
                mainCamera.targetTexture = null;
                duplicatedCamera.targetTexture = null;
                mainCameraForeground.targetTexture = mainForegroundTexture;
                duplicatedCameraForeground.targetTexture = duplicatedForegroundTexture;

                //AudioListener
                listenerMain.enabled = true;
                listenerDuplicated.enabled = false;
                break;
        }
    }

    IEnumerator PassThroughGate(Vector3 direction, Vector3 duplicatedOffset)
    {
        //Kopyalan / kopyayı sil
        SetIsDuplicated(duplicatedOffset);

        CharacterControlling mainController = GetComponent<CharacterControlling>();
        CharacterControlling duplController = fpsController.GetComponent<CharacterControlling>();

        mainController.movement.movementVector = Vector3.zero;
        duplController.movement.movementVector = Vector3.zero;

        //Geçitten geç
        float slideStart = Time.time;
        mainController.movement.canMove = false;
        duplController.movement.canMove = false;
        while(slideStart + 0.5f > Time.time)
        {
            if(Time.time - slideStart < 0.25f) mainController.movement.AdditionalForce(direction * Time.deltaTime * 6);

            //İkizi eşitle
            if (isDuplicated)
            {
                duplController.movement.Blink(transform.position + duplicatedOffset);

                fpsController.eulerAngles = transform.eulerAngles;
                duplController.camera.cameraRotX = mainController.camera.cameraRotX;
            }

            yield return null;
        }
        mainController.movement.canMove = true;
        duplController.movement.canMove = true;
    }

    public void SetIsDuplicated(Vector3 duplicatedOffset)
    {
        if (isDuplicated)
        {
            //Değerleri Eşitleme
            fpsController.gameObject.SetActive(true);
            fpsController.GetComponent<CharacterControlling>().movement.Blink(transform.position + duplicatedOffset);

            fpsController.eulerAngles = transform.eulerAngles;
            fpsController.GetComponent<CharacterControlling>().camera.cameraRotX = GetComponent<CharacterControlling>().camera.cameraRotX;

            //Kamera ayarlama
            cameraMode = 0;
            SetCameras(0);
            SetViewRanges(0.5f);

            //Envanter değişimi
            var interaction = GetComponent<Interaction>();
            interaction.ChangeItem();
            interaction.ChangeItem();
        }
        else
        {
            SetCameras(2);
            SetViewRanges(1);

            fpsController.gameObject.SetActive(false);
        }
    }

    IEnumerator FlashEffect()
    {
        //Geçitlerden geçmek, ekranın parlamasına neden olur
        flashEffectImage.color = Color.white;
        flashEffectImage.color = new Color(1, 1, 1, 1);

        for (int i = 100; i > 0; i--)
        {
            flashEffectImage.color = new Color(1, 1, 1, i / 100f);
            yield return null;
        }
        flashEffectImage.color = new Color(1, 1, 1, 0);
    }
    
}
