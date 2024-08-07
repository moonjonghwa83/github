using UnityEngine;
using System.Collections;

abstract public class cCameraControllerBase
{
    protected Camera mMainCamera = null;
    protected Transform mCameraParentTrans = null;
    protected cCameraFrameworkManager mCameraFrameworkManager = null;    

    //=======================================================

    public cCameraControllerBase(cCameraFrameworkManager _cameraManager, Camera _mainCamera)
    {
        mCameraFrameworkManager = _cameraManager;
        mMainCamera = _mainCamera;
        mCameraParentTrans = _cameraManager.CameraParentTrans;
    }

    abstract public void OnInit();
    abstract public void OnDestroy();
    abstract public void OnUpdate();
    abstract public void OnLateUpdate();    
}
