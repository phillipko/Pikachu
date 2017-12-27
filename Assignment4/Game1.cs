//Phillip Ko
//C#268 Assignment4
//Aug 21, 2016
//Game1.cs
//This is the main class of our game

//The following resources are adapted from the internet
//gastly.png/pikachu.png/gengar.png/haunter.png/rareCandy.png: downloaded from http://bulbapedia.bulbagarden.net/wiki
//opening.wav: downloaded from http://downloads.khinsider.com/game-soundtracks/album/pokemon-gameboy-sound-collection
//teleport.wav: downloaded from https://www.youtube.com/watch?v=wa6_3zkNGKI
//gameover.wav: downloaded from https://www.youtube.com/watch?v=xYK47fb2f-s
//collected.wav: downloaded from http://www.superluigibros.com/super-mario-rpg-sound-effects-wav
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System;

namespace Assignment4
{ 
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics; //namely, for graphics, rendering
        SpriteBatch spriteBatch;
        SpriteFont spriteFont; //like spriteBatch, but this is for texts
        KeyboardState previousState; //prevent the player from consecutive pressing 
        Texture2D pikachuTexture; //for rendering pikachu
        Texture2D enemyTexture; //for rendering gastly/haunter/gengar
        Texture2D rareCandyTexture; //for rendering rare_candy
        Texture2D titleTexture; //for rendering the level information
        
        //game objects
        Pikachu pikachu; //pikachu object
        EnemyPokemon enemy; //enemy object: use polymorphism to change it from gastly->haunter->gengar
        RareCandy rareCandy; //rareCandy object
        
        //Sounds
        List<SoundEffect> soundEffects; //A list of sound effects we use 
        
        //helper variables
        float deltaTime; //We use this to direct the enemy for updating position frequency 
        string levelText; //Pikachu's level (A fake level) for the player
        bool start; //Is the game started?
        bool over; //is the game over?
        bool win; // Did the player win?
        int inheritedLevel; //We use this to know the pikachu's level. 
                        //After enemy's evolution, we reset pikachu's level to restart. However, we need to let the 
                        //player know his/her pikachu's level from the beginning instead of the erased level.
        bool pause;
        //Vector2 position;
        Vector2 origin; //origin reference point. Redundant since we use the default (0,0)
        
        //Sound instances
        SoundEffectInstance openingInstance;
        SoundEffectInstance battleInstance;
        SoundEffectInstance battleOpeningInstance;
        SoundEffectInstance collectedInstance;
        SoundEffectInstance teleportInstance;
        SoundEffectInstance victoryInstance;
        SoundEffectInstance lossInstance;
        
        public Game1() //set up stuff
        {
            //needless to say...
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //////////////////////////////////////
            Window.Title = "Pikachu's Adventure";// our title
            graphics.IsFullScreen = false; 
            graphics.PreferredBackBufferWidth = 800;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = 600;   // set this value to the desired height of your window
            graphics.ApplyChanges(); //apply
            IsMouseVisible = true; //see the cursor
            pause = false;
            //Allocate the game objects
            pikachu = new Pikachu(50.0f, -50.0f);
            enemy = EnemyFactory.CreateEnemy(EnemyType.Gastly, new Vector2(700.00f, -50.00f)); //We use the factory design pattern to create a enemy
            rareCandy = new RareCandy();
            
            //allocate the soundEffect objects
            soundEffects = new List<SoundEffect>();
        }

        protected override void Initialize()
        {
            base.Initialize();
            //position = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2,
            //    graphics.GraphicsDevice.Viewport.Height / 2);
            //position = new Vector2(50.0f, 25.0f);
            
            //Initialize 
            deltaTime = 0.0f; 
            levelText = "Pikachu's Level: 1";
            start = false;
            over = false;
            win = false;
            inheritedLevel = 0;
            origin = new Vector2(0, 0);
        }
         
        protected override void LoadContent()
        {
            //load our wav files  

            openingInstance = Content.Load<SoundEffect>("opening").CreateInstance(); //background music. looping
            openingInstance.IsLooped = true; //loop (over and over again)
            openingInstance.Volume = 0.2f; //from 0.0f to 1.0f. not too loud...
            openingInstance.Play(); //start playing 

            battleOpeningInstance = Content.Load<SoundEffect>("battleOpening").CreateInstance();
            battleInstance = Content.Load<SoundEffect>("battle1").CreateInstance();
            collectedInstance = Content.Load<SoundEffect>("collected").CreateInstance(); //collected sound
            teleportInstance = Content.Load<SoundEffect>("teleport").CreateInstance();
            victoryInstance = Content.Load<SoundEffect>("victory").CreateInstance(); //applause sound
            lossInstance = Content.Load<SoundEffect>("gameover").CreateInstance(); //gameover sound

            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteFont = this.Content.Load<SpriteFont>("basicFont"); //our font

            //textures
            pikachuTexture = this.Content.Load<Texture2D>("pikaChu");
            enemyTexture = this.Content.Load<Texture2D>("gastly");
            rareCandyTexture = this.Content.Load<Texture2D>("rareCandy");
            titleTexture = this.Content.Load<Texture2D>("title");
        } 
        protected override void UnloadContent()
        { 
        }
         
        protected override void Update(GameTime gameTime) //Called 60 timees per second
        {
            KeyboardState state = Keyboard.GetState(); //get keyboard input

            if (state.IsKeyDown(Keys.Escape)) //<Esc> pressed
            {
                Exit();
            }

            //if (state.IsKeyDown(Keys.Space) && !previousState.IsKeyDown(Keys.Space))
            //{
                //pause = !pause;
                //System.Diagnostics.Debug.WriteLine("Cool");
                //System.Diagnostics.Debug.WriteLine(pause.ToString());
                //enemy = EnemyFactory.CreateEnemy(EnemyType.Gengar, enemy.Position);
                //enemyTexture = this.Content.Load<Texture2D>(enemy.Name);
                //Thread thread = new Thread(KillAnimation); //Play Evolving animation
                //thread.Start();
            //}
            if (state.IsKeyDown(Keys.Enter) && !previousState.IsKeyDown(Keys.Enter)  && !start) //<Enter> pressed
            {
                start = true;
                openingInstance.Stop();
                battleOpeningInstance.IsLooped = false; //
                battleOpeningInstance.Volume = 0.2f; //from 0.0f to 1.0f. not too loud...
                battleOpeningInstance.Play(); //start playing 
                Thread thread = new Thread(PlayBattleInstance); //When battleOpeningInstance is done, we play (and loop) another soundeffect in this threads
                thread.Start();
                pause = true; //prevent the player from moving when animation is playing 
                Thread threadLoad = new Thread(LoadAnimation); //loading objects animation
                threadLoad.Start();
            }
            
            if (start && !over && !pause ) //When the game is running
            {
                deltaTime += (float)gameTime.ElapsedGameTime.TotalSeconds; //deltaTime cumulated
                if (deltaTime > enemy.Rate) //enemy.Rate: The lower the faster
                {
                    enemy.UpdatePosition(pikachu.Position); //move position
                    deltaTime = 0.0f; //reset for the next move.
                } 
                //We use previousState to prevent the user from consecutively pressing a button
                if (state.IsKeyDown(Keys.Right) && !previousState.IsKeyDown(Keys.Right)) //right arrow key
                {
                    pikachu.Position = new Vector2(pikachu.Position.X + 50.0f, pikachu.Position.Y); //move right
                }
                else if (state.IsKeyDown(Keys.Left) && !previousState.IsKeyDown(Keys.Left)) //left arrow key
                {
                    pikachu.Position = new Vector2(pikachu.Position.X - 50.0f, pikachu.Position.Y); //move left
                }
                else if (state.IsKeyDown(Keys.Up) && !previousState.IsKeyDown(Keys.Up)) //up arrow key
                {
                    pikachu.Position = new Vector2(pikachu.Position.X, pikachu.Position.Y - 50.0f); //move up
                }
                else if (state.IsKeyDown(Keys.Down) && !previousState.IsKeyDown(Keys.Down)) //down arrow key
                {
                    pikachu.Position = new Vector2(pikachu.Position.X, pikachu.Position.Y + 50.0f); //move down
                }

                if (pikachu.IfSamePosition(enemy.Position)) //loss, since pikachu's position == enemy's position. i.e. captured
                {
                    //pikachu.Kill();
                    over = true; //done
                    win = false; //loss
                    battleInstance.Stop();
                    lossInstance.IsLooped = false; //no looping
                    lossInstance.Volume = 1.0f;
                    lossInstance.Play();
                }
            }

            
                 
                 
            if (pikachu.Level > 24 && !over) //When pikachu's level has increased 24 levels-> Go to the next level or win.
            {
                if (enemy is Gengar) //this is the final boss, so win.
                {
                    over = true; //game over
                    win = true; //win
                    battleInstance.Stop();
                    victoryInstance.IsLooped = false; //no looping
                    victoryInstance.Volume = 0.9f;
                    victoryInstance.Play();
                    Thread thread = new Thread(KillAnimation); //Play Evolving animation
                    thread.Start();
                }
                else if (enemy is Haunter) //haunter evolves into gengar
                {
                    inheritedLevel += pikachu.Level;  //pikachu's cumulative level for showing the player
                    pikachu.Level = 0; //reset pikachu's level, but we show the player "inheritedLevel+this level"
                    pause = true; //prevent the player from moving when animation is playing 
                    Thread thread = new Thread(EnemyEvolvingAnimation); //Play Evolving animation
                    thread.Start(EnemyType.Gengar);
                    
                    //enemy = EnemyFactory.CreateEnemy(EnemyType.Gengar, enemy.Position); //Factoroy for creating Gengar
                    //enemy.Level = inheritedLevel + 24; //Show the enemy's level. Pikachu's level has to increace 24 to beat the enemy
                                                //Therefore, we make an assumption that pikachu's level must be higher to win
                }
                else //gastly evolves into haunter
                {
                    //Same as (enemy is Haunter) exception the factory create enemy
                    inheritedLevel += pikachu.Level;
                    pikachu.Level = 0;
                    pause = true; //prevent the player from moving when animation is playing 
                    Thread thread = new Thread(EnemyEvolvingAnimation); //Play Evolving animation
                    thread.Start(EnemyType.Haunter);
                    
                    //enemy = EnemyFactory.CreateEnemy(EnemyType.Haunter);
                    //enemy.Level = inheritedLevel + 24;
                    
                }
                //enemyTexture = this.Content.Load<Texture2D>(enemy.Name); //enemy.Name corresponds to the image's name
            } 

            if (pikachu.IfSamePosition(rareCandy.Position)) //candy collected
            {
                pikachu.Level = pikachu.Level + 1; //level up
                if (rareCandy.GetColor() == Color.Gold) //if it is a golden candy. 2 more level points 
                    pikachu.Level = pikachu.Level + 2;
                rareCandy.UpdatePositionColor(); //move the rareCandy to a different position with a chance of turning golden. 
                                                //This makes the user think it is a new one.
                //Play sound

                collectedInstance.IsLooped = false; //no looping
                collectedInstance.Volume = 0.3f;
                collectedInstance.Play();
            }
            //System.Diagnostics.Debug.WriteLine(pikachu.Level.ToString());
            //System.Diagnostics.Debug.WriteLine(pikachu.Position.ToString());
            levelText = "Pikachu's Level: " + (pikachu.Level+inheritedLevel).ToString(); //pikachu's level for showing the player
            

            previousState = state; //save the previous state for the next update
            base.Update(gameTime);
        }
         
        protected override void Draw(GameTime gameTime) //draw method
        {
            GraphicsDevice.Clear(Color.Gray); //clear the screen first with the gray color
            
            spriteBatch.Begin(); //Everything to be drawn should be between spriteBatch.Begin() and spriteBatch.End();
            for (float y = 50.0f; y < 600.0f; y+= 50.0f) //draw the horizontal lines
            { 
                Primitives2D.DrawLine(spriteBatch, new Vector2(50.0f, y), new Vector2(750f, y), Color.Blue, 4.0f); 
            }
            for (float x = 50.0f; x < 800.0f; x += 50.0f) //draw the vertical lines
            {
                Primitives2D.DrawLine(spriteBatch, new Vector2(x, 50.0f), new Vector2(x, 550.0f), Color.Blue, 4.0f);
            } 
            
            spriteBatch.Draw(pikachuTexture, pikachu.Position, origin: origin); //render pikachu
            //spriteBatch.Draw(pikachuTexture, position, origin: origin);
            spriteBatch.Draw(enemyTexture, enemy.Position, origin: origin); //render enemy
            spriteBatch.Draw(rareCandyTexture, rareCandy.Position, origin: origin, color: rareCandy.GetColor()); //render rarecandy
            if (!start) //Welcome message 
                spriteBatch.Draw(titleTexture, new Vector2(0.00f,220.00f), origin: origin);
            if(over) //Game over message: (Win/Loss)
            {
                if (win) //Win
                    titleTexture = this.Content.Load<Texture2D>("win"); //load the congrats message
                else //Loss
                    titleTexture = this.Content.Load<Texture2D>("loss"); //needless to explain...
                spriteBatch.Draw(titleTexture, new Vector2(0.00f, 220.00f), origin: origin); //render it
            }
            spriteBatch.DrawString(spriteFont, levelText, new Vector2(50.0f, 12.0f), Color.Black); //render pikachu's level on the screen
            spriteBatch.DrawString(spriteFont, "Enemy's Level: " + enemy.Level.ToString(), new Vector2(400.0f, 12.0f), Color.Black); //render enemy's level on the screen
            spriteBatch.End();
            
            base.Draw(gameTime);
        }



        //Helper functions for loading threads
        private void PlayBattleInstance()
        {
            while (true) //keep checking if battleOpeningInstance is finished
            {
                if (battleOpeningInstance.State == Microsoft.Xna.Framework.Audio.SoundState.Stopped)
                {
                    battleInstance.IsLooped = true; //
                    battleInstance.Volume = 0.2f; //from 0.0f to 1.0f. not too loud...
                    battleInstance.Play(); //start playing 
                    return;
                }
            }
        }

        private void LoadAnimation() //load pikachu and enemy animation
        {
            Thread.Sleep(2000);
            Stopwatch stopWatch = new Stopwatch(); //clock
            int dt = 0; //delta time
            bool pikachuOnPosition = false;
            bool enemyOnPosition = false;
            while (!enemyOnPosition || !pikachuOnPosition)
            {
                stopWatch.Start();
                Thread.Sleep(25); //sleep here because we don't what this while loop to blow up
                stopWatch.Stop();
                //TimeSpan ts = stopWatch.Elapsed;
                dt += stopWatch.Elapsed.Milliseconds;
                //System.Diagnostics.Debug.WriteLine(stopWatch.Elapsed.Milliseconds.ToString());
                //System.Diagnostics.Debug.WriteLine(dt.ToString());

                if (dt > 40) //the lower the faster 
                { 
                    dt = 0;
                    if (!pikachuOnPosition) //shift
                        pikachu.SetPosition(new Vector2(pikachu.Position.X, pikachu.Position.Y + 2.5f));
                    if (!enemyOnPosition)
                        enemy.Position = new Vector2(enemy.Position.X, enemy.Position.Y + 10.0f);
                }
                if (pikachu.Position.Y > 48.00f) //at the correct location
                {
                    pikachuOnPosition = true;
                }
                if (enemy.Position.Y > 495.00f)//at the correct location
                {
                    enemyOnPosition = true;
                }
            }
            pause = false;
        }

        private void KillAnimation() //moves enemy out of the screen.
        { 
            Stopwatch stopWatch = new Stopwatch();
            int dt = 0;
            bool outOfScreen = false; 
            while (!outOfScreen)
            {

                stopWatch.Start();
                Thread.Sleep(20); //sleep here because we don't what this while loop to blow up
                stopWatch.Stop();
                //TimeSpan ts = stopWatch.Elapsed;
                dt += stopWatch.Elapsed.Milliseconds;
                //System.Diagnostics.Debug.WriteLine(stopWatch.Elapsed.Milliseconds.ToString());
                //System.Diagnostics.Debug.WriteLine(dt.ToString());

                if (dt > 30)
                {
                    dt = 0;
                    enemy.Position = new Vector2(enemy.Position.X, enemy.Position.Y - 10.0f);
                }
                if (enemy.Position.Y < -50.00f)
                {
                    outOfScreen = true;
                }
            }
            pause = false;
        }

        private void EnemyEvolvingAnimation(object type) //moves enemy out of the screen, and insert the evolved enemy back
        { 
            battleInstance.Pause();
            teleportInstance.Play(); //play teleport sound effect
            int dt = 0;
            bool outOfScreen = false;
            float originalY = enemy.Position.Y;
            Stopwatch stopWatch = new Stopwatch(); 

            while (!outOfScreen)
            {
                
                stopWatch.Start();
                Thread.Sleep(50); //sleep here because we don't what this while loop to blow up
                stopWatch.Stop();
                //TimeSpan ts = stopWatch.Elapsed;
                dt += stopWatch.Elapsed.Milliseconds;
                //System.Diagnostics.Debug.WriteLine(stopWatch.Elapsed.Milliseconds.ToString());
                //System.Diagnostics.Debug.WriteLine(dt.ToString());

                if (dt > 140)
                {
                    dt = 0;
                    enemy.Position = new Vector2(enemy.Position.X, enemy.Position.Y - 10.0f);
                }
                if (enemy.Position.Y < -50.00f)
                {
                    outOfScreen = true;
                }
            }
            bool onPosition = false;
            enemy = EnemyFactory.CreateEnemy((EnemyType)type, enemy.Position);
            enemyTexture = this.Content.Load<Texture2D>(enemy.Name); //enemy.Name corresponds to the image's name
             
            while (!onPosition)
            {
                stopWatch.Start();
                Thread.Sleep(50); //sleep here because we don't what this while loop to blow up
                stopWatch.Stop();
                //TimeSpan ts = stopWatch.Elapsed;
                dt += stopWatch.Elapsed.Milliseconds;
                //System.Diagnostics.Debug.WriteLine(stopWatch.Elapsed.Milliseconds.ToString());
                //System.Diagnostics.Debug.WriteLine(dt.ToString());

                if (dt > 140)
                {
                    dt = 0;
                    enemy.Position = new Vector2(enemy.Position.X, enemy.Position.Y + 10.0f);
                }
                if (enemy.Position.Y > originalY-0.1f)
                {
                    onPosition = true;
                }
            }
            enemy.Level = inheritedLevel + 24;
            pause = false;
            teleportInstance.Stop();
            battleInstance.Resume();
        }
    }
}
