using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum eUI_NAME
{
    UI_CommonPopUp,
    UIHomePanel,
    UI_StartGame,
    UI_GameVictory,
    UI_GameLose,
    UIBattlePanel,
    UILogin,
    UILogo,
    UI_AdventureStage,
    UI_Inventory,
    UI_UnitDetail,
    UI_InGameResultSuccess,
    UI_InGameResultFailure,
    UI_TeamMgr,
    UI_Notice,
    UI_Levelup,
    UIRadeBoss,
    UI_Quest,
    UI_Friend,
    UI_MailBox,
    UI_Challenge,
}

public enum eBackKeyType
{
    INTROLOGIN = 0,
    LOBBY,
    INGAME,
    SELECT,
}

public class cUIFrameworkManager : cSingleton<cUIFrameworkManager>
{
    static long m_uiIndex = 0;
    public static long m_SelectIdx = 0;
    static int m_renderq = 2000;
    static int m_ItemRenderq = 1000;
    static long m_uiItemIndex = 0;
    static UIBase m_SelectUI = null;
    private UIPanel mUIBack;

    UIRoot m_2DRoot;

    public void Init()
    {

        GameObject uiroot = GameObject.Find("UI Root");
        if (uiroot != null)
            m_2DRoot = GameObject.Find("UI Root").GetComponent<UIRoot>();

        if (m_2DRoot == null)
        {
            GameObject obj = GameObject.Instantiate(Resources.Load("UI/UI Root")) as GameObject;
            obj.name = "UI Root";
            m_2DRoot = obj.GetComponent<UIRoot>();
            MonoBehaviour.DontDestroyOnLoad(m_2DRoot.gameObject);
        }

        m_2DRoot.transform.localPosition = new Vector3(0.0f, 1000.0f, 0.0f);
        m_2DRoot.scalingStyle = UIRoot.Scaling.ConstrainedOnMobiles;
        m_2DRoot.fitHeight = true;
        m_2DRoot.manualHeight = 768;
        m_2DRoot.fitWidth = true;
        m_2DRoot.manualWidth = 1024;

        mUIBack = m_2DRoot.transform.Find("UI_Back").GetComponent<UIPanel>();
        mUIBack.renderQueue = UIPanel.RenderQueue.StartAt;
    }

    public void Destroy()
    {
        UIDestroy();
        m_UILiat.Clear();
        m_UIAttach.Clear();
        m_uiIndex = 0;
        m_SelectIdx = 0;
        m_renderq = 2000;
        m_ItemRenderq = 1000;
        m_uiItemIndex = 0;
        m_SelectUI = null;
        settingBack(null, false);

    }

    List<UIBase> m_UILiat = new List<UIBase>();
    List<UIBase> mPopupList = new List<UIBase>();
    List<GameObject> m_UIAttach = new List<GameObject>();
    Stack<long> m_history = new Stack<long>();
    GameObject m_notice = null;
    GameObject m_firstOfAll = null;

    private GameObject mUIExiptPopup = null;
    public GameObject FirstOfAllCreate(eUI_NAME _uiIndex)
    {
        if (m_firstOfAll == null)
        {
            m_firstOfAll = CreateUI(_uiIndex, true, false, true);
        }
        else
        {
            Debug.LogWarning("더이상 생성할 수 없습니다.");
        }

        return m_firstOfAll;
    }

    public void NoticeCreate(string _content, bool _overlap = false)
    {
        if (m_notice == null)
        {
            m_notice = GameObject.Instantiate(Resources.Load(string.Format("UI/UICommon/{0}", eUI_NAME.UI_Notice))) as GameObject;
            Transform t = m_notice.transform;
            m_notice.transform.parent = m_2DRoot.transform;
            t.localPosition = new Vector3(0f, 190.0f, 0f);
            t.localRotation = Quaternion.identity;
            t.localScale = Vector3.one;
            m_notice.name = string.Format("{0}_{1}", eUI_NAME.UI_Notice, m_uiIndex);
            m_notice.layer = m_2DRoot.gameObject.layer;

            UIPanel panel = m_notice.GetComponent<UIPanel>();

            panel.renderQueue = UIPanel.RenderQueue.StartAt;
            panel.startingRenderQueue = m_SelectUI.GetComponent<UIPanel>().startingRenderQueue + 20;
            panel.depth = (int)m_uiIndex + 5;
            m_uiIndex++;
        }
        else
        {
            if (_overlap)
                if (m_notice.GetComponent<cUI_Notice>().SearchOverlap(_content))
                    return;

            UIPanel panel = m_notice.GetComponent<UIPanel>();

            panel.renderQueue = UIPanel.RenderQueue.StartAt;
            panel.startingRenderQueue = m_SelectUI.GetComponent<UIPanel>().startingRenderQueue + 20;
            panel.depth = m_SelectUI.GetComponent<UIPanel>().depth + 5;
        }

        m_notice.GetComponent<cUI_Notice>().SetNotice(_content);
    }

    public GameObject CreateUI(eUI_NAME _uiIndex, bool _isBack = false, bool _history = false, bool _isNewUI = false)
    {
        UIBase basecs = null;

        if (_isNewUI == false)
        {
            basecs = GetUI(_uiIndex, -1);
            if (basecs != null)
            {
                SetHistory(basecs.m_index, _history);
                setSelectUI(basecs, _isBack);
                if (basecs.gameObject.activeSelf == false)
                    basecs.gameObject.SetActive(true);
                return basecs.gameObject;
            }
        }
        GameObject obj = null;
        obj = GameObject.Instantiate(Resources.Load(string.Format("UI/UICommon/{0}", _uiIndex))) as GameObject;
        Transform t = obj.transform;
        obj.transform.parent = m_2DRoot.transform;
        t.localPosition = Vector3.zero;
        t.localRotation = Quaternion.identity;
        t.localScale = Vector3.one;
        obj.name = string.Format("{0}_{1}", _uiIndex, (m_uiIndex + 5));
        obj.layer = m_2DRoot.gameObject.layer;

        UIBase uibase = obj.GetComponent<UIBase>();
        uibase.m_index = m_uiIndex;
        uibase.m_IdxBefore = m_uiIndex - 1;
        uibase.m_enum = _uiIndex;

        if (m_SelectUI == null)
        {
            m_SelectUI = uibase;
        }
        else
        {
            uibase.m_ExistionUI = m_SelectUI;
            m_SelectUI = uibase;
        }

        UIPanel panel = obj.GetComponent<UIPanel>();

        panel.renderQueue = UIPanel.RenderQueue.StartAt;
        panel.startingRenderQueue = m_renderq;
        panel.depth = (int)m_uiIndex + 3;

        m_renderq += 30;
        obj.GetComponent<UIBase>().m_renderq = m_renderq;
        setSelectUI(uibase, _isBack);

        if (m_notice != null)
        {
            UIPanel noticepanel = m_notice.GetComponent<UIPanel>();
            noticepanel.startingRenderQueue = m_renderq + 30;
            noticepanel.depth = (int)m_uiIndex + 3;
        }

        m_UILiat.Add(obj.GetComponent<UIBase>());
        SetHistory(m_uiIndex, _history);
        m_uiIndex += 3;
        return obj;
    }

    void SetHistory(long _index, bool _history = false)
    {
        if (_history == true)
            m_history.Push(_index);
    }

    public UIBase GetUI(eUI_NAME _uiIndex, int _index = -1)
    {
        for (int i = 0; i < m_UILiat.Count; i++)
        {
            if (m_UILiat[i].m_enum == _uiIndex || m_UILiat[i].m_index == _index)
            {
                return m_UILiat[i];
            }
        }

        return null;
    }

    public GameObject GetUIGameObject(eUI_NAME _uiIndex, int _index = -1)
    {
        for (int i = 0; i < m_UILiat.Count; i++)
        {
            if (m_UILiat[i].m_enum == _uiIndex || m_UILiat[i].m_index == _index)
            {
                return m_UILiat[i].gameObject;
            }
        }
        return null;
    }

    /*
    public void SelectUI(eUI_NAME _uiIndex)
	{
		UIBase ui = GetUI (_uiIndex);
		if (ui != null) {
			if(ui.gameObject.activeSelf == false)
				ui.gameObject.SetActive(true);

			ui.GetComponent<UIPanel>().startingRenderQueue = m_renderq + 30;
            setSelectUI(ui);
			SetHistory(ui.m_index);
			UIRefresh ();
		}
	}
    */
    public void UIActive(long _index, bool _isActive = false)
    {
        for (int i = 0; i < m_UILiat.Count; i++)
        {
            if (m_UILiat[i].m_index == _index)
            {
                m_UILiat[i].gameObject.SetActive(_isActive);
            }
        }

        if (m_history.Peek() == _index)
        {
            m_history.Pop();
        }
        long index = m_history.Pop();
        for (int i = 0; i < m_UILiat.Count; i++)
        {
            if (m_UILiat[i].m_index == index)
            {
                m_UILiat[i].gameObject.SetActive(true);
                setSelectUI(m_UILiat[i]);
                m_history.Push(m_UILiat[i].m_index);
            }
        }
        UIRefresh();
    }

    public void UIDestroy(long _index)
    {
        for (int i = 0; i < m_UILiat.Count; i++)
        {
            if (m_UILiat[i].m_index == _index)
            {
                if (m_UILiat[i].m_ExistionUI != null)
                    setSelectUI(m_UILiat[i].m_ExistionUI);

                GameObject.Destroy(m_UILiat[i].gameObject);
                m_UILiat.RemoveAt(i);
            }
        }
    }

    public void UIDestroy(eUI_NAME _index)
    {
        for (int i = 0; i < m_UILiat.Count; i++)
        {
            if (m_UILiat[i].m_enum == _index)
            {
                if (m_UILiat[i].m_ExistionUI != null)
                    setSelectUI(m_UILiat[i].m_ExistionUI);

                GameObject.Destroy(m_UILiat[i].gameObject);
                m_UILiat.RemoveAt(i);
            }
        }
    }

    public void UIAttachDestroy<T>()
    {
        for (int i = 0; i < m_UIAttach.Count; i++)
        {
            if (m_UIAttach[i] == null)
                continue;

            if (!m_UIAttach[i].GetComponent<cFollowUI>())
                continue;

            if (m_UIAttach[i].GetComponent<cFollowUI>() is T)
            {
                GameObject.Destroy(m_UIAttach[i].gameObject);
            }
        }
    }

    public void UIDestroy()
    {
        for (int i = 0; i < m_UILiat.Count; i++)
        {
            GameObject.Destroy(m_UILiat[i].gameObject);
        }

        for (int i = 0; i < m_UIAttach.Count; i++)
        {
            GameObject.Destroy(m_UIAttach[i].gameObject);
        }

        for (int i = 0; i < mPopupList.Count; ++i)
        {
            if(mPopupList[i] != null)
                GameObject.Destroy(mPopupList[i].gameObject);
        }
        mPopupList.Clear();
        m_UILiat.Clear();
        mUIBack.gameObject.SetActive(false);
    }

    void setSelectUI(UIBase _selectUI, bool _isBack = false)
    {
        m_SelectIdx = _selectUI.m_index;
        ButtonEvent.g_UIIndex = _selectUI.m_index;
        m_SelectIdx = _selectUI.m_index;
        m_SelectUI = _selectUI;
        settingBack(_selectUI, _isBack);
    }

    public void UIRefresh()
    {
        for (int i = 0; i < m_UILiat.Count; i++)
        {
            m_UILiat[i].UIRefresh(m_SelectIdx);
        }
    }



    public GameObject CreateUI(string _name)
    {
        GameObject obj = null;
        obj = cDataManager.InstantiateGo(_name);
        Transform t = obj.transform;
        obj.transform.parent = m_2DRoot.transform;
        t.localPosition = Vector3.zero;
        t.localRotation = Quaternion.identity;
        t.localScale = Vector3.one;
        obj.layer = m_2DRoot.gameObject.layer;

        return obj;
    }

    public GameObject CreatePopupUI(string _name, int _add = 0)
    {
        GameObject obj = null;
        obj = cDataManager.InstantiateGo(_name);

        mPopupList.Add(obj.GetComponent<UIBase>());

        Transform t = obj.transform;
        obj.transform.parent = m_2DRoot.transform;
        t.localPosition = Vector3.zero;
        t.localRotation = Quaternion.identity;
        t.localScale = Vector3.one;
        obj.layer = m_2DRoot.gameObject.layer;

        UIPanel panel = obj.GetComponent<UIPanel>();

        panel.renderQueue = UIPanel.RenderQueue.StartAt;
        panel.startingRenderQueue = m_renderq + _add;
        panel.depth = (int)m_uiIndex + 3 + _add;

        return obj;
    }

    public GameObject CreateItemUI(string _name, string objName)
    {

        GameObject obj = null;
        obj = cDataManager.InstantiateGo(_name);
        Transform t = obj.transform;
        obj.transform.parent = m_2DRoot.transform;
        t.localPosition = Vector3.zero;
        t.localRotation = Quaternion.identity;
        t.localScale = Vector3.one;
        obj.name = string.Format("{0}_{1}", objName, m_uiItemIndex);
        obj.layer = m_2DRoot.gameObject.layer;

        UIPanel panel = obj.GetComponent<UIPanel>();
        panel.renderQueue = UIPanel.RenderQueue.StartAt;
        panel.startingRenderQueue = m_ItemRenderq;
        panel.depth = (int)m_uiItemIndex + 3;
        m_ItemRenderq += 30;
        m_uiItemIndex += 3;

        m_UIAttach.Add(obj);

        if (m_ItemRenderq >= 2000)
            m_ItemRenderq = 1000;
        return obj;
    }

    public void SetHide(eUI_NAME _uiIndex, bool isHide)
    {
        UIBase obj = GetUI(_uiIndex);
        obj.gameObject.SetActive(!isHide);
    }

    public void SetHideAttach(GameObject go, bool isHide)
    {
        for (int i = 0; i < m_UIAttach.Count; ++i)
        {
            if (m_UIAttach[i] != null)
                if (m_UIAttach[i] == go)
                {
                    m_UIAttach[i].gameObject.SetActive(!isHide);
                    return;
                }
        }
    }
    public void SetHideAttachAll(bool isHide)
    {
        for (int i = 0; i < m_UIAttach.Count; ++i)
        {
            if (m_UIAttach[i] != null)
                m_UIAttach[i].gameObject.SetActive(!isHide);
        }
    }

    void settingBack(UIBase _parentUI, bool _isBack = false)
    {
        if (_parentUI == null)
        {
            mUIBack.gameObject.SetActive(false);
            return;
        }

        if (_isBack == false)
        {
            mUIBack.gameObject.SetActive(false);
        }
        else
        {
            mUIBack.gameObject.SetActive(true);
            UIPanel panel = _parentUI.GetComponent<UIPanel>();
            mUIBack.startingRenderQueue = panel.startingRenderQueue - 2;
            mUIBack.depth = panel.depth - 1;
        }
    }

    public void ChangePanel(eUI_NAME forwardNeame, eUI_NAME backNeame)
    {
        UIPanel panel = GetUIGameObject(forwardNeame).GetComponent<UIPanel>();
        UIPanel back = GetUIGameObject(backNeame).GetComponent<UIPanel>();
        int depth = back.depth;
        int startingRenderQueue = back.startingRenderQueue;
        back.depth = panel.depth;
        back.startingRenderQueue = panel.startingRenderQueue;
        panel.depth = depth;
        panel.startingRenderQueue = startingRenderQueue;
        back.GetComponent<UIBase>().Start();
        panel.GetComponent<UIBase>().Start();
        settingBack(back.GetComponent<UIBase>());
    }

    public void BackKeyProcess(eBackKeyType _type)
    {
        if (Input.GetMouseButton(0))
            return;

        switch (_type)
        {
            case eBackKeyType.INTROLOGIN:
                if (mUIExiptPopup != null)
                {
                    GameObject.Destroy(mUIExiptPopup);
                    return;
                }

                mUIExiptPopup = cUIUtility.Instance.CreateUIExit();
                mUIExiptPopup.transform.localPosition = Vector3.back * 600;
                break;
            case eBackKeyType.LOBBY:

                if (HomeScene.GetCurStateKey() == eHOME_SCENE_STATE.LOBBY)
                {
                    if (mUIExiptPopup != null)
                    {
                        GameObject.Destroy(mUIExiptPopup);
                        return;
                    }
                      
                    mUIExiptPopup = cUIUtility.Instance.CreateUIExit();
                    mUIExiptPopup.transform.localPosition = Vector3.back * 600;
                    return;
                }

                cHistoryUIFrameworkManager.Instance.UIHistoryPrev();
                break;
            case eBackKeyType.INGAME:
                cUIUtility.Instance.ButtonClickPause();
                break;
            case eBackKeyType.SELECT:
                break;
        }
    }
}