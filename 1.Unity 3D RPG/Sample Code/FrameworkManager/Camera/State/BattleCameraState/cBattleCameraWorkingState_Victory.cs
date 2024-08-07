using UnityEngine;
using System.Collections;

public class cBattleCameraWorkingState_Victory : cState<cBattleCameraController>
{
    //============================================================================

    public cBattleCameraWorkingState_Victory(cBattleCameraController _parent) : base(_parent)
    {
    }

    public override void OnEnter()
    {
        mParent.RefreshCurCameraDist();
        mParent.SetCameraTargetDist(ClientDefine.BATTLE_CAMERA_DIST_VICTORY);
        mParent.SetCameraVerticalRotation(ClientDefine.BATTLE_CAMERA_VERTICAL_ANGLE_VICTORY);
        mParent.SetCameraLookAtOffsetY(0.0f);

        cPlayer leaderPlayer = BattleScene.BattleSceneManager.PlayerManager.GetLeaderPlayer();
        mParent.SetFollowTargetTransform(leaderPlayer.GetDummyTransform(eDummyType.Middle));
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
    }
}
