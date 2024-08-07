using UnityEngine;
using System.Collections;

public class cBattleCameraWorkingState_ActiveSkill : cState<cBattleCameraController>
{
    private Vector3 mReturnCameraPos = Vector3.zero;
    private float mReturnCameraDist = 0.0f;
    private float mDist = 0.0f;

    //============================================================================

    public cBattleCameraWorkingState_ActiveSkill(cBattleCameraController _parent) : base(_parent)
    {
    }

    public override void OnEnter()
    {
        mParent.RefreshCurCameraDist();
        mParent.SetCameraTargetDist(ClientDefine.BATTLE_CAMERA_DIST_ACTIVE_SKILL);
        mParent.SetCameraVerticalRotation(ClientDefine.BATTLE_CAMERA_VERTICAL_ANGLE_ACTIVE_SKILL);
        mParent.SetDistSpeed(ClientDefine.BATTLE_CAMERA_DIST_SPEED_ACTIVESKILL);
    }

    public override void OnExit()
    {
        mParent.SetDistSpeed(ClientDefine.BATTLE_CAMERA_DIST_SPEED_DEFAULT);
    }

    public override void OnUpdate()
    {
        
    }
}
