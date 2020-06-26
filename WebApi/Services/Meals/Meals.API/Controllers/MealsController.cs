using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Meals.API.Domains;
using Meals.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Meals.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MealsController : ControllerBase
    {
        private readonly IMealDataModel _mealDataModel;
        private readonly ILogger<MealsController> _logger;

        public MealsController(IMealDataModel mealDataModel, ILogger<MealsController> logger)
        {
            _mealDataModel = mealDataModel;
            _logger = logger;
        }

        [HttpGet("get-meals")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<List<Meal>>> GetMeals()
        {
            try
            {
                var meals = await _mealDataModel.FindAllAsync().ConfigureAwait(true);
                return Ok(meals);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, GetType().Name);
                throw;
            }
        }

        [HttpGet("get-meal/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<Meal>> GetMeal(uint id)
        {
            try
            {
                if (id < 1)
                    return BadRequest(id);

                var meal = await _mealDataModel.FindAsync(id).ConfigureAwait(true);
                if (meal != null)
                    return Ok(meal);

                return NotFound(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, GetType().Name);
                throw;
            }
        }

        [HttpPost("add-meal")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<Meal>> AddMeal(Meal item)
        {
            try
            {
                if (item == null)
                    return BadRequest(item);

                var meal = await _mealDataModel.InsertAsync(item).ConfigureAwait(true);
                return CreatedAtAction(nameof(AddMeal), new { id = meal.Id }, meal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, GetType().Name);
                throw;
            }
        }

        [HttpPut("update-meal/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<Meal>> UpdateMeal(uint id, Meal item)
        {
            try
            {
                if (id < 1 || item == null)
                    return BadRequest(id);

                var meal = await _mealDataModel.FindAsync(id).ConfigureAwait(true);
                if (meal != null)
                {
                    meal = await _mealDataModel.UpdateAsync(item).ConfigureAwait(true);
                    return Ok(id);
                }
                _logger.LogInformation($"Meal with id <{id}> not exist, create it.");
                return await AddMeal(item).ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, GetType().Name);
                throw;
            }
        }

        [HttpDelete("delete-meal/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<Meal>> DeleteMeal(uint id)
        {
            try
            {
                if (id < 1)
                    return BadRequest(id);

                var item = await _mealDataModel.FindAsync(id).ConfigureAwait(true);
                if (item != null)
                {
                    await _mealDataModel.DeleteAsync(id).ConfigureAwait(true);
                    return CreatedAtAction(nameof(Meal), new { id }, item);
                }
                return NotFound(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, GetType().Name);
                throw;
            }
        }
    }
}
