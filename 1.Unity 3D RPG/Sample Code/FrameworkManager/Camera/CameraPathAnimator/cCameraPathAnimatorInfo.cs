using UnityEngine;
using System.Collections;


public class cCameraPathAnimatorInfo
{
    private Transform mCameraTrans = null;
    private CameraPathAnimator mCameraPathAnimator = null;
    private CameraPath mCameraPath = null;    

    //=====================================================    
    
    public cCameraPathAnimatorInfo(Transform _cameraTrans)
    {
        mCameraTrans = _cameraTrans;
    }

    public virtual void Init(GameObject _cameraPathAnimatorObj)
    {
        if (_cameraPathAnimatorObj == null)
            return;

        _cameraPathAnimatorObj.SetActive(false);

        mCameraPathAnimator = _cameraPathAnimatorObj.GetComponent<CameraPathAnimator>();
        mCameraPath = _cameraPathAnimatorObj.GetComponent<CameraPath>();
        
        if(mCameraPathAnimator != null)
            mCameraPathAnimator.animationObject = mCameraTrans;

        _cameraPathAnimatorObj.SetActive(true);
    }

    public virtual void Destroy()
    {
        mCameraPathAnimator = null;
        mCameraPath = null;
    }    

    public void StartCameraPathAnimation(CameraPathAnimator.AnimationCustomEventHandler _cameraPathEndDelegate)
    {
        if (mCameraPathAnimator == null)
            return;

        mCameraPathAnimator.Play();

        if(_cameraPathEndDelegate != null)
            this.addCameraPathAnimatorDelegate(_cameraPathEndDelegate);
    }

    public void StopCameraPathAnimation(CameraPathAnimator.AnimationCustomEventHandler _cameraPathEndDelegate)
    {
        if (mCameraPathAnimator == null)
            return;

        mCameraPathAnimator.Stop();

        if (_cameraPathEndDelegate != null)
            this.removeCameraPathAnimatorDelegate(_cameraPathEndDelegate);
    }

    public void SetFollowTargetTransform(Transform _targetTransform)
    {
        if (mCameraPathAnimator == null)
          return;
                
        mCameraPathAnimator.orientationTarget = _targetTransform;
    }

    public Transform GetFollowTargetTransform()
    {
        if (mCameraPathAnimator == null)
            return null;

        return mCameraPathAnimator.orientationTarget;
    }

    public bool IsCameraPathAnimator()
    {
        if (mCameraPathAnimator == null)
            return false;

        return true;
    }

    private void addCameraPathAnimatorDelegate(CameraPathAnimator.AnimationCustomEventHandler _cameraPathEndDelegate)
    {
        if (mCameraPathAnimator == null)
            return;

        mCameraPathAnimator.AnimationCustomEvent += _cameraPathEndDelegate;
    }

    private void removeCameraPathAnimatorDelegate(CameraPathAnimator.AnimationCustomEventHandler _cameraPathEndDelegate)
    {
        if (mCameraPathAnimator == null)
            return;

        mCameraPathAnimator.AnimationCustomEvent -= _cameraPathEndDelegate;
    }    

    public CameraPath GetCameraPath()
    {
        return mCameraPath;
    }
}
