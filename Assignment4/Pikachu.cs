//Phillip Ko
//C#268 Assignment4
//Aug 19, 2016
//Pikachu.cs
//This is our main character
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
    public class Pikachu 
    { 
        private int level; //current level. pikachu's level has to be 25 to go to the next "game level"
        private Vector2 position; //pikachu's current position
        public Vector2 Position //set and get
        {
            get
            {
                return position;
            }
            set
            {
                if (value.X > 49.0f && value.X < 701.0f && value.Y > 49.0f && value.Y < 501.0f) //make sure it doesn't move out of the grid
                    position = value;
            }
        }
        public int Level //simple get and set
        {
            get { return level; }
            set { level = value; }
        }


        public Pikachu(float x, float y) //constructor 
        { 
            //position = new Vector2(50.0f, 50.0f); //initial position 
            position = new Vector2(x, y); //initial position 
            
            level = 1; //initial level
        }
           
        //we use this to check if pikachu is captured or has taken a rare candy
        //we do it by passing enemy or rareCandy's position vector in here
        public bool IfSamePosition(Vector2 v) 
        {
            //sometimes there's some imprecision
            if (Math.Abs(v.X - position.X) < 10.0f && Math.Abs(v.Y - position.Y) < 10.0f) //return true if they are at the same position
            {
                return true;
            }
            return false;
        }

        public void SetPosition(Vector2 p)
        {
            position = p;
        }
    }
}
