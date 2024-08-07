using UnityEngine;
using System.Collections;
using System;

public partial class cUIUtility
{
    // home Control
    public GameObject CreateUIHomeControl()
    {
        GameObject popObj = cUIFrameworkManager.Instance.CreateUI(eUI_NAME.UIHomePanel);
        return popObj;
    }

    public void RefreshHomeAccountInfo(GameObject go)
    {
        if (go != null)
            go.GetComponent<cUIHomeButton>().RefreshAccountInfo();
    }

    public void HomeEnd(GameObject go)
    {
        if (go != null)
            go.GetComponent<cUIHomeButton>().End();
    }

    // Adventure
    public GameObject CreateUIAdventure()
    {
        GameObject popObj = cUIFrameworkManager.Instance.CreateUI(eUI_NAME.UI_AdventureStage);
        return popObj;
    }

    public void AdventureEnd(GameObject go)
    {
        if (go != null)
            go.GetComponent<cUI_AdventureStage>().End();
    }

    // Invnetory
    public GameObject CreateUIInventory()
    {
        GameObject popObj = cUIFrameworkManager.Instance.CreateUI(eUI_NAME.UI_Inventory);
        return popObj;
    }

    public void InventoryEnd(GameObject go)
    {
        if (go != null)
            go.GetComponent<cUI_Inventory>().End();
    }

    /*
    public GameObject CreateUITeamMgr()
    {
        GameObject popObj = cUIFrameworkManager.Instance.CreatePopupUI(ClientConfig.UICOMMON_PREFAB_BASE_DIR + "UI_TeamMgr");
        return popObj;
    }
    */
    // UnitDetail
    public GameObject CreateUIUnitDetail()
    {
        GameObject popObj = cUIFrameworkManager.Instance.CreateUI(eUI_NAME.UI_UnitDetail);
        popObj.GetComponent<cUI_UnitDetail>().Init();
        return popObj;
    }

    public void UnitDetailEnd(GameObject go)
    {
        if (go != null)
            go.GetComponent<cUI_UnitDetail>().End();
    }

    // Quest
    public GameObject CreateQuest()
    {
        GameObject popObj = cUIFrameworkManager.Instance.CreateUI(eUI_NAME.UI_Quest);
        return popObj;
    }

    public void QuestEnd(GameObject go)
    {
        if (go != null)
            go.GetComponent<cUI_Quest>().End();
    }

    // Friend
    public GameObject CreateFriend()
    {
        GameObject popObj = cUIFrameworkManager.Instance.CreateUI(eUI_NAME.UI_Friend);
        return popObj;
    }

    public void FriendEnd(GameObject go)
    {
        if (go != null)
            go.GetComponent<cUI_Friend>().End();
    }

    // MailBox
    public GameObject CreateMailBox()
    {
        GameObject popObj = cUIFrameworkManager.Instance.CreateUI(eUI_NAME.UI_MailBox);
        return popObj;
    }

    public void MailBoxEnd(GameObject go)
    {
        if (go != null)
            go.GetComponent<cUI_MailBox>().End();
    }

    // Challenge
    public GameObject CreateChallenge()
    {
        GameObject popObj = cUIFrameworkManager.Instance.CreateUI(eUI_NAME.UI_Challenge);
        return popObj;
    }

    public void ChallengeEnd(GameObject go)
    {
        if (go != null)
            go.GetComponent<cUI_Challenge>().End();
    }

    // Team
    public GameObject CreateTeam()
    {
        GameObject popObj = cUIFrameworkManager.Instance.CreateUI(eUI_NAME.UI_TeamMgr);
        return popObj;
    }

    public void TeamEnd(GameObject go)
    {
        if (go != null)
            go.GetComponent<cUI_TeamMgr>().End();
    }
}