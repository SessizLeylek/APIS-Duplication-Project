using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterControlling : MonoBehaviour
{
    public CharacterMovement movement;
    public CharacterCamera camera;
    [SerializeField] bool isDuplicatedCharacter;
    [SerializeField] Vector3 secondarySpawnPosition;
    [SerializeField] float secondaryLookAngle;
    Animator animator;
    [HideInInspector] public Interaction interaction;

    bool isAlive = true;

    void Start()
    {
        int lastSaveId = PlayerPrefs.GetInt("progress");
        if (lastSaveId / 2 == UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex - 2 && lastSaveId % 2 == 1)
        {
            movement.Blink(secondarySpawnPosition);
            transform.eulerAngles = Vector3.up * secondaryLookAngle;
        }

        camera.SetCameraFreedom(false);
        camera.SetSettings();

        animator = GetComponentInChildren<Animator>();
        interaction = GetComponent<Interaction>();
    }

    void Update()
    {
        //Sistemleri çalıştırma
        movement.MovementCycle(transform);
        camera.CameraCycle(transform);

        //Kafa sallanması
        camera.DoHeadBob((0.5f - Mathf.Abs(0.5f - movement.stepCycleDelta)) / 3f);

        //Animasyonlar
        AnimationCycle();
    }

    void AnimationCycle()
    {
        animator.SetFloat("speed", movement.movementVector.magnitude);
        animator.SetBool("jumping", movement.isJumping);
        animator.SetBool("haveItem", interaction.havePistol || interaction.haveBattery);
    }

    public void InteractionMessage()
    {
        animator.Play("Pick");
    }

    #region ÖLÜM
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Death")
            KillPlayer();
    }

    public void KillPlayer()
    {
        if (isAlive)
        {
            isAlive = false;

            //Varsa ikizi yok et
            Duplication dupl = FindObjectOfType<Duplication>();
            if (dupl.isDuplicated)
            {
                dupl.isDuplicated = false;
                dupl.SetIsDuplicated(Vector3.zero);
            }

            if (isDuplicatedCharacter)
            {
                //Eğer ikiz öldüyse ana karakteri öldürme
                CharacterControlling mainPlayr = GameObject.Find("MainPlayer").GetComponent<CharacterControlling>();
                mainPlayr.movement.Blink(transform.position);
                mainPlayr.KillPlayer();
            }
            else
            {
                //Kendini öldürme
                movement.gravity = false;
                movement.canMove = false;
                camera.SetCameraFreedom(true);
                camera.camera.cullingMask = 2047;
                camera.camera.transform.Translate(0, 0, -2);
                camera.camera.transform.GetChild(0).Translate(0, 0, 10);
                FindObjectOfType<SfxPlayer>().PlaySFX(11, transform.position);

                animator.Play("Die");

                StartCoroutine(DieWithFlash());
            }
        }
    }

    IEnumerator DieWithFlash()
    {
        //Beyaz Işık
        Image flashEffectImage = GameObject.Find("Canvas/FlashEffect").GetComponent<Image>();
        flashEffectImage.color = new Color(1, 1, 1, 0);

        float deathStartTime = Time.time;
        while (Time.time - deathStartTime < 1.5f)
        {
            flashEffectImage.color = new Color(1, 1, 1, (Time.time - deathStartTime) / 1.5f);
            yield return null;
        }
        flashEffectImage.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.5f);

        //Öl
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    #endregion
}
