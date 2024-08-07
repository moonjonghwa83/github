using UnityEngine;
using System.Collections;

public class cBattleCameraWorkingState_RaidStart : cState<cBattleCameraController>
{
    private float mTime = 0.0f;

    //============================================================================

    public cBattleCameraWorkingState_RaidStart(cBattleCameraController _parent) : base(_parent)
    {
    }

    public override void OnEnter()
    {        
        mTime = 0.0f;
    }

    public override void OnExit()
    {
        cMonster bossMonster = BattleScene.BattleSceneManager.MonsterManager.GetBossMonster();
        mParent.SetFollowTargetTransform(bossMonster.Transform);
        mParent.SetCameraLookAtOffsetY(ClientDefine.BATTLE_CAMERA_LOOKAT_OFFSET_Y_RAID);
        mParent.SetLookAtSpeed(ClientDefine.BATTLE_CAMERA_LOOKAT_SPEED_RAID);
    }    

    public override void OnUpdate()
    {
        mTime += cGameTime.DeltaTime;

        if (mTime > 6.0f)
            mParent.ChangeState(eBATTLE_CAMERA_WORKING_STATE.FOLLOW);
    }
}
