using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class cHistoryUIFrameworkManager : cSingleton<cHistoryUIFrameworkManager>
{
    private Stack<HistoryUI> mHistoryUIList;
    public void Init()
    {
        if (mHistoryUIList == null)
            mHistoryUIList = new Stack<HistoryUI>();
    }

    public void UIHistoryAdd(eHOME_SCENE_STATE _UIScene)
    {
        HistoryUI historyData = new HistoryUI();

        // 히스토리 리스트가 없을 때 기본적으로 로비 씬을 넣어줌.
        if (mHistoryUIList.Count == 0)
        {
            historyData.mHistoryScene = eHOME_SCENE_STATE.LOBBY;
            mHistoryUIList.Push(historyData);
        }

        // 중복 등록되는 UIScene이 있는지 검사
        foreach (HistoryUI o in mHistoryUIList)
        {
            if (o.mHistoryScene == _UIScene)
                return;
        }

        historyData = new HistoryUI();
        historyData.mHistoryScene = _UIScene;


        mHistoryUIList.Push(historyData);
    }

    public void UIHistoryPrev()
    {
        if (mHistoryUIList.Count <= 1)
        {
            Destroy();
            HomeScene.ChangeState(eHOME_SCENE_STATE.LOBBY);
            return;
        }

        mHistoryUIList.Pop();
        HomeScene.ChangeState(mHistoryUIList.Peek().mHistoryScene);

        // 기억된 팝업 리스트가 있다면 차례대로 띄워줌
        if (mHistoryUIList.Count == 0)
            return;
    }

    public Stack<HistoryUI> GetHistoryList()
    {
        return mHistoryUIList;
    }

    // 스택에 저장된 UIScene의 Peek을 얻어옴
    public HistoryUI GetUIHistoryPeek()
    {
        return mHistoryUIList.Peek();
    }

    // 저장된 모든 UIScene 리스트를 삭제
    public void Destroy()
    {
        mHistoryUIList.Clear();
    }

    // 저장된 히스토리 UIScene 검사
    public bool SearchUISceneHistory(eHOME_SCENE_STATE _UIState)
    {
        foreach (HistoryUI o in mHistoryUIList)
        {
            if (o.mHistoryScene == _UIState)
                return true;
        }

        return false;
    }

}

public class HistoryUI
{
    public eHOME_SCENE_STATE mHistoryScene;
}
