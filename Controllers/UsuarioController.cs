using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetoForest.DTO;
using ProjetoForest.Interfaces;

namespace ProjetoForest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuario _usuarioRepository;
        private readonly IMapper _mapper;

        public UsuarioController(IUsuario usuarioRepository, IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
        }

        [HttpGet()]
        [Authorize(Roles = "ADMINISTRADOR")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var users = await _usuarioRepository.GetAllUsers();
                var result = _mapper.Map<UserDTO[]>(users);
                return Ok(
                    new { users = result }
                );
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(500, $"Erro ao recuperar dados! {ex.Message}");
            }
        }

    }
}