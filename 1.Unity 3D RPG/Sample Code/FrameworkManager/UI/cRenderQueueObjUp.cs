using UnityEngine;
using System.Collections;

public class cRenderQueueObjUp : MonoBehaviour {
	[System.Serializable]
	public struct RendererEffect
	{
		public UIWidget drawCall; 
		public Renderer[] m_rendererFront;
		public Renderer[] m_rendererBack;
		public bool m_isOrderRRenderQueue;
	}

//	[System.NonSerialized]
	public RendererEffect[] m_effect;

	void OnDestroy()
	{
		for (int i =0; i < m_effect.Length; i++) {
			for (int k =0; k < this.m_effect[i].m_rendererFront.Length; k++) {
				this.m_effect [i].m_rendererFront [k] = null;

		
			}
			for(int l =0; l < this.m_effect[i].m_rendererBack.Length; l++)
			{
				this.m_effect[i].m_rendererBack[l] = null;

			}
			this.m_effect[i].drawCall = null;
		}
		this.m_effect = null;
	}

	void Update () {
		int orderFront = 1;
		int orderBack = 1;
		for(int i =0; i < m_effect.Length; i++)
		{
            if (m_effect[i].drawCall == null || m_effect[i].drawCall.drawCall == null)
				return;

			for(int k =0; k < this.m_effect[i].m_rendererFront.Length; k++)
			{
                if (this.m_effect[i].m_rendererFront[k] == null)
                    continue;
                    
				this.m_effect[i].m_rendererFront[k].GetComponent<Renderer>().material.renderQueue = this.m_effect[i].drawCall.drawCall.renderQueue + orderFront;

				if(this.m_effect[i].m_isOrderRRenderQueue == true)
				{
					orderFront ++;
				}
				else
				{
					orderFront = 1;
				}
			}

			for(int l =0; l < this.m_effect[i].m_rendererBack.Length; l++)
			{
				this.m_effect[i].m_rendererBack[l].GetComponent<Renderer>().material.renderQueue = this.m_effect[i].drawCall.drawCall.renderQueue - orderBack;
				if(this.m_effect[i].m_isOrderRRenderQueue == true)
				{
					orderBack ++;
				}
				else
				{
					orderBack = 1;
				}
			}
		}
	}

    void OnEnable()
    {
        for (int i = 0; i < m_effect.Length; i++)
        {
            for (int k = 0; k < this.m_effect[i].m_rendererFront.Length; k++)
            {
                ParticleSystem particl = this.m_effect[i].m_rendererFront[k].gameObject.GetComponent<ParticleSystem>();
                if (particl != null)
                    particl.Play();

            }
            for (int l = 0; l < this.m_effect[i].m_rendererBack.Length; l++)
            {
                ParticleSystem particl = this.m_effect[i].m_rendererBack[l].gameObject.GetComponent<ParticleSystem>();
                if (particl != null)
                    particl.Play();
            }
        }
    }
}
