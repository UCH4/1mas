using System;
using System.Net.Http; // Esto ya no es estrictamente necesario si solo usas Plugin.Firebase.Firestore
using System.Text;     // Esto ya no es estrictamente necesario si solo usas Plugin.Firebase.Firestore
using Plugin.Firebase.Auth; // Necesario para obtener el token del usuario (si lo necesitas)
using Newtonsoft.Json; // Puedes mantenerlo si lo usas para tus propios objetos de modelo, Plugin.Firebase maneja su propio JSON.
using Plugin.Firebase.Firestore;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace one_.Services // <<<< ¡AJUSTA AQUÍ!
{

    public class FirestoreService
    {
        private readonly IFirestore _firestore;

        public FirestoreService(IFirestore firestore)
        {
            _firestore = firestore;
        }

        // Ejemplo: Guardar datos de un usuario
        public async Task<bool> SaveUserDataAsync(string userId, Dictionary<string, object> userData)
        {
            try
            {
                var userDocument = _firestore.Collection("users").Document(userId);
                await userDocument.SetDataAsync(userData);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar datos de usuario: {ex.Message}");
                return false;
            }
        }

        // Ejemplo: Obtener datos de un usuario
        public async Task<Dictionary<string, object>> GetUserDataAsync(string userId)
        {
            try
            {
                var userDocument = _firestore.Collection("users").Document(userId);
                var snapshot = await userDocument.GetDocumentSnapshotAsync();

                if (snapshot.Exists)
                {
                    return snapshot.Data;
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener datos de usuario: {ex.Message}");
                return null;
            }
        }
    }
}