namespace Mesa.HUM.Application.Features.Profiles.Authorization.CompleteChppAuthorization
{
    using FluentValidation;

    internal class CompleteChppAuthorizationValidator : AbstractValidator<CompleteChppAuthorizationCommand>
    {
        public CompleteChppAuthorizationValidator ( )
        {
            RuleFor ( x => x.RequestToken )
                .NotNull ( );

            When ( x => x.RequestToken is not null , ( ) =>
            {
                RuleFor ( x => x.RequestToken.Token ).NotEmpty ( );
                RuleFor ( x => x.RequestToken.TokenSecret ).NotEmpty ( );
            } );

            RuleFor ( x => x.Verifier )
                .NotNull ( )
                .NotEmpty ( );
        }
    }
}