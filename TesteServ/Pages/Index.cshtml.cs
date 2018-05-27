using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TesteServ.Model;

namespace TesteServ.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public Requisicao Requisicao { get; set; }

        public List<Thread> threadsInstanciadas = new List<Thread>();

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            for (int i = 1; i <= Requisicao.QuantidadeRequisicoes; i++)
            {
                if (!Requisicao.Assincrono)
                {
                    ThreadStart ts = new ThreadStart(SomarTempo);
                    Thread thread = new Thread(ts);
                    thread.IsBackground = true;
                    threadsInstanciadas.Add(thread);
                    //thread.Start();
                }
                else
                    SomarTempo();

            }

            foreach (Thread curThread in threadsInstanciadas)
                curThread.Start();

            foreach (Thread curThread in threadsInstanciadas)
                curThread.Join();


            stopwatch.Stop();
            Requisicao.TempoDeRespostaEmMS = stopwatch.ElapsedMilliseconds;

            return Page();
        }

        private void SomarTempo()
        {
            Requisicao.TempoTotalEmMS = Requisicao.TempoTotalEmMS + EnviarRequisicoesGETAsync(Requisicao).Result;
        }

        private async Task<long> EnviarRequisicoesGETAsync(Requisicao requisicao)
        {
            var r = await ObterDados(requisicao.Url);
            return r;
        }

        private async Task<long> ObterDados(string url)
        {
            using (var client = new HttpClient())
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                using (var response = await client.GetAsync(new Uri(url)))
                {
                    stopwatch.Stop();
                    string result = await response.Content.ReadAsStringAsync();
                    SalvarResultado(result);
                    //return result;
                    return stopwatch.ElapsedMilliseconds;
                }
            }
        }

        private void SalvarResultado(string result)
        {
            string folder = String.Concat(@"Resultados", DateTime.Now.ToString("dd MM yyyy HH mm"));
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            FileStream fs = new FileStream(String.Concat(folder, "/Resultado", DateTime.Now.Ticks, ".txt"),
                FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(result);
            sw.Flush();
            sw.Close();
            fs.Close();
        }

        //private void EnviaRequisicaoPOST()
        //{
        //    string dadosPOST = "title=macoratti";
        //    dadosPOST = dadosPOST + "&body=teste de envio de post";
        //    dadosPOST = dadosPOST + "&userId=1";
        //    var dados = Encoding.UTF8.GetBytes(dadosPOST);
        //    var requisicaoWeb = WebRequest.CreateHttp("http://jsonplaceholder.typicode.com/posts");
        //    requisicaoWeb.Method = "POST";
        //    requisicaoWeb.ContentType = "application/x-www-form-urlencoded";
        //    requisicaoWeb.ContentLength = dados.Length;
        //    requisicaoWeb.UserAgent = "RequisicaoWebDemo";
        //    //precisamos escrever os dados post para o stream
        //    using (var stream = requisicaoWeb.GetRequestStream())
        //    {
        //        stream.Write(dados, 0, dados.Length);
        //        stream.Close();
        //    }
        //    //ler e exibir a resposta
        //    using (var resposta = requisicaoWeb.GetResponse())
        //    {
        //        var streamDados = resposta.GetResponseStream();
        //        StreamReader reader = new StreamReader(streamDados);
        //        object objResponse = reader.ReadToEnd();
        //        //var post = JsonConvert.DeserializeObject<Post>(objResponse.ToString());
        //        streamDados.Close();
        //        resposta.Close();
        //    }
        //    Console.ReadLine();
        //}

        public void OnGet()
        {
            Requisicao = new Requisicao
            {
                QuantidadeRequisicoes = 1,
                TempoDeRespostaEmMS = 0,
                TempoTotalEmMS = 0
            };
        }
    }
}
