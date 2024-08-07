using UnityEngine;
using System.Collections.Generic;

public class cStageMonsterSpawnGroup
{
    private List<Vector3> mPositionList = null;

    //=====================================================

    public cStageMonsterSpawnGroup()
    {
        if (mPositionList == null)
            mPositionList = new List<Vector3>();

        mPositionList.Clear();
    }

    public void Destroy()
    {
        if (mPositionList == null)
            return;

        mPositionList.Clear();
        mPositionList = null;
    }

    public void AddSpawnPosition(Vector3 _position)
    {
        if (mPositionList == null)
            return;

        mPositionList.Add(_position);
    }

    public Vector3 GetSpawnPosition(int _index)
    {
        return mPositionList[_index];
    }

    public int GetSpawnGroupPositionCount()
    {
        return mPositionList.Count;
    }
}
