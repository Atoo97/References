﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeLib.Persistence
{
    /// <summary>
    /// Snake adatelérés kivétel típusa.
    /// </summary>
    public class SnakeDataException : Exception
    {
        /// <summary>
        /// Snake adatelérés kivétel példányosítása.
        /// </summary>
        public SnakeDataException(String message) : base(message) { }
    }
}
