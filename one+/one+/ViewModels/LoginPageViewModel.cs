// ViewModels/LoginPageViewModel.cs
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using one_.Services; // Asegúrate de tener el namespace correcto

namespace one_.ViewModels
{
    public partial class LoginPageViewModel : ObservableObject
    {
        private readonly AuthService _authService;
        private readonly FirestoreService _firestoreService; // Si necesitas Firestore en el login

        [ObservableProperty]
        private string _email;

        [ObservableProperty]
        private string _password;

        [ObservableProperty]
        private string _message;

        public LoginPageViewModel(AuthService authService, FirestoreService firestoreService)
        {
            _authService = authService;
            _firestoreService = firestoreService; // Inyecta el servicio de Firestore
        }

        [RelayCommand]
        private async Task Login()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                Message = "Por favor, ingresa correo y contraseña.";
                return;
            }

            Message = "Iniciando sesión...";
            var (success, msg) = await _authService.LoginUserAsync(Email, Password);
            Message = msg;

            if (success)
            {
                // Navegar a la página principal o realizar alguna acción post-login
                Console.WriteLine($"Usuario logeado: {_authService.GetCurrentUserId()}");
                // Ejemplo de navegación simple (esto dependerá de tu arquitectura de navegación)
                // await Shell.Current.GoToAsync("//HomePage");
            }
        }

        [RelayCommand]
        private async Task Register()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                Message = "Por favor, ingresa correo y contraseña para registrarte.";
                return;
            }

            Message = "Registrando usuario...";
            var (success, msg) = await _authService.RegisterUserAsync(Email, Password);
            Message = msg;

            if (success)
            {
                // Opcional: Guardar datos iniciales en Firestore después del registro
                if (_authService.IsUserLoggedIn())
                {
                    var userId = _authService.GetCurrentUserId();
                    if (!string.IsNullOrEmpty(userId))
                    {
                        var userData = new Dictionary<string, object>
                        {
                            { "email", Email },
                            { "createdAt", DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() }
                        };
                        await _firestoreService.SaveUserDataAsync(userId, userData);
                        Message += "\nDatos de usuario guardados en Firestore.";
                    }
                }
            }
        }
    }
}