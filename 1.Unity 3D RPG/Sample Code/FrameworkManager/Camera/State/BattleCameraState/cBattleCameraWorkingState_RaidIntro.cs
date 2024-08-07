using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class cBattleCameraWorkingState_RaidIntro : cState<cBattleCameraController>
{
    private float mTime = 0.0f;
    private bool mIsDelay = false;
    private bool mIsPause = false;
    protected float mAnimationTime = 0.0f;
    cMonster bossMonster = null;

    //============================================================================

    public cBattleCameraWorkingState_RaidIntro(cBattleCameraController _parent) : base(_parent)
    {
    }

    public override void OnEnter()
    {
        mIsDelay = false;
        mIsPause = false;
        mTime = 0.0f;

        bossMonster = BattleScene.BattleSceneManager.MonsterManager.GetBossMonster();

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
        if (_eventName == "EndPath")
        {
            mIsDelay = true;
            mParent.StopCameraPathAnimation(cBattleCameraController.eBATTLE_CAMERA_PATH_ANIMATOR_TYPE.INTRO, CameraPathEndEvent);
            cMonster bossMonster = BattleScene.BattleSceneManager.MonsterManager.GetBossMonster();

            bossMonster.PlayAnimation("idle02", false);
            mAnimationTime = bossMonster.GetAnimationLenth("idle02");

            cUIUtility.Instance.CreateUIRadeBoss();
        }
    }

    public override void OnUpdate()
    {
        if (mIsDelay == false)
            return;

        mTime += cGameTime.DeltaTime;

        if (mTime >mAnimationTime)
        {
            bossMonster.PlayAnimation("battle_idle", true);
        }


        if (mTime > 3.0f)
        {
            cUIUtility.Instance.EndUIRadeBoss();

            mParent.ChangeState(eBATTLE_CAMERA_WORKING_STATE.FOLLOW);

        }
    }
}
