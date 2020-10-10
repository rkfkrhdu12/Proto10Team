using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Networking.Types;
using System.Runtime.InteropServices.WindowsRuntime;

public class OverUIManager : MonoBehaviour
{
    public LobbyManager lobbyManager;
    public GameObject targetObject; //타겟 오브젝트(테스트용)

    [SerializeField]
    private bool isTargetOver; //타겟이 버튼 위에 있냐?

    [SerializeField]
    private float targetOverTime; //타겟이 버튼 위에 올라가있는 현재의 시간... 

    [Range(0f, 0.5f)]
    public float exitFillSpped = 0.03f; // 타겟이 버튼 위에서 나갔을 때, 이미지의 fillAmount가 0이 될 때 까지의 가속도?...

    [Range(0f, 30f)]
    public float successTime = 5f; //얼마나 올라가 있어야 버튼이 눌린 것으로 판단하는가?

    [SerializeField]
    private float successPer; // targetOvertime/successTime

    public bool isSuccess; //결국...버튼이 눌린거임?

    private bool isExiting; //exitButton 코루틴이 돌아가는 중인지를 알아내기 위해

    private Image buttonImage;

    [Tooltip("버튼의 타입임. 게임시작버튼 = 1, ...")]
    public int buttonType;
    private void Awake()
    {
        if (buttonImage == null)
        {
            buttonImage = GetComponent<Image>();
        }

        if (lobbyManager ==null)
        {
            LogManager.Log("이상해;;;");
            lobbyManager = GameObject.FindObjectOfType<LobbyManager>(); //어차피 하나 밖에 없겠지?
        }
    }
    void Start()
    {
        isTargetOver = false;
        isSuccess = false;
        isExiting = false;


        if (targetOverTime != 0f)
        {

            targetOverTime = 0f;

        }

    }

    void Update()
    {
        successPer = targetOverTime / successTime;
        buttonImage.fillAmount = successPer;
    }


    IEnumerator ExitButton()
    {
        switch (buttonType)
        {
            case 1:
                if (isSuccess)
                {
                    yield break;
                }
                break;
            default:
                isSuccess = false;
                break;
        }

        LogManager.Log("ButtonExiting...");
        isExiting = true;


        float accelValue = 0f;

        while (targetOverTime > 0f)
        {
            if (isTargetOver)
            {
                yield break;
            }
            accelValue += Time.smoothDeltaTime*exitFillSpped;
            targetOverTime -= accelValue;
            yield return null;
        }

        if (targetOverTime != 0f)
        {
            targetOverTime = 0f;
        }

        isExiting = false;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == targetObject)
        {
            isTargetOver = true;

            if (isExiting)
            {
                StopCoroutine(ExitButton());
                isExiting = false;
            }

            if (targetOverTime < successTime && !isSuccess)
            {
                targetOverTime += Time.smoothDeltaTime;
            }
            else if(targetOverTime >=successTime&&!isSuccess)
            {
                LogManager.Log("Success!");

                switch (buttonType)
                {
                    case 1:
                        isSuccess = true;
                        lobbyManager.JoinRandomRoom();
                        break;

                    default:
                        isSuccess = true;
                        break;
                }
            }
            else if (isSuccess)
            {
                return;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == targetObject)
        {
            isTargetOver = false;
            StartCoroutine(ExitButton());
        }
    }
}
