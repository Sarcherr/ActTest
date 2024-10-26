using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unit;
using System.Xml.Xsl;
using UnityEngine.SceneManagement;
using Boss;
using BossAct;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public Player player;
    [HideInInspector] public BossUpdate boss;

    [HideInInspector] public GameObject settings;

    [HideInInspector] public GameObject _HP;
    [HideInInspector] public GameObject _SP;
    [HideInInspector] public GameObject _Curer;
    [HideInInspector] public GameObject _BossHP;
    [HideInInspector] public Animator SP;
    [HideInInspector] public Image Curer;
    [HideInInspector] public Image HP;
    [HideInInspector] public Image BossHP;

    [Header("震动时间")] public float shakeForce = 10f;
    [Header("震动强度")] public float shakeTime = 0.2f;
    [HideInInspector] public bool hasShake = false;
    [HideInInspector] public bool settingOpen = false;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        boss = GameObject.Find("Boss").GetComponent<BossUpdate>();
        settings = transform.Find("SettingBackGround").gameObject;

        _HP = transform.Find("_HP").gameObject;
        _SP = transform.Find("_SP").gameObject;
        _Curer = transform.Find("_Curer").gameObject;
        _BossHP = transform.Find("_BossHP").gameObject;

        SP = _SP.transform.Find("SP").GetComponent<Animator>();
        Curer = _Curer.transform.Find("Curer").GetComponent<Image>();
        HP = _HP.transform.Find("HP").GetComponent<Image>();
        BossHP = _BossHP.transform.Find("BossHP").GetComponent<Image>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            settingOpen = !settingOpen;
            settings.SetActive(settingOpen);
            if(settingOpen)
            {
                Time.timeScale = 0;
            }
        }

        #region UI调整
        Curer.fillAmount = (float)player.currentCure / player.maxCure;
        HP.fillAmount = (float)player.currentHP / player.maxHP;
        BossHP.fillAmount = boss.preBhp / boss.BossHP;
        if (player.currentSP ==  player.maxSP)
        {
            SP.Play("SP_5");
        }
        else if(player.currentSP >= (int)(player.maxSP * 0.75f))
        {
            SP.Play("SP_4");
        }
        else if (player.currentSP >= (int)(player.maxSP * 0.5f))
        {
            SP.Play("SP_3");
        }
        else if (player.currentSP >= (int)(player.maxSP * 0.25f))
        {
            SP.Play("SP_2");
        }
        else
        {
            SP.Play("SP_1");
        }
        #endregion
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("CCZ_Scene_Begin");
    }

    public void ContinueGame()
    {
        settingOpen = false;
        settings.SetActive(settingOpen);
        Time.timeScale = 1f;
    }

    public void Shake(GameObject ui)
    {
        if(!hasShake)
        {
            StartCoroutine(MakeShake(ui));
            hasShake = true;
        }
    }

    public IEnumerator MakeShake(GameObject ui)
    {
        RectTransform ui_transform = ui.GetComponent<RectTransform>();
        Vector2 originPos = ui_transform.localPosition;
        float shakeForce = this.shakeForce;
        float timer = shakeTime;
        float _timer = 0.04f;
        while(timer > 0)
        {
            timer -= Time.deltaTime;
            _timer -= Time.deltaTime;

            ui_transform.localPosition = originPos + Random.insideUnitCircle * shakeForce;
            if (_timer < 0 )
            {
                ui_transform.localPosition = originPos;
                _timer = 0.04f;
            }
            
            yield return null;
        }

        ui_transform.localPosition = originPos;
        hasShake = false;
    }
}