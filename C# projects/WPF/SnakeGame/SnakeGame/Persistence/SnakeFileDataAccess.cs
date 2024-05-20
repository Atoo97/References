using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnakeGame.Model;

namespace SnakeGame.Persistence
{
    public class SnakeFileDataAcess : ISnakeDataAccess
    {
        /// <summary>
        /// Fájl betöltése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        /// <returns>A fájlból beolvasott játéktábla.</returns>
        public async Task<SnakeTable> LoadAsync(String path, Model.MapSize _fieldsize)
        {

            int? length = null;

            try
            {
                using (StreamReader reader = new StreamReader(path)) // fájl megnyitása
                {
                    /*_fileldsize értéke szerint kiválasztjuk melyik sort akarjuk beolvasni ami adott pályaméret értékeit tartalmazza,
                        a pályaméretek feltételezhetően csökkenő sorrendben vannak megadva az input file-ban*/
                    switch (_fieldsize)
                    {
                        case Model.MapSize.Large:
                            length = 1;
                            break;
                        case Model.MapSize.Medium:
                            length = 2;
                            break;
                        case Model.MapSize.Small:
                            length = 3;
                            break;
                    }

                    String[] datas = null!;
                    for (int i = 0; i < length; i++)
                    {
                        String line = await reader.ReadLineAsync() ?? String.Empty;
                        datas = line.Split(' '); // beolvasunk egy sort, és a szóköz mentén széttöredezzük
                    }

                    Int32 tableSize = Int32.Parse(datas[0]); // beolvassuk a játéktábla méretét
                    Int32 bordersNum = Int32.Parse(datas[1]); // beolvassuk az akadályok számát
                    SnakeTable table = new SnakeTable(tableSize, bordersNum); // létrehozzuk a táblát

                    //akadályok x/y koordinátái betöltése a Borders listába
                    for (int i = 2; i < bordersNum + 2; i += 2)
                    {
                        //Akadály típusának példányosítása
                        FigShapes wall = new FigShapes
                        {
                            X = int.Parse(datas[i]),
                            Y = int.Parse(datas[i + 1])
                        };

                        table.BordersCoordinates.Add(wall);
                    }

                    return table;
                }
            }
            catch (Exception)
            {

                throw new SnakeDataException();
            }
        }
    }
}
