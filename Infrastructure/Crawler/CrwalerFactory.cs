using HtmlAgilityPack;
using Infrastructure.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class CrwalerFactory : ICrwaler
    {
        private readonly HtmlWeb _web = new();
        private readonly ILogger<CrwalerFactory> _logger;
        private readonly string _rootWebUrl = "https://www.basketball-reference.com/";
        private readonly string _scoreTitle = "Player,G,PTS,TRB,AST,FG(%),FG3(%),FT(%),eFG(%),PER,WS";

        public CrwalerFactory(ILogger<CrwalerFactory> logger) => _logger = logger;

        public async IAsyncEnumerable<ValueTuple<string, byte>> ExecuteAsync(string url)
        {
            byte alphabet = 0;
            await foreach (var page in GetRecordsAsync(url))
            {
                yield return (page, alphabet++);
                _logger.LogTrace("Crwaler: " + (char)(65 + alphabet - 1) + " has passed to repo.");
            }
        }
        
        private async Task VirtueOfCrawling(double interval)
        {
            await Task.Delay(TimeSpan.FromSeconds(interval));
        }

        private async IAsyncEnumerable<string> GetRecordsAsync(string url)
        {
            var record = new StringBuilder();
            var pageOfAlphabet = new StringBuilder();

            await foreach (var document in GetAlphabetDocumentsAsync(url))
            {
                int? numOfPlayers = GetNumberOfPlayerNodes(document);
                numOfPlayers ??= 0;

                pageOfAlphabet.AppendLine(_scoreTitle);
                var players = new BasketballPlayer[(int)numOfPlayers];

                for (int i = 0; i < numOfPlayers; i++)
                {
                    await IterateEveryPlayer(document, players, i);
                    ConvertCsv(players[i], record);

                    pageOfAlphabet.Append(record + "\n");
                    record.Clear();
                }
                yield return pageOfAlphabet.ToString();
                pageOfAlphabet.Clear();
            }
        }

        private async Task IterateEveryPlayer(HtmlDocument document, BasketballPlayer[] players, int i)
        {
            var node = GetNodeOfPlayer(document, in i);
            SetPlayer(ref players[i], node);
            
            var href = node[0].Attributes["href"].Value;
            await FillPlayerScore(players[i], href);
        }

        private async Task FillPlayerScore(BasketballPlayer player, string playerHref)
        {
            int index = 0;
            await foreach (var score in GetDetailPageAsync(playerHref))
            {
                //await VirtueOfCrawling(interval: 0.7);

                if (score == null) continue;
                player.Scores[index++] = score;
            }
        }

        private void SetPlayer(ref BasketballPlayer player, HtmlNodeCollection playerNode)
        {
            player = new BasketballPlayer();
            player.Name = playerNode[0].InnerText;
            player.Scores = new string[10];
        }

        private void ConvertCsv(BasketballPlayer players, StringBuilder sb)
        {
            sb.Append(players.Name + ',');
            for (int i = 0; i < players.Scores.Count; i++)
                sb.Append(players.Scores[i] + ',');

            sb.Remove(sb.Length - 1, 1);
        }

        private async IAsyncEnumerable<HtmlDocument> GetAlphabetDocumentsAsync(string url)
        {
            byte alphabet = 0;
            for (int i = 0; i < 26; i++)
            {
                //await VirtueOfCrawling(interval: 1.5);
                _logger.LogTrace("Crwaler: " + (char)(65 + alphabet++) + " is started.");

                yield return await _web.LoadFromWebAsync(url + "/" + (char)(97 + i), Encoding.UTF8);
            }
        }

        private async IAsyncEnumerable<string> GetDetailPageAsync(string suburl)
        {
            var detailDocument = await _web.LoadFromWebAsync(_rootWebUrl + suburl, Encoding.ASCII);
            for (int row = 2; row <= 4; row++)
            {
                for (int col = 1; col <= 4; col++)
                {
                    yield return GetPlayerEachScore(detailDocument, in row, in col);
                }
            }
        }

        private string GetPlayerEachScore(HtmlDocument detailDocument, in int row, in int col)
        {
            return detailDocument.DocumentNode.SelectNodes($"//*[@id='info']/div[4]/div[{row}]/div[{col}]/p[2]")?[0].InnerText;
        }

        private HtmlNodeCollection GetNodeOfPlayer(HtmlDocument document, in int i)
        {
            return document.DocumentNode.SelectNodes($"//*[@id='players']/tbody/tr[{i + 1}]/th/a")
                ?? document.DocumentNode.SelectNodes($"//*[@id='players']/tbody/tr[{i + 1}]/th/strong/a");
        }

        private int? GetNumberOfPlayerNodes(HtmlDocument document)
        {
            return document.DocumentNode.SelectNodes($"//*[@id='players']/tbody/tr")?.Count;
        }
    }
}
