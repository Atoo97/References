using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnakeGame.Model;

namespace SnakeGame.Persistence
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
        Task<SnakeTable> LoadAsync(String path, Model.MapSize fieldSize);
    }
}
