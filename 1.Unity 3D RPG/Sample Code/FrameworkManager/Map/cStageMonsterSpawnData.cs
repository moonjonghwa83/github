using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;


public class cStageMonsterSpawnData
{
    private List<cStageMonsterSpawnGroup> mMonsterSpawnGroupList = null;    

    //=============================================

    public cStageMonsterSpawnData()
    {
        if (mMonsterSpawnGroupList == null)
            mMonsterSpawnGroupList = new List<cStageMonsterSpawnGroup>();

        mMonsterSpawnGroupList.Clear();        
    }    

    public void Destroy()
    {
        if (mMonsterSpawnGroupList != null)
        {
            for (int i = 0; i < mMonsterSpawnGroupList.Count; ++i)
            {
                mMonsterSpawnGroupList[i].Destroy();
                mMonsterSpawnGroupList[i] = null;
            }

            mMonsterSpawnGroupList.Clear();
            mMonsterSpawnGroupList = null;
        }
    }

    public IEnumerator Load(string _resourceName)
    {
        StringBuilder pathName = new StringBuilder();
        pathName.Append("Map/MonsterSpawn/");
        pathName.Append(_resourceName);

        GameObject monsterSpawnObj = cDataManager.InstantiateGo(pathName.ToString());

        int groupIdx = 0;        
        Transform groupTrans = null;
        cStageMonsterSpawnGroup stageMonsterSpawnGroup = null;

        while (true)
        {
            pathName.Length = 0;

            pathName.Append("Group_");
            pathName.Append(groupIdx.ToString());

            groupTrans = monsterSpawnObj.transform.Find(pathName.ToString());

            if (groupTrans == null)
                break;

            stageMonsterSpawnGroup = this.createMonsterSpawnGroup(groupTrans);

            if (stageMonsterSpawnGroup == null)
                break;

            mMonsterSpawnGroupList.Add(stageMonsterSpawnGroup);

            groupIdx++;
        }

        GameObject.Destroy(monsterSpawnObj);
        monsterSpawnObj = null;

        yield return null;
    }
    
    private cStageMonsterSpawnGroup createMonsterSpawnGroup(Transform _groupTrans)
    {
        cStageMonsterSpawnGroup monsterSpawnGroup = new cStageMonsterSpawnGroup();
        StringBuilder spawnName = new StringBuilder();
        Transform spawnTrans = null;

        for (int i = 0 ; i < 10 ; ++i)
        {
            spawnName.Length = 0;
            spawnName.Append("Spawn_");
            spawnName.Append(i.ToString());

            spawnTrans = _groupTrans.Find(spawnName.ToString());

            if (spawnTrans == null)
                break;

            monsterSpawnGroup.AddSpawnPosition(spawnTrans.position);
        }

        return monsterSpawnGroup;
    }

    public cStageMonsterSpawnGroup GetStageMonsterSpawnGroup(int _index)
    {
        if (mMonsterSpawnGroupList.Count <= _index)
            return null;

        return mMonsterSpawnGroupList[_index];
    }

    public int GetStageMonsterSpawnGroupCount()
    {
        return mMonsterSpawnGroupList.Count;
    }
}
