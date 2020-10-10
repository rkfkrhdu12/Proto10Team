using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreEffectSystem : MonoBehaviour
{
    public TMP_Text redTeamText;
    public TMP_Text blueTeamText;

    [Tooltip("몇 초 동안 뜨르르르르르...를 할지 정합니다.")]
    public float tararaSec;

    [Tooltip("몇 초의 간격으로 뜨르르르를 할지 정합니다.")]
    public float tararaTerm;

    int redScore;
    int blueScore;

    bool doTararaStop;
    private void Awake()
    {
        redScore = 0;
        blueScore = 0;
        doTararaStop = false;
    }
    // Update is called once per frame
    void Update()
    {
        redTeamText.text = "blueTeam Score : " + System.Convert.ToString(redScore);
        blueTeamText.text = "redTeam Score : " + System.Convert.ToString(blueScore);
    }
    public void StartTarara()
    {
        StartCoroutine(GoTarara());

    }
    private IEnumerator GoTarara()
    {
        doTararaStop = false;
        StartCoroutine(StopTarara(tararaSec));
        while (doTararaStop == false)
        {
            redScore = Random.Range(11, 98);
            blueScore = Random.Range(11, 98);
            yield return new WaitForSeconds(tararaTerm);

        }
        redScore = TartSystemManager.Instance.teamRedScore;
        blueScore = TartSystemManager.Instance.teamBlueScore;

    }
    private IEnumerator StopTarara(float sec)
    {

        yield return new WaitForSecondsRealtime(sec);
        Debug.Log("타라라 끝");
        doTararaStop = true;

    }

}
