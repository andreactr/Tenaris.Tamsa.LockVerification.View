using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Tenaris.Tamsa.LockVerification.LanguageResource;

namespace Tenaris.Tamsa.LockVerification.View
{
    public static class Extensions
    {
        /// <summary>
        /// Trace Exceptions.
        /// </summary>
        public static void Trace(this Exception ex, bool showMessage = false)
        {
            Tenaris.Library.Log.Trace.Exception(ex);
            if (showMessage)
            {
                var stackFrame = new System.Diagnostics.StackFrame(1, true);
                var method = stackFrame.GetMethod();
                var detail = string.Format("[{0}.{1}({2})]", method.ReflectedType.Name, method.Name, stackFrame.GetFileLineNumber());
                var message = string.Format("{0}{1}{1}{2}", detail, Environment.NewLine, ex.Message);
                Xceed.Wpf.Toolkit.MessageBox.Show(message, Language.TitleError, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Trace RemotingErrors.
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="manager"></param>
        public static void TraceRemotingHumanError(this Exception ex, string manager)
        {
            string error = string.Format(Language.RemotingManagerCreateError, manager);
            new System.Runtime.Remoting.RemotingException(error, ex).Trace(true);
        }
    }
}
