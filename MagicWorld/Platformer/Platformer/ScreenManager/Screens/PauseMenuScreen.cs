#region Using Statements
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using MagicWorld.Audio;
#endregion

namespace MagicWorld
{
    /// <summary>
    /// The pause menu comes up over the top of the game,
    /// giving the player options to resume or quit.
    /// </summary>
    class PauseMenuScreen : MenuScreen
    {
        #region Initialization

        Level level;
        IAudioService audioService;
        /// <summary>
        /// Constructor.
        /// </summary>
        public PauseMenuScreen(Level level)
            : base("Paused")
        {
            // Create our menu entries.
            MenuEntry resumeGameMenuEntry = new MenuEntry("Resume Game");
            MenuEntry quitGameMenuEntry = new MenuEntry("Main Menu");
            MenuEntry restartLevelMenuEntry = new MenuEntry("Restart Level");
            MenuEntry optionsGameMenuEntry = new MenuEntry("Options");

            this.level = level;

            // Hook up menu event handlers.
            resumeGameMenuEntry.Selected += OnCancel;
            quitGameMenuEntry.Selected += QuitGameMenuEntrySelected;
            restartLevelMenuEntry.Selected += RestartLevelMenuEntrySelected;
            optionsGameMenuEntry.Selected += OptionsMenuEntrySelected;

            // Add entries to the menu.
            MenuEntries.Add(resumeGameMenuEntry);
            MenuEntries.Add(restartLevelMenuEntry);
            MenuEntries.Add(optionsGameMenuEntry);
            MenuEntries.Add(quitGameMenuEntry);

        }


        #endregion

        #region Handle Input


        /// <summary>
        /// Event handler for when the Quit Game menu entry is selected.
        /// </summary>
        void QuitGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            const string message = "Are you sure you want to quit this game?";

            MessageBoxScreen confirmQuitMessageBox = new MessageBoxScreen(message);

            confirmQuitMessageBox.Accepted += ConfirmQuitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmQuitMessageBox, ControllingPlayer);
        }


        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to quit" message box. This uses the loading screen to
        /// transition from the game back to the main menu screen.
        /// </summary>
        void ConfirmQuitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {   
                
            LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(),
                                                           new MainMenuScreen());
        }

        void RestartLevelMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            
            const string message = "Are you sure you want to restart the level?";

            MessageBoxScreen confirmQuitMessageBox = new MessageBoxScreen(message);

            confirmQuitMessageBox.Accepted += RestartLevelMessageBoxAccepted;

            ScreenManager.AddScreen(confirmQuitMessageBox, ControllingPlayer);

            
        }

        void RestartLevelMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {           

            GameplayScreen gameScreen = (GameplayScreen)ScreenManager.GetPlayScreen();
            
            gameScreen.ReloadCurrentLevel();

            ExitScreen();          
           
            
        }

        /// <summary>
        /// Event handler for when the Options menu entry is selected.
        /// </summary>
        void OptionsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenuScreen(), e.PlayerIndex);
        }

        protected override void OnCancel(PlayerIndex playerIndex)
        {
            audioService = (IAudioService)ScreenManager.Game.Services.GetService(typeof(IAudioService));
            audioService.playSound(SoundType.pause);
            audioService.setBackgroundVolume(1f);
            level.Pause = false;
            ExitScreen();
        }

        #endregion
    }
}
