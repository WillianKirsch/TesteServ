using System.ComponentModel.DataAnnotations;

namespace TesteServ.Model
{
    public class Requisicao : ViewModel
    {
        [StringLength(2000, MinimumLength = 3, ErrorMessage = "A \"URL\" deve possuir entre {2} e {1} caracteres.")]
        [DataType(DataType.Url)]
        [Required(ErrorMessage = "O campo \"URL\" é obrigatório.")]
        public string Url { get; set; }

        [Range(1, 1000, ErrorMessage = "A \"Quantidade de requisições\" deve ser entre {1} e {2}.")]
        [Required(ErrorMessage = "O campo \"Quantidade de requisições\" é obrigatório.")]
        public int QuantidadeRequisicoes { get; set; }

        public bool Assincrono { get; set; }

        public long TempoTotalEmMS { get; set; }
        public long TempoDeRespostaEmMS { get; set; }
    }
}
