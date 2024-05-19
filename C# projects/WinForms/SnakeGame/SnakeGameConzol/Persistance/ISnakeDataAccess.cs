using System;
using System.Threading.Tasks;

namespace Game.SnakeGameConzol.Persistance
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
