using Microsoft.AspNetCore.Mvc;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;

namespace TrilhaApiDesafio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly OrganizadorContext _context;

        public TarefaController(OrganizadorContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
            var tarefa = _context.Tarefas.Find(id);
            if (tarefa == null)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos()
        {
            var tarefas = _context.Tarefas.ToList();
            if (tarefas.Count == 0)
            {
                return NoContent(); 
            }
            return Ok(tarefas);
        }
        [HttpGet("ObterPorTitulo")]
        public IActionResult ObterPorTitulo(string titulo)
        {
            var tarefas = _context.Tarefas
                .Where(t => t.Titulo.Contains(titulo))
                .ToList();
            if (tarefas.Count == 0)
            {
                return NotFound();
            }
            return Ok(tarefas);
        }

        [HttpGet("ObterPorData")]
        public IActionResult ObterPorData(DateTime data)
        {
            var tarefa = _context.Tarefas.Where(x => x.Data.Date == data.Date);
            return Ok(tarefa);
        }

        [HttpGet("ObterPorStatus")]
        public IActionResult ObterPorStatus(EnumStatusTarefa status)
        {
            var tarefas = _context.Tarefas
                .Where(t => t.Status == status)
                .ToList();
            if (tarefas.Count == 0)
            {
                return NotFound();
            }
            return Ok(tarefas);
        }

       [HttpPost]
        public IActionResult Criar([FromBody] Tarefa tarefa) 
        {
            if (tarefa.Data == DateTime.MinValue)
            {
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });
            }
            _context.Tarefas.Add(tarefa);

            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Erro = "Erro ao salvar a tarefa: " + ex.Message });
            }
            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, [FromBody] Tarefa tarefaAtualizada)
        {
            var tarefaBanco = _context.Tarefas.Find(id);
            if (tarefaBanco == null)
            {
                return NotFound(); 
            }
            if (tarefaAtualizada.Data == DateTime.MinValue)
            {
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });
            }
            tarefaBanco.Titulo = tarefaAtualizada.Titulo;
            tarefaBanco.Descricao = tarefaAtualizada.Descricao;
            tarefaBanco.Data = tarefaAtualizada.Data;
            tarefaBanco.Status = tarefaAtualizada.Status;
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Erro = "Erro ao atualizar a tarefa: " + ex.Message });
            }
            return Ok();
        }

         [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            var tarefaBanco = _context.Tarefas.Find(id);
            if (tarefaBanco == null)
            {
                return NotFound(); 
            }
            _context.Tarefas.Remove(tarefaBanco);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Erro = "Erro ao remover a tarefa: " + ex.Message });
            }
            return NoContent();
        }
    }
}
