using Microsoft.Extensions.Logging;
using Plugin.Firebase.Auth; // Para Firebase Authentication
using Plugin.Firebase.Firestore; // Para Firebase Firestore
using Plugin.Firebase.Shared; // Para la inicialización general de Firebase
using one_.Services; // ¡Ajusta este namespace al nombre de tu proyecto!

namespace one_

{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Inicialización de Firebase
            builder.UseFirebase(firebaseBuilder =>
            {
                firebaseBuilder.AddAuthentication(authBuilder =>
                    {
                        // Puedes configurar proveedores de autenticación aquí si es necesario,
                        // por ejemplo, authBuilder.AddGoogle();
                    })
                    .AddFirestore(); // Agrega el soporte para Firestore
                    // .AddCloudMessaging(); // Si necesitas notificaciones push
                    // .AddStorage(); // Si necesitas Firebase Storage
                    // etc. Puedes agregar otros servicios de Firebase aquí.
            });
            // Registrar tus servicios personalizados
            builder.Services.AddSingleton<AuthService>();
            builder.Services.AddSingleton<FirestoreService>();
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}