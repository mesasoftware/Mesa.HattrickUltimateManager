namespace Mesa.HUM.Application.Behaviors
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentValidation;
    using MediatR;
    using Mesa.HUM.Domain.Common;

    internal sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest , TResponse>
        where TRequest : notnull
        where TResponse : Result
    {
        /// <summary>
        /// Resolved once per closed TResponse.
        /// </summary>
        /// <remarks>
        /// Both Result and Result<T> expose a public static Failure ( Error ) declared on the type itself,
        /// so DeclaredOnly avoids the AmbiguousMatchException the hidden base method would otherwise cause.
        /// </remarks>
        private static readonly MethodInfo FailureMethod = typeof ( TResponse ).GetMethod (
            nameof ( Result.Failure ) ,
            BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly ,
            binder: null ,
            types: [ typeof ( Error ) ] ,
            modifiers: null )!;

        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior ( IEnumerable<IValidator<TRequest>> validators )
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle ( TRequest request , RequestHandlerDelegate<TResponse> next , CancellationToken cancellationToken )
        {
            if ( !_validators.Any ( ) )
            {
                return await next ( cancellationToken );
            }

            var context = new ValidationContext<TRequest> ( request );

            var results = await Task.WhenAll (
                _validators.Select ( v => v.ValidateAsync ( context , cancellationToken ) ) );

            var failures = results
                .SelectMany ( r => r.Errors )
                .Where ( f => f is not null )
                .ToList ( );

            if ( failures.Count == 0 )
            {
                return await next ( cancellationToken );
            }

            string description = string.Join ( " " , failures.Select ( f => f.ErrorMessage ) );

            return CreateFailure ( Error.Validation ( "VALIDATION_ERROR" , description ) );
        }

        private static TResponse CreateFailure ( Error error )
        {
            return ( TResponse ) FailureMethod.Invoke ( obj: null , parameters: [ error ] )!;
        }
    }
}