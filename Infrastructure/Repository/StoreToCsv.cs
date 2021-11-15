using System.Collections.Generic;
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

            Directory.CreateDirectory(fileName);
            using (var fileStream = File.Create(fileName + suffix))
            {
                using (var streamWriter = new StreamWriter(fileStream, Encoding.Default))
                {
                    await foreach (var csvRecord in SortRecordsAsync(response))
                    {
                        await streamWriter.WriteLineAsync(csvRecord);
                    }
                }
            }
        }

        private async IAsyncEnumerable<string> SortRecordsAsync(string records)
        {
            var lineOfContent = records.Split('\n');
            var sortedRecords = lineOfContent[1..].OrderBy(record => record);

            var newPage = Enumerable.Empty<string>()
                .Append(lineOfContent[0][..^1])
                .Concat(sortedRecords.Skip(1).AsEnumerable());

            await foreach (var record in newPage.ToAsyncEnumerable())
            {
                yield return record;
            }
        }
    }
}
