using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class cUITapBar : MonoBehaviour {


	public UIButton[] m_TapBar;
	public Dictionary<int, bool> m_IsLock;
	public int m_startIndex = -1;
	public int m_UseTapCount = 4;
	ButtonEvent.BtnDelegate m_mainDelegate = null;
	public bool m_isEventBlock = false;

	// Use this for initialization
	void Start () {

		HideTap();

	}

	public void InitDictionary()
	{
		m_IsLock = new Dictionary<int, bool> ();
		for (int i = 0; i < m_TapBar.Length; i++) 
		{
			ActiveLock (i, false);
		}
	}
	
	void BtnDelegates (ButtonEvent.eButtonEventType _eventType, object _data)
	{

		if(ButtonEvent.eButtonEventType.ON_CLICK != _eventType)
			return;

		if (m_isEventBlock == true)
			return;

		int objCount = (int)_data;
		if (m_IsLock != null) {
			if (m_IsLock [objCount] == true) {
					m_mainDelegate (_eventType, _data);
					return;
			}
		}

		if(m_startIndex == objCount)
		{
			return;
		}
		else
		{
			m_startIndex = objCount;
		}

		SetTapPos(objCount);

		if(m_mainDelegate != null)
		{

			m_mainDelegate(_eventType, _data);
		}
		/*
		switch(objCount)
		{
		case 0:
			m_TapBar[objCount].transform.Find("Label").GetComponent<UILabel>().enabled = false;
			m_TapBar[objCount].transform.Find("Sprite_Click").GetComponent<UISprite>().enabled = true;
			m_TapBar[objCount].transform.Find("Sprite_Click/Label").GetComponent<UILabel>().enabled = true;
			break;
		case 1:
			m_TapBar[objCount].transform.Find("Label").GetComponent<UILabel>().enabled = false;
			m_TapBar[objCount].transform.Find("Sprite_Click").GetComponent<UISprite>().enabled = true;
			m_TapBar[objCount].transform.Find("Sprite_Click/Label").GetComponent<UILabel>().enabled = true;
			break;
		case 2:
			m_TapBar[objCount].transform.Find("Label").GetComponent<UILabel>().enabled = false;
			m_TapBar[objCount].transform.Find("Sprite_Click").GetComponent<UISprite>().enabled = true;
			m_TapBar[objCount].transform.Find("Sprite_Click/Label").GetComponent<UILabel>().enabled = true;
			break;
		case 3:
			m_TapBar[objCount].transform.Find("Label").GetComponent<UILabel>().enabled = false;
			m_TapBar[objCount].transform.Find("Sprite_Click").GetComponent<UISprite>().enabled = true;
			m_TapBar[objCount].transform.Find("Sprite_Click/Label").GetComponent<UILabel>().enabled = true;
			break;
		}
		*/
	}

	public void InitTap(int _index)
	{
		m_startIndex = _index;
		SetTapPos(_index);
		m_mainDelegate(ButtonEvent.eButtonEventType.ON_CLICK, _index);
	}

	public void InitDelegates(ButtonEvent.BtnDelegate _delegate, int[] _index = null)
	{
		m_mainDelegate = _delegate;
		Tapstring(_index);
	}

	public void InitDelegates(ButtonEvent.BtnDelegate _delegate, string[] _text)
	{
		m_mainDelegate = _delegate;
		Tapstring(_text);
	}

	void Tapstring(int[] _index)
	{
		for(int i = 0; i < m_UseTapCount; i++)
		{

            if (m_TapBar[i] != null)
            {
                m_TapBar[i].transform.Find("Label").GetComponent<UILabel>().text = _index[i].ToString();
                m_TapBar[i].transform.Find("Sprite_Click/Label").GetComponent<UILabel>().text = _index[i].ToString();
            }
            else
                Debug.Log("Tapstring ==  " + i);
            

		}
	}

	void Tapstring(string[] _text)
	{
		for(int i = 0; i < m_UseTapCount; i++)
		{
			m_TapBar[i].transform.Find("Label").GetComponent<UILabel>().text = _text[i];
			m_TapBar[i].transform.Find("Sprite_Click/Label").GetComponent<UILabel>().text = _text[i];	
		}
	}

	public void SetTapPos(int _objCount)
	{
		for(int i = 0; i < m_UseTapCount; i++)
		{
			TapClickEnabled(i, _objCount == i? true : false);
		}
	}

	void TapClickEnabled(int _objCount ,bool _isClick)
	{
		LabelEnabled(m_TapBar[_objCount].transform.Find("Label"), _isClick == true ? false : true);
		LabelEnabled(m_TapBar[_objCount].transform.Find("Sprite_Click/Label"), _isClick == true ? true : false);
     	SpriteEnabled(m_TapBar[_objCount].transform.Find("Sprite_Top"), _isClick == true ? false : true);
		SpriteEnabled(m_TapBar[_objCount].transform.Find("Sprite_Click"), _isClick == true ? true : false);        
		SpriteEnabled(m_TapBar[_objCount].transform.Find("Sprite_Click/Sprite_Top"), _isClick == true ? true : false);
		SpriteEnabled(m_TapBar[_objCount].transform.Find("Sprite_Click/Sprite_Tile"), _isClick == true ? true : false);
		SpriteEnabled(m_TapBar[_objCount].transform.Find("Sprite_Click/Sprite_Arrow"), _isClick == true ? true : false);
	}

	public void ActiveLock(int _objCount ,bool _isLock)
	{
		m_TapBar[_objCount].transform.Find("Sprite_Lock").gameObject.SetActive(_isLock == true ? true : false);
		IsLock (_objCount, _isLock);
	}

	void LabelEnabled(Transform _labelObj, bool _isEnabled)
	{
		if(_labelObj != null)
		{
			_labelObj.GetComponent<UILabel>().enabled = _isEnabled;
		}
	}

	void SpriteEnabled(Transform _sprite, bool _isEnabled)
	{
		if(_sprite != null)
		{
			_sprite.GetComponent<UISprite>().enabled = _isEnabled;
		}
	}

	void HideTap()
	{
		for(int i = 0; i < m_TapBar.Length; i++)
		{
			if(m_UseTapCount <= i)
			{
				m_TapBar[i].gameObject.SetActive(false);
			} else {
				m_TapBar[i].gameObject.SetActive(true);
				m_TapBar[i].GetComponent<ButtonEvent>().Init(BtnDelegates, -1, i);
			}
		}
	}

	void IsLock(int _key, bool _isLock)
	{
		if (m_IsLock.ContainsKey (_key)) 
		{
			m_IsLock [_key] = _isLock;
		} else 
		{
			m_IsLock.Add(_key, _isLock);
		}
	}

	public bool GetIsLock(int _key)
	{
		if (m_IsLock.ContainsKey (_key)) 
		{
			return m_IsLock [_key];
		}
		return false;
	}

	public void HideTap(int _Count)
	{
		m_UseTapCount = _Count;
		HideTap();
	}

	public void CardDepthBackUp()
	{

	}
}
