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
    UnderCartTask
}
public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance;
    //public List<TaskCompletionSource<bool>> TaskBlockList = new List<TaskCompletionSource<bool>>();
    public Dictionary<TaskType, TaskCompletionSource<bool>> TaskBlockDic = new Dictionary<TaskType, TaskCompletionSource<bool>>();
    public TaskCompletionSource<bool> DelayTipTask = new TaskCompletionSource<bool>();
    private void Awake()
    {
        Instance = this;
    }

    public void AddATask(TaskType taskType)
    {
        TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
        TaskManager.Instance.TaskBlockDic[taskType] = tcs;
    }

    public async Task Block(TaskType taskType)
    {
        await TaskManager.Instance.TaskBlockDic[taskType].Task;
    }

    public async Task DontAwait()
    {
        await Task.Run(() => { Debug.Log("不需要阻塞"); });
    }

    public async Task BlockTask(TaskType taskType)
    {
        AddATask(taskType);
        foreach (TaskType taskt in TaskManager.Instance.TaskBlockDic.Keys)
        {
            Debug.Log("加入阻塞后，还有几个阻塞任务~~~~~~~~~~~" + taskt + "~~~~~~~~~~~~~~~");
        }
        await TaskManager.Instance.TaskBlockDic[taskType].Task;
    }

    public void UnBlockTask(TaskType taskType)
    {
        foreach (TaskType taskt in TaskManager.Instance.TaskBlockDic.Keys)
        {
            Debug.Log("需要解除任务~~~~~~~~~~~" + taskt + "~~~~~~~~~~~~~~~");
        }
        if (TaskManager.Instance.TaskBlockDic.ContainsKey(taskType))
        {
            Debug.Log("1解除阻塞后，还有几个阻塞任务~~~~~~~~~~~~~~~~~~~~~~~~~~" + TaskManager.Instance.TaskBlockDic.Keys.Count);
            TaskManager.Instance.TaskBlockDic[taskType].SetResult(true);
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
            TaskManager.Instance.TaskBlockDic[taskType].SetException(exception);
            TaskManager.Instance.TaskBlockDic.Remove(taskType);
        }
    }

}
