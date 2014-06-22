using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IrrKlang;

namespace IMPORT_PLATFORM
{
    public enum SFXType { Hurt = 0, PickUp = 1, Jump = 2, Fall = 3, ButtenPress = 4, GameOver = 5, WonLevel = 6 }
    public sealed class MusicManager
    {
        #region Singleton Initialization

        static readonly MusicManager _instance = new MusicManager();
        public static MusicManager Instance
        {
            get
            {
                return _instance;
            }
        }
        private MusicManager()
        {
        }

        #endregion

        #region Declerations

        private FarioMain mainGame;

        int musicIndex = 0;

        float musicVolume;
        float effectsVolume;
        float masterVolume;

        public bool Stopped { get; set; }

        List<string> BackgroundMusics;
        ISound backgroundSound;

        Dictionary<SFXType, string> Effects;
        ISound fallSound;
        ISound hurtSound;
        ISound effectSound;

        ISoundEngine soundEngine;

        #endregion

        #region Properties

        private string SoundFolderPath
        {
            get
            {
                return (mainGame.Configurations.StartUpPath + "/Content/Sounds/");
            }
        }

        #endregion

        public void LoadContent(FarioMain parentGame)
        {
            this.mainGame = parentGame;

            this.masterVolume = mainGame.SoundSettings.MasterVolume;
            this.musicVolume = mainGame.SoundSettings.MasterVolume * mainGame.SoundSettings.MusicVolume;
            this.effectsVolume = mainGame.SoundSettings.MasterVolume * mainGame.SoundSettings.EffectsVolume;

            soundEngine = new ISoundEngine();
            soundEngine.SoundVolume = masterVolume;

            BackgroundMusics = new List<string>();
            BackgroundMusics.Add(SoundFolderPath + "Background/Background0.mp3");
            BackgroundMusics.Add(SoundFolderPath + "Background/Background1.mp3");

            Effects = new Dictionary<SFXType, string>();
            Effects.Add(SFXType.ButtenPress, SoundFolderPath + "Effects/ButtenPress.wav");
            Effects.Add(SFXType.Fall, SoundFolderPath + "Effects/Fall.wav");
            Effects.Add(SFXType.GameOver, SoundFolderPath + "Effects/GameOver.ogg");
            Effects.Add(SFXType.Hurt, SoundFolderPath + "Effects/Hurt.wav");
            Effects.Add(SFXType.Jump, SoundFolderPath + "Effects/Jump.wav");
            Effects.Add(SFXType.PickUp, SoundFolderPath + "Effects/PickUp.wav");
            Effects.Add(SFXType.WonLevel, SoundFolderPath + "Effects/WonLevel.wav");

            if (mainGame.SoundSettings.UseSounds)
            {
                if (mainGame.SoundSettings.useMusic)
                {
                    musicIndex = 0;
                    backgroundSound = soundEngine.Play2D(BackgroundMusics[musicIndex]);
                    musicIndex++;
                    backgroundSound.Volume = musicVolume;
                    musicIndex = musicIndex % BackgroundMusics.Count;
                }
            }
        }

        #region Helper Methods

        public void PlayEffect(SFXType effectType)
        {
            if (mainGame.SoundSettings.UseSounds)
            {
                if (mainGame.SoundSettings.useEffects)
                {
                    if (Effects.ContainsKey(effectType))
                    {
                        if (effectType == SFXType.Fall)
                        {
                            if (!mainGame.GamePlayer.Dead)
                            {
                                if (fallSound == null)
                                {
                                    fallSound = soundEngine.Play2D(Effects[effectType]);
                                    fallSound.Volume = effectsVolume;
                                }
                                else
                                {
                                    if (fallSound.Finished)
                                    {
                                        fallSound = soundEngine.Play2D(Effects[effectType]);
                                        fallSound.Volume = effectsVolume;
                                    }
                                }
                            }
                        }
                        else if (effectType == SFXType.Hurt)
                        {
                            if (!mainGame.GamePlayer.Dead)
                            {
                                if (hurtSound == null)
                                {
                                    hurtSound = soundEngine.Play2D(Effects[effectType]);
                                    hurtSound.Volume = effectsVolume;
                                }
                                else
                                {
                                    if (hurtSound.Finished)
                                    {
                                        hurtSound = soundEngine.Play2D(Effects[effectType]);
                                        hurtSound.Volume = effectsVolume;
                                    }
                                }
                            }
                        }
                        else
                        {
                            effectSound = soundEngine.Play2D(Effects[effectType]);
                            effectSound.Volume = effectsVolume;
                        }
                    }
                }
            }
        }

        public void StopLoop()
        {
            if (backgroundSound != null)
            {
                backgroundSound.Stop();
                Stopped = true;
            }                
        }

        public void PauseLoop()
        {
            backgroundSound.Paused = true;
            Stopped = true;
        }

        public void StartLoop()
        {
            if (mainGame.SoundSettings.UseSounds)
            {
                if (mainGame.SoundSettings.useMusic)
                {
                    if (backgroundSound.Finished || backgroundSound.Paused)
                    {
                        backgroundSound = soundEngine.Play2D(BackgroundMusics[musicIndex]);
                        backgroundSound.Volume = musicVolume;
                        musicIndex++;
                        musicIndex = musicIndex % BackgroundMusics.Count;
                        Stopped = false;
                    }
                }
            }
        }

        public void ResumeLoop()
        {
            if (mainGame.SoundSettings.UseSounds)
            {
                if (mainGame.SoundSettings.useMusic)
                {
                    backgroundSound.Paused = false;
                    Stopped = false;
                }
            }
        }

        #endregion

        public void Update()
        {
            if (mainGame.SoundSettings.UseSounds)
            {
                if (mainGame.SoundSettings.useMusic)
                {
                    if (!Stopped)
                    {
                        if (backgroundSound == null)
                            backgroundSound = soundEngine.Play2D(BackgroundMusics[musicIndex]);

                        if (backgroundSound.Finished)
                        {
                            backgroundSound = soundEngine.Play2D(BackgroundMusics[musicIndex]);
                            backgroundSound.Volume = musicVolume;
                            musicIndex++;
                            musicIndex = musicIndex % BackgroundMusics.Count;
                        }
                    }
                    soundEngine.Update();
                }
            }
        }
    }
}
