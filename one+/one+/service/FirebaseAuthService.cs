using System;
using Plugin.Firebase.Auth;
using System.Threading.Tasks;
namespace one_.Services // <<<< ¡AJUSTA AQUÍ!
{
    public class AuthService
    {
        private readonly IFirebaseAuth _firebaseAuth;

        public AuthService(IFirebaseAuth firebaseAuth)
        {
            _firebaseAuth = firebaseAuth;
        }

        public async Task<(bool Success, string Message)> RegisterUserAsync(string email, string password)
        {
            try
            {
                var user = await _firebaseAuth.CreateUserAsync(email, password);
                return (true, "Usuario registrado exitosamente.");
            }
            catch (FirebaseAuthException ex)
            {
                // Manejo de errores específicos de Firebase Auth
                return (false, $"Error al registrar: {ex.Reason} - {ex.InnerException?.Message}");
            }
            catch (Exception ex)
            {
                return (false, $"Error inesperado al registrar: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> LoginUserAsync(string email, string password)
        {
            try
            {
                var user = await _firebaseAuth.SignInAsync(email, password);
                return (true, "Inicio de sesión exitoso.");
            }
            catch (FirebaseAuthException ex)
            {
                // Manejo de errores específicos de Firebase Auth
                return (false, $"Error al iniciar sesión: {ex.Reason} - {ex.InnerException?.Message}");
            }
            catch (Exception ex)
            {
                return (false, $"Error inesperado al iniciar sesión: {ex.Message}");
            }
        }

        public async Task LogoutUserAsync()
        {
            await _firebaseAuth.SignOutAsync();
        }

        public bool IsUserLoggedIn()
        {
            return _firebaseAuth.CurrentUser != null;
        }

        public string GetCurrentUserId()
        {
            return _firebaseAuth.CurrentUser?.Uid;
        }
    }

}