//Phillip Ko
//C#268 Assignment4
//Aug 19, 2016
//EnemyFactor.cs
//This is the implementation of factory design pattern
//This class creates an enemy object for the main game
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment4
{
    public class EnemyFactory
    {
        public static EnemyPokemon CreateEnemy(EnemyType type, Vector2 v) //EnemyType can be found in EnemyPokemon.cs
        {
            EnemyPokemon enemy; //enemy object to be allocated 
            switch (type) 
            {
                case EnemyType.Gastly: //gastly
                    enemy = new Gastly();
                    //enemy.SetMethods(new Vector2(700.00f, 500.00f), 0.8f, "gastly"); //initialize it with our settings
                    enemy.SetMethods(v, 0.8f, "gastly"); //initialize it with our settings
                    //the vector is the position, and the float is the rate.
                    break;
                case EnemyType.Haunter:
                    enemy = new Haunter();
                    //enemy.SetMethods(new Vector2(150.00f, -100.00f), 0.5f, "haunter");//initialize it with our settings
                    enemy.SetMethods(v, 0.5f, "haunter");//initialize it with our settings
                    //haunter should be faster than gastly
                    break;
                case EnemyType.Gengar:
                    enemy = new Gengar();
                    //enemy.SetMethods(new Vector2(350.00f, 350.00f), 0.25f, "gengar");//initialize it with our settings
                    enemy.SetMethods(v, 0.25f, "gengar");//initialize it with our settings
                    //gengar should be the fastest
                    break;
                default: //default is gengar
                    enemy = new Gastly();
                    //enemy.SetMethods(new Vector2(700.00f, 500.00f), 0.8f, "gastly");//initialize it with our settings
                    enemy.SetMethods(v, 0.8f, "gastly");//initialize it with our settings
                    break;
            }
            return enemy;
        }
    }
}
