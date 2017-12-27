//Phillip Ko
//C#268 Assignment4
//Aug 19, 2016
//Haunter.cs
//This is Haunter EnemyPokemon 
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment4
{
    public class Haunter : EnemyPokemon
    {
        public override void UpdatePosition(Vector2 v)//capture pikachu. Gastly moves horizontally then vertically.
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
    }
}
