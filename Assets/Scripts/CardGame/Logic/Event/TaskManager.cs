using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance;
    public List<TaskCompletionSource<bool>> TaskBlockList = new List<TaskCompletionSource<bool>>();
    private void Awake()
    {
        Instance = this;
    }

    public void AddATask()
    {
        TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
        TaskBlockList.Add(tcs);
    }

    public async Task BlockTask()
    {
        AddATask();
        Debug.Log("新增之后，有几个阻塞任务~~~~~~~~~~~~~~~~~~~~~~~~~~" + TaskBlockList.Count);
        await TaskManager.Instance.TaskBlockList[TaskManager.Instance.TaskBlockList.Count - 1].Task;
    }

    public void UnBlockTask()
    {
        if (TaskBlockList.Count == 0)
        {
            return;
        }
        TaskBlockList[TaskBlockList.Count - 1].SetResult(true);
        TaskBlockList.RemoveAt(TaskBlockList.Count - 1);
        Debug.Log("解除阻塞后，还有几个阻塞任务~~~~~~~~~~~~~~~~~~~~~~~~~~" + TaskBlockList.Count);
    }

}
