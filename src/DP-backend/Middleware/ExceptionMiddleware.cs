﻿using DP_backend.Common.Exceptions;
using DP_backend.Models.DTOs;

namespace DP_backend.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);

                if (context.Response.StatusCode == 401)
                {
                    await context.Response.WriteAsJsonAsync(new ErrorDto { Status = "401", Message = "Unauthorized" });
                }
            }
            catch (BadDataException e)
            {
                _logger.LogInformation(e, "Ожидаемая ошибка обработана ExceptionMiddleware");
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(new ErrorDto { Status = "400", Message = e.Message });
            }
            catch (NotFoundException e)
            {
                _logger.LogInformation(e, "Ожидаемая ошибка обработана ExceptionMiddleware");
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsJsonAsync(new ErrorDto { Status = "404", Message = e.Message });
            }
            catch (NoPermissionException e)
            {
                _logger.LogInformation(e, "Ожидаемая ошибка обработана ExceptionMiddleware");
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsJsonAsync(new ErrorDto { Status = "403", Message = e.Message });
            }
            catch (InvalidOperationException e)
            {
                _logger.LogInformation(e, "Ожидаемая ошибка обработана ExceptionMiddleware");
                context.Response.StatusCode = StatusCodes.Status409Conflict;
                await context.Response.WriteAsJsonAsync(new ErrorDto { Status = "409", Message = e.Message });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Неизвестная ошибка обработана ExceptionMiddleware");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(new ErrorDto { Status = "500", Message = e.Message });
            }
        }
    }

}
