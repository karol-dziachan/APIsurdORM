using __ProjectName__.Api.Abstraction;
using __ProjectName__.Application.Features.__Entity__.Commands.Add__Entity__;
using __ProjectName__.Application.Features.__Entity__.Commands.Remove__Entities__ByParameters;
using __ProjectName__.Application.Features.__Entity__.Commands.Remove__Entity__;
using __ProjectName__.Application.Features.__Entity__.Commands.Update__Entity__;
using __ProjectName__.Application.Features.__Entity__.Queries.Count__Entities__;
using __ProjectName__.Application.Features.__Entity__.Queries.Find__Entities__;
using __ProjectName__.Application.Features.__Entity__.Queries.Get__Entities__ByParameter;
using __ProjectName__.Application.Features.__Entity__.Queries.Get__Entity__ById;
using __ProjectName__.Application.Features.__Entity__.Queries.GetAll__Entity__;
using __ProjectName__.Application.Features.__Entity__.Queries.GetPaged__Entities__;
using __ProjectName__.Application.Features.__Entity__.Queries.IsConcrete__Entity__Exist;
using __ProjectName__.Common.Extensions;
using __ProjectName__.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace __ProjectName__.Api.Controllers
{
    [Route("api/v1/[Controller]")]
    [ApiController]
    public class __Entity__Controller : BaseController
    {
        /// <summary>
        ///   Get all __Entities__
        /// </summary>
        /// <returns>List of __Entities__</returns>
        /// <response code="200">If everything is ok</response>
        /// <response code="403">If the user is not authorization</response>
        /// <response code="404">If the book not found</response>
        [HttpGet("/get-all-__Entities__")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<__ProjectName__.Application.Features.__Entity__.Queries.GetAll__Entities__.QueryResult>> GetAll__Entities__()
        {
            var query = new GetAll__Entities__Query() { };
            var response = await Mediator.Send(query);

            if(!response.Success)
            {
                return BadRequest(response);
            }

            if(response is null || !response.Data.Any())
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        /// <summary>
        ///   Get page with __Entities__
        /// </summary>
        /// <param name="pageNumber">Current page number</param>
        /// <param name="pageSize">Current page size</param>
        /// <returns>List of __Entities__</returns>
        /// <response code="200">If everything is ok</response>
        /// <response code="403">If the user is not authorization</response>
        /// <response code="404">If the book not found</response>
        [HttpGet("/get-page-with-__Entities__/{pageNumber}/{pageSize}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<__ProjectName__.Application.Features.__Entity__.Queries.GetAll__Entities__.QueryResult>> GetPageWith__Entities__([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var query = new GetPaged__Entities__Query(pageNumber, pageSize);
            var response = await Mediator.Send(query);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            if (response is null || !response.Data.Any())
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        /// <summary>
        ///   Get a __Entity__ by ID
        /// </summary>
        /// <param name="id">ID of the __Entity__</param>
        /// <returns>The requested __Entity__</returns>
        /// <response code="200">If everything is ok</response>
        /// <response code="403">If the user is not authorized</response>
        /// <response code="404">If the entity not found</response>
        /// <response code="400">If the request is invalid</response>
        [HttpGet("/get-__Entity__-by-id/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<__ProjectName__.Application.Features.__Entity__.Queries.Get__Entity__ById.QueryResult>> Get__Entity__ById(Guid id)
        {
            var query = new Get__Entity__ByIdQuery(id);
            var response = await Mediator.Send(query);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            if (response is null || response.Data is null)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        /// <summary>
        ///   Get __Entities__ by parameters
        /// </summary>
        /// <param name="parameters">Dictionary of parameters</param>
        /// <returns>List of __Entities__</returns>
        /// <response code="200">If everything is ok</response>
        /// <response code="403">If the user is not authorized</response>
        /// <response code="404">If no entities found</response>
        /// <response code="400">If the request is invalid</response>
        [HttpGet("/get-__Entities__-by-parameters")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<__ProjectName__.Application.Features.__Entity__.Queries.Get__Entities__ByParameter.QueryResult>> Get__Entities__ByParameters([FromQuery] Dictionary<string, string> parameters)
        {
            var query = new Get__Entities__ByParameterQuery(parameters);
            var response = await Mediator.Send(query);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            if (response is null || !response.Data.Any())
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        /// <summary>
        ///   Find __Entities__ by parameters
        /// </summary>
        /// <param name="parameter">A parameter for filtering the entities</param>
        /// <returns>List of __Entities__</returns>
        /// <response code="200">If everything is ok</response>
        /// <response code="403">If the user is not authorized</response>
        /// <response code="404">If no entities found</response>
        /// <response code="400">If the request is invalid</response>
        [HttpGet("/find-__Entities__-by-parameters")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<__ProjectName__.Application.Features.__Entity__.Queries.Find__Entities__.QueryResult>> Find__Entities__ByParameters([FromQuery] string parameters)
        {
            var query = new Find__Entities__Query(parameters.BuildPredicate<__Entity__>());
            var response = await Mediator.Send(query);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            if (response.Data == null || !response.Data.Any())
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        /// <summary>
        ///   Count the number of __Entities__ matching a specific parameter
        /// </summary>
        /// <param name="parameter">A parameter for filtering the entities</param>
        /// <returns>Count of matching __Entities__</returns>
        /// <response code="200">If everything is ok</response>
        /// <response code="403">If the user is not authorized</response>
        /// <response code="404">If no entities found</response>
        /// <response code="400">If the request is invalid</response>
        [HttpGet("/count-__Entities__-by-parameter")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Application.Features.__Entity__.Queries.Count__Entities__.QueryResult>> Count__Entities__ByParameter([FromQuery] string parameter)
        {
            var query = new Find__Entities__Query(parameter.BuildPredicate<__Entity__>());
            var response = await Mediator.Send(query);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            if (response.Data == null || !response.Data.Any())
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        /// <summary>
        ///   Check if a specific __Entity__ exists based on the provided parameter
        /// </summary>
        /// <param name="parameter">The parameter to check the existence of the __Entity__</param>
        /// <returns>True if the entity exists, otherwise false</returns>
        /// <response code="200">If the request is successful and the entity exists or not</response>
        /// <response code="403">If the user is not authorized</response>
        /// <response code="404">If the entity was not found</response>
        /// <response code="400">If the request is invalid</response>
        [HttpGet("/is-__Entity__-exist")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Application.Features.__Entity__.Queries.IsConcrete__Entity__Exist.QueryResult>> IsConcrete__Entity__Exist([FromQuery] string parameter)
        {
            var query = new Find__Entities__Query(parameter.BuildPredicate<__Entity__>());
            var response = await Mediator.Send(query);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response); // Entity exists
        }

        /// <summary>
        ///   Add a new __Entity__
        /// </summary>
        /// <param name="command">The command containing the data for the new __Entity__</param>
        /// <returns>The ID of the created __Entity__</returns>
        /// <response code="200">If the entity was successfully added</response>
        /// <response code="400">If the request data is invalid</response>
        /// <response code="403">If the user is not authorized</response>
        [HttpPost("/add-__Entity__")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<Application.Features.__Entity__.Commands.Add__Entity__.CommandResult>> Add__Entity__([FromBody] Add__Entity__Command command)
        {
            var result = await Mediator.Send(command);

            if (!result.Success)
            {
                return BadRequest(result.Message); 
            }

            return Ok(result.NewEntityId); 
        }

        /// <summary>
        ///   Remove __Entities__ based on the provided parameters
        /// </summary>
        /// <param name="parameters">The parameters used to identify the entities to be removed</param>
        /// <returns>A result indicating whether the entities were successfully removed</returns>
        /// <response code="200">If the entities were successfully removed</response>
        /// <response code="400">If the request data is invalid</response>
        /// <response code="403">If the user is not authorized</response>
        /// <response code="404">If no matching entities were found to remove</response>
        [HttpDelete("/remove-__Entities__-by-parameters")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Application.Features.__Entity__.Commands.Remove__Entities__ByParameters.CommandResult>> Remove__Entities__ByParameters([FromQuery] Dictionary<string, string> parameters)
        {
            var command = new Remove__Entities__ByParametersCommand(parameters);
            var result = await Mediator.Send(command);

            if (!result.Success)
            {
                return BadRequest(result.Message); 
            }

            if (result.AffectedRows == 0)
            {
                return NotFound("No entities found to remove."); 
            }

            return Ok(result); 
        }

        /// <summary>
        ///   Remove a specific __Entity__ by its ID
        /// </summary>
        /// <param name="id">The ID of the entity to be removed</param>
        /// <returns>A result indicating whether the entity was successfully removed</returns>
        /// <response code="200">If the entity was successfully removed</response>
        /// <response code="400">If the request data is invalid</response>
        /// <response code="403">If the user is not authorized</response>
        /// <response code="404">If the entity with the given ID was not found</response>
        [HttpDelete("/remove-__Entity__/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Application.Features.__Entity__.Commands.Remove__Entity__.CommandResult>> Remove__Entity__([FromRoute] Guid id)
        {
            var command = new Remove__Entity__Command(id);
            var result = await Mediator.Send(command);

            if (!result.Success)
            {
                return BadRequest(result.Message); 
            }

            if (result.DeletedId == Guid.Empty)
            {
                return NotFound(result); 
            }

            return Ok(result); 
        }

        /// <summary>
        ///   Update a __Entity__
        /// </summary>
        /// <param name="command">The command containing the data to update</param>
        /// <returns>The result of the update operation</returns>
        /// <response code="200">If the update is successful</response>
        /// <response code="400">If the request is invalid</response>
        /// <response code="404">If the entity to update is not found</response>
        [HttpPut("/update-__Entity__/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Application.Features.__Entity__.Commands.Update__Entity__.CommandResult>> Update__Entity__([FromRoute] Guid id, [FromBody] __Entity__ entity)
        {
            var command = new Update__Entity__Command(entity, id);
            var result = await Mediator.Send(command);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            if (!result.AffectedRows.HasValue)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

    }
}
