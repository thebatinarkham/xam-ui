using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using UIKit;
using AppName.Core;

namespace AppName.iOS
{
    public class Application
    {   
        // Punto de entrada y salida de la aplicaci�n
        static void Main(string[] args)
        {
            // si se desa utilizar una clase diferente de "AppDelegate"
            // se debe especificar aqu�
            UIApplication.Main(args, null, "AppDelegate");
        }
    }
}
