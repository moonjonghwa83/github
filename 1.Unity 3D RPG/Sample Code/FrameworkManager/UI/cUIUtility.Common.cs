using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class cUIUtility : cSingleton<cUIUtility>
{
    // login
    public GameObject CreateUILogin()
    {
        GameObject popObj = cUIFrameworkManager.Instance.CreateUI(eUI_NAME.UILogin);
        return popObj;
    }

    public void LoginEnd(GameObject go)
    {
        if (go != null)
            go.GetComponent<cUILogin>().End();
    }

    // logo
    public GameObject CreateUILogo()
    {
        GameObject popObj = cUIFrameworkManager.Instance.CreateUI(eUI_NAME.UILogo);
        return popObj;
    }

    public void LogoEnd(GameObject go)
    {
        if (go != null)
            go.GetComponent<cUILogo>().End();
    }

    // notice

    public void NoticeCreate(string _content, bool _overlap = false)
    {
        cUIFrameworkManager.Instance.NoticeCreate(_content, _overlap);
    }

    // common
    public void CreateCommonPopup(string _text, string _rightBtnText, string _leftBtnText, System.Action _rightFunc, System.Action _leftFunc = null, bool _backUse = true)
    {
        GameObject popObj = cUIFrameworkManager.Instance.CreateUI(eUI_NAME.UI_CommonPopUp);

        cUI_CommonPopup commonPopup = popObj.GetComponent<cUI_CommonPopup>();
        commonPopup.AssignString(cUI_CommonPopup.eCommonPopupType.ONE_BUTTON, _text, _rightBtnText, _leftBtnText, _rightFunc, _leftFunc);
        commonPopup.BackUse(_backUse);
        popObj.transform.localPosition = Vector3.back * 600;
    }

    // level
    public GameObject CreateUILevelup(int _preLevel, int _curLevel)
    {
        //GameObject popObj = cUIFrameworkManager.Instance.CreateUI(eUI_NAME.UI_Levelup);

        GameObject popObj = cUIFrameworkManager.Instance.CreatePopupUI(ClientConfig.UICOMMON_PREFAB_BASE_DIR + "UI_Levelup");
        popObj.GetComponent<cUI_Levelup>().Init(_preLevel, _curLevel);

        return popObj;
    }

    public void UILevelupEnd(GameObject go)
    {
        if (go != null)
            go.GetComponent<cUI_Levelup>().End();
    }

    public string StringCommaAdd(int _number)
    {
        int number = Mathf.Abs(_number);
        string strnumber = number.ToString();
        string str = strnumber;

        if (strnumber.Length != 0)
        {
            str = string.Format("{0:N0}", Convert.ToInt32(strnumber));
        }

        if (_number < 0)
        {
            str = str.Insert(0, "-");
        }
        return str;
    }

    // level
    public GameObject CreateUIAdventureDetailInfo()
    {
        GameObject popObj = cUIFrameworkManager.Instance.CreatePopupUI(ClientConfig.UICOMMON_PREFAB_BASE_DIR + "UI_AdventureDetailInfo");

        return popObj;
    }

    public GameObject CreateUIExit()
    {
        GameObject popObj = cUIFrameworkManager.Instance.CreatePopupUI(ClientConfig.UICOMMON_PREFAB_POPUP_DIR + "UI_ExitPopUp", 1000);

        return popObj;
    }

    public void UIAdventureDetailInfoEnd(GameObject go)
    {
        if (go != null)
            go.GetComponent<cUI_AdventureDetailInfo>().End();
    }
}
