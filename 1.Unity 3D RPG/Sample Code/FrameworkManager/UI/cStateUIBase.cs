using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class cStateUIBase : UIBase
{

    protected void InitBackButton(string _title = "")
    {
        Transform backButtonTransform = this.transform.Find("Button_Back");
        if (backButtonTransform != null)
        {
            backButtonTransform.GetComponent<ButtonEvent>().Init(BackButtonDelegate, -1);
            BoxCollider boxc = backButtonTransform.GetComponent<BoxCollider>();
            if (boxc != null)
            {
                boxc.center = new Vector3(45, 0, 0);
                boxc.size = new Vector3(160, 61, 0);
            }
        }
    }

    protected virtual void BackButtonDelegate(ButtonEvent.eButtonEventType _eventType, object _data)
    {
        if (_eventType != ButtonEvent.eButtonEventType.ON_CLICK)
            return;

        //HomeScene.ChangeState(eHOME_SCENE_STATE.LOBBY);
        cHistoryUIFrameworkManager.Instance.UIHistoryPrev();
    }
}
