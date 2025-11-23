using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterCamera
{
    public Camera camera;
    public float sensitivity = 10;

    int standardFov;    //Kamera'nın olması gereken fov değeri
    bool isCameraFree = false;   //Özgür kamera, kamera bakışından etkilenmez
    bool headBob;       //Kafa sallanması ayarı
    [HideInInspector] public float cameraRotX = 0;   //Kameranın dikey olarak bakışı
    float remainingCameraShakeTime = 0; //Kamera'nın daha kaç saniye sallanacağı
    float cameraShakeMagnitude = 0; //Kameranın ne şiddetle sallanacağı

    //Kameranın çalışma sistemi
    public void CameraCycle(Transform transform)
    {
        if (isCameraFree)
        {
            //do nothing for now...
        }
        else
        {
            if(Time.timeScale > 0)
                CameraLook(transform);

            //Zoom
            if(Input.GetButton("Zoom") && camera.fieldOfView > standardFov / 2f)
            {
                camera.fieldOfView -= standardFov * Time.deltaTime * 2;
            }
            else if (!Input.GetButton("Zoom") && camera.fieldOfView < standardFov)
            {
                camera.fieldOfView += standardFov * Time.deltaTime * 2;
            }

            //Kamera sallantısı

            if (remainingCameraShakeTime > 0)
            {
                camera.transform.localEulerAngles = Vector3.forward * Mathf.Sin(remainingCameraShakeTime * Mathf.PI * cameraShakeMagnitude * 2) * cameraShakeMagnitude * remainingCameraShakeTime;
                remainingCameraShakeTime -= Time.deltaTime;
            }
            else
            {
                camera.transform.localEulerAngles = Vector3.zero;
            }
        }
    }

    //Kameranın özgürülüğünü değiştirme
    public void SetCameraFreedom(bool isFree)
    {
        isCameraFree = isFree;
    }

    //Oyun başı ayarları
    public void SetSettings()
    {
        standardFov = PlayerPrefs.GetInt("setting_fov");

        camera.fieldOfView = standardFov;
        sensitivity = PlayerPrefs.GetInt("setting_sensitivity");
        headBob = PlayerPrefs.GetInt("setting_headbob") == 1;
    }

    public void DoHeadBob(float headHeight)
    {
        if (headBob && !isCameraFree)
        {
            camera.transform.localPosition = new Vector3(0, Mathf.Abs(headHeight) > 0.02f ? headHeight: 0, 0);
        }
    }

    //Kamera ile etrafa bakma
    void CameraLook(Transform transform)
    {
        //Kamera bakışı
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        float sensitivityMultiplier = Input.GetButton("Zoom") ? 0.1f : 0.2f;
        cameraRotX -= mouseY * sensitivity * sensitivityMultiplier;
        cameraRotX = Mathf.Clamp(cameraRotX, -80, 80);
        camera.transform.parent.localEulerAngles = new Vector3(cameraRotX, 0, 0);       //Bug yapmaması için kamera değil cameraParent değişmeli

        transform.Rotate(transform.up * mouseX * sensitivity * sensitivityMultiplier);
    }

    //Kamerayı sallama
    public void ShakeCamera(float duration, float magnitude)
    {
        remainingCameraShakeTime = duration;
        cameraShakeMagnitude = magnitude / duration;
    }
}
