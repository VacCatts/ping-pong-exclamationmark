using System;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using static Raylib_cs.KeyboardKey;
using ping_pong;

namespace ping_pong
{
    class Game
    {
        // DEVELOPER NOTE:
        // i am finally done with this
        // this took me around 2 hours of actual coding
        // it works but the code sucks, i might update it later
        static int screenWidth = 800;
        static int screenHeight = 450;
        static int gamePhase = 0; // 0 - menu, 1 - game, 2 - fail
        static int p1score = 0;
        static int p2score = 0;
        static float playerspeed = 7;
        static float ballSpeed = 6;
        static int ballDirectionX = 1; // 1 = right, -1 = left
        static int ballDirectionY = 1; // 1 = down, -1 = up
        static int frameCount = 0;
        static bool manualplayer2 = false;
        static Player player = new Player(50, screenHeight / 2, 10, 50);
        static Player player2 = new Player(screenWidth - 60, screenHeight / 2, 10, 50);
        static Player ball = new Player(65, screenHeight / 2, 5, 5);

        static void Main()
        {
            // TODO: comment everything

            InitWindow(screenWidth, screenHeight, "Ping Pong!");
            //SetTargetFPS(60);

            while(!WindowShouldClose()) {
                frameCount++;
                Update();
                Draw();
            }
        }

        static void Update() {
            // game logic
            playerspeed = (7 * (GetFrameTime() * 85)); 
            ballSpeed = (6 * (GetFrameTime() * 80));
            switch (gamePhase)
            {
                case 0: // menu
                    if (IsKeyPressed(KEY_ENTER))
                    {
                        gamePhase = 1;
                    }
                    if (IsKeyPressed(KEY_Q))
                    {
                        manualplayer2 = !manualplayer2;
                    }
                    break;
                case 1: // game
                    if (IsKeyPressed(KEY_ESCAPE))
                    {
                        gamePhase = 0;
                    }
                    if (IsKeyDown(KEY_W))
                    {
                        if (player.y < 0)
                        {
                            player.y = 0;
                        }
                        player.y -= playerspeed;
                    }
                    else if (IsKeyDown(KEY_S))
                    {
                        if (player.y > screenHeight - player.height)
                        {
                            player.y = screenHeight - player.height;
                        }
                        player.y += playerspeed;
                    }

                    if (manualplayer2) {
                        if (IsKeyDown(KEY_I) || IsKeyDown(KEY_UP))
                        {
                            if (player2.y < 0)
                            {
                                player2.y = 0;
                            }
                            player2.y -= playerspeed;
                        }
                        else if (IsKeyDown(KEY_K) || IsKeyDown(KEY_DOWN))
                        {
                            if (player2.y > screenHeight - player2.height)
                            {
                                player2.y = screenHeight - player2.height;
                            }
                            player2.y += playerspeed;
                        }
                    } else {
                        //player2.y = (ball.y + ball.height / 2 - player2.height / 2);
                        //GetRandomValue(-10,10);

                        if (ball.x > screenWidth / 2 && !(GetRandomValue(1,6) == 1)) {
                            if (player2.y < ball.y) {
                                if (player2.y > screenHeight - player2.height)
                                {
                                    player2.y = screenHeight - player2.height;
                                }
                                player2.y += playerspeed;
                            } else if (player2.y > ball.y) {
                                if (player2.y < 0)
                                {
                                    player2.y = 0;
                                }
                                player2.y -= playerspeed;
                            }
                        }
                    }
                    if (GetCollisions(ball.x, ball.y, ball.width, ball.height, player.x, player.y, player.width, player.height))
                    {
                        ballDirectionX *= -1;
                    }
                    if (GetCollisions(ball.x, ball.y, ball.width, ball.height, player2.x, player2.y, player2.width, player2.height))
                    {
                        ballDirectionX *= -1;
                    }

                    if (ball.x < 0)
                    {
                        ballDirectionX *= -1;
                        p2score++;
                        ball.x = screenWidth / 2 + 20;
                        ball.y = screenHeight / 2;
                        gamePhase = 2;
                    }
                    else if (ball.x > screenWidth - ball.width)
                    {
                        ballDirectionX *= -1;
                        p1score++;
                        ball.x = screenWidth / 2 - 20;
                        ball.y = screenHeight / 2;
                        gamePhase = 2;
                    }

                    if (ball.y < 0)
                    {
                        ballDirectionY *= -1;
                    }
                    else if (ball.y > screenHeight - ball.height)
                    {
                        ballDirectionY *= -1;
                    }

                    ball.x += (ballSpeed * ballDirectionX);
                    ball.y += (ballSpeed * ballDirectionY);
                    break;
                case 2: // fail
                    if (IsKeyPressed(KEY_ENTER))
                    {
                        gamePhase = 1;
                    }
                    break;
            }
        }

        static void Draw() {
            BeginDrawing();
            ClearBackground(WHITE);
            switch (gamePhase)
            {
                case 0: // menu
                    DrawText("Ping Pong!", screenWidth / 2 - MeasureText("Ping Pong!", 30) / 2, screenHeight / 2 - 100, 30, BLUE);
                    DrawText("Press ENTER to start", screenWidth / 2 - MeasureText("Press ENTER to start", 30) / 2, screenHeight / 2, 30, BLUE);
                    DrawText("Press Q to switch Player 2 Mode", screenWidth / 2 - MeasureText("Press Q to switch Player 2 Mode", 30) / 2, screenHeight / 2 + 40, 30, BLUE);
                    DrawText("You control player 2: " + manualplayer2, screenWidth / 2 - MeasureText("You control player 2: " + manualplayer2, 30) / 2, screenHeight / 2 + 70, 30, BLUE);
                    break;
                case 1: // game
                    DrawText("p2 Score: " + p2score, screenWidth - MeasureText("p2 Score: " + p2score, 30) - 10, 10, 30, BLUE);
                    DrawText("p1 Score: " + p1score, MeasureText("p1 Score: " + p1score, 30) - 120, 10, 30, BLUE);
                    DrawText("fps: " + GetFPS(), MeasureText("fps: " + GetFPS(), 30) + 100, 10, 30, BLUE);
                    DrawLine(screenWidth / 2, screenHeight - 10, screenWidth / 2, 0, BLUE);

                    DrawRectangle((int)player.x, (int)player.y, player.width, player.height, BLUE);
                    DrawRectangle((int)player2.x, (int)player2.y, player2.width, player2.height, BLUE);
                    DrawRectangle((int)ball.x, (int)ball.y, ball.width, ball.height, BLUE);
                    break;
                case 2: // fail
                    DrawText("p2 Score: " + p2score, screenWidth - MeasureText("p2 Score: " + p2score, 30) - 10, 10, 30, BLUE);
                    DrawText("p1 Score: " + p1score, MeasureText("p1 Score: " + p1score, 30) - 120, 10, 30, BLUE);
                    DrawLine(screenWidth / 2, screenHeight - 10, screenWidth / 2, 0, BLUE);

                    DrawRectangle((int)player.x, (int)player.y, player.width, player.height, BLUE);
                    DrawRectangle((int)player2.x, (int)player2.y, player2.width, player2.height, BLUE);
                    DrawRectangle((int)ball.x, (int)ball.y, ball.width, ball.height, BLUE);
                    DrawText("Press ENTER to continue", screenWidth / 2 - MeasureText("Press ENTER to continue", 30) / 2, screenHeight / 3, 30, BLUE);
                    break;
            }
            EndDrawing();
        }
        public static bool GetCollisions(float x, float y, int width, int height, float otherX, float otherY, int otherWidth, int otherHeight) {
            // square collisions
            if (x < otherX + otherWidth &&
                x + width > otherX &&
                y < otherY + otherHeight &&
                height + y > otherY)
            {
                return true;
            }
            return false;
        }

        public static float DeltaTime() {
            DateTime time1 = DateTime.Now;
            DateTime time2 = DateTime.Now;

            time2 = DateTime.Now;
            float deltaTime = (time2.Ticks - time1.Ticks) / 10000000f; 
            time1 = time2;
            return deltaTime;
        }
    }
}
