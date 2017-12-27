//Phillip Ko
//C#268 Assignment4
//Aug 19, 2016
//Gengar.cs
//This is Gengar EnemyPokemon 
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment4
{
    public class Gengar : EnemyPokemon
    { 
        //Gengar's method is better
        //it moves horizontally or vertically by 50-50 chance
        //harder to predict by the player
        public override void UpdatePosition(Vector2 v) //moves toward v
        {
            Random rnd = new Random();
            if (rnd.Next(0, 2) == 0)
            {
                if (position.X - v.X > 0.0f)
                {
                    position.X -= 50.0f;
                }
                else if (position.X - v.X < 0.0f)
                {
                    position.X += 50.0f;
                }
                else if (position.Y - v.Y < 0.0f)
                {
                    position.Y += 50.0f;
                }
                else if (position.Y - v.Y > 0.0f)
                {
                    position.Y -= 50.0f;
                }
            }
            else
            {
                if (position.Y - v.Y < 0.0f)
                {
                    position.Y += 50.0f;
                }
                else if (position.Y - v.Y > 0.0f)
                {
                    position.Y -= 50.0f;
                }
                else if (position.X - v.X > 0.0f)
                {
                    position.X -= 50.0f;
                }
                else if (position.X - v.X < 0.0f)
                {
                    position.X += 50.0f;
                }

            }

        }
         
    }
}
