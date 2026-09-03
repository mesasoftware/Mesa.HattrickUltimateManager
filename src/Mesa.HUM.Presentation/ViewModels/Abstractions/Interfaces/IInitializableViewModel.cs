namespace Mesa.HUM.Presentation.ViewModels.Abstractions.Interfaces
{
    using System.Threading;
    using System.Threading.Tasks;

    internal interface IInitializableViewModel
    {
        Task InitializeAsync ( CancellationToken cancellationToken = default );
    }
}