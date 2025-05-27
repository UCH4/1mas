using Microsoft.Extensions.Logging;
using Plugin.Firebase.Auth;
using Plugin.Firebase.Firestore;
using Plugin.Firebase.Shared; // Asegúrate de tener este using
using one_.Services; // Tus servicios
using one_.ViewModels; // Tus ViewModels
using one_.Views; // Tus Views (ASEGÚRATE QUE ESTA CARPETA/NAMESPACE EXISTE)

#if IOS
using Plugin.Firebase.iOS;
#endif

#if ANDROID
using Plugin.Firebase.Android;
#endif

namespace one_;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            // ===> MUEVE LA CONFIGURACIÓN DE FIREBASE AQUÍ, ANTES DE CONFIGUREFONTS <===
            .ConfigureLifecycleEvents(events => {
                #if IOS
                events.AddiOS(iOS => iOS.FinishedLaunching((app, ui) => {
                    CrossFirebase.Current.Configure();
                    return false;
                }));
                #elif ANDROID
                events.AddAndroid(android => android.OnCreate((activity, bundle) => {
                    CrossFirebase.Current.Configure(activity);
                    return false;
                }));
                #endif
            })
            .ConfigureFonts(fonts => // Luego las fuentes
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // ===> REGISTRAR SERVICIOS DE FIREBASE INMEDIATAMENTE DESPUÉS DE UseMauiApp <===
        // Registrar los servicios de Firebase
        builder.Services.AddSingleton(_ => CrossFirebaseAuth.Current); // Para IFirebaseAuth
        builder.Services.AddSingleton(_ => CrossFirebaseFirestore.Current); // Para IFirestore
        // Asegúrate de que los using para Plugin.Firebase estén presentes al inicio del archivo

        // Registrar tus propios servicios y ViewModels para Inyección de Dependencias
        builder.Services.AddSingleton<AuthService>();
        builder.Services.AddSingleton<FirestoreService>();
        builder.Services.AddTransient<LoginPageViewModel>();

        // Registrar tus vistas (páginas)
        builder.Services.AddTransient<LoginPage>(); // <-- ASEGÚRATE DE QUE LoginPage EXISTA EN one_.Views

        #if DEBUG
        builder.Logging.AddDebug();
        #endif

        return builder.Build();
    }
}