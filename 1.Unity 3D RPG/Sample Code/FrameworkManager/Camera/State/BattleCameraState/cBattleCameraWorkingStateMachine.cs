using UnityEngine;


public enum eBATTLE_CAMERA_WORKING_STATE
{
    NONE = 0,
    INTRO,
    FOLLOW,
    VICTORY,
    ACTIVE_SKILL,
    RAID_INTRO,
    RAID_START,
    MAX,
}

public class cBattleCameraWorkingStateMachine : cStateMachine<eBATTLE_CAMERA_WORKING_STATE, cBattleCameraController>
{
    public cBattleCameraWorkingStateMachine(cBattleCameraController _parent) : base(_parent)
    {
        if (mStateArray == null)
            mStateArray = new cState<cBattleCameraController>[(int)eBATTLE_CAMERA_WORKING_STATE.MAX];

        mStateArray[(int)eBATTLE_CAMERA_WORKING_STATE.NONE] = new cBattleCameraWorkingState_None(_parent);
        mStateArray[(int)eBATTLE_CAMERA_WORKING_STATE.INTRO] = new cBattleCameraWorkingState_Intro(_parent);
        mStateArray[(int)eBATTLE_CAMERA_WORKING_STATE.FOLLOW] = new cBattleCameraWorkingState_Follow(_parent);
        mStateArray[(int)eBATTLE_CAMERA_WORKING_STATE.VICTORY] = new cBattleCameraWorkingState_Victory(_parent);
        mStateArray[(int)eBATTLE_CAMERA_WORKING_STATE.ACTIVE_SKILL] = new cBattleCameraWorkingState_ActiveSkill(_parent);
        mStateArray[(int)eBATTLE_CAMERA_WORKING_STATE.RAID_INTRO] = new cBattleCameraWorkingState_RaidIntro(_parent);
        mStateArray[(int)eBATTLE_CAMERA_WORKING_STATE.RAID_START] = new cBattleCameraWorkingState_RaidStart(_parent);
    }
}
