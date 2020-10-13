using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using NetWork;
using System.Runtime.CompilerServices;
using UnityEngine.Rendering;

public class ScoreEffectSystem : MonoBehaviour
{

    #region 변수들
    enum TeamColor
    {
        None,
        Red,
        Blue
    }
    enum WinCheck
    {
        Draw = 0,
        RedWin,
        BlueWin
    }
    private TeamColor teamColor;
    private WinCheck winCheck;

    public GameObject endingGroup;
    public Image recipeOne;
    public Image recipeTwo;
    public GameObject recipeGroup;

    public Image spotLightBlue;
    public Image spotLightRed;
    private Color spotLightColor;


    public TMP_Text redTeamText;
    public TMP_Text blueTeamText;

    public GameObject blueBlackCanvas;
    public GameObject redBlackCanvas;
    public Image tenPos;
    public Image sabackPos;

    public Image crownRedImg;
    public Image crownBlueImg;

    private bool isLightOn;
    [Tooltip("몇 초 동안 뜨르르르르르...를 할지 정합니다.")]
    public float tararaSec;

    [Tooltip("몇 초의 간격으로 뜨르르르를 할지 정합니다.")]
    public float tararaTerm;

    public int redScore;
    public int blueScore;

    bool doTararaStop;

    [Header("점수 산출 후 엔딩(결산) 관련")]
    public Image whitePanel;
    public GameObject RedCharacter;
    public GameObject BlueCharacter;

    public Image teamWin;
    public Image teamLose;
    public Image teamDraw;
    public TMP_Text teamScoreText;

    public Button goIntroButton;

    [Header("사운드 관련")]
    public TestSoundSystem testSoundSystem;


    //[SerializeField]
    //private PlayerController _pCtrl = null;

    //private Animator _pAnim = null;

    //private struct Anikey
    //{
    //    public const string Win = "Win";
    //    public const string Lose = "Lose";
    //}
    #endregion
    public void Init()
    {

        goIntroButton.gameObject.SetActive(false);
        teamScoreText.enabled = false;
        teamWin.enabled = false;
        teamLose.enabled = false;
        teamDraw.enabled = false;
        whitePanel.enabled = false;
        RedCharacter.SetActive(false);
        BlueCharacter.SetActive(false);

        spotLightColor = new Color(1, 1, 1, 0.5f);
        spotLightBlue.enabled = false;
        spotLightRed.enabled = false;
        isLightOn = false;
        blueBlackCanvas.transform.position = new Vector3(blueBlackCanvas.transform.position.x, blueBlackCanvas.transform.position.y, tenPos.transform.position.z);
        redBlackCanvas.transform.position = new Vector3(redBlackCanvas.transform.position.x, redBlackCanvas.transform.position.y, tenPos.transform.position.z);
        redScore = 0;
        blueScore = 0;
        doTararaStop = false;


        crownBlueImg.enabled = false;
        crownRedImg.enabled = false;

        recipeGroup.transform.position = recipeOne.transform.position;
        endingGroup.SetActive(false);

        //int curTeam = (int)_pCtrl.Team;
        //_pCtrl.GetComponent<Rigidbody>().isKinematic = true;
        //var pObject = _pCtrl.transform.GetChild(((int)curTeam)).gameObject;
        //pObject.SetActive(true);
        //_pAnim = pObject.GetComponent<Animator>();
    }

    private void Awake()
    {
        Init();
    }
    // Update is called once per frame
    void Update()
    {
        if (isLightOn == false)
        {
            redTeamText.text = System.Convert.ToString(redScore) + "%";
            blueTeamText.text = System.Convert.ToString(blueScore) + "%";
        }

    }
    public void StartTarara()
    {

        StartCoroutine(GoTarara());

    }

    /// <summary>
    /// 내 팀이 뭔지 정합니다. 결산 화면에서 사용됩니다. 0 : None, 1 : Red, 2 : Blue
    /// </summary>
    /// <param name="teamNumber">0 : None, 1 : Red, 2 : Blue</param>
    public void SetMyTeam(int teamNumber)
    {
        if (teamNumber == 0 || teamNumber == 1 || teamNumber == 2)
        {
            teamColor = (TeamColor)teamNumber;
        }
        else
        {
            LogManager.Log("teamNumber는 0, 1, 2 중 하나여야 함.");
        }
    }

    public void GoHomeScene()
    {
        Destroy(TartSystemManager.Instance.teamRedTart.gameObject);
        Destroy(TartSystemManager.Instance.teamRedTart.gameObject);

        UnityEngine.SceneManagement.SceneManager.LoadScene("HomeScene");

    }

    //private void PlayerWin()
    //{
    //    if (_pCtrl == null) return;
    //    if (_pAnim == null) return;

    //    _pAnim.SetTrigger(Anikey.Win);
    //}
    //private void PlayerLose()
    //{
    //    if (_pCtrl == null) return;
    //    if (_pAnim == null) return;

    //    _pAnim.SetTrigger(Anikey.Lose);
    //}

    private WinCheck WhoIsWinner()
    {
        if (redScore > blueScore)
        {
            return WinCheck.RedWin;
        }
        else if (blueScore > redScore)
        {
            return WinCheck.BlueWin;
        }
        else
        {
            return WinCheck.Draw;
        }
    }

    private IEnumerator GoTarara()
    {
        Init();
        #region 레시피 이동
        float timer = 0f;
        while (true)
        {
            timer += Time.deltaTime;
            recipeGroup.transform.Translate(Vector3.down * timer * 10f);

            if (recipeGroup.transform.position.y < recipeTwo.transform.position.y)
            {
                LogManager.Log("끝.");
                break;
            }
            yield return null;
        }
        #endregion

        yield return new WaitForSecondsRealtime(0.75f);
        testSoundSystem.PlayOneShotSFX(1);
        //testSoundSystem.SetSFXLoop(false);
        doTararaStop = false;
        StartCoroutine(StopTarara(tararaSec));



        while (doTararaStop == false)
        {
            #region 스포트라이트 이미지 깜빡임 연출

            redScore = Random.Range(11, 98);
            spotLightRed.enabled = true;
            spotLightBlue.enabled = false;

            yield return new WaitForSecondsRealtime(tararaTerm);

            blueScore = Random.Range(11, 98);
            spotLightRed.enabled = false;
            spotLightBlue.enabled = true;


            yield return new WaitForSecondsRealtime(tararaTerm);
            #endregion

        }
        TartSystemManager.Instance.CheckBothScore();
        redScore = TartSystemManager.Instance.teamRedScore;
        blueScore = TartSystemManager.Instance.teamBlueScore;
        //redScore = 10;
        //blueScore = 10;
        redTeamText.text = " ";
        blueTeamText.text = " ";
        isLightOn = true;


        spotLightRed.enabled = false;
        spotLightBlue.enabled = false;

        #region 이미지 이동 연출구간 2
        yield return new WaitForSecondsRealtime(1f);
        testSoundSystem.StopSFX();
        testSoundSystem.PlayOneShotSFX(2);

        isLightOn = false;

        switch (WhoIsWinner())
        {
            case WinCheck.Draw:
                spotLightRed.color = spotLightColor;
                spotLightBlue.color = spotLightColor;
                spotLightRed.enabled = true;
                spotLightBlue.enabled = true;
                break;

            case WinCheck.RedWin:
                redBlackCanvas.transform.position = new Vector3(redBlackCanvas.transform.position.x, redBlackCanvas.transform.position.y, sabackPos.transform.position.z);
                crownRedImg.enabled = true;
                spotLightRed.enabled = true;
                spotLightBlue.enabled = false;
                break;

            case WinCheck.BlueWin:
                blueBlackCanvas.transform.position = new Vector3(blueBlackCanvas.transform.position.x, blueBlackCanvas.transform.position.y, sabackPos.transform.position.z);
                crownBlueImg.enabled = true;
                spotLightRed.enabled = false;
                spotLightBlue.enabled = true;
                break;

            default:
                LogManager.Log("WhoIsWinner에서 다른 값 발생");
                break;
        }

        #endregion

        yield return new WaitForSecondsRealtime(3f);
        endingGroup.SetActive(true);
        StartCoroutine(GoEndScreen());

    }
    private IEnumerator GoEndScreen()
    {
        teamColor = (TeamColor)GameManager.Instance.PlayerTeam;
        if (teamColor == 0)
        {
            LogManager.Log("Team None. -> Team Blue");
            teamColor = TeamColor.Blue;
        }

        whitePanel.enabled = true;
        goIntroButton.gameObject.SetActive(true);

        WinCheck winCheckTemp = WhoIsWinner();
        RedCharacter.SetActive(false);
        BlueCharacter.SetActive(false);

        switch (teamColor)
        {
            case TeamColor.None:
                LogManager.Log("None Team");
                RedCharacter.SetActive(false);
                BlueCharacter.SetActive(false);
                break;
            case TeamColor.Red:
                LogManager.Log("Red Team");
                teamScoreText.enabled = true;
                RedCharacter.SetActive(true);
                if (winCheckTemp == WinCheck.RedWin)
                {
                    testSoundSystem.PlayOneShotSFX(3);
                    teamScoreText.text = redScore + "% 완성!";
                    teamWin.enabled = true;
                    //PlayerWin();
                }
                else if (winCheckTemp == WinCheck.Draw)
                {
                    testSoundSystem.PlayOneShotSFX(4);
                    teamScoreText.text = redScore + "% 완성";
                    teamDraw.enabled = true;
                    //PlayerLose();
                }
                else
                {
                    testSoundSystem.PlayOneShotSFX(4);
                    teamScoreText.text = redScore + "% 완성...";
                    teamLose.enabled = true;
                    //PlayerLose();
                }
                break;
            case TeamColor.Blue:
                LogManager.Log("Blue Team");
                teamScoreText.enabled = true;
                BlueCharacter.SetActive(true);
                if (winCheckTemp == WinCheck.BlueWin)
                {
                    testSoundSystem.PlayOneShotSFX(3);
                    teamScoreText.text = blueScore + "% 완성!";
                    teamWin.enabled = true;
                    //PlayerWin();
                }
                else if (winCheckTemp == WinCheck.Draw)
                {
                    testSoundSystem.PlayOneShotSFX(4);
                    teamScoreText.text = blueScore + "% 완성";
                    teamDraw.enabled = true;
                    // PlayerLose();
                }
                else
                {
                    testSoundSystem.PlayOneShotSFX(4);
                    teamScoreText.text = blueScore + "% 완성...";
                    teamLose.enabled = true;
                    // PlayerLose();
                }
                break;
            default:
                break;
        }
        yield return null;
    }


    private IEnumerator StopTarara(float sec)
    {
        yield return new WaitForSecondsRealtime(sec);
        LogManager.Log("타라라 끝");
        doTararaStop = true;
    }

}
