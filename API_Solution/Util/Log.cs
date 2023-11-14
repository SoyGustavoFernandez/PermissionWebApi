namespace Util
{
    public class LogStorageService : IStorageService
    {
        public void Save(string data)
        {
            try
            {
                if (!File.Exists("log.txt"))
                {
                    using (File.Create("log.txt")) { }
                }

                File.AppendAllText("log.txt", data + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to log file: {ex.Message}");
            }
        }
    }
}