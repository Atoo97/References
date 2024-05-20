using SnakeLib.Model;
using SnakeGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeLib.Persistence
{
    /// <summary>
    /// Snake fájl kezelő felülete.
    /// </summary>
    public interface ISnakeDataAccess
    {
        /// <summary>
        /// Fájl betöltése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        /// <returns>A fájlból beolvasott játéktábla.</returns>
        Task<SnakeTable> LoadAsync(GameTableSize tablesize);
    }
}
