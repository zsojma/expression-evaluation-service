using System;
using ExpressionEvaluation.Core.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ExpressionEvaluation.Api.Controllers
{
    /// <summary>
    /// Evaluates expressions in input string
    /// </summary>
    [ApiController]
    [Route("")]
    public class ExpressionEvaluationController : ControllerBase
    {
        private readonly IEvaluator _evaluator;
        private readonly ILogger<ExpressionEvaluationController> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="evaluator">Expression evaluation functionality</param>
        /// <param name="logger">Logger</param>
        public ExpressionEvaluationController(IEvaluator evaluator, ILogger<ExpressionEvaluationController> logger)
        {
            _evaluator = evaluator;
            _logger = logger;
        }

        /// <summary>
        /// Evaluates expression in input query string
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
                return Ok(_evaluator.Compute(expr));
            }
            catch (EvaluatorException ex)
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
