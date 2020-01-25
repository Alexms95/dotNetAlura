using Alura.ListaLeitura.App.Negocio;
using Alura.ListaLeitura.App.Repositorio;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Linq;
using System.IO;

namespace Alura.ListaLeitura.App
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();
        }
        public void Configure(IApplicationBuilder app)
        {
            RouteBuilder builder = new RouteBuilder(app);
            builder.MapRoute("Livros/ParaLer", LivrosParaLer);
            builder.MapRoute("Livros/Lendo", LivrosLendo);
            builder.MapRoute("Livros/Lido", LivrosLidos);
            builder.MapRoute("Cadastro/NovoLivro/{nome}/{autor}", NovoLivroParaLer);
            builder.MapRoute("Livros/Detalhes/{id:int}", ExibirDetalhes);
            builder.MapRoute("Cadastro/NovoLivro", ExibeFormulario);
            builder.MapRoute("Cadastro/Incluir", ProcessaFormulario);
            IRouter rotas = builder.Build();

            app.UseRouter(rotas);
            //app.Run(Roteamento);
        }

        private Task ProcessaFormulario(HttpContext context)
        {
            Livro livro = new Livro
            {
                Titulo = context.Request.Query["Titulo"].First(),
                Autor = context.Request.Query["Autor"].First()
            };
            LivroRepositorioCSV repo = new LivroRepositorioCSV();
            repo.Incluir(livro);
            return context.Response.WriteAsync("O livro foi adicionado com sucesso");
        }

        private Task ExibeFormulario(HttpContext context)
        {
            string formulario = CarregaArquivoHtml("formulario");
            return context.Response.WriteAsync(formulario);
        }

        private string CarregaArquivoHtml(string nomeArquivo)
        {
            string nomeCompletoArquivo = $"HTML/{nomeArquivo}.html";
            using (var arquivo = File.OpenText(nomeCompletoArquivo))
            {
                return arquivo.ReadToEnd();
            };
        }

        private Task ExibirDetalhes(HttpContext context)
        {
            int id = Convert.ToInt32(context.GetRouteValue("id"));
            LivroRepositorioCSV repo = new LivroRepositorioCSV();
            Livro livro = repo.Todos.First(l => l.Id == id);

            return context.Response.WriteAsync(livro.Detalhes());
        }

        private Task NovoLivroParaLer(HttpContext context)
        {
            Livro livro = new Livro
            {
                Titulo = context.GetRouteValue("nome").ToString(),
                Autor = context.GetRouteValue("autor").ToString()
            };
            LivroRepositorioCSV repo = new LivroRepositorioCSV();
            repo.Incluir(livro);
            return context.Response.WriteAsync("O livro foi adicionado com sucesso");
        }

        public Task Roteamento(HttpContext context)
        {
            LivroRepositorioCSV _repo = new LivroRepositorioCSV();
            Dictionary<string, RequestDelegate> caminhosAtendidos = new Dictionary<string, RequestDelegate>
            {
                { "/Livros/ParaLer", LivrosParaLer },
                { "/Livros/Lendo", LivrosLendo },
                { "/Livros/Lido", LivrosLidos }
            };

            if (caminhosAtendidos.ContainsKey(context.Request.Path))
            {
                return caminhosAtendidos[context.Request.Path].Invoke(context);
            }

            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            return context.Response.WriteAsync("Caminho não encontrado");
        }

        public Task LivrosParaLer(HttpContext context)
        {
            var _repo = new LivroRepositorioCSV();
            return context.Response.WriteAsync(_repo.ParaLer.ToString());
        }

        public Task LivrosLendo(HttpContext context)
        {
            var _repo = new LivroRepositorioCSV();
            return context.Response.WriteAsync(_repo.Lendo.ToString());
        }

        public Task LivrosLidos(HttpContext context)
        {
            var _repo = new LivroRepositorioCSV();
            return context.Response.WriteAsync(_repo.Lidos.ToString());
        }
    }
}