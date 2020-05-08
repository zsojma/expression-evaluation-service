using System;
using ExpressionEvaluation.Api.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ExpressionEvaluation.Api.Controllers
{
    /// <summary>
    /// Evaluates expressions in input string
    /// </summary>
    [ApiController]
    [Route("api/numeric")]
    public class NumericExpressionEvaluationController : ControllerBase
    {
        private readonly NumericExpressionEvaluationFacade _evaluationFacade;
        private readonly ILogger<NumericExpressionEvaluationController> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="evaluationFacade">Expression evaluation facade</param>
        /// <param name="logger">Logger</param>
        public NumericExpressionEvaluationController(NumericExpressionEvaluationFacade evaluationFacade, ILogger<NumericExpressionEvaluationController> logger)
        {
            _evaluationFacade = evaluationFacade;
            _logger = logger;
        }

        /// <summary>
        /// Evaluates numeric expression in input query string
        /// </summary>
        /// <param name="expr">Input query string</param>
        /// <returns>Computed value</returns>
        [HttpGet]
        [Route("compute")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Compute(string expr)
        {
            try
            {
                return Ok(_evaluationFacade.Evaluate(expr));
            }
            catch (ExpressionEvaluationFacadeException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Expression computation returned unknown exception!");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
