using Alura.ListaLeitura.App.HTML;
using Alura.ListaLeitura.App.Negocio;
using Alura.ListaLeitura.App.Repositorio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alura.ListaLeitura.App.Logica
{
    public class CadastroLogica
    {
        public static Task ProcessaFormulario(HttpContext context)
        {
            Livro livro = new Livro
            {
                Titulo = context.Request.Form["Titulo"].First(),
                Autor = context.Request.Form["Autor"].First()
            };
            LivroRepositorioCSV repo = new LivroRepositorioCSV();
            repo.Incluir(livro);
            return context.Response.WriteAsync("O livro foi adicionado com sucesso");
        }

        public static Task ExibeFormulario(HttpContext context)
        {
            string formulario = HtmlUtils.CarregaArquivoHtml("formulario");
            return context.Response.WriteAsync(formulario);
        }

        public static Task NovoLivroParaLer(HttpContext context)
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
    }
}
