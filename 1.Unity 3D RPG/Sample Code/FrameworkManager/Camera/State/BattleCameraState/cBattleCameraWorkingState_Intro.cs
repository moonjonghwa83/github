using UnityEngine;
using System.Collections;

public class cBattleCameraWorkingState_Intro : cState<cBattleCameraController>
{
    private float mTime = 0.0f;
    private bool mIsDelay = false;

    //============================================================================

    public cBattleCameraWorkingState_Intro(cBattleCameraController _parent) : base(_parent)
    {
    }

    public override void OnEnter()
    {
        mIsDelay = false;
        mTime = 0.0f;

        if (mParent.IsCameraPathAnimator(cBattleCameraController.eBATTLE_CAMERA_PATH_ANIMATOR_TYPE.INTRO) == false)
        {
            mParent.ChangeState(eBATTLE_CAMERA_WORKING_STATE.NONE);
            return;
        }

        mParent.StartCameraPathAnimation(cBattleCameraController.eBATTLE_CAMERA_PATH_ANIMATOR_TYPE.INTRO, CameraPathEndEvent);
    }

    public override void OnExit()
    {
        
    }

    public void CameraPathEndEvent(string _eventName)
    {
        mIsDelay = true;
        mParent.StopCameraPathAnimation(cBattleCameraController.eBATTLE_CAMERA_PATH_ANIMATOR_TYPE.INTRO, CameraPathEndEvent);
    }

    public override void OnUpdate()
    {
        if (mIsDelay == false)
            return;

        mTime += cGameTime.DeltaTime;

        if(mTime > 1.0f)
            mParent.ChangeState(eBATTLE_CAMERA_WORKING_STATE.FOLLOW);
    }
}
