using Android.Content;

namespace Android.App
{
    public class AppContextProvider
    {
        public AppContextProvider(Context context)
        {
            CurrentContext = context;
        }

        public Context CurrentContext { get; private set; }     

        public static Context DefaultApplicationContext
        {
            get { return Android.App.Application.Context; }
        }

        public static AppContextProvider DefaultAppContextProvider
        {
            get { return new AppContextProvider(DefaultApplicationContext); }
        }
    }
}




