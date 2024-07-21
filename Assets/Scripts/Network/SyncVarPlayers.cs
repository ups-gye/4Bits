using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class SyncVarPlayers : NetworkBehaviour
{
    [SerializeField] private Slider taskSldr = null; // Asigna el Slider en el inspector de Unity
    private const int maxTasks = 10;
    
    [SyncVar(hook = nameof(OnTaskChange))]
    private int task_completed = 0;

    private void Start()
    {
        if (taskSldr != null)
        {
            taskSldr.maxValue = maxTasks; // Asigna el valor máximo apropiado para el slider
            taskSldr.value = task_completed;
        }
    }

    private void Update()
    {
        if (isLocalPlayer && Input.GetKeyDown(KeyCode.T))
        {
            CompleteTask();
        }
    }

    [Server]
    public void SetTaskCompleted(int taskCompleted)
    {
        task_completed = taskCompleted;
    }

    [Command]
    public void CmdSetTaskCompleted(int taskCompleted)
    {
        if(taskCompleted >= 0 && taskCompleted <= maxTasks)
        {
            SetTaskCompleted(taskCompleted);
        }
    }

    public void CompleteTask()
    {
        int newTaskCompleted = task_completed + 1;
        CmdSetTaskCompleted(newTaskCompleted);
    }

    private void OnTaskChange(int oldTaskCompleted, int newTaskCompleted)
    {
        if (taskSldr != null)
        {
            taskSldr.value = newTaskCompleted;
        }
    }
}
