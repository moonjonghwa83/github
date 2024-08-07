using UnityEngine;
using System.Collections;

public enum eCONTROL_TYPE
{
    NONE = 0,
    TAP,
    DRAG,
}

public class cInputFrameworkManager : cSingleton<cInputFrameworkManager>
{

    static private GameObject             mManagerObject      = null;

    private eCONTROL_TYPE mControlType = eCONTROL_TYPE.NONE;
    private bool mEnableControl = true;
    //=====================================================================================================================

    public void Init()
    {
    }

    private void setControlType(eCONTROL_TYPE _type)
    {
        mControlType = _type;
    }

    public eCONTROL_TYPE GetControlType()
    {
        return mControlType;
    }

    public void EnableConrtol()
    {
        mEnableControl = true;
    }

    public void DisableControl()
    {
        mEnableControl = false;
    }

    // 플레이어 무브 프로세스
    private void moveProcess(Vector3 _inputPos, bool _drag)
    {

    }

    private bool mDrag = false;
    private float mTime = 0.0f;
    private Vector3 mDragOriginPoint = Vector3.zero;
    private Vector3 mDragDist = Vector3.zero;

    public void Update()
    {        
        //-------------------------------------------------------------------------------------------------
        // 터치 이동                
        
        if (Input.GetMouseButtonDown(0) == true)
        {          
            mTime = 0.0f;
            mDrag = false;

            //if( cGameInput.IsUiEvent( Input.mousePosition ) == true )
            //    return;

            Vector3 pos = Vector3.zero;

            if (GetClickScreenToWorldPosition(out pos) == true)
            {
                mDragOriginPoint = pos;
                mDragDist = Vector3.zero;
            }            
        }
        else if (Input.GetMouseButtonUp(0) == true)
        {            
            mDrag = false;

            if (ClientMain.GetCurrentSceneID() == eSCENE_ID.BATTLE)
                this.updateBattleSceneInput();
        }
        /*
        else if (Input.GetMouseButton(0) == true)
        {
            if( cGameInput.IsUiEvent( Input.mousePosition ) == true )
            {
                mDrag = false;
                mTime = 0.0f;
                return;
            }            
            if (mDrag == false)
            {
                mTime += Time.deltaTime;

                if (mTime > 0.15f)
                {                    
                    mDrag = true;
                    mTime = 0.0f;                    
                }
            }
        }
        */

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            //cCameraFrameworkManager.Instance.HomeCameraController.ZoomOut();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            //cCameraFrameworkManager.Instance.HomeCameraController.ZoomIn();
        }

        if (Input.GetAxis("Horizontal") < 0)
        {
            //cCameraFrameworkManager.Instance.HomeCameraController.CameraRotationLeft();
        }
        else if (Input.GetAxis("Horizontal") > 0)
        {
            //cCameraFrameworkManager.Instance.HomeCameraController.CameraRotationRight();
        }

        /*
        for (int i = 0; i < Input.touches.Length; ++i)
        {
            if( cGameInput.IsUiEvent( new Vector3( Input.touches[ i ].position.x, Input.touches[ i ].position.y ) ) == true )
            {
                mTime = 0.0f;
                mDrag = false;
                break;
            }

            if (Input.touches[i].phase == TouchPhase.Began)
            {
                mTime = 0.0f;
                mDrag = false;

                Vector3 pos = Vector3.zero;

                if (GetClickScreenToWorldPosition(out pos) == true)
                {
                    mDragOriginPoint = pos;
                    mDragDist = Vector3.zero;
                }
            }
            else if (Input.touches[i].phase == TouchPhase.Moved)
            {
                if (mDrag == false)
                {
                    mTime += Time.deltaTime;

                    //if (mTime > 0.1f)
                    {
                        mDrag = true;
                        mTime = 0.0f;
                    }
                }
            }
            else if (Input.touches[i].phase == TouchPhase.Ended)
            {
                mDrag = false;

                if (ClientMain.GetCurrentSceneID() == eSCENE_ID.HOME)
                    this.updateHomeSceneInput();
                else if (ClientMain.GetCurrentSceneID() == eSCENE_ID.BATTLE)
                    this.updateBattleSceneInput();
            }
        }


        if (mDrag == true)
        {
            if( cGameInput.IsUiEvent( Input.mousePosition ) == true )
            {
                mDrag = false;
                mTime = 0.0f;
                return;
            }

            Vector3 pos = Vector3.zero;
            Vector3 inputPoint = Vector3.zero;

            if (this.GetClickScreenToWorldPosition(out pos) == true)
            {
                inputPoint = pos;

                mDragDist = inputPoint - mDragOriginPoint;
                Vector3 direction = mDragDist.normalized * -1.0f;
                float dist = Mathf.Sqrt(mDragDist.sqrMagnitude);

                cCameraFrameworkManager.Instance.HomeCameraController.SetTargetViewPosition(direction, dist);
            }
        }
        */
    }

    
    private bool GetClickScreenToWorldPosition(out Vector3 _pos)
    {
        _pos = Vector3.zero;
        RaycastHit hit;
        Camera mainCamera = cCameraFrameworkManager.Instance.MainCamera;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 500.0f, 1 << LayerMask.NameToLayer("Water")))
        {
            _pos = hit.point;
            return true;
        }        

        return false;
    }

    private void updateBattleSceneInput()
    {
        if (BattleScene.BattleSceneManager == null)
            return;

        RaycastHit hit;
        Camera mainCamera = cCameraFrameworkManager.Instance.MainCamera;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 500.0f, 1 << LayerMask.NameToLayer("Ground")))
        {
            BattleScene.BattleSceneManager.FormationManager.ClickMovePos = hit.point;

            BattleScene.BattleSceneManager.BattleManager.SetClickPointPosition(hit.point);
            /*
            BattleScene.BattleSceneManager.PlayerManager.GetLeaderPlayer().SetDestination(BattleScene.BattleSceneManager.FormationManager.ClickMovePos);

            BattleScene.BattleSceneManager.PlayerManager.ChangeStateAll(eBATTLE_UNIT_STATE.NONE);

            BattleScene.BattleSceneManager.FormationManager.ChangeState(eFORMATION_STATE.RUN);
            */
        }
    }
    
    public void Destroy()
    {
        if (mManagerObject != null)
        {
            mManagerObject = null;
            GameObject.Destroy(mManagerObject);
        }
    }

}
