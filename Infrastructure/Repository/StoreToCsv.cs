using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class StoreToCsv : IRepository
    {
        public async Task StoreAsync(string response, short alphabet)
        {
            var fileName = "Players_Csv_File";
            var suffix = "\\" + (char)(65 + alphabet) + ".csv";
            
            var sortedRecords = SortRecords(response);

            Directory.CreateDirectory(fileName);
            using (var fileStream = File.Create(fileName + suffix))
            {
                using (var streamWriter = new StreamWriter(fileStream, Encoding.Default))
                {
                    await streamWriter.WriteAsync(sortedRecords);
                }
            }
        }

        private string SortRecords(string records)
        {
            var playerRecords = records.Split('\n');
            var sortedRecords = playerRecords[1..].OrderBy(r => r);

            var sb = new StringBuilder();
            sb.Append(playerRecords[0]);

            foreach (var record in sortedRecords)
                sb.Append(record + "\n");

            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }
    }
}
