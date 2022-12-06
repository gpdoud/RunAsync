using Dsi.RunConcurrentAsync;

public class RunConcurrentAsync {

    public int NumberOfTasks() => TaskList.Count;
    public int ConcurrentTasks { get; set; }
    private int ProcessorCount { get; init; } = Environment.ProcessorCount;
    private List<Task> TaskList = new();

    public RunConcurrentAsync(int concurrentTasks = 0) {
        ConcurrentTasks = concurrentTasks == 0 || concurrentTasks > ProcessorCount ? ProcessorCount : concurrentTasks;
    }

    public void AddTask(IRunConcurrentAsync rca) {
        TaskList.Add(new Task(rca.RunAsync));
    }
    public void AddTasks(IEnumerable<IRunConcurrentAsync> rcas) {
        foreach(var rca in rcas) {
            AddTask(rca);
        }
    }

    public void RunAllTasks() {
        if(TaskList.Count == 0) {
            Console.WriteLine($"TaskList is empty! - Existing.");
            return;
        }

        if (ConcurrentTasks > ProcessorCount) {
            ConcurrentTasks = ProcessorCount;
        }
        Console.WriteLine("Run Concurrent Async Tasks (c) DSI 2022" +
                            $"\nSystem: Number of processors: {ProcessorCount}" +
                            $"\nParameters: number of tasks: {NumberOfTasks()}" +
                            $"\nParameters: concurrent tasks: {ConcurrentTasks} (May not exceed ProcessorCount!)");

        var start = DateTime.Now;
        RunAllAsync(TaskList, ConcurrentTasks);
        var end = DateTime.Now;
        var dur = end - start;
        //System.Console.Write("\r                                                        ");
        System.Console.WriteLine($"\nCompleted in {dur.ToString()}");
        NotifyDone();
    }

    private void NotifyDone() {
        Console.Beep();
        System.Threading.Thread.Sleep(250);
        Console.Beep();
        System.Threading.Thread.Sleep(250);
        Console.Beep();
    }

    private void RunAllAsync(IEnumerable<Task> tasks, int concurrentTasks) {
        Queue<Task> queuedTasks = new Queue<Task>(tasks);
        var totalTasks = tasks.Count();
        var finishedTasks = 0;
        List<Task> activeTasks = new();
        for (var idx = 0; idx < concurrentTasks && queuedTasks.Count > 0; idx++) {
            var task = queuedTasks.Dequeue();
            activeTasks.Add(task);
            task.Start();
        }
        while (true) {
            // wait for any task to complete
            int idxCompletedTask = Task.WaitAny(activeTasks.ToArray());
            finishedTasks++;
            Console.Write($"\r Progress: {finishedTasks:N0} / {totalTasks:N0} {(decimal)finishedTasks / totalTasks * 100:N0}%");
            // remove completed task from active list
            activeTasks.Remove(activeTasks[idxCompletedTask]);
            // more tasks to run? 
            if (queuedTasks.Count() > 0) {
                // get next task
                Task nextTask = queuedTasks.Dequeue();
                activeTasks.Add(nextTask);
                nextTask.Start();
            }
            // all active tasks finished and no more tasks to run? 
            if (activeTasks.Count() == 0) {
                break;
            }
        }

    }
}
