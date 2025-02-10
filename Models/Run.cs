namespace dotnet_client
{
    public class Run
    {
        /// <summary>
        /// The unique identifier for the run.
        /// </summary>
        public string RunId { get; set; }

        /// <summary>
        /// The current status of the run.
        /// </summary>
        public RunState RunStatus { get; set; }
    }
}
