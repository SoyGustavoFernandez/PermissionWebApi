using API_Models.Models;
using Microsoft.AspNetCore.Mvc;
using Nest;
using Newtonsoft.Json;
using Serilog;

namespace API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class PermissionsController : ControllerBase
    {
        private readonly BackendSrContext _dbContext;
        private readonly IElasticClient _elasticClient;
        private readonly Serilog.ILogger _logger;

        public PermissionsController(BackendSrContext dbContext, IElasticClient elasticClient, Serilog.ILogger logger)
        {
            _dbContext = dbContext;
            _elasticClient = elasticClient;
            _logger = logger;
        }

        [HttpPost]
        [Route("RequestPermission")]
        public IActionResult RequestPermission([FromBody] TblPermission permission)
        {
            try
            {
                // Lógica para agregar un nuevo permiso a la base de datos SQL
                _dbContext.TblPermissions.Add(permission);
                _dbContext.SaveChanges();

                // Indexar en Elasticsearch
                var indexResponse = _elasticClient.IndexDocument(permission);

                _logger.Information("Permission requested correctly");
                return Ok("Permission requested correctly");
            }
            catch (Exception ex)
            {
                _logger.Error($"Error requesting permission: {ex.Message}");
                return BadRequest($"Error requesting permission: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetPermissions")]
        public IActionResult GetPermissions()
        {
            try
            {
                List<TblPermission> permissions;

                // Lógica para obtener todos los permisos de la base de datos SQL
                permissions = _dbContext.TblPermissions.ToList();

                // Buscar en Elasticsearch
                var searchResponse = _elasticClient.Search<TblPermission>(s => s
                    .Query(q => q.MatchAll()));

                var elasticsearchPermissions = searchResponse.Documents.ToList();

                return Ok(elasticsearchPermissions);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error obtaining permissions: {ex.Message}");
                return BadRequest($"Error obtaining permissions: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("ModifyPermission/{id}")]
        public IActionResult ModifyPermission(int id, [FromBody] TblPermission modifiedPermission)
        {
            try
            {
                // Lógica para modificar un permiso en la base de datos SQL
                var existingPermission = _dbContext.TblPermissions.Find(id);

                if (existingPermission == null)
                {
                    _logger.Warning($"Permission with ID {id} not found");
                    return NotFound($"Permission with ID {id} not found");
                }

                // Actualizar propiedades del permiso existente
                existingPermission.EmployeeForename = modifiedPermission.EmployeeForename;
                existingPermission.EmployeeSurename = modifiedPermission.EmployeeSurename;
                existingPermission.PermissionDate = modifiedPermission.PermissionDate;
                existingPermission.PermissionType = modifiedPermission.PermissionType;

                _dbContext.SaveChanges();

                // Actualizar en Elasticsearch
                var updateResponse = _elasticClient.Update<TblPermission, object>(id, u => u
                    .Doc(modifiedPermission)
                );

                _logger.Information($"Successfully modified permission");
                return Ok("Successfully modified permission");
            }
            catch (Exception ex)
            {
                _logger.Error($"Error modifying permission: {ex.Message}");
                return BadRequest($"Error modifying permission: {ex.Message}");
            }
        }
    }
}