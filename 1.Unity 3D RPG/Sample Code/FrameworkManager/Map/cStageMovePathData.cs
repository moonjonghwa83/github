using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;


public class cStageMovePathData
{
    private List<Vector3> mMovePathPositionList = null;
    private int mCurMovePathIndex = 0;

    private bool IsArrived = false;

    //=====================================================

    public cStageMovePathData()
    {
        if (mMovePathPositionList == null)
            mMovePathPositionList = new List<Vector3>();

        mMovePathPositionList.Clear();
        mCurMovePathIndex = 0;
    }

    public void Destroy()
    {
        if (mMovePathPositionList != null)
        {
            mMovePathPositionList.Clear();
            mMovePathPositionList = null;
        }
    }

    public IEnumerator Load(string _resourceName)
    {
        StringBuilder pathName = new StringBuilder();
        pathName.Append("Map/MapMovePath/");
        pathName.Append(_resourceName);

        GameObject movePathObj = cDataManager.InstantiateGo(pathName.ToString());

        int pathIdx = 0;
        Transform pathTrans = null;        

        while (true)
        {
            pathName.Length = 0;
            
            pathName.Append("MovePath_");
            pathName.Append(pathIdx.ToString());

            pathTrans = movePathObj.transform.Find(pathName.ToString());

            if (pathTrans == null)
                break;

            mMovePathPositionList.Add(pathTrans.position);

            pathIdx++;
        }

        GameObject.Destroy(movePathObj);
        movePathObj = null;

        yield return null;
    }

    public bool GetIsArrived()
    {
        return IsArrived;
    }

    public Vector3 GetCurMovePathPosition()
    {
        return mMovePathPositionList[mCurMovePathIndex];
    }

    public int GetCurMovePathIdx()
    {
        return mCurMovePathIndex;
    }

    public Vector3 GetMovePathPositionIdx(int _idx)
    {
        return mMovePathPositionList[_idx];
    }

    public int GetMovePathCount()
    {
        return mMovePathPositionList.Count;
    }

    public void SetCurMovePathIdx(int _idx)
    {
        mCurMovePathIndex = _idx;
    }

    public void SetNextMovePath()
    {
        mCurMovePathIndex++;

        if (mCurMovePathIndex >= mMovePathPositionList.Count)
        {
            mCurMovePathIndex = mMovePathPositionList.Count - 1;
            IsArrived = true;
        }
    }
}
