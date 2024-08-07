using UnityEngine;
using System.Collections.Generic;


public class cCameraFrameworkManager : cSingleton<cCameraFrameworkManager>
{
    private Transform mCameraParentTrans = null;
    public Transform CameraParentTrans
    {
        get { return mCameraParentTrans; }
    }

    private Camera mMainCamera = null;
    public Camera MainCamera
    {
        get { return mMainCamera; }
    }
    private Camera mSubCamera = null;
    public Camera SubCamera
    {
        get { return mSubCamera; }
    }

    private Camera mUICamera = null;
    public Camera UICamera
    {
        get { return mUICamera; }
    }

    private cBattleCameraController mBattleCameraController = null;
    public cBattleCameraController BattleCameraController
    {
        get { return mBattleCameraController; }
    }
    /*
    private cHomeCameraController mHomeCameraController = null;
    public cHomeCameraController HomeCameraController
    {
        get { return mHomeCameraController; }
    }
    private AmplifyColorEffect mAmplifyColor = null;
    public AmplifyColorEffect AmplifyColorLink
    {
        get { return mAmplifyColor; }
    }
    */

    private cCameraShake mCameraShake = null;
    private GlowEffect.GlowEffect mGlowEffect = null;
    public GlowEffect.GlowEffect GlowEffectLink
    {
        get { return mGlowEffect; }
    }

    //===================================================================================

    public void Init(ClientMain _clientMain)
    {
        this.createCamera(_clientMain);
        this.initMainCameraProperty();
        this.SetActiveMainCamera(false);

        if (mBattleCameraController == null)
            mBattleCameraController = new cBattleCameraController(this, mMainCamera);

        mBattleCameraController.OnInit();
        /*
        if(mHomeCameraController == null)
            mHomeCameraController = new cHomeCameraController(this, mMainCamera);

        mHomeCameraController.OnInit();
        */
    }

    private void createCamera(ClientMain _clientMain)
    {
        if (mMainCamera != null)
            return;

        mCameraParentTrans = _clientMain.transform.Find("CameraParent");

        GameObject cameraObj = mCameraParentTrans.transform.Find("MainCamera").gameObject;
                
        mMainCamera = cameraObj.GetComponent<Camera>();

        if (mMainCamera == null)
            mMainCamera = cameraObj.AddComponent<Camera>();

        if (mUICamera == null)
            mUICamera = GameObject.Find("UI Root").transform.Find("Camera").GetComponent<Camera>();

        
        mCameraShake = mMainCamera.gameObject.GetComponent<cCameraShake>();
        if (mCameraShake == null)
            mCameraShake = mMainCamera.gameObject.AddComponent<cCameraShake>();

        //Transform t = mMainCamera.transform.FindChild("SubCamera");
        Transform t = mCameraParentTrans.transform.Find("MainCamera");
        Debug.Assert( t != null );
        mSubCamera = t.GetComponent( typeof( Camera ) ) as Camera;
        Debug.Assert( mSubCamera != null );
        

        mGlowEffect = t.GetComponent( typeof( GlowEffect.GlowEffect ) ) as GlowEffect.GlowEffect;
        if( mGlowEffect != null )
            mGlowEffect.enabled = ClientConfig.USE_CAMERA_GLOW;

        //mAmplifyColor = mMainCamera.gameObject.GetComponent( typeof( AmplifyColorEffect ) ) as AmplifyColorEffect;
        
    }

    private void initMainCameraProperty()
    {        
        mMainCamera.backgroundColor = Color.black;
        mMainCamera.fieldOfView = 30.0f;
        mMainCamera.nearClipPlane = 2.0f;
        mMainCamera.farClipPlane = 500.0f;
        mMainCamera.useOcclusionCulling = false;        
    }

    public void Destroy()
    {
        mCameraShake = null;
        mMainCamera = null;        
        //mHomeCameraController.OnDestroy();
        mBattleCameraController.OnDestroy();
    }

    public void SetActiveMainCamera(bool _isActive)
    {
        if (mMainCamera == null)
            return;

        mMainCamera.gameObject.SetActive(_isActive);
    }

    public void StartShake(float _shakeAmount, float _shakeTime)
    {
        
        if (mCameraShake == null)
            return;

        mCameraShake.StartCameraShake(mMainCamera, _shakeAmount, _shakeTime);
        
    }

    public void Update()
    {

        if (ClientMain.GetCurrentSceneID() == eSCENE_ID.HOME)
        {/* mHomeCameraController.OnUpdate();*/}
        else if (ClientMain.GetCurrentSceneID() == eSCENE_ID.BATTLE)
        { mBattleCameraController.OnUpdate(); }
            
    }

    public void LateUpdate()
    {
        
        if (ClientMain.GetCurrentSceneID() == eSCENE_ID.HOME)
        {/*mHomeCameraController.OnLateUpdate();*/}
        else if (ClientMain.GetCurrentSceneID() == eSCENE_ID.BATTLE)
            {mBattleCameraController.OnLateUpdate();}
            
    }    
}
