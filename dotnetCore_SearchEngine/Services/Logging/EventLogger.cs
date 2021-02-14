using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace dotnetCore_SearchEngine.Services.Logging
{
    public class EventLogger
    {
        private static EventLogger instance;
        private EventLogger() { }
        public static EventLogger Instance { get; } = instance ?? (instance = new EventLogger());

        private readonly static int maxLineCount = 300;
        private readonly static string logPath = $"EventLogs\\";
        private readonly static string filePath = AppDomain.CurrentDomain.BaseDirectory + logPath;

        protected Encoding encoding = Encoding.UTF8;
        static SemaphoreSlim locker = new SemaphoreSlim(1, 1);

        public async Task LogEvent(string message)
        {
            await LogData(message);
        }

        private async Task LogData(string data)
        {
            string logFile = "SearchEngineApi.txt";
            string extFilePath = filePath + logFile;
            await locker.WaitAsync();

            try
            {
                if (!Directory.Exists(filePath))
                    CreateFolder(filePath);
                if (File.Exists(extFilePath))
                    await ReadFile(extFilePath);

                using (StreamWriter writer = new StreamWriter(extFilePath, true, encoding))
                {
                    await writer.WriteLineAsync($"{DateTime.UtcNow}  - {data}");
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                locker.Release();
            }
        }

        private async Task ReadFile(string extFilePath)
        {
            string fileData = string.Empty;
            Stream fileStream = null;

            using (StreamReader reader = new StreamReader(extFilePath))
            {
                fileStream = reader.BaseStream;
                if (!reader.EndOfStream)
                    fileData = await reader.ReadToEndAsync();
            }

            string[] lineCount = fileData.Split(new char[] { '\n' });
            int count = lineCount.Length;

            if (count > maxLineCount)
                TruncateFile(fileStream, extFilePath);
        }

        private void TruncateFile(Stream fileStream, string filePath)
        {
            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    fileStream.Close();
                }
            }
        }

        private void CreateFolder(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException(filePath);
            try
            {
                DirectoryInfo directory = Directory.CreateDirectory(filePath);
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
