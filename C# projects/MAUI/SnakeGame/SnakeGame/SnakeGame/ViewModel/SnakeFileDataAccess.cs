using SnakeGame.Model;
using SnakeLib.Model;
using SnakeLib.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.ViewModel
{
    /// <summary>
    /// Snake fájlkezelő típusa.
    /// </summary>
    public class SnakeFileDataAccess : ISnakeDataAccess
    {
        /// <summary>
        /// Könyvtár.
        /// </summary>
        private string _directory = string.Empty;
        private string _line = string.Empty;

        public SnakeFileDataAccess(string saveDirectory)
        {
            _directory = saveDirectory;

        }


        /// <summary>
        /// Fájl betöltése.
        /// </summary>
        /// <param name="_directory">Elérési útvonal.</param>
        /// <returns>A fájlból beolvasott játéktábla.</returns>
        public async Task<SnakeTable> LoadAsync(GameTableSize tablesize)
        {

            if (string.IsNullOrEmpty(_directory))
                throw new SnakeDataException("Hibás elérési útvonal!");

            int? length = null;

            try
            {
                //StreamReader reader = new StreamReader(_directory!)
                using var stream = await FileSystem.OpenAppPackageFileAsync(_directory);
                using (StreamReader reader = new StreamReader(stream)) // fájl megnyitása
                {
                    switch (tablesize)
                    {
                        case GameTableSize.Small:
                            length = 0;
                            break;
                        case GameTableSize.Medium:
                            length = 1;
                            break;
                        case GameTableSize.Large:
                            length = 2;
                            break;
                    }


                    //Beolvasás megváltoztat table-re!
                    string[] datas = null!;
                    for (int i = -1; i < length; i++)
                    {
                        string line = await reader.ReadLineAsync() ?? string.Empty;
                        datas = line.Split(' '); // beolvasunk egy sort, és a szóköz mentén széttöredezzük
                    }

                    int tableSize = int.Parse(datas[0]); // beolvassuk a játéktábla méretét
                    int bordersNum = int.Parse(datas[1]); // beolvassuk az akadályok számát
                    SnakeTable table = new SnakeTable(tableSize, bordersNum); // létrehozzuk a táblát

                    //x/y koordinátái betöltése a GameFields listába
                    int c = 2;
                    for (int x = 0; x < int.Parse(datas[0]); x++)
                    {
                        //Oszlop feltölt
                        for (int y = 0; y < int.Parse(datas[0]); y++)
                        {
                            if (c <= bordersNum && x == int.Parse(datas[c]) && y == int.Parse(datas[c + 1]))
                            {
                                SnakeField field = new SnakeField
                                {
                                    X = x,
                                    Y = y,
                                    Border = true

                                };
                                c += 2;
                                table.FieldsCoordinate.Add(field);
                            }
                            else
                            {
                                SnakeField field = new SnakeField
                                {
                                    X = x,
                                    Y = y,
                                };
                                table.FieldsCoordinate.Add(field);
                            }
                        }
                    }

                    return table;

                }
            }
            catch
            {

                throw new SnakeDataException("Hibás elérési útvonal, a file nem létezik!");
            }

        }


    }
}
