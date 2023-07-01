using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;

public enum TaskType
{
    DyingTask,
    DelayTask,
    SpecialTargetsTask,
    CixiongShuangguTask,
    GuanshifuTask,
    ZhangbashemaoTask,
    FangtianhuajiTask,
    ZhuqueyushanTask,
    FrostBladeTask,
    QilingongTask,
    SilverMoonTask,
    BaguazhenTask,
    RenwangdunTask,
    TengjiaTask,
    UnderCartTask,
    QinglongyanyueTask,
    ThunderHarmerTask,
    VictorySwordTask,
    //技能
    AthenaSkill1,
    AthenaSkill2,
    MaatSkill1,
    MaatSkill2,
    FenrirSkill1,
    FenrirSkill2
}
public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance;
    //public List<TaskCompletionSource<bool>> TaskBlockList = new List<TaskCompletionSource<bool>>();
    public Dictionary<TaskType, List<TaskCompletionSource<bool>>> TaskBlockDic = new Dictionary<TaskType, List<TaskCompletionSource<bool>>>();
    public TaskCompletionSource<bool> DelayTipTask = new TaskCompletionSource<bool>();
    private void Awake()
    {
        Instance = this;
    }

    public void AddATask(TaskType taskType)
    {
        //if (TaskManager.Instance.TaskBlockDic.ContainsKey(taskType))
        //{
        //    Debug.Log("*************************之前有" + taskType);
        //    TaskManager.Instance.TaskBlockDic[taskType].SetResult(true);
        //}
        TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
        if (TaskManager.Instance.TaskBlockDic.ContainsKey(taskType))
        {
            TaskManager.Instance.TaskBlockDic[taskType].Add(tcs);
        }
        else
        {
            List<TaskCompletionSource<bool>> newTcss = new List<TaskCompletionSource<bool>>();
            newTcss.Add(tcs);
            TaskManager.Instance.TaskBlockDic[taskType] = newTcss;
        }
        if (TaskManager.Instance.TaskBlockDic.Count > 0)
        {
            TurnManager.Instance.IsInactiveStatus = false;
        }
    }

    public async Task Block(TaskType taskType)
    {
        await TaskManager.Instance.TaskBlockDic[taskType][0].Task;
    }

    public async Task DontAwait()
    {
        await Task.Run(() => { Debug.Log(""); });
    }

    public async Task ReturnException(string message)
    {
        Exception exception = new Exception(message);
        await Task.FromException(exception);
    }

    public async Task BlockTask(TaskType taskType)
    {
        AddATask(taskType);
        foreach (TaskType taskt in TaskManager.Instance.TaskBlockDic.Keys)
        {
            Debug.Log("加入阻塞后，还有几个阻塞任务~~~~~~~~~~~" + taskt + "~~~~~~~~~~~~~~~");
        }
        await TaskManager.Instance.TaskBlockDic[taskType][0].Task;
    }

    public void UnBlockTask(TaskType taskType)
    {
        foreach (TaskType taskt in TaskManager.Instance.TaskBlockDic.Keys)
        {
            Debug.Log("需要解除任务~~~~~~~~~~~" + taskt + "~~~~~~~~~~~~~~~");
        }
        if (TaskManager.Instance.TaskBlockDic.ContainsKey(taskType))
        {
            if (TaskManager.Instance.TaskBlockDic.Count == 1 && TaskManager.Instance.TaskBlockDic[taskType].Count == 1)
            {
                TurnManager.Instance.IsInactiveStatus = true;
            }
            Debug.Log("1解除阻塞后，还有几个阻塞任务~~~~~~~~~~~~~~~~~~~~~~~~~~" + TaskManager.Instance.TaskBlockDic.Keys.Count);
            TaskManager.Instance.TaskBlockDic[taskType][0].SetResult(true);
            TaskManager.Instance.TaskBlockDic[taskType].RemoveAt(0);
            if (TaskManager.Instance.TaskBlockDic[taskType].Count == 0)
            {
                TaskManager.Instance.TaskBlockDic.Remove(taskType);
            }
        }
    }

    public void UnBlockTaskForAll(TaskType taskType)
    {
        if (TaskManager.Instance.TaskBlockDic.ContainsKey(taskType))
        {
            if (TaskManager.Instance.TaskBlockDic.Count == 1)
            {
                TurnManager.Instance.IsInactiveStatus = true;
            }
            foreach (TaskCompletionSource<bool> tcs in TaskManager.Instance.TaskBlockDic[taskType])
            {
                Debug.Log("1解除阻塞后，还有几个阻塞任务~~~~~~~~~~~~~~~~~~~~~~~~~~" + TaskManager.Instance.TaskBlockDic.Keys.Count);
                TaskManager.Instance.TaskBlockDic[taskType][0].SetResult(true);
            }
            TaskManager.Instance.TaskBlockDic.Remove(taskType);
        }
    }

    public void ExceptionBlockTask(TaskType taskType, string exMessage = "Jump Out")
    {
        foreach (TaskType taskt in TaskManager.Instance.TaskBlockDic.Keys)
        {
            Debug.Log("需要异常任务~~~~~~~~~~~" + taskt + "~~~~~~~~~~~~~~~");
        }
        if (TaskManager.Instance.TaskBlockDic.ContainsKey(taskType))
        {
            Exception exception = new Exception(exMessage);
            TaskManager.Instance.TaskBlockDic[taskType][0].SetException(exception);
            TaskManager.Instance.TaskBlockDic[taskType].RemoveAt(0);
            if (TaskManager.Instance.TaskBlockDic[taskType].Count == 0)
            {
                TaskManager.Instance.TaskBlockDic.Remove(taskType);
            }
        }
        if (TaskManager.Instance.TaskBlockDic.Count > 0)
        {
            TurnManager.Instance.IsInactiveStatus = false;
        }
        else
        {
            TurnManager.Instance.IsInactiveStatus = true;
        }
    }

}
