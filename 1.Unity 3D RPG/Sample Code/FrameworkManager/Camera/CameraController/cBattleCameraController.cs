using UnityEngine;
using System.Collections;
using System;

public class cBattleCameraController : cCameraControllerBase
{
    public enum eBATTLE_CAMERA_PATH_ANIMATOR_TYPE
    {
        INTRO = 0,
        FOLLOW,
        MAX,
    }

    private cBattleCameraWorkingStateMachine mBattleCameraWorkingStateMachine = null;
    private cCameraPathAnimatorInfo[] mCameraPathAnimatorArray = null;

    private Vector3 mCurCameraPosition = Vector3.zero;
    private Vector3 mCurCameraLookAtPosition = Vector3.zero;
    private Vector3 mCurCameraRotation = Vector3.zero;

    private float mCurDist = 0.0f;
    private float mTargetDist = 0.0f;
    private float mLookAtOffsetY = 0.0f;

    private float mPositionSpeed = ClientDefine.BATTLE_CAMERA_POSITION_SPEED_DEFAULT;
    private float mLookAtSpeed = ClientDefine.BATTLE_CAMERA_LOOKAT_SPEED_DEFAULT;
    private float mDistSpeed = ClientDefine.BATTLE_CAMERA_DIST_SPEED_DEFAULT;

    //==========================================================================================

    public cBattleCameraController(cCameraFrameworkManager _cameraManager, Camera _mainCamera) : base(_cameraManager, _mainCamera)
    {
        if (mBattleCameraWorkingStateMachine == null)
            mBattleCameraWorkingStateMachine = new cBattleCameraWorkingStateMachine(this);

        if (mCameraPathAnimatorArray == null)
            mCameraPathAnimatorArray = new cCameraPathAnimatorInfo[(int)eBATTLE_CAMERA_PATH_ANIMATOR_TYPE.MAX];
    }

    public override void OnInit()
    {
        mCurCameraPosition = Vector3.zero;
        mCurCameraLookAtPosition = Vector3.zero;
        mCurCameraRotation = Vector3.zero;

        mCurDist = 0.0f;
        mTargetDist = 0.0f;
        mLookAtOffsetY = 0.0f;

        mPositionSpeed = ClientDefine.BATTLE_CAMERA_POSITION_SPEED_DEFAULT;
        mDistSpeed = ClientDefine.BATTLE_CAMERA_DIST_SPEED_DEFAULT;

        this.ChangeState(eBATTLE_CAMERA_WORKING_STATE.NONE);
    }

    public override void OnDestroy()
    {
        if (mBattleCameraWorkingStateMachine != null)
        {
            mBattleCameraWorkingStateMachine.Destroy();
            mBattleCameraWorkingStateMachine = null;
        }        

        this.RemoveAllCameraPathAnimator();
        mCameraPathAnimatorArray = null;

        mMainCamera = null;
    }

    public override void OnUpdate()
    {
        if (mBattleCameraWorkingStateMachine != null)
            mBattleCameraWorkingStateMachine.Update();
    }

    public override void OnLateUpdate()
    {
        this.updateCamera();
    }

    public void ChangeState(eBATTLE_CAMERA_WORKING_STATE _state, Action _endEventFunc = null)
    {
        mBattleCameraWorkingStateMachine.ChangeState(_state, _endEventFunc);
    }

    public eBATTLE_CAMERA_WORKING_STATE GetCurStateKey()
    {
        if (mBattleCameraWorkingStateMachine == null)
            return eBATTLE_CAMERA_WORKING_STATE.NONE;

        return mBattleCameraWorkingStateMachine.GetCurStateKey();
    }

    public void InitMovePosition(Transform _targetTransform)
    {
        this.SetFollowTargetTransform(_targetTransform);

        CameraPath cameraPath = mCameraPathAnimatorArray[(int)eBATTLE_CAMERA_PATH_ANIMATOR_TYPE.FOLLOW].GetCameraPath();
        Transform targetTrans = this.GetFollowTargetTransform();
        float nearestPercent = cameraPath.GetNearestPoint(targetTrans.position, false, 5);
        Vector3 pathPos = cameraPath.GetPathPosition(nearestPercent, false);

        mCurCameraPosition = this.getCameraMovePos(targetTrans, pathPos, eBATTLE_CAMERA_WORKING_STATE.FOLLOW);
        mCurCameraLookAtPosition = targetTrans.position;

        mCurDist = 0.0f;
        mTargetDist = 0.0f;
        mLookAtOffsetY = 0.0f;

        mPositionSpeed = ClientDefine.BATTLE_CAMERA_POSITION_SPEED_DEFAULT;
        mLookAtSpeed = ClientDefine.BATTLE_CAMERA_LOOKAT_SPEED_DEFAULT;
        mDistSpeed = ClientDefine.BATTLE_CAMERA_DIST_SPEED_DEFAULT;
}

    public void InitCameraPathAnimator(eBATTLE_CAMERA_PATH_ANIMATOR_TYPE _type, GameObject _caemraPathInfoObj)
    {
        if (mCameraPathAnimatorArray[(int)_type] == null)
            mCameraPathAnimatorArray[(int)_type] = new cCameraPathAnimatorInfo(mCameraParentTrans);        

        mCameraPathAnimatorArray[(int)_type].Init(_caemraPathInfoObj);
    }

    public void RemoveAllCameraPathAnimator()
    {
        if (mCameraPathAnimatorArray != null)
        {
            for (int i = 0; i < mCameraPathAnimatorArray.Length; ++i)
            {
                if (mCameraPathAnimatorArray[i] == null)
                    continue;

                mCameraPathAnimatorArray[i].Destroy();
                mCameraPathAnimatorArray[i] = null;
            }
        }
    }

    public void SetFollowTargetTransform(Transform _targetTransform)
    {
        if (mCameraPathAnimatorArray == null)
            return;

        if (mCameraPathAnimatorArray[(int)eBATTLE_CAMERA_PATH_ANIMATOR_TYPE.FOLLOW] == null)
            return;

        mCameraPathAnimatorArray[(int)eBATTLE_CAMERA_PATH_ANIMATOR_TYPE.FOLLOW].SetFollowTargetTransform(_targetTransform);        
    }

    public Transform GetFollowTargetTransform()
    {
        if (mCameraPathAnimatorArray == null)
            return null;

        if (mCameraPathAnimatorArray[(int)eBATTLE_CAMERA_PATH_ANIMATOR_TYPE.FOLLOW] == null)
            return null;

        return mCameraPathAnimatorArray[(int)eBATTLE_CAMERA_PATH_ANIMATOR_TYPE.FOLLOW].GetFollowTargetTransform();
    }

    public void StartCameraPathAnimation(eBATTLE_CAMERA_PATH_ANIMATOR_TYPE _type, CameraPathAnimator.AnimationCustomEventHandler _cameraPathEndDelegate)
    {
        if (mCameraPathAnimatorArray == null)
            return;

        if (mCameraPathAnimatorArray[(int)_type] == null)
            return;

        mCameraPathAnimatorArray[(int)_type].StartCameraPathAnimation(_cameraPathEndDelegate);
    }

    public void StopCameraPathAnimation(eBATTLE_CAMERA_PATH_ANIMATOR_TYPE _type, CameraPathAnimator.AnimationCustomEventHandler _cameraPathEndDelegate)
    {
        if (mCameraPathAnimatorArray == null)
            return;

        if (mCameraPathAnimatorArray[(int)_type] == null)
            return;

        mCameraPathAnimatorArray[(int)_type].StopCameraPathAnimation(_cameraPathEndDelegate);
    }

    public bool IsCameraPathAnimator(eBATTLE_CAMERA_PATH_ANIMATOR_TYPE _type)
    {
        if (mCameraPathAnimatorArray == null)
            return false;

        if (mCameraPathAnimatorArray[(int)_type] == null)
            return false;

        return mCameraPathAnimatorArray[(int)_type].IsCameraPathAnimator();
    }    

    public void SetCameraTargetDist(float _dist)
    {
        mTargetDist = _dist;
    }    

    public void RefreshCurCameraDist()
    {
        Transform targetTrans = this.GetFollowTargetTransform();
        float dist = Mathf.Sqrt( (mCurCameraPosition - targetTrans.position).sqrMagnitude );
        mCurDist = dist;
    }

    public void SetCameraVerticalRotation(float _angle)
    {
        mCurCameraRotation.x = 1.0f;
        mCurCameraRotation.y = Mathf.Tan(_angle * Mathf.Deg2Rad);
        mCurCameraRotation.z = 1.0f;        
        mCurCameraRotation.Normalize();        
    }    

    public void SetCameraLookAtOffsetY(float _offsetY)
    {
        mLookAtOffsetY = _offsetY;
    }

    private bool checkUpdate(eBATTLE_CAMERA_WORKING_STATE _state)
    {
        if (_state == eBATTLE_CAMERA_WORKING_STATE.INTRO || _state == eBATTLE_CAMERA_WORKING_STATE.NONE ||
            _state == eBATTLE_CAMERA_WORKING_STATE.RAID_INTRO)
            return false;

        return true;
    }

    protected void updateCamera()
    {
        if (mMainCamera == null)
            return;

        if (mMainCamera.enabled == false)
            return;

        eBATTLE_CAMERA_WORKING_STATE state = this.GetCurStateKey();

        if (this.checkUpdate(state) == false)
            return;

        Transform targetTrans = this.GetFollowTargetTransform();

        if (targetTrans == null)
            return;

        CameraPath cameraPath = mCameraPathAnimatorArray[(int)eBATTLE_CAMERA_PATH_ANIMATOR_TYPE.FOLLOW].GetCameraPath();

        if (cameraPath == null)
            return;

        float nearestPercent = cameraPath.GetNearestPoint(targetTrans.position, false, 5);
        Vector3 pathPos = cameraPath.GetPathPosition(nearestPercent, false);        

        Vector3 movePos = this.getCameraMovePos(targetTrans, pathPos, state);

        mCurCameraPosition = Vector3.Lerp(mCurCameraPosition, movePos, cGameTime.CameraDeltaTime * mPositionSpeed);
        mCameraParentTrans.position = mCurCameraPosition;

        mCurCameraLookAtPosition = Vector3.Lerp(mCurCameraLookAtPosition, (targetTrans.position + (Vector3.up * mLookAtOffsetY)) , cGameTime.CameraDeltaTime * mLookAtSpeed);
        mCameraParentTrans.LookAt(mCurCameraLookAtPosition, Vector3.up);
    }    

    private Vector3 getCameraMovePos(Transform _targetTrans, Vector3 _pathPos, eBATTLE_CAMERA_WORKING_STATE _state)
    {
        Vector3 movePos = Vector3.zero;

        if (_state == eBATTLE_CAMERA_WORKING_STATE.FOLLOW /*|| _state == eBATTLE_CAMERA_WORKING_STATE.RAID_START*/)
        {
            movePos = _pathPos;            
        }        
        else
        {
            mCurDist = Mathf.Lerp(mCurDist, mTargetDist, cGameTime.CameraDeltaTime * mDistSpeed);
            movePos = _targetTrans.position + (mCurCameraRotation * mCurDist);
        }        

        return movePos;
    }

    public void SetPositionSpeed(float _speed)
    {
        mPositionSpeed = _speed;
    }

    public void SetDistSpeed(float _speed)
    {
        mDistSpeed = _speed;
    }

    public void SetLookAtSpeed(float _speed)
    {
        mLookAtSpeed = _speed;
    }
}
