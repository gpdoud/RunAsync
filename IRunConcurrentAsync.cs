public interface IRunConcurrentAsync {
    static bool CancellationSwitch { get; set; }
    void RunAsync();
}