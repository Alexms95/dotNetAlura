using Alura.ListaLeitura.App.HTML;
using Alura.ListaLeitura.App.Negocio;
using Alura.ListaLeitura.App.Repositorio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alura.ListaLeitura.App.Logica
{
    public class LivrosLogica
    {
        public static Task ExibirDetalhes(HttpContext context)
        {
            int id = Convert.ToInt32(context.GetRouteValue("id"));
            LivroRepositorioCSV repo = new LivroRepositorioCSV();
            Livro livro = repo.Todos.First(l => l.Id == id);

            return context.Response.WriteAsync(livro.Detalhes());
        }

        public static string CarregaLista(IEnumerable<Livro> livros)
        {
            var conteudoArquivo = HtmlUtils.CarregaArquivoHtml("lista");

            foreach (var livro in livros)
            {
                conteudoArquivo = conteudoArquivo.Replace("#NOVOITEM#", $"<li>{livro.Titulo} - {livro.Autor}</li>#NOVOITEM#");
            }

            return conteudoArquivo.Replace("#NOVOITEM#", "");
        }

        public static Task LivrosParaLer(HttpContext context)
        {
            var _repo = new LivroRepositorioCSV();
            var html = CarregaLista(_repo.ParaLer.Livros);
            return context.Response.WriteAsync(html);
        }


        public static Task LivrosLendo(HttpContext context)
        {
            var _repo = new LivroRepositorioCSV();
            var html = CarregaLista(_repo.Lendo.Livros);
            return context.Response.WriteAsync(html);
        }

        public static Task LivrosLidos(HttpContext context)
        {
            var _repo = new LivroRepositorioCSV();
            var html = CarregaLista(_repo.Lidos.Livros);
            return context.Response.WriteAsync(html);
        }
    }
}
