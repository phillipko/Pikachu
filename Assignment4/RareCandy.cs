//Phillip Ko
//C#268 Assignment4
//Aug 19, 2016
//RareCandy.cs
//Pikachu's level goes up by one (or three) when RareCandy is taken by it.
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
    public class RareCandy
    {
        private bool gold; //if it is a golden candy
        private Vector2 position; //current position

        public RareCandy()
        {
            position = new Vector2(350.0f, 350.0f); //initial position
            Random rnd = new Random();
            if (rnd.Next(0, 10) > 7) //80% of chance of being gold
                gold = true;
            else
                gold = false;
        }


        public Vector2 Position //simple get and set method
        {
            get
            {
                return position;
            }
            set 
            {
                position = value;
            }
        }

        
        public Color GetColor() // for renderring. return Color.Gold to our texture render scheme
        {
            if (gold) //if it is golden
                return Color.Gold;
            else
                return Color.White;
        }

        public void UpdatePositionColor() //when the rare candy is taken, we move it to a new position to make the player think it is a new one.
        {
            //(50,50) to (700, 500)
            Random rnd = new Random();
            float tempX, tempY; //temperary position. we need to test it with the current position to make sure it is far enough
            do
            {
                tempX = (float)rnd.Next(1, 14) * 50;
                tempY = (float)rnd.Next(1, 10) * 50;
            } while(Math.Abs(tempX - position.X) < 151.0f || Math.Abs(tempY - position.Y) < 151.0f); //if it is too close by this rule, find another position

            position.X = tempX; //set the new position
            position.Y = tempY;

            if (rnd.Next(0, 10) > 7) //again. 80% of chance of being golden
                gold = true;
            else
                gold = false;
        }
         
    }
}
