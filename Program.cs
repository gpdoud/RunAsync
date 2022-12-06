using Dsi.RunConcurrentAsync;

public class Program {
    private void Run(string[] args) {
        var parms = Dsi.Utility.ProcessArgs.Parse(args);
        RunConcurrentAsync rca = new();
        if(parms.ContainsKey("--concurrentTasks")) {
            rca.ConcurrentTasks = Convert.ToInt32(parms["--concurrentTasks"]);
        }
        rca.AddTasks(CreateTasks());
        rca.RunAllTasks();
    }
    private IEnumerable<IRunConcurrentAsync> CreateTasks() {
        int nbrTasks = 50;
        // ulong inc = 1_000_000_000; // 1 billion
        // ulong start = 0;
        // ulong end = inc;
        List<IRunConcurrentAsync> tasks = new(nbrTasks);
        for(int idx = 0; idx < nbrTasks; idx++) {
            tasks.Add( new Countdown {} );
        }
        return tasks;
    }
    public static void Main(string[] args) {
        (new Program()).Run(args);
    }
}