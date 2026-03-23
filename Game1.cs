using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game1;

public class Game1 : Game
{
    KeyboardState lastKeyboardState;
    GraphicsDeviceManager _graphics;
    SpriteBatch _spriteBatch;
    bool isActive = true;
    
    Cursor _cursor;

    Texture2D goodTexture;
    Texture2D evilTexture;
    bool goodSpriteActive = true;
    Vector2 goodSpritePosition;
    Vector2 evilSpritePosition;
    Point goodSpriteSize;
    Point evilSpriteSize;
    float goodSpriteSpeed = 5f;
    float evilSpriteSpeed = 2f;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        TargetElapsedTime = new System.TimeSpan(0, 0, 0, 0, 10);
    }

    protected override void Initialize()
    {
        _cursor = new Cursor();
        evilSpritePosition = new Vector2(Window.ClientBounds.Width / 2,
                                            Window.ClientBounds.Height / 2);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _cursor.LoadContent(Content);

        goodTexture = Content.Load<Texture2D>("good");
        evilTexture = Content.Load<Texture2D>("evil");

        goodSpriteSize = new Point(goodTexture.Width, goodTexture.Height);
        evilSpriteSize = new Point(evilTexture.Width, evilTexture.Height);
    }

    protected override void Update(GameTime gameTime)
    {
        KeyboardState keyboardState = Keyboard.GetState();

        if (keyboardState.IsKeyDown(Keys.Escape) && lastKeyboardState.IsKeyUp(Keys.Escape))
        {
            isActive = !isActive;
        }

        lastKeyboardState = keyboardState;
        _cursor.Update();

        if (isActive)
        {
            if (goodSpriteActive)
            {
                if (keyboardState.IsKeyDown(Keys.Left) && goodSpritePosition.X > 0)
                    goodSpritePosition.X -= goodSpriteSpeed;
                if (keyboardState.IsKeyDown(Keys.Right) && goodSpritePosition.X < Window.ClientBounds.Width - goodSpriteSize.X)
                    goodSpritePosition.X += goodSpriteSpeed;
                if (keyboardState.IsKeyDown(Keys.Up) && goodSpritePosition.Y > 0)
                    goodSpritePosition.Y -= goodSpriteSpeed;
                if (keyboardState.IsKeyDown(Keys.Down) && goodSpritePosition.Y < Window.ClientBounds.Height - goodSpriteSize.Y)
                    goodSpritePosition.Y += goodSpriteSpeed;

                if (evilSpritePosition.X < goodSpritePosition.X)
                    evilSpritePosition.X += evilSpriteSpeed;
                else if (evilSpritePosition.X > goodSpritePosition.X)
                    evilSpritePosition.X -= evilSpriteSpeed;

                if (evilSpritePosition.Y < goodSpritePosition.Y)
                    evilSpritePosition.Y += evilSpriteSpeed;
                else if (evilSpritePosition.Y > goodSpritePosition.Y)
                    evilSpritePosition.Y -= evilSpriteSpeed;
            }
        }

        // Проверка столкновения
        if (goodSpriteActive)
        {
            Rectangle goodRectangle = new(
                (int)goodSpritePosition.X,
                (int)goodSpritePosition.Y,
                goodSpriteSize.X, goodSpriteSize.Y
            );

            Rectangle evilRectangle = new(
                (int)evilSpritePosition.X,
                (int)evilSpritePosition.Y,
                evilSpriteSize.X, evilSpriteSize.Y
            );
            
            if (goodRectangle.Intersects(evilRectangle))
            {
                goodSpriteActive = false;
            }
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        _spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

        _cursor.Draw(_spriteBatch);

        if (goodSpriteActive)
            _spriteBatch.Draw(goodTexture, goodSpritePosition, Color.White);
        _spriteBatch.Draw(evilTexture, evilSpritePosition, Color.Black);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}