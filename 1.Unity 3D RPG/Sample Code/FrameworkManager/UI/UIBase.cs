using UnityEngine;
using System.Collections;

public class UIBase : MonoBehaviour
{
    public UIPanel[] m_childPanel;
    public long m_index;
    public long m_IdxBefore;
    public eUI_NAME m_enum;
    public int m_renderq;
    public UIBase m_ExistionUI = null;
    public virtual void UIRefresh(long _selectIdx) { }

    public virtual void Awake()
    {
       /* UIPanel basePanel = gameObject.GetComponent<UIPanel>();
        for (int i = 0; i < m_childPanel.Length; i++)
        {
            int depth = basePanel.depth + 2;
            int renderq = basePanel.startingRenderQueue + 2;
            m_childPanel[i].renderQueue = UIPanel.RenderQueue.StartAt;
            m_childPanel[i].depth = depth;
            m_childPanel[i].startingRenderQueue = renderq;
            
            depth++;
            renderq++;
        }
        */
    }

    public virtual void Start()
    {
        UIPanel basePanel = gameObject.GetComponent<UIPanel>();
        for (int i = 0; i < m_childPanel.Length; i++)
        {
            int depth = basePanel.depth + 2;
            int renderq = basePanel.startingRenderQueue + 2;
            m_childPanel[i].renderQueue = UIPanel.RenderQueue.StartAt;
            m_childPanel[i].depth = depth;
            m_childPanel[i].startingRenderQueue = renderq;

            depth++;
            renderq++;
        }
    }
}
