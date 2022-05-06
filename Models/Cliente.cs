using System.ComponentModel.DataAnnotations;

namespace DesafioSidequestMinimalAPI.Models;

public class Cliente : Entity
{
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [StringLength(400, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
    public string? Nome { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [StringLength(14, ErrorMessage = "O campo {0} precisa ter {1} caracteres")]
    public string? Documento { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [EmailAddress(ErrorMessage = "E-mail em formato inválido")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    public string? Telefone { get; set; }
}