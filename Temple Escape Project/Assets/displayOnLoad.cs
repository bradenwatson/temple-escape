using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class displayOnLoad : MonoBehaviour
{
    //public int levelCleared = 0;
    //public AudioSource voiceOverLevel1;
    //public AudioSource voiceOverLevel2;
    //public AudioSource voiceOverLevel3;
    //private TMP_Text m_TextComponent;

    //[Header("Inputs")]
    //public Camera mainCamera;

    [Header("Level Text")]
    public GameObject LevelDisplay;
    //public GameObject LevelDisplayText;


    // Start is called before the first frame update
    void Start()
    {
        //m_TextComponent = LevelDisplayText.GetComponent<TMP_Text>();
        //levelCleared += 1;
        //voiceOverLevel1 = GetComponent<AudioSource>();
        AnnounceLevel();
    }

    //IEnumerator LevelTextDelay()
    //{
    //    yield return new WaitForSeconds(20f);
    //    Debug.Log("level text delay 20f");
    //}

    private void AnnounceLevel()
    {
        //m_TextComponent.text = "Level " + levelCleared; 
        LevelDisplay.SetActive(true);
        //voiceOverLevel1.Play();
        //LevelTextDelay();
        Invoke("AnnounceLevelRemove", 3);

    }

    private void AnnounceLevelRemove()
    {
        LevelDisplay.SetActive(false);
    }
}

