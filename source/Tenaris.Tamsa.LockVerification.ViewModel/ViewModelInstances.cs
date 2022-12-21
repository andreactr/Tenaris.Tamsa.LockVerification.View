using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tenaris.Library.Log;

namespace Tenaris.Tamsa.LockVerification.ViewModel
{
    /// <summary>
    /// Clase que permite acceder a la jerarquia de DataContext de cada una de las vistas
    /// de forma directa y desde un solo punto en la solucion
    /// 
    /// El contructor de esta clase debe ser llamado solamente una vez, la cual se hace
    /// desde la definicion dentro del app.xaml, todos los demas acceso se debe hacen
    /// desde la propiedad Instance
    /// </summary>
    public class ViewModelInstances
    {
        // propiedad para el acceso directo de los view model desde codigo
        public static ViewModelInstances Instance { get; private set; }

        // instancias de cada viewmodel unico utilizado
        public MainWindowViewModel MainViewModel { get; private set; }

        #region ViewModels
        
        #endregion

        public ViewModelInstances()
        {
            try
            {
                Instance = this;

                if (MainViewModel == null)
                    MainViewModel = new MainWindowViewModel();                
            }
            catch (Exception ex)
            {
                Trace.Exception(ex, "ViewModelInstances()");
            }
        }
    }
}
