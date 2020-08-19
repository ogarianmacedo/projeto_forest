using System.ComponentModel.DataAnnotations;

namespace ProjetoForest.DTO
{
    public class UserLoginDTO
    {
        [Required(ErrorMessage="Campo {0} obrigatório!")]
        public string UserName { get; set; }

        [Required(ErrorMessage="Campo {0} obrigatório!")]
        public string Password { get; set; }
    }
}