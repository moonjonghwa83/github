using UnityEngine;
using System.Collections;

public class cFollowUI : UIBase {

    public Transform m_target;
    Transform m_thisTrans;
    Vector3 m_pos;
    Vector3 m_targetpos;
    bool m_isTransPos = false;
    void Start()
    {
        
    }

    public void Init(Transform _target)
    {
        m_target = _target;
        m_thisTrans = this.transform;
    }

    public void Init(Vector3 _targetPos)
    {
        m_isTransPos = true;
        m_targetpos = _targetPos;
        m_thisTrans = this.transform;
    }

    virtual protected void Update()
    {
        if (m_target == null)
            return;
        if (m_target != null && cCameraFrameworkManager.Instance.UICamera != null)
        {
            if (cCameraFrameworkManager.Instance.MainCamera != null)
            {
                m_pos = cCameraFrameworkManager.Instance.MainCamera.WorldToScreenPoint(m_isTransPos == true? m_targetpos : m_target.position);
                m_pos.z = 0;
                m_pos = cCameraFrameworkManager.Instance.UICamera.ScreenToWorldPoint(m_pos);
                m_thisTrans.position = m_pos;
            }
        }
        else
        {
            if (cCameraFrameworkManager.Instance.MainCamera != null)
                this.transform.LookAt(cCameraFrameworkManager.Instance.UICamera.transform);
        }
    }
}
