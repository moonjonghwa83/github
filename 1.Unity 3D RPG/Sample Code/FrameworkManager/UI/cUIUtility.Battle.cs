using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public partial class cUIUtility : cSingleton<cUIUtility>
{
    // UIBattle ============================================
    public GameObject CreateUIBattlePanel()
    {
        GameObject uibattle = cUIFrameworkManager.Instance.CreateUI(eUI_NAME.UIBattlePanel, false, false);
        //uibattle.GetComponent<cUIBattlePanel>().Init();//inFuncpause, inFuncauto);
        return uibattle;
    }

    public void ButtonClickPause()
    {
        cUIFrameworkManager.Instance.GetUI(eUI_NAME.UIBattlePanel).GetComponent<cUIBattlePanel>().BackKeyProcessBattle();
    }
    public void BattleEnd(GameObject go)
    {
        if (go != null)
            go.GetComponent<cUIBattlePanel>().End(go);
    }

    public void UpdateGameLimitTime(int _time)
    {
        cUIFrameworkManager.Instance.GetUI(eUI_NAME.UIBattlePanel).GetComponent<cUIBattlePanel>().TimeUpDate(_time);
    }

    public void SetBossActive(cBattleUnitBase _bossUnit)
    {
        cUIFrameworkManager.Instance.GetUI(eUI_NAME.UIBattlePanel).GetComponent<cUIBattlePanel>().BossActive(_bossUnit);
    }

    public void RefreshBattlePlayer()
    {
        cUIFrameworkManager.Instance.GetUI(eUI_NAME.UIBattlePanel).GetComponent<cUIBattlePanel>().UIHpReflash();
    }

    // UI hpbar ============================================
    public GameObject CreateHpBar(cBattleUnitBase _unit)
    {
        GameObject HPbar = cUIFrameworkManager.Instance.CreateItemUI(string.Format("{0}{1}", ClientConfig.UICOMMON_PREFAB_BASE_DIR, "UIHPBar"), "UIHPBar");
        HPbar.GetComponent<cUIHPBar>().Init(_unit);
        return HPbar;
    }

    public void HpBarHpUpdate(GameObject go, int _nowHp, int _baseHp)
    {
        if (go != null)
            go.GetComponent<cUIHPBar>().HeroHpUpDate(_nowHp, _baseHp);
    }

    public void HpBarUnitBuffIcon(GameObject go, cBattleUnitBase _unit, List<cBuffBase> _listBuff)
    {
        if (go != null)
            go.GetComponent<cUIHPBar>().SetBuffIconList(_unit, _listBuff);
    }

    public void HpBarEnd(GameObject go)
    {
        if (go != null)
            go.GetComponent<cUIHPBar>().End(go);
    }

    // UIGameStart ============================================
    public GameObject CreateGameStart()
    {
        GameObject popObj = cUIFrameworkManager.Instance.CreateUI(eUI_NAME.UI_StartGame);
        return popObj;
    }

    public void GameStartEnd(GameObject go)
    {
        if (go != null)
            go.GetComponent<cUI_StartGame>().End(go);
    }

    // UIGameVictory ============================================
    public GameObject CreateGameVictory()
    {
        GameObject popObj = cUIFrameworkManager.Instance.CreateUI(eUI_NAME.UI_GameVictory);
        return popObj;
    }

    public void GameVictoryEnd(GameObject go)
    {
        if (go != null)
            go.GetComponent<cUI_GameVictory>().End(go);
    }

    // UIGamelose ============================================
    public GameObject CreateGameLose()
    {
        GameObject popObj = cUIFrameworkManager.Instance.CreateUI(eUI_NAME.UI_GameLose);
        return popObj;
    }

    public void GameLoseEnd(GameObject go)
    {
        if (go != null)
            go.GetComponent<cUI_GameLose>().End(go);
    }

    // UIResult ============================================
    public GameObject CreateUIResultSuccess(cRewardInfo _battleRewardInfo, cInGameRewardInfo _InGameRewardInfo)
    {
        GameObject uibattle = cUIFrameworkManager.Instance.CreateUI(eUI_NAME.UI_InGameResultSuccess, true);
        uibattle.GetComponent<cUI_InGameResultSuccess>().Init(_battleRewardInfo, _InGameRewardInfo);
        return uibattle;
    }
    public void ResultSuccessEnd(GameObject go)
    {
        if (go != null)
            go.GetComponent<cUI_InGameResultSuccess>().End();
    }

    public GameObject CreateUIResultFailure()
    {
        GameObject uibattle = cUIFrameworkManager.Instance.CreateUI(eUI_NAME.UI_InGameResultFailure, true);
        return uibattle;
    }
    public void ResultFailureEnd(GameObject go)
    {
        if (go != null)
            go.GetComponent<cUI_InGameResultFailure>().End();
    }

    // Boss
    public void CreateUIRadeBoss()
    {
        cUIFrameworkManager.Instance.CreateUI(eUI_NAME.UIRadeBoss);
    }

    public void EndUIRadeBoss()
    {
        cUIFrameworkManager.Instance.UIDestroy(eUI_NAME.UIRadeBoss);
    }


}
