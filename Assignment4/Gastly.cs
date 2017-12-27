//Phillip Ko
//C#268 Assignment4
//Aug 19, 2016
//Gastly.cs
//This is Gastly EnemyPokemon 
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment4
{
    public class Gastly : EnemyPokemon
    {
        public Gastly()
        {
            level = 24; //initial level that pikachu has to surpass
        }

        public override void UpdatePosition(Vector2 v)//capture pikachu. Gastly moves vertically then horizontally.
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
