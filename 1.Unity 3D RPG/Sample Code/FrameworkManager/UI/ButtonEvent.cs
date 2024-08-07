using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ButtonEvent : MonoBehaviour {
	public static long g_UIIndex = 0;
	public static List<ButtonEvent> g_first = new List<ButtonEvent>();
	public delegate void BtnDelegate (eButtonEventType _eventType, object _data);
	public enum eButtonEventType
	{
		ON_SUBMIT = 0,
		ON_CLICK,
		ON_DOUBLECLICK,
		ON_HOVER_OVER,
		ON_HOVER_OUT,
		ON_PRESS,
		ON_RELEASE,
		ON_SELECT,
		ON_DESELECT,
		ON_DRAG,
	};

	public object m_Data = 0;
    //private float m_BtnClickTime = 0.0f;
	long m_ButtonUIIndex = -1;
	int m_firstindex = -1;
	BtnDelegate m_btnDelegate;
	public BtnDelegate BtnDelegates
	{
		get{return m_btnDelegate;}
		set{m_btnDelegate = value;}
	}

    public void ButtonClick()
    {
        m_btnDelegate(eButtonEventType.ON_CLICK, m_Data);
    }

	void Start () {

		UIEventListener.Get(gameObject).onSubmit = delegate(GameObject go) 
		{
			BtnCallBack(go, eButtonEventType.ON_SUBMIT);
		};

		UIEventListener.Get(gameObject).onClick = delegate(GameObject go) 
		{
            //cSoundPlayer.PlaySFX(ClientDefine.UI_CLICK_SFX_NAME, true);
			BtnCallBack(go, eButtonEventType.ON_CLICK);		
        };
		
		UIEventListener.Get(gameObject).onDoubleClick = delegate(GameObject go) 
		{
			BtnCallBack(go, eButtonEventType.ON_DOUBLECLICK);
		};
		
		UIEventListener.Get(gameObject).onHover = delegate(GameObject go, bool isOver) 
		{
			if (isOver) BtnCallBack(go, eButtonEventType.ON_HOVER_OVER);
			else BtnCallBack(go, eButtonEventType.ON_HOVER_OUT);
		};
		
		UIEventListener.Get(gameObject).onPress = delegate(GameObject go, bool isPressed) 
		{
			if (isPressed) BtnCallBack(go, eButtonEventType		.ON_PRESS);
			else BtnCallBack(go, eButtonEventType.ON_RELEASE);
		};
		
		UIEventListener.Get(gameObject).onSelect = delegate(GameObject go, bool selected) 
		{
			if (selected) BtnCallBack(go, eButtonEventType.ON_SELECT);
			else BtnCallBack(go, eButtonEventType.ON_DESELECT);
		};

		UIEventListener.Get(gameObject).onDrag = delegate(GameObject go, Vector2 selected) 
		{
			BtnCallBack(go, eButtonEventType.ON_DRAG);
		};
	}

	public void Init (BtnDelegate _btnDelegate, long _buttonUIIndex = -1, object _data = null, bool _first = false)
	{
		m_btnDelegate = _btnDelegate;
		m_Data = _data;
		m_ButtonUIIndex = _buttonUIIndex;
		if (_first == true) {
			FirstEvent(this);
		}
	}

	public static void FirstEvent (ButtonEvent _event)
	{
		_event.m_firstindex = g_first.Count;
		g_first.Add (_event);
	}
	
	void BtnCallBack(GameObject go, eButtonEventType _eventType)
	{
        // 페이드 끝나기전 UI 클릭 금지
        if (cScreenFaderManager.Instance.GetFadeState() != ScreenFaderComponents.Enumerators.FadeState.OutEnd)
            return;

        if (m_btnDelegate != null)
        {
            if (_eventType == eButtonEventType.ON_CLICK)
			{
                if (g_first.Count > 0)
				{
					if(g_first[0].m_firstindex != m_firstindex)
						return;
					else
						g_first.RemoveAt(0);
				}
			}
			if(m_ButtonUIIndex == ButtonEvent.g_UIIndex || m_ButtonUIIndex == -1)
            	m_btnDelegate(_eventType, m_Data);
        }
	}
}