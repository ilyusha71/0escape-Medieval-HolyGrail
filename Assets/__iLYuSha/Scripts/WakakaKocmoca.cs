/**************************************************************************************** 
 * Wakaka Studio 2017
 * Author: iLYuSha Dawa-mumu Wakaka Kocmocovich Kocmocki KocmocA
 * Project:: 0escape Medieval - Holy Grail
 * Tools: Unity 5/C# + Arduino/C++
 * Last Updated: 2017/07/02
 ****************************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using DG.Tweening;
using System.IO;

public class WakakaKocmoca : MonoBehaviour
{
    public void Quit()
    {
        Application.Quit();
    }

    /* Arduino Debug*/
    public GameObject Observatory;
    public GameObject RFID;
    public GameObject Bridge;
    public GameObject Release;
    public GameObject Refuse;
    public GameObject Lock;

    public GameObject badge;
    public GameObject sword;


    /* Story */
    private enum Action
    {
        Opening,
        Start,
        Story,
        End,
    }
    private Action action = Action.Opening;
    public GameObject[] who;
    public Text textStory;
    private CanvasGroup textFade;
    public Image openingBG;
    public GameObject openingTips;
    private float durationOpening = 9.37f;
    private string story;
    private string[] clipStory;
    private string nameKnight;
    private int index;
    private float timeNext;
    private float durationClip = 5.37f;
    /* Holy Grail */
    public enum HolyState
    {
        Initial,
        Recharge,
        HolyGrail,
        Oracle,
    }
    public HolyState holyState = HolyState.Initial;
    public Image holyGrail;
    private bool locked = true;
    private int countRefuse = 1;
    private float timeHoly;
    private float durationHoly = 7.0f;
    private float durationRelease = 10.0f;
    /* Oracle */
    public AudioClip[] sfxOracle;
    public Text textOracle;
    private AudioSource sfxAudio;
    private Sequence sequence;
    private string[] listOracle = new string[3];
    private string msgOracle;
    /* BG */
    public Text track;

    void Awake()
    {
        if (ArduinoController.instance == null)
        {
            PlayerPrefs.SetInt("lastScene", SceneManager.GetActiveScene().buildIndex + 100);
            SceneManager.LoadScene("Arduino Controller");
        }

        Observatory.SetActive(false);
        openingBG.gameObject.SetActive(true);
        story =
           "莫德雷德:我告知了亞瑟關於前方探察的情報，\n他推斷聖杯可能就在<color=#191970>康沃爾</color>地區。\n　　　　　　　　　- 莫德雷德 -@" +
           "莫德雷德:本來想找<color=#191970>貝德維爾</color>一同尋找聖杯，\n但被他拒絕了。\n　　　　　　　　　- 莫德雷德 -@" +
           "蘭斯洛特:亞瑟王開始召集願意幫他尋找聖杯的騎士，\n我與<color=#191970>蘭馬洛克</color>和<color=#191970>凱</color>決定前往康沃爾。\n　　　　　　　　　- 蘭斯洛特 -@" +
           "蘭斯洛特:路途中有一位老者，\n說只要給他五百枚金幣，\n他就告訴我們<color=#191970>聖杯的下落</color>。\n　　　　　　　　　- 蘭斯洛特 -@" +
           "高文:<color=#191970>盧坎</color>與我來到康沃爾的郊外，\n並在附近教堂尋找聖杯的線索。\n　　　　　　　　　　　- 高文 -@" +
           "教士:<color=#191970>聖杯一直被存放在康沃爾東郊的教堂內!</color>@" +
           "鮑斯:在康沃爾東南方的一座城市，\n我遇到了一個<color=#191970>自稱見過聖杯的老者</color>。\n　　　　　　　　　　　- 鮑斯 -@" +
           "鮑斯:我跟著加拉哈德與珀西瓦爾進入城內的教堂，\n見到了一名<color=#191970>教士</color>。\n　　　　　　　　　　　- 鮑斯 -@" +
           "高文:沒想到才剛踏進教堂，\n敵軍就包圍了教堂，\n也許我們<color=#191970>被假情報給騙了</color>。\n　　　　　　　　　　　- 高文 -@" +
           "教士:<color=#191970>只有被命運揀選的人才有機會見著聖杯!!</color>@" +
           "蘭斯洛特:老者帶我進入教堂的密道，走了很久，\n盡頭處有個<color=#191970>石棺</color>，老者說聖杯就在其中。\n　　　　　　　　　- 蘭斯洛特 -@" +
           "蘭斯洛特:我打開石棺發現有一具<color=#191970>骸骨</color>!抱著金色聖杯，\n我正要伸手去觸摸時，它卻將我的手彈開。\n　　　　　　　　　- 蘭斯洛特 -@" +
           "蘭斯洛特:回過神時<color=#191970>聖杯已失去光澤</color>，變成生鏽的金屬杯。\n　　　　　　　　　- 蘭斯洛特 -@" +
           "鮑斯:教士一見到加拉哈德，\n就請他進入了一個<color=#191970>密室</color>，\n並告知聖杯就在那兒了。\n　　　　　　　　　　　- 鮑斯 -@" +
           "鮑斯:教士還沒來得及阻止，<color=#191970>珀西瓦爾</color>竟也追了進去，\n我也立刻跟了上去。\n　　　　　　　　　　　- 鮑斯 -@" +
           "鮑斯:我才剛到門口，\n一陣強烈的光芒閃過，密室內已<color=#191970>空無一人</color>。\n　　　　　　　　　　　- 鮑斯 -@" +
           "教士:<color=#191970>只有內心純潔的人才能真正觸摸到聖杯!!!</color>";
        clipStory = story.Split('@');
        textStory.text = "聖杯會揀選它的持有者~";
        textFade = textStory.GetComponent<CanvasGroup>();
        timeNext = Time.time + 1.37f;

        Color c = holyGrail.color;
        c.a = 0;
        holyGrail.color = c;

        sfxAudio = GetComponent<AudioSource>();
        listOracle[0] = "";
        listOracle[1] = "你正是聖杯的揀選者"; // Bingo!
        listOracle[2] = "你的命運不在此處\n它已拒絕了你"; // Punishment
        msgOracle = listOracle[0];
        textOracle.text = msgOracle;        
    }

    void Update()
    {
        /* Arduino Debug */
        #region Arduino Debug
        if (Input.GetKeyDown(KeyCode.F10))
        {
            Observatory.SetActive(ArduinoController.instance.panelSetting.activeSelf);
        }
        badge.SetActive(false);
        sword.SetActive(false);
        RFID.SetActive(false);
        Bridge.SetActive(false);
        Release.SetActive(false);
        Refuse.SetActive(false);
        if (ArduinoController.command == "RFID" || Input.GetKey(KeyCode.R))
        {
            badge.SetActive(true);
            RFID.SetActive(true);
        }
        if (ArduinoController.command == "Bridge" || Input.GetKey(KeyCode.B))
        {
            sword.SetActive(true);
            Bridge.SetActive(true);
        }
        if (ArduinoController.command == "Release")
            Release.SetActive(true);
        if (ArduinoController.command == "Refuse")
            Refuse.SetActive(true);
        if (ArduinoController.command == "Locked" || Input.GetKeyDown(KeyCode.L))
        {
            locked = true;
            Lock.SetActive(true);
        }
        #endregion

        /* Background Music Control */
        if (ArduinoController.command == "RFID" || ArduinoController.command == "Bridge")
        {
            track.text = MusicLoader.PlayTrack();
        }
        if(Input.GetKeyDown(KeyCode.F6))
        {
            track.text = MusicLoader.NextTrack();
        }

        /* Story */
        #region Story
        if (Time.time > timeNext)
        {
            if (action == Action.Opening)
                action = Action.Start;
            else if (action == Action.Story)
            {
                if (index == clipStory.Length)
                    action = Action.End;
                else
                    TellStory();
            }
        }
        /* Start */
        #region Start
        if (action == Action.Start)
        {
            openingTips.SetActive(false);
            Color c = openingBG.color;
            c.a = Mathf.Lerp(c.a, 0, Time.deltaTime * 1.55f); // During 3s
            openingBG.color = c;
            if (c.a < 0.004f)
            {
                HolyGrailRecharge();
                index = 0;
                action = Action.Story;
                c.a = 0;
                openingBG.color = c;
            }
        }
        #endregion
        /* Night */
        #region Night
        else if (action == Action.End)
        {
            Color c = openingBG.color;
            c.a = Mathf.Lerp(c.a, 1, Time.deltaTime * 1.37f); // During 4s
            openingBG.color = c;
            if (c.a > 0.996f)
            {
                HolyGrailDisappear();
                openingTips.SetActive(true);
                timeNext = Time.time + durationOpening;
                action = Action.Opening;
                c.a = 1;
                openingBG.color = c;
                textStory.text = "聖杯會揀選它的持有者~";
            }
        }
        #endregion
        /* Holy Grail & Oracle */
        #region Holy Grail & Oracle
        else if (action == Action.Story)
        {
            if (holyState == HolyState.Oracle)
            {
                if (Time.time > timeHoly)
                    HolyGrailRecharge();
            }
            else if (holyState == HolyState.HolyGrail && locked)
            {
                if (ArduinoController.command == "Release" || Input.GetKeyDown(KeyCode.D))
                {
                    msgOracle = listOracle[1];
                    Oracle();
                }
                else if (ArduinoController.command == "Refuse" || Input.GetKeyDown(KeyCode.F))
                {
                    msgOracle = listOracle[2];
                    Oracle();
                }
            }
        }
        #endregion
        #endregion
    }

    #region Story
    void TellStory()
    {
        if (!locked)
        {
            timeNext = Time.time + durationClip;
            nameKnight = "教士";
            WhosSay();
            textStory.text = "<color=#191970>珀西瓦爾</color>聖杯就交給你守護了！";
            SayWhat();
        }
        else
        {
            timeNext = Time.time + durationClip;
            nameKnight = clipStory[index].Split(':')[0];
            WhosSay();
            textStory.text = clipStory[index].Split(':')[1];
            SayWhat();
            index++;
        }
    }
    void WhosSay()
    {
        for (int i = 0; i < who.Length; i++)
        {
            who[i].SetActive(false);
        }
        if (nameKnight == "莫德雷德")
            who[0].SetActive(true);
        else if (nameKnight == "蘭斯洛特")
            who[1].SetActive(true);
        else if (nameKnight == "鮑斯")
            who[2].SetActive(true);
        else if (nameKnight == "高文")
            who[3].SetActive(true);
        else if (nameKnight == "教士")
            who[4].SetActive(true);
    }
    void SayWhat()
    {
        textFade.alpha = 0;
        sequence = DOTween.Sequence();
        Tweener alpha1 = DOTween.To(() => textFade.alpha, x => textFade.alpha = x, 1, 2.0f);
        Tweener alpha2 = DOTween.To(() => textFade.alpha, x => textFade.alpha = x, 0, 0.6f);
        sequence.Append(alpha1);
        sequence.AppendInterval(2.73f);
        sequence.Append(alpha2);
    }
    #endregion

    void HolyGrailRecharge()
    {
        holyState = HolyState.Recharge;
        sfxAudio.clip = sfxOracle[0];
        sfxAudio.Play();
        sequence = DOTween.Sequence();
        Tweener alpha1 = DOTween.To(() => holyGrail.color, x => holyGrail.color = x, new Color(holyGrail.color.r, holyGrail.color.g, holyGrail.color.b, 1), 3.0f);
        sequence.Append(alpha1);
        sequence.OnComplete(HolyGrailRebuild);
    }
    void HolyGrailRebuild()
    {
        holyState = HolyState.HolyGrail;
    }
    float HolyGrailRelease()
    {
        timeHoly = Time.time + durationRelease;
        locked = false;
        Lock.SetActive(false);
        ArduinoController.arduinoSerialPort.WriteLine("1");
        sfxAudio.clip = sfxOracle[1];
        sfxAudio.Play();
        HolyGrailDisappear();
        return durationRelease;
    }
    float HolyGrailRefuse()
    {
        durationHoly = 7 + countRefuse;
        timeHoly = Time.time + durationHoly;
        sfxAudio.clip = sfxOracle[2];
        sfxAudio.Play();
        HolyGrailDisappear();
        countRefuse++;
        if (countRefuse > 3)
            countRefuse = 1;
        return durationHoly;
    }
    void HolyGrailDisappear()
    {
        sequence = DOTween.Sequence();
        Tweener alpha2 = DOTween.To(() => holyGrail.color, x => holyGrail.color = x, new Color(holyGrail.color.r, holyGrail.color.g, holyGrail.color.b, 0), 1.0f);
        sequence.Append(alpha2);
    }
    void Oracle()
    {
        holyState = HolyState.Oracle;
        textOracle.text = msgOracle;
        textOracle.color = new Color(textOracle.color.r, textOracle.color.g, textOracle.color.b, 0);
        // Release
        if (msgOracle == listOracle[1])
            durationHoly = HolyGrailRelease();
        // Refuse
        else if (msgOracle == listOracle[2])
            durationHoly = HolyGrailRefuse();
        sequence = DOTween.Sequence();
        Tweener alpha1 = DOTween.To(() => textOracle.color, x => textOracle.color = x, new Color(textOracle.color.r, textOracle.color.g, textOracle.color.b, 1), 2.0f);
        Tweener alpha2 = DOTween.To(() => textOracle.color, x => textOracle.color = x, new Color(textOracle.color.r, textOracle.color.g, textOracle.color.b, 0), 1f);

        sequence.Append(alpha1);
        sequence.AppendInterval(durationHoly - 1.73f);
        sequence.Append(alpha2);
        sequence.AppendInterval(1.0f);
    }
}
