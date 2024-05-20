using System;

namespace ELTE.Calculator.ViewModel
{
    /// <summary>
    /// Hibaüzenet eseményargumentum típusa.
    /// </summary>
    public class ErrorMessageEventArgs : EventArgs
    {
        /// <summary>
        /// Üzenet lekérdezése.
        /// </summary>
        public ErrorMessage Message { get; private set; }

        /// <summary>
        /// Hibaüzenet eseményargumentum példányosítása.
        /// </summary>
        /// <param name="message">Üzenet.</param>
        public ErrorMessageEventArgs(ErrorMessage message)
        {
            Message = message;
        }
    }
}
