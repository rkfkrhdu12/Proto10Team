using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScoreEffectSystem : MonoBehaviour
{
    public Image recipeOne;
    public Image recipeTwo;
    public GameObject recipeGroup;

    public Image lightRedPos;
    public Image lightBluePos;
    public Image lightImg;

    public TMP_Text redTeamText;
    public TMP_Text blueTeamText;

    public GameObject blueBlackCanvas;
    public GameObject redBlackCanvas;
    public Image tenPos;
    public Image sabackPos;

    public bool isLightOn;
    [Tooltip("몇 초 동안 뜨르르르르르...를 할지 정합니다.")]
    public float tararaSec;

    [Tooltip("몇 초의 간격으로 뜨르르르를 할지 정합니다.")]
    public float tararaTerm;

    int redScore;
    int blueScore;

    bool doTararaStop;
    private void Awake()
    {
        isLightOn = false;
        blueBlackCanvas.transform.position = new Vector3(blueBlackCanvas.transform.position.x, blueBlackCanvas.transform.position.y, tenPos.transform.position.z);
        redBlackCanvas.transform.position = new Vector3(redBlackCanvas.transform.position.x, redBlackCanvas.transform.position.y, tenPos.transform.position.z);
        redScore = 0;
        blueScore = 0;
        doTararaStop = false;

        recipeGroup.transform.position = recipeOne.transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        if (isLightOn==false)
        {
            redTeamText.text = System.Convert.ToString(redScore);
            blueTeamText.text = System.Convert.ToString(blueScore);
        }

    }
    public void StartTarara()
    {
        StartCoroutine(GoTarara());

    }
    private IEnumerator GoTarara()
    {
        #region 이미지 이동 연출 구간
        float timer = 0f;
        while (true)
        {
            timer += Time.deltaTime;
            recipeGroup.transform.Translate(Vector3.down*timer*10f);

            if (recipeGroup.transform.position.y<recipeTwo.transform.position.y)
            {
                LogManager.Log("끝.");
                break;
            }
            yield return null;
        }
        #endregion


        doTararaStop = false;
        StartCoroutine(StopTarara(tararaSec));
        while (doTararaStop == false)
        {
            redScore = Random.Range(11, 98);

            #region 스포트라이트 이미지 이동 연출
            lightImg.transform.position = lightRedPos.transform.position;
           
            
            yield return new WaitForSecondsRealtime(tararaTerm);
            #endregion

            blueScore = Random.Range(11, 98);

            #region 스라이동연
            lightImg.transform.position = lightBluePos.transform.position;

            yield return new WaitForSecondsRealtime(tararaTerm);
            #endregion

        }
        //redScore = TartSystemManager.Instance.teamRedScore;
        //blueScore = TartSystemManager.Instance.teamBlueScore;
        redScore = 92;
        blueScore = 57;
        redTeamText.text = " ";
        blueTeamText.text = " ";
        isLightOn = true;

        #region 이미지 이동 연출구간 2
        if (redScore>blueScore)
        {
            
            lightImg.transform.position = lightRedPos.transform.position;
            lightImg.enabled = false;
            yield return new WaitForSecondsRealtime(1f);
            lightImg.enabled = true;
  
            isLightOn = false;
            redBlackCanvas.transform.position = new Vector3(redBlackCanvas.transform.position.x, redBlackCanvas.transform.position.y, sabackPos.transform.position.z);
        }
        else
        {
            lightImg.transform.position = lightBluePos.transform.position;
            lightImg.enabled = false;
            yield return new WaitForSecondsRealtime(1f);
            lightImg.enabled = true;
            isLightOn = false;
 
            blueBlackCanvas.transform.position = new Vector3(blueBlackCanvas.transform.position.x, blueBlackCanvas.transform.position.y, sabackPos.transform.position.z);
        }
        #endregion

    }
    private IEnumerator StopTarara(float sec)
    {

        yield return new WaitForSecondsRealtime(sec);
        LogManager.Log("타라라 끝");
        doTararaStop = true;

    }

}
