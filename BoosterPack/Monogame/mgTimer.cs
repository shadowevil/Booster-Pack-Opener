using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoosterPack
{
    public class mgTimer
    {
        public double delay { get; private set; } = 1000.0;
        private double timer = 0.0;

        public mgTimer(double _delay)
        {
            delay = _delay;
        }

        public bool hasTicked(GameTime gameTime)
        {
            if(gameTime.TotalGameTime.TotalMilliseconds - timer >= delay)
            {
                timer = gameTime.TotalGameTime.TotalMilliseconds;
                return true;
            }
            return false;
        }
    }
}
