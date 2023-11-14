using API_Models.Models;
using Microsoft.AspNetCore.Mvc;
using Nest;
using Elasticsearch.Net;

namespace API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class PermissionsController : ControllerBase
    {
        private readonly BackendSrContext _dbContext;
        private readonly IElasticClient _elasticClient;

        public PermissionsController(BackendSrContext dbContext, IElasticClient elasticClient)
        {
            _dbContext = dbContext;
            _elasticClient = elasticClient;

            // Verificar y crear el índice si no existe
            //EnsureIndexExists();
        }

        //private void EnsureIndexExists()
        //{
        //    var indexName = "apiindex";

        //    var settings = new ConnectionSettings(new Uri("http://localhost:9200/"))
        //        .DefaultIndex(indexName);

        //    var client = new ElasticClient(settings);

        //    // Verificar si el índice existe
        //    var indexExistsResponse = client.IndexExists(indexName);

        //    if (!indexExistsResponse.Exists)
        //    {
        //        // Si no existe, crear el índice
        //        var createIndexResponse = client.CreateIndex(indexName, c => c
        //            .Mappings(ms => ms
        //                .Map<TblPermission>(m => m.AutoMap())
        //            )
        //        );

        //        if (!createIndexResponse.IsValid)
        //        {
        //            // Manejar el caso de error al crear el índice
        //            throw new InvalidOperationException($"Error al crear el índice {indexName}: {createIndexResponse.DebugInformation}");
        //        }
        //    }
        //}

        [HttpPost]
        [Route("RequestPermission")]
        public IActionResult RequestPermission([FromBody] TblPermission permission)
        {
            try
            {
                using (var context = new BackendSrContext())
                {
                    // Lógica para agregar un nuevo permiso a la base de datos SQL
                    context.TblPermissions.Add(permission);
                    context.SaveChanges();
                }

                // Indexar en Elasticsearch
                var settings = new ConnectionSettings(new Uri("http://localhost:9200/"))
                    .DefaultIndex("apiindex");
                var client = new ElasticClient(settings);

                var indexResponse = client.IndexDocument(permission);

                return Ok("Permiso solicitado correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al solicitar el permiso: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetPermissions")]
        public IActionResult GetPermissions()
        {
            try
            {
                List<TblPermission> permissions;

                using (var context = new BackendSrContext())
                {
                    // Lógica para obtener todos los permisos de la base de datos SQL
                    permissions = context.TblPermissions.ToList();
                }

                // Buscar en Elasticsearch
                var settings = new ConnectionSettings(new Uri("http://localhost:9200/"))
                    .DefaultIndex("apiindex");
                var client = new ElasticClient(settings);

                var searchResponse = client.Search<TblPermission>(s => s
                    .Query(q => q.MatchAll())
                );

                var elasticsearchPermissions = searchResponse.Documents.ToList();

                return Ok(elasticsearchPermissions);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener los permisos: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("ModifyPermission/{id}")]
        public IActionResult ModifyPermission(int id, [FromBody] TblPermission modifiedPermission)
        {
            try
            {
                using (var context = new BackendSrContext())
                {
                    // Lógica para modificar un permiso en la base de datos SQL
                    var existingPermission = context.TblPermissions.Find(id);

                    if (existingPermission == null)
                    {
                        return NotFound($"Permiso con ID {id} no encontrado");
                    }

                    // Actualizar propiedades del permiso existente
                    existingPermission.EmployeeForename = modifiedPermission.EmployeeForename;
                    existingPermission.EmployeeSurename = modifiedPermission.EmployeeSurename;
                    existingPermission.PermissionDate = modifiedPermission.PermissionDate;
                    existingPermission.PermissionType = modifiedPermission.PermissionType;

                    context.SaveChanges();
                }

                // Actualizar en Elasticsearch
                var settings = new ConnectionSettings(new Uri("http://localhost:9200/"))
                    .DefaultIndex("apiindex");
                var client = new ElasticClient(settings);

                var updateResponse = client.Update<TblPermission, object>(id, u => u
                    .Doc(modifiedPermission)
                );

                return Ok("Permiso modificado correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al modificar el permiso: {ex.Message}");
            }
        }

    }
}