using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource BGM;
    public AudioSource SFX;

    public AudioClip ac_BGM;
    public AudioClip ac_Select;
    public AudioClip ac_Click;
    public AudioClip ac_Success;
    public AudioClip ac_GreatSuccess;
    public AudioClip ac_Fail;
    public AudioClip ac_BuySuccess;
    public AudioClip ac_BuyFail;
    public AudioClip ac_Shop;



    private static SoundManager sInstance;
    public static SoundManager Instance
    {
        get
        {
            if (sInstance == null)
            {                
                sInstance = new GameObject("GameManager").AddComponent<SoundManager>();
            }

            return sInstance;
        }
    }

    private void Awake()
    {
        sInstance = GetComponent<SoundManager>();
    }

    private void Start()
    {
        BGM.volume = 0.8f;
        SFX.volume = 0.8f;

        PlaySound("BGM");
    }

    public void PlaySound(string _sound)
    {
        switch(_sound)
        {
            case "BGM":
                BGM.clip = ac_BGM;
                BGM.Play();
                BGM.loop = true;
                break;
        }
    }


    public void PlaySFX(string _sfx)
    {
        switch(_sfx)
        {
            case "Select":
                SFX.PlayOneShot(ac_Select);
                break;
            case "Click":
                SFX.PlayOneShot(ac_Click);
                break;
            case "Success":
                SFX.PlayOneShot(ac_Success);
                break;
            case "GreatSuccess":
                SFX.PlayOneShot(ac_GreatSuccess);
                break;
            case "Fail":
                SFX.PlayOneShot(ac_Fail);
                break;
            case "BuySuccess":
                SFX.PlayOneShot(ac_BuySuccess);
                break;
            case "BuyFail":
                SFX.PlayOneShot(ac_BuyFail);
                break;
            case "Shop":
                SFX.PlayOneShot(ac_Shop);
                break;
        }
    }



}
