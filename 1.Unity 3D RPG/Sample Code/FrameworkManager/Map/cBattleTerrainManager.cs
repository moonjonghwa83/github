using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using BinaryGameData;


public class cBattleTerrainManager : MonoBehaviour
{
    public class cFogInfo
    {
        public string FogColor = string.Empty;
        public eFOG_MODE FogMode = eFOG_MODE.NONE;
        public float FogDensity = 0.0f;
        public float LinearFogStart = 0.0f;
        public float LinearFogEnd = 0.0f;
        public string AmbientLight = "";
        public string SkyboxMaterialPath = "";
        public float HaloStrength = 0.0f;
        public float FlareStrength = 0.0f;
        public float FlareFadeSpeed = 0.0f;
    }

    //----------------------------------------------------------------------

    private string mStageMapName = string.Empty;
    private GameObject mMapObject = null;

    private cMapRenderEffectController mMapRenderEffectController = null;

    private cMapStageTable mMapStageData = null;

    private cStageMonsterSpawnData mStageMonsterSpawnData = null;
    private cStageMovePathData mStageMovePathData = null;

    //=========================================================================

    public void Init()
    {
        if (mStageMonsterSpawnData == null)
            mStageMonsterSpawnData = new cStageMonsterSpawnData();

        if (mStageMovePathData == null)
            mStageMovePathData = new cStageMovePathData();
    }

    private void initMapRenderEffectController(GameObject _mapObject)
    {
        if (mMapRenderEffectController == null)
            mMapRenderEffectController = new cMapRenderEffectController(_mapObject);
    }

    void OnDestroy()
    {
        this.clearFog();
        this.deleteMapGameObject();

        if (mMapRenderEffectController != null)
        {
            mMapRenderEffectController.Destroy();
            mMapRenderEffectController = null;
        }

        if (mStageMonsterSpawnData != null)
        {
            mStageMonsterSpawnData.Destroy();
            mStageMonsterSpawnData = null;
        }

        if (mStageMovePathData != null)
        {
            mStageMovePathData.Destroy();
            mStageMovePathData = null;
        }

    }

    public void LoadMap(BattleScene _battleScene, cMapStageTable _mapStageData)
    {
        this.deleteMapGameObject();

        mStageMapName = _mapStageData.StageScene;
        mMapStageData = _mapStageData;

        mMapObject = GameObject.Find(mStageMapName);

        if (mMapObject != null)
            UnityEngine.SceneManagement.SceneManager.UnloadScene(mStageMapName);

        cDataManager.LoadGameLevelAsync( null, mStageMapName, true, this.callbackLoadMapComplete );
    }

    void callbackLoadMapComplete()
    {
        mMapObject = GameObject.Find( mStageMapName );

        mMapObject.transform.position = Vector3.zero;

        this.initMapRenderEffectController(mMapObject);

        cCameraFrameworkManager.Instance.BattleCameraController.InitCameraPathAnimator(cBattleCameraController.eBATTLE_CAMERA_PATH_ANIMATOR_TYPE.INTRO, GameObject.Find("IntroCameraPath"));
        cCameraFrameworkManager.Instance.BattleCameraController.InitCameraPathAnimator(cBattleCameraController.eBATTLE_CAMERA_PATH_ANIMATOR_TYPE.FOLLOW, GameObject.Find("FollowCameraPath"));

        cMapStageTable mapStageData = cDataManager.GetMapStageTable(cGameInfoData.GetStageIndexTable());

        StartCoroutine(mStageMonsterSpawnData.Load(mapStageData.StagePrefabs));
        StartCoroutine(mStageMovePathData.Load(mapStageData.MapPath));


        StartCoroutine(setFog());

        BattleScene battleScene = ClientMain.GetScene( eSCENE_ID.BATTLE ) as BattleScene;
        Debug.Assert( battleScene != null );
        battleScene.OnMapLoadComplete();
    }

    private void deleteMapGameObject()
    {
        if (mMapObject != null)
        {
            Destroy(mMapObject);
            mMapObject = null;
        }
    }        

    void Update()
    {
        if(mMapRenderEffectController != null)
            mMapRenderEffectController.Update();
    }

    
    public cStageMonsterSpawnData GetStageMonsterSpawnData()
    {
        return mStageMonsterSpawnData;
    }

    public cStageMovePathData GetStageMovePathData()
    {
        return mStageMovePathData;
    }

    public cMapStageTable GetStageMapStageData()
    {
        return mMapStageData;
    }

    public void ChangeRenderEffectState(eMAP_RENDER_EFFECT_STATE _state)
    {
        mMapRenderEffectController.ChangeState(_state);
    }

    public eMAP_RENDER_EFFECT_STATE GetCurRenderEffectStateKey()
    {
        return mMapRenderEffectController.GetCurStateKey();
    }

    public IEnumerator setFog()
    {
        if (mMapStageData == null)
            yield return 0;

        RenderSettings.fog = false;

        if (mMapStageData.FogMode == eFOG_MODE.NONE)
        {
            RenderSettings.fog = false;
        }
        else
        {
            RenderSettings.fog = true;
            
            Color fogColor = cUtilFunc.ConvertColor(mMapStageData.FogColor_R, mMapStageData.FogColor_G, mMapStageData.FogColor_B, mMapStageData.FogColor_A);
            RenderSettings.fogColor = fogColor;

            switch (mMapStageData.FogMode)
            {
                case eFOG_MODE.LINEAR:
                    {
                        RenderSettings.fogMode = FogMode.Linear;
                    }
                    break;
                case eFOG_MODE.EXPONENTIAL:
                    {
                        RenderSettings.fogMode = FogMode.Exponential;
                    }
                    break;
                case eFOG_MODE.EXPONENTIAL_SQUARED:
                    {
                        RenderSettings.fogMode = FogMode.ExponentialSquared;
                    }
                    break;
            }

            RenderSettings.fogDensity = mMapStageData.FogDensity;
            RenderSettings.fogStartDistance = mMapStageData.FogStartDistance;
            RenderSettings.fogEndDistance = mMapStageData.FogEndDistance;
            RenderSettings.haloStrength = 0.0f;
            RenderSettings.flareStrength = 0.0f;
            RenderSettings.flareFadeSpeed = 0.0f;            
        }
        
        if (mMapStageData.SkyBox != string.Empty)
        {
            Material mat = Resources.Load("Map/SkyBox/" + mMapStageData.SkyBox) as Material;

            if (mat != null)
            {
                RenderSettings.skybox = mat;
                //RenderSettings.ambientLight = new Color(238.0f / 255.0f, 245.0f / 255.0f, 255.0f / 255.0f);
                RenderSettings.ambientLight = cUtilFunc.ConvertColor(mMapStageData.AmbientLight_R, mMapStageData.AmbientLight_G, mMapStageData.AmbientLight_B, mMapStageData.AmbientLight_A);
            }
        }

        yield return null;
    }

    private void clearFog()
    {
        RenderSettings.skybox = null;
        RenderSettings.fog = false;
        RenderSettings.ambientLight = new Color(1.0f, 1.0f, 1.0f);
    }
}
