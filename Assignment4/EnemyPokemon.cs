//Phillip Ko
//C#268 Assignment4
//Aug 19, 2016
//EnemyPokemon.cs
//This is the super class of gastly, haunter, and gengar.
//They are all enemys. 
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
    public enum EnemyType //Enemy Type mainly used by our enemy factory
    {
        Gastly,
        Haunter,
        Gengar
    }


    public abstract class EnemyPokemon 
    {
        protected Vector2 position; //position 
        protected float rate; //move Rate
        protected string name; //name for loading the texture 
        protected int level; //current level for player referencing

        public EnemyPokemon() //Constructor
        {
            position = new Vector2(0, 0);
            rate = 0.0f;
            name = "";
            level = 0;
        }
        public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                //if (value.X > 49.0f && value.X < 701.0f && value.Y > 49.0f && value.Y < 501.0f) //Make sure it doesn't move out of the grid
                    position = value;
            }
        }
        public string Name
        {
            get { return name; }
        }
        public float Rate
        {
            get { return rate; }
        }
        public int Level
        {
            get { return level; }
            set { level = value; }
        }

        public void SetMethods(Vector2 v, float r, string n ) //Our factory can initialize these attributes with this method easily!
        {
            position = v;
            rate = r;
            name = n; 
        }
        
        //This method shifts enemy toward to position of v
        //every enemy's updatePosition is different because some enemy has better AI of capturing
        public abstract void UpdatePosition(Vector2 v); 
    }
}
