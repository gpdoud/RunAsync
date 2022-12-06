using System;
namespace Dsi.RunConcurrentAsync;

public class Countdown : IRunConcurrentAsync {
    // 218_340_105_584_896
    //var idx = (long)Math.Pow(62, 8);
    private static long counter = 1;
    private static ulong start = 0;
    private static ulong increment = 1_000_000_000;
    private static ulong end = start + increment;
    public string? Tag { get; set; } = string.Empty;
    public ulong Start { get; set; }
    public ulong End { get; set; }

    public void RunAsync() {
        //Console.Write($"\r{Tag} counter: {Start:N0} to {End:N0}");
        for (ulong uidx = Start; uidx <= End; uidx++) {
            // does nothing
        }
    }

    public Countdown() {
        Tag = $"Task-{counter:000000}";
        Start = start;
        End = end;
        counter++;
        start = end + 1;
        end = end + increment;
    }
}