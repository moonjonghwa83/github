using UnityEngine;
using System.Collections;

public class cUIAnimationEventController : MonoBehaviour {
	public delegate void AnimationDelegate (object _data);
	public object m_endData = null;
	public object m_middleData = null;
	public object m_middleData1 = null;
	public object m_startData = null;
	public AnimationDelegate m_EndDelehate = null;
	public AnimationDelegate m_MiddleDelehate = null;
	public AnimationDelegate m_MiddleDelehate1 = null;
	public AnimationDelegate m_StartDelehate = null;

	public void EndCallBack()
	{
		if (this.m_EndDelehate != null)
			this.m_EndDelehate (m_endData);
	}

	public void MiddleCallBack()
	{
		if (this.m_MiddleDelehate != null)
			this.m_MiddleDelehate (m_middleData);
	}

	public void Middle1CallBack()
	{
		if (this.m_MiddleDelehate1 != null)
			this.m_MiddleDelehate1 (m_middleData1);
	}

	public void StartCallBack()
	{
		if (this.m_StartDelehate != null) 
		{
			this.m_StartDelehate(m_startData);
		}
	}

	
}
