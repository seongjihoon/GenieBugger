using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    static SoundManager instance = null;

    AudioSource BGMSource;
    AudioSource UIEffectSource;

    public List<AudioClip> m_BGMClips;
    public List<AudioClip> m_EffectClips;
    public List<AudioClip> m_GuestInSound;
    public List<AudioClip> m_SuccessSound;
    public static SoundManager getInstance()
    {
        if (instance == null)
            instance = new SoundManager();
        return instance;
    }

    private void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        if (instance == null)
            instance = this;

        BGMSource = GameObject.Find("BGM Source").GetComponent<AudioSource>();
        UIEffectSource = GameObject.Find("Effect Sound").GetComponent<AudioSource>();
        BGMSource.volume = 0.5f;

        DontDestroyOnLoad(this);

    }


    // Start is called before the first frame update
    void Start()
    {
        BGMSource.clip = m_BGMClips[0];
        BGMSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeBGM(int number)
    {
        try
        {
            if (number == 2)
                BGMSource.loop = false;
            else
                BGMSource.loop = true;
            BGMSource.clip = m_BGMClips[number];
            BGMSource.Play();
        }
        catch { }

    }

    public void PlaySound(int number)
    {
        //UIEffectSource.clip = m_EffectClips[number];
        //UIEffectSource.Play();
        try
        {
            if (number >= m_EffectClips.Count)
                Debug.Log("Out Of Range");
            else
            {
                UIEffectSource.PlayOneShot(m_EffectClips[number]);
            }
        }
        catch
        {
            Debug.Log("Have Not AudioSource");
        }
    }

    public void StopBGM()
    {
        try
        { 
            BGMSource.Stop();
        }
        catch
        { 
        }
        
        
    }

    public void guestSound(int num)
    {
        //if (num == 4)
        //    UIEffectSource.pitch = 2.0f;
        //else
        //    UIEffectSource.pitch = 1.0f;
        try
        {
            UIEffectSource.clip = m_GuestInSound[num - 1];
            UIEffectSource.Play();
        }
        catch
        {
            Debug.Log("Have Not AudioSource");
        }
    }

    public void SuccessSound(int num)
    {
        try
        {
            UIEffectSource.clip = m_SuccessSound[num];
            UIEffectSource.Play();
        }
        catch
        {
            Debug.Log("Have Not AudioSource");
        }

    }

}
